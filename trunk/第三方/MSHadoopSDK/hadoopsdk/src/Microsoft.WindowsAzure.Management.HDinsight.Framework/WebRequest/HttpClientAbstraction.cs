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
namespace Microsoft.WindowsAzure.Management.HDInsight.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Logging;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.ServiceLocation;

    internal class HttpClientAbstraction : DisposableObject, IHttpClientAbstraction
    {
        private const string LogFormat =
            " HTTP Request:\r\n          Uri: {0}\r\n       Method: {1}\r\n Content Type: {2}\r\n      Headers: {3}\r\n      Content:\r\n{4}\r\n\r\nHTTP Response:\r\n  Status Code: {5}\r\n      Headers: {6}\r\n      Content:\r\n{7}\r\n\r\n";

        private HttpClient client;
        private CancellationToken cancellationToken = CancellationToken.None;
        private readonly ILogger instanceLogger;

        internal HttpClientAbstraction(HttpClient client)
        {
            this.client = client;
            this.Timeout = new TimeSpan(0, 5, 0);
            this.Method = HttpMethod.Get;
            this.RequestHeaders = new Dictionary<string, string>();
            this.ContentType = HttpConstants.ApplicationXml;
        }

        internal HttpClientAbstraction(HttpClient client, IAbstractionContext context)
            : this(client)
        {
            this.instanceLogger = context.Logger;
            this.cancellationToken = context.CancellationToken;
        }

        public TimeSpan Timeout { get; set; }

        public HttpMethod Method { get; set; }

        public Uri RequestUri { get; set; }

        public string Content { get; set; }

        public IDictionary<string, string> RequestHeaders { get; private set; }

        public string ContentType { get; set; }

        public async Task<IHttpResponseMessageAbstraction> SendAsync()
        {
            var requestMessage = new HttpRequestMessage();

            requestMessage.Method = this.Method;
            requestMessage.RequestUri = this.RequestUri;
            if (this.Method == HttpMethod.Post || this.Method == HttpMethod.Put)
            {
                requestMessage.Content = new StringContent(this.Content);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(this.ContentType);
            }
            requestMessage.Headers.Clear();
            foreach (KeyValuePair<string, string> header in this.RequestHeaders)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
            this.client.Timeout = this.Timeout;

            DateTime start = DateTime.Now;
            try
            {
                var result = await this.client.SendAsync(requestMessage, this.cancellationToken);
                string content = null;
                if (result.Content.IsNotNull())
                {
                    content = result.Content.ReadAsStringAsync().WaitForResult();
                }
                var retval = new HttpResponseMessageAbstraction(result.StatusCode, new HttpResponseHeadersAbstraction(result.Headers), content);

                this.LogRequestResponseDetails(retval);
                return retval;
            }
            catch (TaskCanceledException tcex)
            {
                if (this.cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException("The operation was canceled at the users request.", tcex, this.cancellationToken);
                }
                else if (DateTime.Now - start > this.Timeout)
                {
                    throw new TimeoutException("The operation timed out.", tcex);
                }
                throw;
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.WindowsAzure.Management.HDInsight.Logging.ILogger.LogMessage(System.String,Microsoft.WindowsAzure.Management.HDInsight.Logging.Severity,Microsoft.WindowsAzure.Management.HDInsight.Logging.Verbosity)",
            Justification = "Strictly a logging format string.")]
        private void LogRequestResponseDetails(HttpResponseMessageAbstraction response)
        {
            bool isFirst = true;
            StringBuilder requestHeaders = new StringBuilder();

            foreach (var header in this.RequestHeaders)
            {
                if (!isFirst)
                {
                    requestHeaders.Append(string.Empty.PadLeft(15));
                }
                requestHeaders.Append(header.Key);
                requestHeaders.Append(" = ");
                requestHeaders.AppendLine(header.Value);
                isFirst = false;
            }

            isFirst = true;
            StringBuilder responseHeaders = new StringBuilder();
            var responseContentType = HttpConstants.ApplicationXml;
            foreach (var header in response.Headers)
            {
                if (!isFirst)
                {
                    responseHeaders.Append(string.Empty.PadLeft(15));
                }

                if (header.Key == HttpConstants.ContentTypeHeader && header.Value.IsNotNull())
                {
                    responseContentType = string.Join(",", header.Value);
                }

                responseHeaders.Append(header.Key);
                responseHeaders.Append(" = ");
                bool innerFirst = true;
                foreach (var value in header.Value)
                {
                    if (!innerFirst)
                    {
                        responseHeaders.Append(string.Empty.PadLeft(15 + header.Key.Length + 3));
                    }
                    responseHeaders.AppendLine(value);
                    innerFirst = false;
                }
                isFirst = false;
            }

            var formattedRequestContent = this.Content;
            var formattedResponseContent = response.Content;

            if (this.ContentType == HttpConstants.ApplicationXml)
            {
                TryPrettyPrintXml(formattedRequestContent, out formattedRequestContent);
            }

            if (responseContentType == HttpConstants.ApplicationXml)
            {
                TryPrettyPrintXml(formattedResponseContent, out formattedResponseContent);
            }

            string message = string.Format(CultureInfo.InvariantCulture,
                                           LogFormat,
                                           this.RequestUri,
                                           this.Method,
                                           this.ContentType,
                                           requestHeaders,
                                           formattedRequestContent,
                                           response.StatusCode,
                                           responseHeaders,
                                           formattedResponseContent);

            var frameworkLogger = ServiceLocator.Instance.Locate<ILogger>();
            frameworkLogger.LogMessage(message, Severity.Informational, Verbosity.Diagnostic);
            if (this.instanceLogger.IsNotNull())
            {
                this.instanceLogger.LogMessage(message, Severity.Informational, Verbosity.Detailed);
            }
        }

        public static IHttpClientAbstraction Create(X509Certificate2 cert)
        {
            var handler = new WebRequestHandler();
            handler.ClientCertificates.Add(cert);
            return Help.SafeCreate(() => new HttpClientAbstraction(Help.SafeCreate(() => new HttpClient(handler))));
        }

        public static IHttpClientAbstraction Create(IAbstractionContext context)
        {
            var handler = new WebRequestHandler();
            return Help.SafeCreate(() => new HttpClientAbstraction(Help.SafeCreate(() => new HttpClient(handler)), context));
        }

        public static IHttpClientAbstraction Create(X509Certificate2 cert, IAbstractionContext context)
        {
            var handler = new WebRequestHandler();
            handler.ClientCertificates.Add(cert);
            return Help.SafeCreate(() => new HttpClientAbstraction(Help.SafeCreate(() => new HttpClient(handler)), context));
        }

        public static IHttpClientAbstraction Create()
        {
            return Help.SafeCreate(() => new HttpClientAbstraction(Help.SafeCreate<HttpClient>()));
        }

        internal static bool TryPrettyPrintXml(string content, out string formattedXml)
        {
            formattedXml = content;
            if (content.IsNullOrEmpty())
            {
                return false;
            }

            try
            {
                var document = XDocument.Parse(content, LoadOptions.PreserveWhitespace);
                using (var memoryStream = new MemoryStream())
                {
                    var prettyPrinter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                    prettyPrinter.Formatting = Formatting.Indented;
                    document.Save(prettyPrinter);
                    prettyPrinter.Flush();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    formattedXml = new StreamReader(memoryStream).ReadToEnd();
                    prettyPrinter.Close();
                }
            }
            catch (XmlException formatException)
            {
                // if the xml content is malformed because of errors, we don't want this function to throw.
                formatException.IsNotNull();
                return false;
            }

            return true;
        }
    }
}
