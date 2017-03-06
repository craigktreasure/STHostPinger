namespace SmartThingsHostPinger.Options
{
	public interface ISmartThingsAppConfig
    {
		/// <summary>
		/// Gets the access token.
		/// </summary>
		/// <value>The access token.</value>
		string AccessToken { get; }

		/// <summary>
		/// Gets the application identifier.
		/// </summary>
		/// <value>The application identifier.</value>
		string AppId { get; }

		/// <summary>
		/// Gets the online endpoint.
		/// </summary>
		/// <value>The online endpoint.</value>
		string OnlineEndpoint { get; }

		/// <summary>
		/// Gets the offline endpoint.
		/// </summary>
		/// <value>The offline endpoint.</value>
		string OfflineEndpoint { get; }

		/// <summary>
		/// Gets a value indicating that simulation is enabled.
		/// Updates will not be sent to the Smart Things app.
		/// </summary>
		/// <value><c>true</c> if simulation is enabled; otherwise, <c>false</c>.</value>
		bool Simulate { get; }

		/// <summary>
		/// Gets the Smart Things API endpoint.
		/// </summary>
		/// <value>The Smart Things API endpoint.</value>
		string SmartThingsApiEndpoint { get; }
    }
}