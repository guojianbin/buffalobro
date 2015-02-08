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
namespace Microsoft.Hadoop.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Hadoop.Client.ClientLayer;
    using Microsoft.Hadoop.Client.HadoopJobSubmissionPocoClient;
    using Microsoft.Hadoop.Client.HadoopJobSubmissionPocoClient.RemoteHadoop;
    using Microsoft.WindowsAzure.Management.HDInsight;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.ServiceLocation;

    /// <summary>
    /// Represents a Hadoop client that can be used to submit jobs to an Hadoop cluster.
    /// </summary>
    internal class RemoteHadoopClient : ClientBase, IJobSubmissionClient
    {
        private BasicAuthCredential credentials;

        internal RemoteHadoopClient(BasicAuthCredential credentials)
        {
            this.credentials = credentials;
        }

        public event EventHandler<WaitJobStatusEventArgs> JobStatusEvent;

        internal void RaiseJobStatusEvent(object sender, WaitJobStatusEventArgs e)
        {
        }

        /// <inheritdoc />
        public void HandleClusterWaitNotifyEvent(JobDetails jobDetails)
        {
            var handler = this.JobStatusEvent;
            if (handler.IsNotNull())
            {
                handler(this, new WaitJobStatusEventArgs(jobDetails));
            }
        }

        /// <inheritdoc />
        public async Task<JobList> ListJobsAsync()
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            var jobList = await pocoClient.ListJobs();
            return jobList;
        }

        /// <inheritdoc />
        public Task<JobDetails> GetJobAsync(string jobId)
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            return pocoClient.GetJob(jobId);
        }

        /// <inheritdoc />
        public async Task<JobCreationResults> CreateMapReduceJobAsync(MapReduceJobCreateParameters mapReduceJobCreateParameters)
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            return await pocoClient.SubmitMapReduceJob(mapReduceJobCreateParameters);
        }

        /// <inheritdoc />
        public Task<JobCreationResults> CreateHiveJobAsync(HiveJobCreateParameters hiveJobCreateParameters)
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            return pocoClient.SubmitHiveJob(hiveJobCreateParameters);
        }

        /// <inheritdoc />
        public Task<JobCreationResults> CreatePigJobAsync(PigJobCreateParameters pigJobCreateParameters)
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            return pocoClient.SubmitPigJob(pigJobCreateParameters);
        }

        public Task<JobCreationResults> CreateSqoopJobAsync(SqoopJobCreateParameters sqoopJobCreateParameters)
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            return pocoClient.SubmitSqoopJob(sqoopJobCreateParameters);
        }

        /// <inheritdoc />
        public Task<JobDetails> StopJobAsync(string jobId)
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            return pocoClient.StopJob(jobId);
        }

        /// <inheritdoc />
        public Task<Stream> GetJobOutputAsync(string jobId)
        {
            throw new NotSupportedException("Access to cluster resources requires Subscription details.");
        }

        /// <inheritdoc />
        public Task<Stream> GetJobErrorLogsAsync(string jobId)
        {
            throw new NotSupportedException("Access to cluster resources requires Subscription details.");
        }

        public Task<Stream> GetJobTaskLogSummaryAsync(string jobId)
        {
            throw new NotSupportedException("Access to cluster resources requires Subscription details.");
        }

        /// <inheritdoc />
        public Task DownloadJobTaskLogsAsync(string jobId, string targetDirectory)
        {
            throw new NotSupportedException("Access to cluster resources requires Subscription details.");
        }

        /// <inheritdoc />
        public Task<JobCreationResults> CreateStreamingJobAsync(StreamingMapReduceJobCreateParameters streamingMapReduceJobCreateParameters)
        {
            var factory = ServiceLocator.Instance.Locate<IRemoteHadoopJobSubmissionPocoClientFactory>();
            var pocoClient = factory.Create(this.credentials, this.Context);
            return pocoClient.SubmitStreamingJob(streamingMapReduceJobCreateParameters);
        }

        /// <inheritdoc />
        public JobList ListJobs()
        {
            return this.ListJobsAsync().WaitForResult();
        }

        /// <inheritdoc />
        public JobDetails GetJob(string jobId)
        {
            return this.GetJobAsync(jobId).WaitForResult();
        }

        /// <inheritdoc />
        public JobCreationResults CreateMapReduceJob(MapReduceJobCreateParameters mapReduceJobCreateParameters)
        {
            return this.CreateMapReduceJobAsync(mapReduceJobCreateParameters).WaitForResult();
        }

        /// <inheritdoc />
        public JobCreationResults CreateStreamingJob(StreamingMapReduceJobCreateParameters streamingMapReduceJobCreateParameters)
        {
            return this.CreateStreamingJobAsync(streamingMapReduceJobCreateParameters).WaitForResult();
        }

        /// <inheritdoc />
        public JobCreationResults CreateHiveJob(HiveJobCreateParameters hiveJobCreateParameters)
        {
            return this.CreateHiveJobAsync(hiveJobCreateParameters).WaitForResult();
        }

        /// <inheritdoc />
        public JobCreationResults CreatePigJob(PigJobCreateParameters pigJobCreateParameters)
        {
            return this.CreatePigJobAsync(pigJobCreateParameters).WaitForResult();
        }

        /// <inheritdoc />
        public JobCreationResults CreateSqoopJob(SqoopJobCreateParameters sqoopJobCreateParameters)
        {
            return this.CreateSqoopJobAsync(sqoopJobCreateParameters).WaitForResult();
        }

        /// <inheritdoc />
        public JobDetails StopJob(string jobId)
        {
            return this.StopJobAsync(jobId).WaitForResult();
        }

        /// <inheritdoc />
        public Stream GetJobOutput(string jobId)
        {
            return this.GetJobOutputAsync(jobId).WaitForResult();
        }

        /// <inheritdoc />
        public Stream GetJobErrorLogs(string jobId)
        {
            return this.GetJobErrorLogsAsync(jobId).WaitForResult();
        }

        /// <inheritdoc />
        public Stream GetJobTaskLogSummary(string jobId)
        {
            return this.GetJobTaskLogSummaryAsync(jobId).WaitForResult();
        }

        /// <inheritdoc />
        public void DownloadJobTaskLogs(string jobId, string targetDirectory)
        {
            this.DownloadJobTaskLogsAsync(jobId, targetDirectory).WaitForResult();
        }
    }
}