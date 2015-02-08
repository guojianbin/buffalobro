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
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities.PowerShellTestAbstraction.Interfaces;

    [TestClass]
    public class GrantHttpAccessCmdletTests : IntegrationTestBase
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("PowerShell")]
        [TestCategory("Scenario")]
        public void CanGrantHttpAccessToHDInsightCluster()
        {
            var creds = GetValidCredentials();
            using (var runspace = this.GetPowerShellRunspace())
            {
                var cluster = GetClusterWithHttpAccessDisabled(runspace);
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.GrantAzureHDInsightHttpAccess)
                                      .WithParameter(CmdletConstants.Subscription, creds.SubscriptionId.ToString())
                                      .WithParameter(CmdletConstants.Location, cluster.Location)
                                      .WithParameter(CmdletConstants.Name, cluster.Name)
                                      .WithParameter(CmdletConstants.Credential, IntegrationTestBase.GetAzurePsCredentials())
                                      .Invoke();

                var accessgrantedCluster = GetCluster(creds, cluster.Name, runspace);
                Assert.IsNotNull(accessgrantedCluster);
                Assert.AreEqual(accessgrantedCluster.HttpUserName, IntegrationTestBase.TestCredentials.AzureUserName);
                Assert.AreEqual(accessgrantedCluster.HttpPassword, IntegrationTestBase.TestCredentials.AzurePassword);
            }
        }

        internal static AzureHDInsightCluster GetClusterWithHttpAccessDisabled(IRunspace runspace)
        {
            var creds = GetValidCredentials();
            var results =
                runspace.NewPipeline()
                        .AddCommand(CmdletConstants.GetAzureHDInsightCluster)
                        .WithParameter(CmdletConstants.Subscription, creds.SubscriptionId.ToString())
                        .WithParameter(CmdletConstants.Certificate, creds.Certificate)
                        .Invoke();

            var testClusters = results.Results.ToEnumerable<AzureHDInsightCluster>().ToList();
            var testCluster = testClusters.FirstOrDefault(cluster => cluster.HttpUserName.IsNullOrEmpty());
            if (testCluster == null)
            {
                testCluster = testClusters.Last();
                RevokeHttpAccessToCluster(creds, testCluster, runspace);
            }

            return testCluster;
        }

        internal static void RevokeHttpAccessToCluster(IHDInsightCertificateCredential connectionCredentials, AzureHDInsightCluster cluster, IRunspace runspace)
        {
            var results =
                runspace.NewPipeline()
                        .AddCommand(CmdletConstants.RevokeAzureHDInsightHttpAccess)
                        .WithParameter(CmdletConstants.Subscription, connectionCredentials.SubscriptionId.ToString())
                        .WithParameter(CmdletConstants.Location, cluster.Location)
                        .WithParameter(CmdletConstants.Name, cluster.Name)
                        .Invoke();

            var accessRevokedCluster = GetCluster(connectionCredentials, cluster.Name, runspace);
            Assert.IsNotNull(accessRevokedCluster);
            Assert.IsTrue(string.IsNullOrEmpty(accessRevokedCluster.HttpUserName));
            Assert.IsTrue(string.IsNullOrEmpty(accessRevokedCluster.HttpPassword));
        }

        internal static AzureHDInsightCluster GetCluster(IHDInsightCertificateCredential connectionCredentials, string clusterName, IRunspace runspace)
        {
            var results =
              runspace.NewPipeline()
                      .AddCommand(CmdletConstants.GetAzureHDInsightCluster)
                      .WithParameter(CmdletConstants.Subscription, connectionCredentials.SubscriptionId.ToString())
                      .WithParameter(CmdletConstants.Name, clusterName)
                      .Invoke();

            var clusters = results.Results.ToEnumerable<AzureHDInsightCluster>().ToList();
            return clusters.FirstOrDefault();
        }
    }
}
