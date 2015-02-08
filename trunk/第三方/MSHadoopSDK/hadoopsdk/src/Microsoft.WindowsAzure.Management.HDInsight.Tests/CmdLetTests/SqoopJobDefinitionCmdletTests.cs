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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities.PowerShellTestAbstraction.Interfaces;

    [TestClass]
    public class SqoopJobDefinitionCmdletTests : IntegrationTestBase
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
        [TestCategory("Scenario")]
        public void CanCreateSDKObjectFromPowershellObject()
        {
            var sqoopJobDefinition = new AzureHDInsightSqoopJobDefinition()
            {
                Command = "Import into sqlserver",
                File = "http://myfileshare.txt",
                StatusFolder = Guid.NewGuid().ToString(),
            };

            sqoopJobDefinition.Arguments.Add("arg1");
            sqoopJobDefinition.Files.Add("file1.sqoop");
            var sdkObject = sqoopJobDefinition.ToSqoopJobCreateParameters();

            Assert.AreEqual(sqoopJobDefinition.StatusFolder, sdkObject.StatusFolder);
            Assert.AreEqual(sqoopJobDefinition.File, sdkObject.File);
            Assert.AreEqual(sqoopJobDefinition.Command, sdkObject.Command);

            foreach (var file in sqoopJobDefinition.Files)
            {
                Assert.IsTrue(sdkObject.Files.Contains(file), file);
            }
        }
    }
}
