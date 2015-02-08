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
namespace Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning.PocoClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Hadoop.Client;
    using Microsoft.WindowsAzure.Management.HDInsight.ClusterProvisioning;
    using Microsoft.WindowsAzure.Management.HDInsight;
    using Microsoft.WindowsAzure.Management.HDInsight.Framework.Core.Library;
    using Microsoft.WindowsAzure.Management.HDInsight.Logging;

    /// <summary>
    /// Provides helper extension methods for the Management Poco Client.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IHD",
        Justification = "Correct casing for circumstance and to match Interface name. [TGS]")]
    internal static class IHDInsightManagementPocoClientExtensions
    {
        /// <summary>
        /// Waits for a condition to be satisfied.
        /// </summary>
        /// <typeparam name="T">
        /// The changeType of object that will be operated on.
        /// </typeparam>
        /// <param name="client">
        /// The client instance this is extending.
        /// </param>
        /// <param name="poll">
        /// The function to poll the client for state.
        /// </param>
        /// <param name="continuePolling">
        /// A function to evaluate the state to see if it matches the condition.
        /// </param>
        /// <param name="notifyHandler">
        /// A notification handler used to rais events when the status changes.
        /// </param>
        /// <param name="interval">
        /// A time frame to wait between polls.
        /// </param>
        /// <param name="timeout">
        /// The amount of time to wait for the condition to be satisfied.
        /// </param>
        /// <param name="cancellationToken">
        /// A Cancelation Token that can be used to cancel the request.
        /// </param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public static async Task WaitForCondition<T>(this IHDInsightManagementPocoClient client, Func<Task<T>> poll, Func<T, PollResult> continuePolling, Action<T> notifyHandler, TimeSpan interval, TimeSpan timeout, CancellationToken cancellationToken)
        {
            client.ArgumentNotNull("client");
            poll.ArgumentNotNull("poll");
            continuePolling.ArgumentNotNull("continuePolling");
            var start = DateTime.Now;
            int pollingFailures = 0;
            const int MaxPollingFailuresCount = 10;
            T pollingResult = default(T);
            PollResult result = PollResult.Continue;
            do
            {
                try
                {
                    pollingResult = await poll();
                    result = continuePolling(pollingResult);
                    if (notifyHandler.IsNotNull())
                    {
                        notifyHandler(pollingResult);
                    }

                    if (result == PollResult.PosibleError)
                    {
                        pollingFailures++;
                        IHadoopClientExtensions.WaitForInterval(interval, cancellationToken);
                    }
                    else if (result == PollResult.Continue)
                    {
                        pollingFailures = 0;
                        IHadoopClientExtensions.WaitForInterval(interval, cancellationToken);
                    }
                }
                catch (WebException pollingException)
                {
                    Help.DoNothing(pollingException);
                    pollingFailures++;
                    if (pollingFailures >= MaxPollingFailuresCount)
                    {
                        throw;
                    }
                }
                if (pollingFailures >= MaxPollingFailuresCount)
                {
                    return;
                }
            }
            while ((result == PollResult.Continue || result == PollResult.PosibleError) && DateTime.Now - start < timeout);
        }

        /// <summary>
        /// Waits for the cluster to not exist (null) or go into an error state.
        /// </summary>
        /// <param name="client">
        /// The client instance this is extending.
        /// </param>
        /// <param name="dnsName">
        /// The dnsName of the cluster.
        /// </param>
        /// <param name="timeout">
        /// The amount of time to wait for the condition to be satisfied.
        /// </param>
        /// <param name="cancellationToken">
        /// A Cancelation Token that can be used to cancel the request.
        /// </param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public static async Task WaitForClusterNullOrError(this IHDInsightManagementPocoClient client, string dnsName, TimeSpan timeout, CancellationToken cancellationToken)
        {
            await client.WaitForCondition(() => client.ListContainer(dnsName), c => c == null ? PollResult.Stop : c.Error != null ? PollResult.Stop : PollResult.Continue, null, TimeSpan.FromMilliseconds(100), timeout, cancellationToken);
        }

        /// <summary>
        /// Waits for the cluster to not exist (null).
        /// </summary>
        /// <param name="client">
        /// The client instance this is extending.
        /// </param>
        /// <param name="dnsName">
        /// The dnsName of the cluster.
        /// </param>
        /// <param name="timeout">
        /// The amount of time to wait for the condition to be satisfied.
        /// </param>
        /// <param name="cancellationToken">
        /// A Cancelation Token that can be used to cancel the request.
        /// </param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public static async Task WaitForClusterNull(this IHDInsightManagementPocoClient client, string dnsName, TimeSpan timeout, CancellationToken cancellationToken)
        {
            await client.WaitForCondition(() => client.ListContainer(dnsName), c => c == null ? PollResult.Stop : PollResult.Continue, null, TimeSpan.FromMilliseconds(100), timeout, cancellationToken);
        }

        /// <summary>
        /// Waits for the cluster to exist (!null).
        /// </summary>
        /// <param name="client">
        /// The client instance this is extending.
        /// </param>
        /// <param name="dnsName">
        /// The dnsName of the cluster.
        /// </param>
        /// <param name="timeout">
        /// The amount of time to wait for the condition to be satisfied.
        /// </param>
        /// <param name="cancellationToken">
        /// A Cancelation Token that can be used to cancel the request.
        /// </param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public static async Task WaitForClusterNotNull(this IHDInsightManagementPocoClient client, string dnsName, TimeSpan timeout, CancellationToken cancellationToken)
        {
            await client.WaitForCondition(() => client.ListContainer(dnsName), c => c != null ? PollResult.Stop : PollResult.Continue, null, TimeSpan.FromMilliseconds(100), timeout, cancellationToken);
        }

        /// <summary>
        /// Waits for the cluster to not exist (null) or go into an error state or be in one of the listed states.
        /// </summary>
        /// <param name="client">
        /// The client instance this is extending.
        /// </param>
        /// <param name="notifyHandler">
        /// A notification handler used to notify when the state changes.
        /// </param>
        /// <param name="dnsName">
        /// The dnsName of the cluster.
        /// </param>
        /// <param name="timeout">
        /// The amount of time to wait for the condition to be satisfied.
        /// </param>
        /// <param name="interval">
        /// A time frame to wait between polls.
        /// </param>
        /// <param name="context">
        /// A Cancelation Token that can be used to cancel the request.
        /// </param>
        /// <param name="states">
        /// The set of states that would cause this funciton to terminate.
        /// </param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public static async Task WaitForClusterInConditionOrError(this IHDInsightManagementPocoClient client, Action<ClusterDetails> notifyHandler, string dnsName, TimeSpan timeout, TimeSpan interval, IAbstractionContext context, params ClusterState[] states)
        {
            await client.WaitForCondition(() => client.ListContainer(dnsName), c => client.PollSignal(c, states), notifyHandler, interval, timeout, context.CancellationToken);
        }

        internal enum PollResult
        {
            Continue,
            PosibleError,
            Stop
        }

        /// <summary>
        /// Poll Signal that is triggered on every poll for the cluster condition.
        /// </summary>
        /// <param name="client">
        /// The client instance this is extending.
        /// </param>
        /// <param name="cluster">HDInsight cluster.</param>
        /// <param name="states">Acceptable states at which the polling can stop.</param>
        /// <returns>True, if we want polling to continue, false otherwise.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.WindowsAzure.Management.HDInsight.Logging.ILogger.LogMessage(System.String,Microsoft.WindowsAzure.Management.HDInsight.Logging.Severity,Microsoft.WindowsAzure.Management.HDInsight.Logging.Verbosity)",
            Justification = "This is for logging the literal is acceptable. [TGS]")]
        internal static PollResult PollSignal(this IHDInsightManagementPocoClient client, ClusterDetails cluster, params ClusterState[] states)
        {
            if (cluster != null)
            {
                client.RaiseClusterProvisioningEvent(client, new ClusterProvisioningStatusEventArgs(cluster, cluster.State));
                var msg = string.Format(CultureInfo.CurrentCulture, "Current State {0} -> waiting for one state of {1}", cluster.State, string.Join(",", states.Select(s => s.ToString())));
                client.Context.Logger.LogMessage(msg, Severity.Informational, Verbosity.Diagnostic);
            }

            if (cluster == null || cluster.State == ClusterState.Unknown)
            {
                return PollResult.PosibleError;
            }
            if (cluster.State == ClusterState.Error || cluster.Error != null || states.Contains(cluster.State))
            {
                return PollResult.Stop;
            }
            return PollResult.Continue;
        }

        /// <summary>
        /// Waits for an operation on the cluster to complete.
        /// </summary>
        /// <param name="client">
        /// The client instance this is extending.
        /// </param>
        /// <param name="dnsName">
        /// The dnsName of the cluster.
        /// </param>
        /// <param name="location">
        /// The location of the cluster.
        /// </param>
        /// <param name="operation">
        /// The operation Id to check.
        /// </param>
        /// <param name="timeout">
        /// The amount of time to wait for the condition to be satisfied.
        /// </param>
        /// <param name="cancellationToken">
        /// A Cancelation Token that can be used to cancel the request.
        /// </param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public static async Task WaitForOperationCompleteOrError(this IHDInsightManagementPocoClient client, string dnsName, string location, Guid operation, TimeSpan timeout, CancellationToken cancellationToken)
        {
            await client.WaitForCondition(() => client.GetStatus(dnsName, location, operation), s => s.State == UserChangeRequestOperationStatus.Pending ? PollResult.Continue : PollResult.Stop, null, TimeSpan.FromMilliseconds(500), timeout, cancellationToken);
        }
    }
}
