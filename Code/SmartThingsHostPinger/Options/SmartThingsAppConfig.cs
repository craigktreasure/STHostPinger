namespace SmartThingsHostPinger.Options
{
	public class SmartThingsAppConfig : ISmartThingsAppConfig
	{
		/// <summary>
		/// The default simulate setting.
		/// </summary>
		internal const bool DefaultSimulate = false;

		/// <summary>
		/// The offline endpoint format.
		/// </summary>
		private const string offlineEndpointFormat = "{0}/api/smartapps/installations/{1}/statechanged/offline";

		/// <summary>
		/// The online endpoint format.
		/// </summary>
		private const string onlineEndpointFormat = "{0}/api/smartapps/installations/{1}/statechanged/online";

		/// <summary>
		/// The offline endpoint.
		/// </summary>
		private string offlineEndpoint = null;

		/// <summary>
		/// The online endpoint.
		/// </summary>
		private string onlineEndpoint = null;

		/// <summary>
		/// Gets the access token.
		/// </summary>
		/// <value>The access token.</value>
		public string AccessToken { get; set; }

		/// <summary>
		/// Gets the application identifier.
		/// </summary>
		/// <value>The application identifier.</value>
		public string AppId { get; set; }

		/// <summary>
		/// Gets the offline endpoint.
		/// </summary>
		/// <value>The offline endpoint.</value>
		public string OfflineEndpoint
		{
			get
			{
				if (string.IsNullOrEmpty(this.offlineEndpoint))
				{
					this.offlineEndpoint = string.Format(offlineEndpointFormat, this.SmartThingsApiEndpoint.TrimEnd('/'), this.AppId);
				}

				return this.offlineEndpoint;
			}
		}

		/// <summary>
		/// Gets the online endpoint.
		/// </summary>
		/// <value>The online endpoint.</value>
		public string OnlineEndpoint
		{
			get
			{
				if (string.IsNullOrEmpty(this.onlineEndpoint))
				{
					this.onlineEndpoint = string.Format(onlineEndpointFormat, this.SmartThingsApiEndpoint.Trim('/'), this.AppId);
				}

				return this.onlineEndpoint;
			}
		}

		/// <summary>
		/// Gets a value indicating that simulation is enabled.
		/// Updates will not be sent to the Smart Things app.
		/// </summary>
		/// <value><c>true</c> if simulation is enabled; otherwise, <c>false</c>.</value>
		public bool Simulate { get; set; } = DefaultSimulate;

		/// <summary>
		/// Gets the Smart Things API endpoint.
		/// </summary>
		/// <value>The Smart Things API endpoint.</value>
		public string SmartThingsApiEndpoint { get; set; }
	}
}