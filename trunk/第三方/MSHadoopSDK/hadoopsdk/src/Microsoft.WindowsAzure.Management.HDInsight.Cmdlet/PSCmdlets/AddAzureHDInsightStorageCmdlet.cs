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
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.BaseCommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.Commands.CommandInterfaces;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.ServiceLocation;
    using Microsoft.WindowsAzure.Management.HDInsight.InversionOfControl;
    using Microsoft.WindowsAzure.Management.HDInsight.Logging;

    /// <summary>
    /// Adds an AzureHDInsight Storage Account to the current configuration.
    /// </summary>
    [Cmdlet(VerbsCommon.Add, AzureHdInsightPowerShellConstants.AzureHDInsightStorage)]
    public class AddAzureHDInsightStorageCmdlet : AzureHDInsightCmdlet, IAddAzureHDInsightStorageBase
    {
        private IAddAzureHDInsightStorageCommand command;

        /// <inheritdoc />
        protected override void StopProcessing()
        {
            this.command.Cancel();
        }

        /// <summary>
        /// Initializes a new instance of the AddAzureHDInsightStorageCmdlet class.
        /// </summary>
        public AddAzureHDInsightStorageCmdlet()
        {
            this.command = ServiceLocator.Instance.Locate<IAzureHDInsightCommandFactory>().CreateAddStorage();
        }

        /// <summary>
        /// Gets or sets the Azure HDInsight Configuration for the Azure HDInsight cluster being constructed.
        /// </summary>
        [Parameter(Position = 0, Mandatory = true,
                   HelpMessage = "The HDInsight cluster configuration to use when creating the new cluster (created by New-AzureHDInsightConfig).",
                   ValueFromPipeline = true,
                   ParameterSetName = AzureHdInsightPowerShellConstants.ParameterSetAddStorageAccount)]
        public AzureHDInsightConfig Config
        {
            get { return this.command.Config; }
            set
            {
                if (value.IsNull())
                {
                    throw new ArgumentNullException("value",
                                                    "The value for the configuration can not be null.");
                }
                this.command.Config.ClusterSizeInNodes = value.ClusterSizeInNodes;
                this.command.Config.DefaultStorageAccount = value.DefaultStorageAccount;
                this.command.Config.HiveMetastore = value.HiveMetastore ?? this.command.Config.HiveMetastore;
                this.command.Config.OozieMetastore = value.OozieMetastore ?? this.command.Config.OozieMetastore;
                this.command.Config.AdditionalStorageAccounts.AddRange(value.AdditionalStorageAccounts);
            }
        }

        /// <summary>
        /// Gets or sets the Storage Account Name for the storage account to be added to the cluster.
        /// </summary>
        [Parameter(Position = 1, Mandatory = true,
            HelpMessage = "The storage account name for the storage account to be added to the new cluster.",
            ValueFromPipeline = false,
            ParameterSetName = AzureHdInsightPowerShellConstants.ParameterSetAddStorageAccount)]
        public string StorageAccountName
        {
            get { return this.command.StorageAccountName; }
            set { this.command.StorageAccountName = value; }
        }

        /// <summary>
        /// Gets or sets the Storage Account key for the storage account to be added to the cluster.
        /// </summary>
        [Parameter(Position = 2, Mandatory = true,
            HelpMessage = "The storage account key for the storage account to be added to the new cluster.",
            ValueFromPipeline = false,
            ParameterSetName = AzureHdInsightPowerShellConstants.ParameterSetAddStorageAccount)]
        public string StorageAccountKey
        {
            get { return this.command.StorageAccountKey; }
            set { this.command.StorageAccountKey = value; }
        }

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }

        /// <inheritdoc />
        protected override void EndProcessing()
        {
            try
            {
                this.command.EndProcessing().Wait();
                foreach (var output in this.command.Output)
                {
                    this.WriteObject(output);
                }
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
