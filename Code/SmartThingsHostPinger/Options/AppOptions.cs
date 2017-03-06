using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SmartThingsHostPinger.Utils;

namespace SmartThingsHostPinger.Options
{
	internal class AppOptions : IAppOptions
	{
		/// <summary>
		/// The default debug setting.
		/// </summary>
		internal const bool DefaultDebug = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AppOptions"/> is debug.
		/// </summary>
		/// <value><c>true</c> if debug; otherwise, <c>false</c>.</value>
		public bool Debug { get; set; } = DefaultDebug;

		/// <summary>
		/// Gets or sets the hosts.
		/// </summary>
		/// <value>The hosts.</value>
		public IEnumerable<string> Hosts { get; set; }

		/// <summary>
		/// Gets or sets the ping options.
		/// </summary>
		/// <value>The ping options.</value>
		[JsonConverter(typeof(ConcreteTypeConverter<PingOptions>))]
		public IPingOptions PingOptions { get; set; } = new PingOptions();

		/// <summary>
		/// Gets or sets the smart application configuration.
		/// </summary>
		/// <value>The smart application configuration.</value>
		[JsonConverter(typeof(ConcreteTypeConverter<SmartThingsAppConfig>))]
		public ISmartThingsAppConfig SmartAppConfig { get; set; }

		/// <summary>
		/// Validates the <see cref="IAppOptions"/> instance.
		/// </summary>
		/// <param name="appOptions">The options to be validated.</param>
		/// <exception cref="InvalidOperationException">Thrown when the options are invalid.</exception>
		public static void Validate(IAppOptions appOptions)
		{
			if (appOptions == null ||
				appOptions.Hosts == null ||
				!appOptions.Hosts.Any() ||
				appOptions.PingOptions == null ||
				appOptions.SmartAppConfig == null ||
				string.IsNullOrWhiteSpace(appOptions.SmartAppConfig.AccessToken) ||
				!Guid.TryParse(appOptions.SmartAppConfig.AccessToken, out _) ||
				string.IsNullOrWhiteSpace(appOptions.SmartAppConfig.AppId) ||
				!Guid.TryParse(appOptions.SmartAppConfig.AppId, out _) ||
				string.IsNullOrWhiteSpace(appOptions.SmartAppConfig.SmartThingsApiEndpoint) ||
				!ValidHttpUrl(appOptions.SmartAppConfig.SmartThingsApiEndpoint))
			{
				throw new InvalidOperationException("Invalid configuration settings");
			}
		}

		/// <summary>
		/// Validates the options.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when the options are invalid.</exception>
		public void Validate()
		{
			Validate(this);
		}

		/// <summary>
		/// Validates the HTTP URL.
		/// </summary>
		/// <param name="urlString">The URL string.</param>
		/// <returns><c>true</c> if the URL is valid, <c>false</c> otherwise.</returns>
		private static bool ValidHttpUrl(string urlString)
		{
			return Uri.IsWellFormedUriString(urlString, UriKind.Absolute) && urlString.StartsWith("http");
		}
	}
}