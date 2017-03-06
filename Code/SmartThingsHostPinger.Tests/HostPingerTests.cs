using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SmartThingsHostPinger.Options;
using Xunit;

namespace SmartThingsHostPinger.Tests
{
	public class HostPingerTests
	{
		private const string TestAccessTokenAppId = "00000000-0000-0000-0000-000000000000";

		[Fact]
		public async Task PingAndReportAllHosts()
		{
			AppOptions options = this.GetMinimalOptions();

			int pingCalls = 0;
			var pingHelperMock = new Mock<IPingHelper>();
			pingHelperMock
				.Setup(x => x.PingHostsAsync(options.Hosts))
				.ReturnsAsync((IEnumerable<string> hosts) => hosts.Select(x => new PingResult(x, PingStatus.Online)))
				.Callback(() =>
				{
					pingCalls++;
				});

			var smartThingsHelperMock = new Mock<ISmartThingsApiHelper>();
			int reportedCalls = 0;
			foreach (var host in options.Hosts)
			{
				smartThingsHelperMock
					.Setup(x => x.SendUpdateAsync(host, true))
					.ReturnsAsync(true)
					.Callback(() =>
					{
						reportedCalls++;
					});
			}

			HostPinger pinger = new HostPinger(options, pingHelperMock.Object, smartThingsHelperMock.Object);

			await pinger.RunSinglePingJob();

			Assert.Equal(1, pingCalls);
			Assert.Equal(options.Hosts.Count(), reportedCalls);
		}

		[Fact]
		public async Task SimulateOn()
		{
			SmartThingsAppConfig stConfig = this.GetSmartThingsAppConfig();
			stConfig.Simulate = true;
			AppOptions options = this.GetMinimalOptions();
			options.SmartAppConfig = stConfig;

			int pingCalls = 0;
			var pingHelperMock = new Mock<IPingHelper>();
			pingHelperMock
				.Setup(x => x.PingHostsAsync(options.Hosts))
				.ReturnsAsync((IEnumerable<string> hosts) => hosts.Select(x => new PingResult(x, PingStatus.Online)))
				.Callback(() =>
				{
					pingCalls++;
				});

			var smartThingsHelper = new SmartThingsApiHelper(options.SmartAppConfig);

			int reportedCalls = 0;
			smartThingsHelper.UpdateRequestEvent += (sender, e) =>
			{
				reportedCalls++;
			};

			HostPinger pinger = new HostPinger(options, pingHelperMock.Object, smartThingsHelper);

			await pinger.RunSinglePingJob();

			Assert.Equal(1, pingCalls);
			Assert.Equal(options.Hosts.Count(), reportedCalls);
		}

		private AppOptions GetMinimalOptions()
		{
			return new AppOptions
			{
				Hosts = new string[] { "host1", "host2", "host3" },
				SmartAppConfig = this.GetSmartThingsAppConfig()
			};
		}

		private SmartThingsAppConfig GetSmartThingsAppConfig()
		{
			return new SmartThingsAppConfig
			{
				AccessToken = TestAccessTokenAppId,
				AppId = TestAccessTokenAppId,
				SmartThingsApiEndpoint = "https://my.smartthings.endpoint.com:443/"
			};
		}
	}
}