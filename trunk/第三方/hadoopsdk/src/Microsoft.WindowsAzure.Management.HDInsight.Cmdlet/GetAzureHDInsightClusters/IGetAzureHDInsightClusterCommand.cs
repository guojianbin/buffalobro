﻿namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters
{
    using System;
    using System.Collections.ObjectModel;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.PSCmdlets;

    internal interface IGetAzureHDInsightClusterCommand : IAzureHDInsightCommand<AzureHDInsightCluster>, IGetAzureHDInsightClusterBase
    {
    }
}