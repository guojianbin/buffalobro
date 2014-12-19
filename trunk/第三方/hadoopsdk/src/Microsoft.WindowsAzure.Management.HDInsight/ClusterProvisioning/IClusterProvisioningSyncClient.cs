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

namespace Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning.Data;

    /// <summary>
    /// Client Library that allows interacting with the Azure HDInsight Deployment Service.
    /// </summary>
    public interface IClusterProvisioningSyncClient : IClusterProvisioningClientBase
    {
        /// <summary>
        /// Queries the HDInsight Clusters registered.
        /// </summary>
        /// <returns>List of registered HDInsight Clusters.</returns>
        Collection<HDInsightCluster> ListClusters();

        /// <summary>
        /// Queries for a specific HDInsight Cluster registered.
        /// </summary>
        /// <param name="dnsName">Name of the HDInsight cluster.</param>
        /// <returns>HDInsight Cluster or NULL if not found.</returns>
        HDInsightCluster GetCluster(string dnsName);

        /// <summary>
        /// Submits a request to create an HDInsight cluster and waits for it to complete.
        /// </summary>
        /// <param name="cluster">Request object that encapsulates all the configurations.</param>
        /// <returns>Object that represents the HDInsight Cluster created.</returns>
        HDInsightCluster CreateCluster(HDInsightClusterCreationDetails cluster);

        /// <summary>
        /// Submits a request to create an HDInsight cluster and waits for it to complete.
        /// </summary>
        /// <param name="cluster">Request object that encapsulates all the configurations.</param>
        /// <param name="timeout">Timeout interval for the operation.</param>
        /// <returns>Object that represents the HDInsight Cluster created.</returns>
        HDInsightCluster CreateCluster(HDInsightClusterCreationDetails cluster, TimeSpan timeout);

        /// <summary>
        /// Submits a request to delete an HDInsight cluster and waits for it to complete.
        /// </summary>
        /// <param name="dnsName">Name of the HDInsight cluster.</param>
        void DeleteCluster(string dnsName);

        /// <summary>
        /// Submits a request to delete an HDInsight cluster and waits for it to complete.
        /// </summary>
        /// <param name="dnsName">Name of the HDInsight cluster.</param>
        /// <param name="timeout">Timeout interval for the operation.</param>
        void DeleteCluster(string dnsName, TimeSpan timeout);
    }
}
