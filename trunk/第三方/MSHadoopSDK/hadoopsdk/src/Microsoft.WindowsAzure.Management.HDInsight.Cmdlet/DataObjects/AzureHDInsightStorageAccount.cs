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
    /// <summary>
    /// Represents a Storage Account for an HD Insight Configuration.
    /// </summary>
    public class AzureHDInsightStorageAccount
    {
        /// <summary>
        /// Gets or sets the Storage Account Name.
        /// </summary>
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Gets or sets the Storage Account Key.
        /// </summary>
        public string StorageAccountKey { get; set; }

        /// <summary>
        /// Overrides the ToString() method to return the storage account name.
        /// </summary>
        /// <returns>The string representation of this object.</returns>
        public override string ToString()
        {
            return this.StorageAccountName;
        }
    }
}
