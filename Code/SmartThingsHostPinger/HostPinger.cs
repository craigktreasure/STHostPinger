using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using SmartThingsHostPinger.Options;

namespace SmartThingsHostPinger
{
	/// <summary>
	/// Class HostPinger.
	/// </summary>
	internal class HostPinger
	{
		/// <summary>
		/// The Smart Things API helper
		/// </summary>
		private ISmartThingsApiHelper apiHelper;

		/// <summary>
		/// The host status infos
		/// </summary>
		private IDictionary<string, HostStatusInfo> hostStatusInfos = new Dictionary<string, HostStatusInfo>();

		/// <summary>
		/// The options
		/// </summary>
		private IAppOptions options;

		/// <summary>
		/// The ping helper
		/// </summary>
		private IPingHelper pingHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="HostPinger" /> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public HostPinger(IAppOptions options)
		{
			options.Validate();
			this.options = options;
			this.pingHelper = new PingHelper(options.PingOptions);
			this.apiHelper = new SmartThingsApiHelper(options.SmartAppConfig);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HostPinger" /> class.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <param name="pingHelper">The ping helper.</param>
		/// <param name="apiHelper">The API helper.</param>
		internal HostPinger(IAppOptions options, IPingHelper pingHelper, ISmartThingsApiHelper apiHelper)
		{
			options.Validate();
			this.options = options;
			this.pingHelper = pingHelper;
			this.apiHelper = apiHelper;
		}

		/// <summary>
		/// Starts the <see cref="HostPinger"/>.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A <see cref="Task"/> for the job.
		/// </returns>
		public async Task Start(CancellationToken cancellationToken)
		{
			try
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					await RunSinglePingJob();

					await Task.Delay(TimeSpan.FromSeconds(this.options.PingOptions.PingIntervalSeconds),
						cancellationToken);
				}
			}
			catch (OperationCanceledException) { }
		}

		/// <summary>
		/// Runs the single ping job.
		/// </summary>
		/// <returns>System.Threading.Tasks.Task.</returns>
		internal async Task RunSinglePingJob()
		{
			Log.Debug("Pinging started");

			IEnumerable<IPingResult> results = await this.pingHelper.PingHostsAsync(this.options.Hosts);

			await this.ProcessResults(results);

			Log.Debug("Pinging and reporting completed");
		}

		/// <summary>
		/// Processes the results.
		/// </summary>
		/// <param name="results">The results.</param>
		/// <returns>A <see cref="Task"/> for the job.</returns>
		private async Task ProcessResults(IEnumerable<IPingResult> results)
		{
			foreach (IPingResult result in results)
			{
				HostStatusInfo statusInfo;

				if (!this.hostStatusInfos.ContainsKey(result.HostNameOrAddress))
				{
					statusInfo = new HostStatusInfo
					{
						HostNameOrAddress = result.HostNameOrAddress,
						Reported = false,
						Status = result.Status.ToHostStatus()
					};
					this.hostStatusInfos.Add(result.HostNameOrAddress, statusInfo);
				}
				else
				{
					statusInfo = this.hostStatusInfos[result.HostNameOrAddress];
					HostStatus newStatus = result.Status.ToHostStatus(statusInfo.Status == HostStatus.Online);
					statusInfo.Reported = statusInfo.Status == newStatus;
					statusInfo.Status = newStatus;
				}

				if (!statusInfo.Reported)
				{
					await this.ReportStatusUpdate(statusInfo);
				}
			}
		}

		/// <summary>
		/// Reports the status update.
		/// </summary>
		/// <param name="statusInfo">The status information.</param>
		/// <returns>A <see cref="Task"/> for the job.</returns>
		private async Task ReportStatusUpdate(HostStatusInfo statusInfo)
		{
			string statusUpdate;

			switch (statusInfo.Status)
			{
				case HostStatus.Offline:
					statusUpdate = "[OFFLINE]      ";
					break;

				case HostStatus.Online:
					statusUpdate = "[ONLINE]       ";
					break;

				case HostStatus.Unknown:
					statusUpdate = "[UNKNOWN]      ";
					break;

				case HostStatus.WentOffline:
					statusUpdate = "[WENT OFFLINE] ";
					break;

				default:
					throw new ArgumentException("Unsupported HostStatus");
			}

			Log.Information("{StatusUpdate}{HostNameOrAddress}", statusUpdate, statusInfo.HostNameOrAddress);

			if (statusInfo.Status != HostStatus.Unknown)
			{
				// Only report updates when we know something.
				statusInfo.Reported = await this.apiHelper.SendUpdateAsync(
					statusInfo.HostNameOrAddress, statusInfo.Status == HostStatus.Online);
			}
		}
	}
}