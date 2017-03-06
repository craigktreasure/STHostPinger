using System;
using SmartThingsHostPinger.Options;
using Xunit;

namespace SmartThingsHostPinger.Tests.Options
{
	public class AppOptionsTests
	{
		[Theory]
		[InlineData("EmptyAccessTokenConfigScenario.json")]
		[InlineData("EmptyApiEndpointConfigScenario.json")]
		[InlineData("EmptyAppIdConfigScenario.json")]
		[InlineData("EmptyHostsConfigScenario.json")]
		[InlineData("InvalidAccessTokenConfigScenario.json")]
		[InlineData("InvalidApiEndpointConfigScenario.json")]
		[InlineData("InvalidAppIdConfigScenario.json")]
		[InlineData("MissingAccessTokenConfigScenario.json")]
		[InlineData("MissingApiEndpointConfigScenario.json")]
		[InlineData("MissingAppIdConfigScenario.json")]
		[InlineData("MissingHostsConfigScenario.json")]
		public void InvalidConfig(string configFileName)
		{
			IAppOptions options = ConfigLoader.LoadRawJsonConfig(configFileName);

			Assert.Throws<InvalidOperationException>(() => options.Validate());
		}

		[Theory]
		[InlineData("FullConfigScenario.json")]
		[InlineData("MinimalConfigScenario.json")]
		public void ValidConfig(string configFileName)
		{
			IAppOptions options = ConfigLoader.LoadRawJsonConfig(configFileName);

			options.Validate();
		}
	}
}