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
namespace Microsoft.WindowsAzure.Management.HDInsight
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library.WebRequest;

    /// <summary>
    ///     Provides a factory for a class that Abstracts Http client requests.
    /// </summary>
    internal interface IHDInsightHttpClientAbstractionFactory
    {
        /// <summary>
        ///     Creates a new HttpClientAbstraction class.
        /// </summary>
        /// <param name="credentials">
        ///     The credentials to use.
        /// </param>
        /// <param name="context">
        ///     The context to use.
        /// </param>
        /// <param name="ignoreSslErrors">
        ///     Indicates that server side SSL Certificate errors should be ignored.
        /// </param>
        /// <returns>
        ///     A new instance of the HttpClientAbstraction.
        /// </returns>
        IHttpClientAbstraction Create(IHDInsightSubscriptionCredentials credentials, IAbstractionContext context, bool ignoreSslErrors);

        /// <summary>
        ///     Creates a new HttpClientAbstraction class.
        /// </summary>
        /// <param name="credentials">
        ///     The credentials to use.
        /// </param>
        /// <param name="ignoreSslErrors">
        ///     Indicates that server side SSL Certificate errors should be ignored.
        /// </param>
        /// <returns>
        ///     A new instance of the HttpClientAbstraction.
        /// </returns>
        IHttpClientAbstraction Create(IHDInsightSubscriptionCredentials credentials, bool ignoreSslErrors);

        /// <summary>
        ///     Creates a new HttpClientAbstraction class.
        /// </summary>
        /// <param name="context">
        ///     The context to use.
        /// </param>
        /// <param name="ignoreSslErrors">
        ///     Indicates that server side SSL Certificate errors should be ignored.
        /// </param>
        /// <returns>
        ///     A new instance of the HttpClientAbstraction.
        /// </returns>
        IHttpClientAbstraction Create(IAbstractionContext context, bool ignoreSslErrors);

        /// <summary>
        ///     Creates a new HttpClientAbstraction class.
        /// </summary>
        /// <param name="ignoreSslErrors">
        ///     Indicates that server side SSL Certificate errors should be ignored.
        /// </param>
        /// <returns>
        ///     A new instance of the HttpClientAbstraction.
        /// </returns>
        IHttpClientAbstraction Create(bool ignoreSslErrors);

        /// <summary>
        /// Performs a retry of an HDInsightHttpClient Operation.
        /// </summary>
        /// <param name="credentials">
        ///     The connection credentials to use.
        /// </param>
        /// <param name="context">
        ///     The abstraction context to use.
        /// </param>
        /// <param name="operation">
        ///     The Operation to perform.
        /// </param>
        /// <param name="shouldRetry">
        ///     A method that determines if the operation should be retried.
        /// </param>
        /// <param name="retryCount">
        ///     The number of times the operation will be attempted before giving up.
        /// </param>
        /// <param name="retryInterval">
        ///     The delay between retries.
        /// </param>
        /// <param name="ignoreSslErrors">
        ///     Specifies that server side SSL Errors should be ignored.
        /// </param>
        /// <returns>
        /// A task representing the Http Response.
        /// </returns>
        Task<IHttpResponseMessageAbstraction> Retry(IHDInsightSubscriptionCredentials credentials, IAbstractionContext context, Func<IHttpClientAbstraction, Task<IHttpResponseMessageAbstraction>> operation, Func<IHttpResponseMessageAbstraction, bool> shouldRetry, int retryCount, TimeSpan retryInterval, bool ignoreSslErrors);

        /// <summary>
        /// Performs a retry of an HDInsightHttpClient Operation.
        /// </summary>
        /// <param name="credentials">
        ///     The connection credentials to use.
        /// </param>
        /// <param name="operation">
        ///     The Operation to perform.
        /// </param>
        /// <param name="shouldRetry">
        ///     A method that determines if the operation should be retried.
        /// </param>
        /// <param name="retryCount">
        ///     The number of times the operation will be attempted before giving up.
        /// </param>
        /// <param name="retryInterval">
        ///     The delay between retries.
        /// </param>
        /// <param name="ignoreSslErrors">
        ///     Specifies that server side SSL Errors should be ignored.
        /// </param>
        /// <returns>
        /// A task representing the Http Response.
        /// </returns>
        Task<IHttpResponseMessageAbstraction> Retry(IHDInsightSubscriptionCredentials credentials, Func<IHttpClientAbstraction, Task<IHttpResponseMessageAbstraction>> operation, Func<IHttpResponseMessageAbstraction, bool> shouldRetry, int retryCount, TimeSpan retryInterval, bool ignoreSslErrors);

        /// <summary>
        /// Performs a retry of an HDInsightHttpClient Operation.
        /// </summary>
        /// <param name="context">
        ///     The abstraction context to use.
        /// </param>
        /// <param name="operation">
        ///     The Operation to perform.
        /// </param>
        /// <param name="shouldRetry">
        ///     A method that determines if the operation should be retried.
        /// </param>
        /// <param name="retryCount">
        ///     The number of times the operation will be attempted before giving up.
        /// </param>
        /// <param name="retryInterval">
        ///     The delay between retries.
        /// </param>
        /// <param name="ignoreSslErrors">
        ///     Specifies that server side SSL Errors should be ignored.
        /// </param>
        /// <returns>
        /// A task representing the Http Response.
        /// </returns>
        Task<IHttpResponseMessageAbstraction> Retry(IAbstractionContext context, Func<IHttpClientAbstraction, Task<IHttpResponseMessageAbstraction>> operation, Func<IHttpResponseMessageAbstraction, bool> shouldRetry, int retryCount, TimeSpan retryInterval, bool ignoreSslErrors);

        /// <summary>
        /// Performs a retry of an HDInsightHttpClient Operation.
        /// </summary>
        /// <param name="operation">
        ///     The Operation to perform.
        /// </param>
        /// <param name="shouldRetry">
        ///     A method that determines if the operation should be retried.
        /// </param>
        /// <param name="retryCount">
        ///     The number of times the operation will be attempted before giving up.
        /// </param>
        /// <param name="retryInterval">
        ///     The delay between retries.
        /// </param>
        /// <param name="ignoreSslErrors">
        ///     Specifies that server side SSL Errors should be ignored.
        /// </param>
        /// <returns>
        /// A task representing the Http Response.
        /// </returns>
        Task<IHttpResponseMessageAbstraction> Retry(Func<IHttpClientAbstraction, Task<IHttpResponseMessageAbstraction>> operation, Func<IHttpResponseMessageAbstraction, bool> shouldRetry, int retryCount, TimeSpan retryInterval, bool ignoreSslErrors);
    }
}
