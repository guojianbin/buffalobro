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
    using System.Linq;
    using System.Management.Automation;
    using Microsoft.Hadoop.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects;
    using Microsoft.WindowsAzure.Management.HDInsight.TestUtilities;

    [TestClass]
    public class NewSqoopJobCmdLetTests : IntegrationTestBase
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
        [TestCategory("New-AzureHDInsightSqoopJobDefinition")]
        public void ICanCallThe_New_HDInsightSqoopJobDefinitionCmdlet()
        {
            var sqoopJobDefinition = new SqoopJobCreateParameters()
            {
                Command = "show tables"
            };

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightSqoopJobDefinition)
                                      .WithParameter(CmdletConstants.Command, sqoopJobDefinition.Command)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var SqoopJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightSqoopJobDefinition>().First();

                Assert.AreEqual(sqoopJobDefinition.Command, SqoopJobFromPowershell.Command);
            }

        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightSqoopJobDefinition")]
        public void ICanCallThe_New_HDInsightSqoopJobDefinitionCmdlet_WithFileParameter()
        {
            var sqoopJobDefinition = new SqoopJobCreateParameters()
            {
                File = Constants.WabsProtocolSchemeName + "filepath.hql"
            };

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightSqoopJobDefinition)
                                      .WithParameter(CmdletConstants.File, sqoopJobDefinition.File)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var SqoopJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightSqoopJobDefinition>().First();

                Assert.AreEqual(sqoopJobDefinition.File, SqoopJobFromPowershell.File);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightSqoopJobDefinition")]
        public void ICannotCallThe_New_HDInsightSqoopJobDefinitionCmdlet_WithoutFileOrCommandParameter()
        {
            var sqoopJobDefinition = new SqoopJobCreateParameters()
            {
                File = Constants.WabsProtocolSchemeName + "filepath.hql"
            };

            try
            {
                using (var runspace = this.GetPowerShellRunspace())
                {
                    runspace.NewPipeline()
                         .AddCommand(CmdletConstants.NewAzureHDInsightSqoopJobDefinition)
                         .Invoke();
                    Assert.Fail("test failed.");
                }
            }
            catch (CmdletInvocationException invokeException)
            {
                var psArgumentException = invokeException.GetBaseException() as PSArgumentException;
                Assert.IsNotNull(psArgumentException);
                Assert.AreEqual("Either File or Command should be specified for Sqoop jobs.", psArgumentException.Message);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightSqoopJobDefinition")]
        public void ICanCallThe_New_HDInsightSqoopJobDefinitionCmdlet_WithoutJobName()
        {
            var sqoopJobDefinition = new SqoopJobCreateParameters()
            {
                Command = "show tables"
            };

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightSqoopJobDefinition)
                                      .WithParameter(CmdletConstants.Command, sqoopJobDefinition.Command)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var SqoopJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightSqoopJobDefinition>().First();

                Assert.AreEqual(sqoopJobDefinition.Command, SqoopJobFromPowershell.Command);
            }
        }

        [TestMethod]
        [TestCategory("CheckIn")]
        [TestCategory("Integration")]
        [TestCategory("Scenario")]
        [TestCategory("Jobs")]
        [TestCategory("New-AzureHDInsightSqoopJobDefinition")]
        public void ICanCallThe_New_HDInsightSqoopJobDefinitionCmdlet_WithResources()
        {
            var sqoopJobDefinition = new SqoopJobCreateParameters()
            {
                Command = "show tables"
            };
            sqoopJobDefinition.Files.Add("pidata.txt");
            sqoopJobDefinition.Files.Add("pidate2.txt");

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightSqoopJobDefinition)
                                      .WithParameter(CmdletConstants.Command, sqoopJobDefinition.Command)
                                      .WithParameter(CmdletConstants.Files, sqoopJobDefinition.Files)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var SqoopJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightSqoopJobDefinition>().First();

                Assert.AreEqual(sqoopJobDefinition.Command, SqoopJobFromPowershell.Command);

                foreach (var file in sqoopJobDefinition.Files)
                {
                    Assert.IsTrue(
                        SqoopJobFromPowershell.Files.Any(arg => string.Equals(file, arg)),
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
        [TestCategory("New-AzureHDInsightSqoopJobDefinition")]
        [TestCategory("Defect")]
        public void ICanCallThe_New_HDInsightSqoopJobDefinitionCmdlet_WithOutputStorageLocation()
        {
            var sqoopJobDefinition = new SqoopJobCreateParameters()
            {
                Command = "show tables",
                StatusFolder = "/tablesList"
            };

            using (var runspace = this.GetPowerShellRunspace())
            {
                var results = runspace.NewPipeline()
                                      .AddCommand(CmdletConstants.NewAzureHDInsightSqoopJobDefinition)
                                      .WithParameter(CmdletConstants.Command, sqoopJobDefinition.Command)
                                      .WithParameter(CmdletConstants.StatusFolder, sqoopJobDefinition.StatusFolder)
                                      .Invoke();
                Assert.AreEqual(1, results.Results.Count);
                var SqoopJobFromPowershell = results.Results.ToEnumerable<AzureHDInsightSqoopJobDefinition>().First();

                Assert.AreEqual(sqoopJobDefinition.Command, SqoopJobFromPowershell.Command);
                Assert.AreEqual(sqoopJobDefinition.StatusFolder, SqoopJobFromPowershell.StatusFolder);
            }
        }
    }
}
