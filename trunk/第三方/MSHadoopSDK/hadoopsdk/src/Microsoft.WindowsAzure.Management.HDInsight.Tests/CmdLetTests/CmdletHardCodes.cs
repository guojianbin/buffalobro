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
namespace Microsoft.WindowsAzure.Management.HDInsight.Tests.CmdLetTests
{
    public static class CmdletConstants
    {
        public const string Force = "Force";
        public const string GetAzureHDInsightCluster = "Get-AzureHDInsightCluster";
        public const string UseAzureHDInsightCluster = "Use-AzureHDInsightCluster";
        public const string InvokeHive = "Invoke-Hive";
        public const string ExecHiveAlias = "hive";
        public const string ExecHiveQueryAlias = "e";
        public const string GrantAzureHDInsightHttpAccess = "Grant-AzureHDInsightHttpServicesAccess";
        public const string RevokeAzureHDInsightHttpAccess = "Revoke-AzureHDInsightHttpServicesAccess";
        public const string GetAzureHDInsightProperties = "Get-AzureHDInsightProperties";
        public const string NewAzureHDInsightCluster = "New-AzureHDInsightCluster";
        public const string RemoveAzureHDInsightCluster = "Remove-AzureHDInsightCluster";
        public const string NewAzureHDInsightClusterConfig = "New-AzureHDInsightClusterConfig";
        public const string SetAzureHDInsightDefaultStorage = "Set-AzureHDInsightDefaultStorage";
        public const string AddAzureHDInsightConfigValues = "Add-AzureHDInsightConfigValues";
        public const string AddAzureHDInsightStorage = "Add-AzureHDInsightStorage";
        public const string AddAzureHDInsightMetastore = "Add-AzureHDInsightMetastore";
        public const string GetAzureHDInsightJob = "Get-AzureHDInsightJob";
        public const string GetAzureHDInsightJobOutput = "Get-AzureHDInsightJobOutput";
        public const string StopAzureHDInsightJob = "Stop-AzureHDInsightJob";
        public const string NewAzureHDInsightMapReduceJobDefinition = "New-AzureHDInsightMapReduceJobDefinition";
        public const string NewAzureHDInsightStreamingMapReduceJobDefinition = "New-AzureHDInsightStreamingMapReduceJobDefinition";
        public const string StartAzureHDInsightJob = "Start-AzureHDInsightJob";
        public const string NewAzureHDInsightPigJobDefinition = "New-AzureHDInsightPigJobDefinition";
        public const string NewAzureHDInsightHiveJobDefinition = "New-AzureHDInsightHiveJobDefinition";
        public const string NewAzureHDInsightSqoopJobDefinition = "New-AzureHDInsightSqoopJobDefinition";
        public const string WaitAzureHDInsightJob = "Wait-AzureHDInsightJob";
        public const string ClusterConfig = "Config";
        public const string Debug = "Debug";
        public const string CoreConfig = "Core";
        public const string HdfsConfig = "Hdfs";
        public const string MapReduceConfig = "MapReduce";
        public const string HiveConfig = "Hive";
        public const string OozieConfig = "Oozie";
        public const string JobDefinition = "Job";
        public const string JarFile = "Jar";
        public const string Job = "Job";
        public const string Query = "QueryText";
        public const string Command = "Command";
        public const string QueryFile = "QueryFile";
        public const string File = "File";
        public const string Files = "Files";
        public const string JobName = "Name";
        public const string ClassName = "Class";
        public const string Parameters = "Params";
        public const string Arguments = "Args";
        public const string LibJars = "LibJars";
        public const string HiveArgs = "Arguments";

        public const string Mapper = "Mapper";
        public const string Reducer = "Reducer";
        public const string Input = "Input";
        public const string Output = "Output";
        public const string InputFormat = "InputFormat";
        public const string OutputFormat = "OutputFormat";
        public const string Combiner = "Combiner";
        public const string Partitioner = "Partitioner";
        public const string InputReader = "InputReader";
        public const string StatusFolder = "StatusFolder";

        public const string Subscription = "Subscription";
        public const string Certificate = "Certificate";
        public const string Versions = "Versions";
        public const string Locations = "Locations";
        public const string Credential = "Credential";
        public const string WaitTimeoutInSeconds = "WaitTimeoutInSeconds";
        public const string Cluster = "Cluster";
        public const string Name = "Name";
        public const string Version = "Version";
        public const string StdErr = "StandardError";
        public const string TaskSummary = "TaskSummary";
        public const string DownloadTaskLogs = "DownloadTaskLogs";
        public const string TaskLogsDirectory = "TaskLogsDirectory";
        public const string JobId = "Id";
        public const string Skip = "Skip";
        public const string Show = "Show";
        public const string FromDateTime = "From";
        public const string ToDateTime = "To";
        public const string Location = "Location";
        public const string EastUs = "East US";
        public const string DefaultStorageAccountName = "DefaultStorageAccountName";
        public const string StorageAccountName = "StorageAccountName";
        public const string DefaultStorageAccountKey = "DefaultStorageAccountKey";
        public const string StorageAccountKey = "StorageAccountKey";
        public const string DefaultStorageContainerName = "DefaultStorageContainerName";
        public const string StorageContainerName = "StorageContainerName";
        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string SqlAzureServerName = "SqlAzureServerName";
        public const string DatabaseName = "DatabaseName";
        public const string MetastoreType = "MetastoreType";
        public const string ClusterSizeInNodes = "ClusterSizeInNodes";
    }
}
