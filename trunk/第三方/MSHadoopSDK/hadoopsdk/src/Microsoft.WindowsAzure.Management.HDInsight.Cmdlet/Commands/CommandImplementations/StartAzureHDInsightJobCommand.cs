﻿// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License.  You may obtain a copy
// of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
// WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABLITY OR NON-INFRINGEMENT.
// 
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.
namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.CommandImplementations
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Hadoop.Client;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.CommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;

    internal class StartAzureHDInsightJobCommand : AzureHDInsightJobCommand<AzureHDInsightJob>, IStartAzureHDInsightJobCommand
    {
        public AzureHDInsightJobDefinition JobDefinition { get; set; }

        private const int MaxLengthForJobName = 20;
        private const string HiveJobNameFormat = "Hive: {0}";

        public override async Task EndProcessing()
        {
            this.JobDefinition.ArgumentNotNull("JobDefinition");

            var client = this.GetClient(this.Cluster);
            JobCreationResults jobCreationResults = null;
            if (this.JobDefinition.StatusFolder.IsNullOrEmpty())
            {
                this.JobDefinition.StatusFolder = Guid.NewGuid().ToString();
            }

            var azureMapReduceJobDefinition = this.JobDefinition as AzureHDInsightMapReduceJobDefinition;
            var azureHiveJobDefinition = this.JobDefinition as AzureHDInsightHiveJobDefinition;
            var azurePigJobDefinition = this.JobDefinition as AzureHDInsightPigJobDefinition;
            var azureStreamingJobDefinition = this.JobDefinition as AzureHDInsightStreamingMapReduceJobDefinition;
            var azureSqoopJobDefinition = this.JobDefinition as AzureHDInsightSqoopJobDefinition;

            if (azureMapReduceJobDefinition != null)
            {
                if (azureMapReduceJobDefinition.JobName.IsNullOrEmpty())
                {
                    azureMapReduceJobDefinition.JobName = azureMapReduceJobDefinition.ClassName;
                }
                jobCreationResults = await SubmitMapReduceJob(azureMapReduceJobDefinition, client);
            }
            else if (azureHiveJobDefinition != null)
            {
                if (azureHiveJobDefinition.JobName.IsNullOrEmpty())
                {
                    azureHiveJobDefinition.JobName = string.Format(CultureInfo.InvariantCulture, HiveJobNameFormat, GetJobNameFromQueryOrFile(azureHiveJobDefinition.Query, azureHiveJobDefinition.File));

                }
                jobCreationResults = await SubmitHiveJob(azureHiveJobDefinition, client);
            }
            else if (azurePigJobDefinition != null)
            {
                jobCreationResults = await SubmitPigJob(azurePigJobDefinition, client);
            }
            else if (azureStreamingJobDefinition != null)
            {
                if (azureStreamingJobDefinition.JobName.IsNullOrEmpty())
                {
                    azureStreamingJobDefinition.JobName = GetLastSegment(azureStreamingJobDefinition.Mapper);
                }
                jobCreationResults = await CreateStreamingJob(azureStreamingJobDefinition, client);
            }
            else if (azureSqoopJobDefinition != null)
            {
                jobCreationResults = await CreateSqoopJob(azureSqoopJobDefinition, client);
            }
            else
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Cannot start jobDetails of type : {0}.", this.JobDefinition.GetType()));
            }

            var startedJob = await client.GetJobAsync(jobCreationResults.JobId);
            if (startedJob.ErrorCode.IsNotNullOrEmpty())
            {
                throw new InvalidOperationException("Failed to start jobDetails :" + startedJob.ErrorCode);
            }

            var jobDetail = new AzureHDInsightJob(startedJob, this.Cluster);
            this.Output.Add(jobDetail);
        }

        private static async Task<JobCreationResults> CreateStreamingJob(AzureHDInsightStreamingMapReduceJobDefinition azureStreamingJobDefinition, IJobSubmissionClient client)
        {
            var streamingJobDefinition = new StreamingMapReduceJobCreateParameters()
            {
                JobName = azureStreamingJobDefinition.JobName,
                Input = azureStreamingJobDefinition.Input,
                Output = azureStreamingJobDefinition.Output,
                Reducer = azureStreamingJobDefinition.Reducer,
                Combiner = azureStreamingJobDefinition.Combiner,
                Mapper = azureStreamingJobDefinition.Mapper
            };
            streamingJobDefinition.StatusFolder = azureStreamingJobDefinition.StatusFolder;
            streamingJobDefinition.CommandEnvironment.AddRange(azureStreamingJobDefinition.CommandEnvironment);
            streamingJobDefinition.Arguments.AddRange(azureStreamingJobDefinition.Arguments);
            streamingJobDefinition.Defines.AddRange(azureStreamingJobDefinition.Defines);
            streamingJobDefinition.Files.AddRange(azureStreamingJobDefinition.Files);

            var jobCreationResults = await client.CreateStreamingJobAsync(streamingJobDefinition);
            return jobCreationResults;
        }

        private static async Task<JobCreationResults> CreateSqoopJob(AzureHDInsightSqoopJobDefinition azureSqoopJobDefinition, IJobSubmissionClient client)
        {
            var sqoopJobDefinition = azureSqoopJobDefinition.ToSqoopJobCreateParameters();

            var jobCreationResults = await client.CreateSqoopJobAsync(sqoopJobDefinition);
            return jobCreationResults;
        }

        private static async Task<JobCreationResults> SubmitPigJob(AzureHDInsightPigJobDefinition azurePigJobDefinition, IJobSubmissionClient client)
        {
            var pigJobDefinition = new PigJobCreateParameters()
            {
                Query = azurePigJobDefinition.Query,
                File = azurePigJobDefinition.File
            };

            pigJobDefinition.StatusFolder = azurePigJobDefinition.StatusFolder;
            pigJobDefinition.Arguments.AddRange(azurePigJobDefinition.Arguments);
            pigJobDefinition.Files.AddRange(azurePigJobDefinition.Files);

            var jobCreationResults = await client.CreatePigJobAsync(pigJobDefinition);
            return jobCreationResults;
        }

        private static async Task<JobCreationResults> SubmitHiveJob(AzureHDInsightHiveJobDefinition azureHiveJobDefinition, IJobSubmissionClient client)
        {
            var hiveJobDefinition = new HiveJobCreateParameters()
            {
                JobName = azureHiveJobDefinition.JobName,
                Query = azureHiveJobDefinition.Query,
                File = azureHiveJobDefinition.File
            };

            hiveJobDefinition.StatusFolder = azureHiveJobDefinition.StatusFolder;
            hiveJobDefinition.Arguments.AddRange(azureHiveJobDefinition.Arguments);
            hiveJobDefinition.Defines.AddRange(azureHiveJobDefinition.Defines);
            hiveJobDefinition.Files.AddRange(azureHiveJobDefinition.Files);

            var jobCreationResults = await client.CreateHiveJobAsync(hiveJobDefinition);
            return jobCreationResults;
        }

        private static async Task<JobCreationResults> SubmitMapReduceJob(AzureHDInsightMapReduceJobDefinition azureMapReduceJobDefinition, IJobSubmissionClient client)
        {
            var mapReduceJobDefinition = azureMapReduceJobDefinition.ToMapReduceJobCreateParameters();
            var jobCreationResults = await client.CreateMapReduceJobAsync(mapReduceJobDefinition);
            return jobCreationResults;
        }

        private static string GetJobNameFromQueryOrFile(string query, string file)
        {
            string jobName;
            if (query.IsNotNullOrEmpty())
            {
                jobName = query.Substring(0, Math.Min(MaxLengthForJobName, query.Length));
            }
            else
            {
                jobName = GetLastSegment(file);
            }
            return jobName;
        }

        private static string GetLastSegment(string uriString)
        {
            Uri uriAddress = null;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uriAddress))
            {
            string fileName = uriAddress.Segments.Last();
            return fileName.Substring(0, Math.Min(MaxLengthForJobName, fileName.Length));
        }

            string lastSegment = uriString.Split('/').Last();
            return lastSegment.Substring(0, Math.Min(MaxLengthForJobName, lastSegment.Length));
    }
}
}
