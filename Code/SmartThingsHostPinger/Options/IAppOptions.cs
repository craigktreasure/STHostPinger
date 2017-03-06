using System.Collections.Generic;

namespace SmartThingsHostPinger.Options
{
	internal interface IAppOptions
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="IAppOptions"/> is debug.
		/// </summary>
		/// <value><c>true</c> if debug; otherwise, <c>false</c>.</value>
		bool Debug { get; }

		/// <summary>
		/// Gets the hosts.
		/// </summary>
		/// <value>The hosts.</value>
		IEnumerable<string> Hosts { get; }

		/// <summary>
		/// Gets the ping options.
		/// </summary>
		/// <value>The ping options.</value>
		IPingOptions PingOptions { get; }

		/// <summary>
		/// Gets the smart application configuration.
		/// </summary>
		/// <value>The smart application configuration.</value>
		ISmartThingsAppConfig SmartAppConfig { get; }

		/// <summary>
		/// Validates the options.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when the options are invalid.</exception>
		void Validate();
	}
}