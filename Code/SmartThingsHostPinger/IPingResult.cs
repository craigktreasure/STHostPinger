namespace SmartThingsHostPinger
{
	internal interface IPingResult
	{
		/// <summary>
		/// Gets the host name or address.
		/// </summary>
		/// <value>The host name or address.</value>
		string HostNameOrAddress { get; }

		/// <summary>
		/// Gets the status.
		/// </summary>
		/// <value>The status.</value>
		PingStatus Status { get; }
	}
}