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
namespace Microsoft.WindowsAzure.Management.HDInsight.Cmdlet.DataObjects
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Structure to contain Hadoop Map-Reduce service configuration.
    /// </summary>
    public class AzureHDInsightOozieConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the AzureHDInsightOozieConfiguration class.
        /// </summary>
        public AzureHDInsightOozieConfiguration()
        {
            this.Configuration = new Hashtable();
        }

        /// <summary>
        /// Gets or sets the configuration settings.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Needed for ease of use in Powershell invocation.")]
        public Hashtable Configuration { get; set; }

        /// <summary>
        /// Gets or sets the location for additional JAR files.
        /// </summary>
        public AzureHDInsightDefaultStorageAccount AdditionalSharedLibraries { get; set; }

        /// <summary>
        /// Gets or sets the location for additional JAR files.
        /// </summary>
        public AzureHDInsightDefaultStorageAccount AdditionalActionExecutorLibraries { get; set; }
    }
}
