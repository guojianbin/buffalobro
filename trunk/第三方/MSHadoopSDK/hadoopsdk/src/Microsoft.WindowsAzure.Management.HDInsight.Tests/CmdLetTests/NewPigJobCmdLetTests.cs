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
    using System.Management.Automation;
    using Microsoft.Hadoop.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.GetAzureHDInsightClusters;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities;

    [TestClass]
    public class NewPigJobCmdLetTests : IntegrationTestBase
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
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightPigJobDefinition")]
        public void ICanCallThe_New_HDInsightPigJobDefinitionCmdlet()
        {
            var pigJobDefinition = new PigJobCreateParameters()
            {
                Query = "load 'passwd' using PigStorage(':'); B = foreach A generate $0 as id;"
            };

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightPigJobDefinition)
                                      .WithParameter(CmdletConstants.Query, pigJobDefinition.Query)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var pigJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightPigJobDefinition>().First();

                Assert.AreEqual(pigJobDefinition.Query, pigJobFromPowershell.Query);
            }
        }


        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightPigJobDefinition")]
        public void ICanCallThe_New_HDInsightPigJobDefinitionCmdlet_WithOutputStorageLocation()
        {
            var pigJobDefinition = new PigJobCreateParameters()
            {
                Query = "load 'passwd' using PigStorage(':'); B = foreach A generate $0 as id;",
                StatusFolder = "/passwordlogs"
            };

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightPigJobDefinition)
                                      .WithParameter(CmdletConstants.Query, pigJobDefinition.Query)
                                      .WithParameter(CmdletConstants.StatusFolder, pigJobDefinition.StatusFolder)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var pigJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightPigJobDefinition>().First();

                Assert.AreEqual(pigJobDefinition.Query, pigJobFromPowershell.Query);
                Assert.AreEqual(pigJobDefinition.StatusFolder, pigJobFromPowershell.StatusFolder);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightPigJobDefinition")]
        public void ICanCallThe_New_HDInsightPigJobDefinitionCmdlet_WithoutJobName()
        {
            var pigJobDefinition = new PigJobCreateParameters()
            {
                Query = "load 'passwd' using PigStorage(':'); B = foreach A generate $0 as id;"
            };

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightPigJobDefinition)
                                      .WithParameter(CmdletConstants.Query, pigJobDefinition.Query)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var pigJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightPigJobDefinition>().First();

                Assert.AreEqual(pigJobDefinition.Query, pigJobFromPowershell.Query);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightHiveJobDefinition")]
        public void ICannotCallThe_New_HDInsightPigJobDefinitionCmdlet_WithoutFileOrQueryParameter()
        {
            try
            {
                using (var runspace = this.GetPowerShellRunspace())
                {
                    runspace.NewPipeline()
                         .AddCommand(CmdletConstants.NewAzureHDInsightPigJobDefinition)
                         .Invoke();
                    Assert.Fail("test failed.");
                }
            }
            catch (CmdletInvocationException invokeException)
            {
                var psArgumentException = invokeException.GetBaseException() as PSArgumentException;
                Assert.IsNotNull(psArgumentException);
                Assert.AreEqual("Either File or Query should be specified for Pig jobs.", psArgumentException.Message);
            }
        }
        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightPigJobDefinition")]
        public void ICanCallThe_New_HDInsightPigJobDefinitionCmdlet_WithResources()
        {
            var pigJobDefinition = new PigJobCreateParameters()
            {
                Query = "load 'passwd' using PigStorage(':'); B = foreach A generate $0 as id;"
            };
            pigJobDefinition.Files.Add("pidata.txt");
            pigJobDefinition.Files.Add("pidate2.txt");

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightPigJobDefinition)
                                      .WithParameter(CmdletConstants.Query, pigJobDefinition.Query)
                                      .WithParameter(CmdletConstants.Files, pigJobDefinition.Files)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var pigJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightPigJobDefinition>().First();

                Assert.AreEqual(pigJobDefinition.Query, pigJobFromPowershell.Query);

                foreach (var file in pigJobDefinition.Files)
                {
                    Assert.IsTrue(
                        pigJobFromPowershell.Files.Any(arg => string.Equals(file, arg)),
                        "Unable to find File '{0}' in value returned from powershell",
                        file);
                }
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightPigJobDefinition")]
        public void ICanCallThe_New_HDInsightPigJobDefinitionCmdlet_WithArguments()
        {
            var pigJobDefinition = new PigJobCreateParameters()
            {
                File = Constants.WabsProtocolSchemeName + "container@accountname/pigquery.q"
            };

            pigJobDefinition.Arguments.Add("map.input.tasks=1000");
            pigJobDefinition.Arguments.Add("map.input.reducers=1000");

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightPigJobDefinition)
                                      .WithParameter(CmdletConstants.File, pigJobDefinition.File)
                                      .WithParameter(CmdletConstants.Arguments, pigJobDefinition.Arguments)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var pigJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightPigJobDefinition>().First();

                Assert.AreEqual(pigJobDefinition.File, pigJobFromPowershell.File);

                foreach (var argument in pigJobDefinition.Arguments)
                {
                    Assert.IsTrue(
                        pigJobFromPowershell.Arguments.Any(arg => string.Equals(argument, arg)), string.Format("Unable to find parameter '{0}' in value returned from powershell", argument));
                }
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightPigJobDefinition")]
        public void ICanCallThe_New_HDInsightPigJobDefinitionCmdlet_WithQueryFile()
        {
            var pigJobDefinition = new PigJobCreateParameters()
            {
                File = Constants.WabsProtocolSchemeName + "container@accountname/pigquery.q"
            };

            pigJobDefinition.Arguments.Add("map.input.tasks=1000");
            pigJobDefinition.Arguments.Add("map.input.reducers=1000");

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightPigJobDefinition)
                                      .WithParameter(CmdletConstants.File, pigJobDefinition.File)
                                      .WithParameter(CmdletConstants.Arguments, pigJobDefinition.Arguments)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var pigJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightPigJobDefinition>().First();

                Assert.AreEqual(pigJobDefinition.File, pigJobFromPowershell.File);
            }
        }
    }
}
