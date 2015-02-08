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
namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.PSCmdlets
{
    using System;
    using System.Management.Automation;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.BaseCommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.CommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.ServiceLocation;
    using Microsoft.WindowsAzure.Management.HDInsight.Logging;

    /// <summary>
    /// Cmdlet that stops a jobDetails running on a HDInsight cluster.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Stop, AzureHdInsightPowerShellConstants.AzureHDInsightJobs)]
    public class StopAzureHDInsightJobCmdlet : AzureHDInsightCmdlet, IStopAzureHDInsightJobBase
    {
        private readonly IStopAzureHDInsightJobCommand command;

        /// <summary>
        /// Initializes a new instance of the StopAzureHDInsightJobCmdlet class.
        /// </summary>
        public StopAzureHDInsightJobCmdlet()
        {
            this.command = ServiceLocator.Instance.Locate<IAzureHDInsightCommandFactory>().CreateStopJob();
        }

        /// <inheritdoc />
        protected override void StopProcessing()
        {
            this.command.Cancel();
        }

        /// <inheritdoc />
        [Parameter(Mandatory = true,
                  HelpMessage = "The JobID of the jobDetails to stop.",
                  ValueFromPipeline = true)]
        [Alias(AzureHdInsightPowerShellConstants.JobId)]
        public string JobId
        {
            get { return this.command.JobId; }
            set { this.command.JobId = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = true,
                  HelpMessage = "The credentials to connect to Azure HDInsight cluster.",
                  ValueFromPipeline = true)]
        [Alias(AzureHdInsightPowerShellConstants.AliasCredentials)]
        public PSCredential Credential
        {
            get { return this.command.Credential; }
            set { this.command.Credential = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = true,
                  HelpMessage = "The endpoint to connect to the Azure HDInsight cluster.",
                  ValueFromPipeline = true)]
        public string Cluster
        {
            get { return this.command.Cluster; }
            set { this.command.Cluster = value; }
        }

        /// <inheritdoc />
        [Parameter(Position = 1, Mandatory = true,
                   HelpMessage = "The subscription id for the Azure subscription.",
                   ParameterSetName = AzureHdInsightPowerShellConstants.ParameterSetStartJobByNameWithSpecificSubscriptionCredentials)]
        [Alias(AzureHdInsightPowerShellConstants.AliasSub)]
        public string Subscription
        {
            get { return this.command.Subscription; }
            set { this.command.Subscription = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The management certificate used to manage the Azure subscription.",
                   ParameterSetName = AzureHdInsightPowerShellConstants.ParameterSetStartJobByNameWithSpecificSubscriptionCredentials)]
        [Alias(AzureHdInsightPowerShellConstants.AliasCert)]
        public X509Certificate2 Certificate
        {
            get { return this.command.Certificate; }
            set { this.command.Certificate = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The Endpoint to use when connecting to Azure.",
                   ParameterSetName = AzureHdInsightPowerShellConstants.ParameterSetStartJobByNameWithSpecificSubscriptionCredentials)]
        public Uri EndPoint
        {
            get { return this.command.EndPoint; }
            set { this.command.EndPoint = value; }
        }

        /// <inheritdoc />
        [Parameter(Mandatory = false,
                   HelpMessage = "The CloudServiceName to use when connecting to the HDInsight cluster.",
                   ParameterSetName = AzureHdInsightPowerShellConstants.ParameterSetStartJobByNameWithSpecificSubscriptionCredentials)]
        public string CloudServiceName
        {
            get { return this.command.CloudServiceName; }
            set { this.command.CloudServiceName = value; }
        }

        /// <inheritdoc />
        protected override void EndProcessing()
        {
            try
            {
                this.command.Logger = this.Logger;
                var task = this.command.EndProcessing();
                var token = this.command.CancellationToken;
                while (!task.IsCompleted)
                {
                    this.WriteDebugLog();
                    task.Wait(1000, token);
                }
                if (task.IsFaulted)
                {
                    throw new AggregateException(task.Exception);
                }
                foreach (var output in this.command.Output)
                {
                    this.WriteObject(output);
                }
                this.WriteDebugLog();
            }
            catch (Exception ex)
            {
                var type = ex.GetType();
                this.Logger.Log(Severity.Error, Verbosity.Normal, this.FormatException(ex));
                this.WriteDebugLog();
                if (type == typeof(AggregateException) || type == typeof(TargetInvocationException) || type == typeof(TaskCanceledException))
                {
                    ex.Rethrow();
                }
                else
                {
                    throw;
                }
            }
            this.WriteDebugLog();
        }
    }
}