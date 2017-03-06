using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Serilog;
using SmartThingsHostPinger.Options;
using SmartThingsHostPinger.Utils;

namespace SmartThingsHostPinger
{
	internal class PingHelper : IPingHelper
	{
		/// <summary>
		/// The options
		/// </summary>
		private IPingOptions options;

		/// <summary>
		/// Initializes a new instance of the <see cref="PingHelper"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public PingHelper(IPingOptions options)
		{
			this.options = options;
		}

		/// <summary>
		/// Ping hosts as an asynchronous operation.
		/// </summary>
		/// <param name="hosts">The hosts.</param>
		/// <returns>The ping results.</returns>
		public async Task<IEnumerable<IPingResult>> PingHostsAsync(IEnumerable<string> hosts)
		{
			IEnumerable<IPingResult> results;

			if (this.options.PingHostsConcurrently)
			{
				results = await this.PingHostsConcurrently(hosts);
			}
			else
			{
				results = await this.PingHostsSynchronously(hosts);
			}

			return results;
		}

		/// <summary>
		/// Pings the host.
		/// </summary>
		/// <param name="hostNameOrAddress">The host name or address.</param>
		/// <returns>The ping result.</returns>
		private async Task<IPingResult> PingHost(string hostNameOrAddress)
		{
			IPingResult hostResult;

			try
			{
				using (Ping pinger = new Ping())
				{
					PingReply reply = await pinger.SendPingAsync(hostNameOrAddress, this.options.TimeoutMilliseconds);
					bool pingable = reply.Status == IPStatus.Success;

					PingStatus status = pingable ? PingStatus.Online : PingStatus.Offline;

					hostResult = new PingResult(hostNameOrAddress, status);
				}
			}
			catch (Exception ex)
			{
				Exception exception = ex.Flatten();
				Log.Debug("Ping error: {HostNameOrAddress} {Message}", hostNameOrAddress, exception.Message);
				hostResult = new PingResult(hostNameOrAddress, PingStatus.Unknown);
			}

			return hostResult;
		}

		/// <summary>
		/// Pings the hosts concurrently.
		/// </summary>
		/// <param name="hosts">The hosts.</param>
		/// <returns>The ping results.</returns>
		private async Task<IEnumerable<IPingResult>> PingHostsConcurrently(IEnumerable<string> hosts)
		{
			ConcurrentBag<IPingResult> result = new ConcurrentBag<IPingResult>();

			Task<IPingResult>[] tasks = hosts.Select(x => this.PingHostWithRetry(x)).ToArray();

			return await Task.WhenAll(tasks);
		}

		/// <summary>
		/// Pings the hosts synchronously.
		/// </summary>
		/// <param name="hosts">The hosts.</param>
		/// <returns>The ping results.</returns>
		private async Task<IEnumerable<IPingResult>> PingHostsSynchronously(IEnumerable<string> hosts)
		{
			IList<IPingResult> result = new List<IPingResult>();

			foreach (string hostNameOrAddress in hosts)
			{
				IPingResult hostResult = await this.PingHostWithRetry(hostNameOrAddress);

				result.Add(hostResult);
			}

			return result;
		}

		/// <summary>
		/// Pings the host with retry.
		/// </summary>
		/// <param name="hostNameOrAddress">The host name or address.</param>
		/// <returns>The ping result.</returns>
		private async Task<IPingResult> PingHostWithRetry(string hostNameOrAddress)
		{
			IPingResult result = null;
			int attempts = 0;

			// Ping the host until we hit the max retry attempts.
			// We do the same for Offline results to avoid false positives.
			do
			{
				result = await this.PingHost(hostNameOrAddress);
			}
			while (++attempts < this.options.RetryAttempts && result.Status != PingStatus.Online);

			return result;
		}
	}
}