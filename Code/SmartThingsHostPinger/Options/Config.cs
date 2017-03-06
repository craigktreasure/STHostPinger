using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json;
using Serilog;

namespace SmartThingsHostPinger.Options
{
	internal static class Config
	{
		/// <summary>
		/// Tries to load a json configuration.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="options">The options.</param>
		/// <returns><c>true</c> if the load was successful, <c>false</c> otherwise.</returns>
		public static bool TryLoadJsonConfig(string filePath, out IAppOptions options)
		{
			options = null;

			try
			{
				if (!File.Exists(filePath))
				{
					return false;
				}

				string configContents = File.ReadAllText(filePath);
				var opts = JsonConvert.DeserializeObject<AppOptions>(configContents);

				opts.Validate();

				options = opts;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to load config.json: {Message}", ex.Message);
			}

			return options != null;
		}

		/// <summary>
		/// Tries to load an XML configuration.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="options">The options.</param>
		/// <returns><c>true</c> if the load was successful, <c>false</c> otherwise.</returns>
		public static bool TryLoadXmlConfig(string filePath, out IAppOptions options)
		{
			options = null;

			try
			{
				if (!File.Exists(filePath))
				{
					return false;
				}

				XDocument doc = XDocument.Load(filePath);

				XElement eConfig = doc.XPathSelectElement("/config");

				// Get app settings.
				XElement IPCheck = eConfig.Element("pingSettings");
				PingOptions pingOptions = new PingOptions();
				bool? debugLogging = null;
				if (IPCheck != null)
				{
					XAttribute checkIntervalAttribute = IPCheck.Attribute("checkInterval");
					if (checkIntervalAttribute != null)
					{
						pingOptions.PingIntervalSeconds = int.Parse(checkIntervalAttribute.Value);
					}

					XAttribute timeoutAttribute = IPCheck.Attribute("timeOut");
					if (timeoutAttribute != null)
					{
						pingOptions.TimeoutMilliseconds = int.Parse(timeoutAttribute.Value);
					}

					XAttribute debugAttribute = IPCheck.Attribute("debug");
					if (debugAttribute != null)
					{
						debugLogging = bool.Parse(debugAttribute.Value);
					}
				}

				// Get Smart Things configuration.
				XElement eSTE = eConfig.Element("smartThingsEndpoints");
				string endpoint_AccessToken = eSTE.Attribute("accessToken").Value;

				// Parse the Smart Things endpoint from the online endpoint.
				if (!Uri.TryCreate(eSTE.Attribute("Online").Value, UriKind.Absolute, out Uri onlineUri))
				{
					throw new InvalidOperationException("Couldn't determine the Online SmartThings API Endpoint.");
				}
				string stEndpoint = $"{onlineUri.Scheme}://{onlineUri.Host}";
				if (!onlineUri.IsDefaultPort)
				{
					stEndpoint += ":" + onlineUri.Port;
				}

				// Parse the App Id from the online endpoint.
				Regex appIdRegex = new Regex(@"\/api\/smartapps\/installations\/(?<appId>.*)\/statechanged\/online");
				Match endpointMatch = appIdRegex.Match(onlineUri.ToString());
				if (!endpointMatch.Success)
				{
					throw new InvalidOperationException("Couldn't determine the AppId.");
				}

				string appId = endpointMatch.Groups["appId"].Value;

				// Process the host list.

				XElement eHostList = eConfig.Element("hostList");

				List<string> hostList = new List<string>();
				foreach (XElement item in eHostList.Elements())
				{
					hostList.Add(item.Attribute("HOST").Value);
				}

				var opts = new AppOptions
				{
					Hosts = hostList,
					Debug = debugLogging ?? AppOptions.DefaultDebug,
					PingOptions = pingOptions,
					SmartAppConfig = new SmartThingsAppConfig
					{
						AccessToken = endpoint_AccessToken,
						AppId = appId,
						SmartThingsApiEndpoint = stEndpoint
					}
				};

				opts.Validate();

				options = opts;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to load {FilePath}: {Message}", filePath, ex.Message);
				options = null;
			}

			return options != null;
		}
	}
}