using System.IO;
using Newtonsoft.Json;
using SmartThingsHostPinger.Options;
using Xunit;

namespace SmartThingsHostPinger.Tests
{
	internal static class ConfigLoader
	{
		/// <summary>
		/// Loads the configuration.
		/// Asserts that the config file exists.
		/// </summary>
		/// <param name="configFileName">Name of the configuration file.</param>
		/// <param name="options">The options.</param>
		/// <returns>A value indicating if the configuration was loaded successfully.</returns>
		public static bool LoadConfig(string configFileName, out IAppOptions options)
		{
			string configFilePath = $"Reference/Options/{configFileName}";

			Assert.True(File.Exists(configFilePath));

			bool loaded = false;

			if (configFileName.EndsWith(".json"))
			{
				loaded = Config.TryLoadJsonConfig(configFilePath, out options);
			}
			else
			{
				loaded = Config.TryLoadXmlConfig(configFilePath, out options);
			}

			return loaded;
		}

		/// <summary>
		/// Loads the raw json configuration.
		/// </summary>
		/// <param name="configFileName">Name of the configuration file.</param>
		/// <returns>The app options.</returns>
		internal static AppOptions LoadRawJsonConfig(string configFileName)
		{
			string configFilePath = $"Reference/Options/{configFileName}";

			Assert.True(File.Exists(configFilePath));

			string configContents = File.ReadAllText(configFilePath);
			return JsonConvert.DeserializeObject<AppOptions>(configContents);
		}
	}
}