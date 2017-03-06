using SmartThingsHostPinger.Options;
using Xunit;

namespace SmartThingsHostPinger.Tests.Options
{
	public class ConfigTests
	{
		private const string TestAccessTokenAppId = "00000000-0000-0000-0000-000000000000";
		private const string TestJsonApiEndpoint = "https://my.smartthings.endpoint.com:443";
		private const string TestXmlApiEndpoint = "https://my.smartthings.endpoint.com";

		[Theory]
		[InlineData("EmptyAccessTokenConfigScenario.json")]
		[InlineData("EmptyAccessTokenConfigScenario.xml")]
		[InlineData("EmptyApiEndpointConfigScenario.json")]
		[InlineData("EmptyApiEndpointConfigScenario.xml")]
		[InlineData("EmptyAppIdConfigScenario.json")]
		[InlineData("EmptyAppIdConfigScenario.xml")]
		[InlineData("EmptyHostsConfigScenario.json")]
		[InlineData("EmptyHostsConfigScenario.xml")]
		[InlineData("InvalidAccessTokenConfigScenario.json")]
		[InlineData("InvalidAccessTokenConfigScenario.xml")]
		[InlineData("InvalidApiEndpointConfigScenario.json")]
		[InlineData("InvalidApiEndpointConfigScenario.xml")]
		[InlineData("InvalidAppIdConfigScenario.json")]
		[InlineData("InvalidAppIdConfigScenario.xml")]
		[InlineData("MissingAccessTokenConfigScenario.json")]
		[InlineData("MissingAccessTokenConfigScenario.xml")]
		[InlineData("MissingApiEndpointConfigScenario.json")]
		[InlineData("MissingApiEndpointConfigScenario.xml")]
		[InlineData("MissingAppIdConfigScenario.json")]
		[InlineData("MissingAppIdConfigScenario.xml")]
		[InlineData("MissingHostsConfigScenario.json")]
		[InlineData("MissingHostsConfigScenario.xml")]
		[InlineData("MissingOnlineEndpointConfigScenario.xml")]
		public void LoadConfigError(string configFileName)
		{
			bool loaded = ConfigLoader.LoadConfig(configFileName, out IAppOptions options);

			Assert.False(loaded);
			Assert.Null(options);
		}

		[Theory]
		[InlineData("FullConfigScenario.json")]
		[InlineData("FullConfigScenario.xml")]
		public void LoadFullConfig(string configFileName)
		{
			bool loaded = ConfigLoader.LoadConfig(configFileName, out IAppOptions options);

			bool json = configFileName.EndsWith(".json");

			Assert.True(loaded);
			Assert.True(options.Debug);
			Assert.NotNull(options.Hosts);
			Assert.Equal(new string[] {
				"google.com",
				"192.168.1.1",
				"SERVER1"
			}, options.Hosts);
			Assert.NotNull(options.PingOptions);
			if (json)
			{
				Assert.False(options.PingOptions.PingHostsConcurrently);
				Assert.Equal(2000, options.PingOptions.RetryAttempts);
				Assert.True(options.SmartAppConfig.Simulate);
			}
			else
			{
				Assert.True(options.PingOptions.PingHostsConcurrently); // Not supported in Xml config
				Assert.Equal(options.PingOptions.RetryAttempts, 5); // Not supported in Xml config
				Assert.False(options.SmartAppConfig.Simulate); // Not supported in Xml config
			}
			Assert.Equal(2000, options.PingOptions.PingIntervalSeconds);
			Assert.Equal(2000, options.PingOptions.TimeoutMilliseconds);
			Assert.NotNull(options.SmartAppConfig);
			Assert.Equal(TestAccessTokenAppId, options.SmartAppConfig.AccessToken);
			Assert.Equal(TestAccessTokenAppId, options.SmartAppConfig.AppId);
			Assert.Equal(json ? TestJsonApiEndpoint : TestXmlApiEndpoint,
				options.SmartAppConfig.SmartThingsApiEndpoint);
		}

		[Theory]
		[InlineData("MinimalConfigScenario.json")]
		[InlineData("MinimalConfigScenario.xml")]
		[InlineData("EmptyPingSettingsConfigScenario.xml")]
		public void LoadMinimalConfig(string configFileName)
		{
			bool loaded = ConfigLoader.LoadConfig(configFileName, out IAppOptions options);

			Assert.True(loaded);
			Assert.Equal(AppOptions.DefaultDebug, options.Debug);
			Assert.NotNull(options.Hosts);
			Assert.Equal(new string[] {
				"google.com",
				"192.168.1.1",
				"SERVER1"
			}, options.Hosts);
			Assert.NotNull(options.PingOptions);
			Assert.Equal(PingOptions.DefaultPingHostsConcurrently, options.PingOptions.PingHostsConcurrently);
			Assert.Equal(PingOptions.DefaultPingIntervalSeconds, options.PingOptions.PingIntervalSeconds);
			Assert.Equal(PingOptions.DefaultRetryAttempts, options.PingOptions.RetryAttempts);
			Assert.Equal(PingOptions.DefaultTimeoutMilliseconds, options.PingOptions.TimeoutMilliseconds);
			Assert.NotNull(options.SmartAppConfig);
			Assert.Equal(SmartThingsAppConfig.DefaultSimulate, options.SmartAppConfig.Simulate);
			Assert.Equal(TestAccessTokenAppId, options.SmartAppConfig.AccessToken);
			Assert.Equal(TestAccessTokenAppId, options.SmartAppConfig.AppId);
			Assert.Equal(configFileName.EndsWith(".json") ? TestJsonApiEndpoint : TestXmlApiEndpoint,
				options.SmartAppConfig.SmartThingsApiEndpoint);
		}

		[Theory]
		[InlineData("TrailingApiSlashConfigScenario.json")]
		[InlineData("TrailingApiSlashConfigScenario.xml")]
		public void EndpointCorrectness(string configFileName)
		{
			bool loaded = ConfigLoader.LoadConfig(configFileName, out IAppOptions options);

			bool json = configFileName.EndsWith(".json");

			string expectedOnlineEndpoint = $"{(json ? TestJsonApiEndpoint : TestXmlApiEndpoint)}/api/smartapps/installations/{TestAccessTokenAppId}/statechanged/online";
			string expectedOfflineEndpoint = $"{(json ? TestJsonApiEndpoint : TestXmlApiEndpoint)}/api/smartapps/installations/{TestAccessTokenAppId}/statechanged/offline";

			Assert.True(loaded);
			Assert.Equal(expectedOfflineEndpoint, options.SmartAppConfig.OfflineEndpoint);
			Assert.Equal(expectedOnlineEndpoint, options.SmartAppConfig.OnlineEndpoint);
		}
	}
}