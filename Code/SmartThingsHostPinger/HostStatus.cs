namespace SmartThingsHostPinger
{
	internal enum HostStatus
	{
		/// <summary>
		/// Indicates that the status of the host is unknown.
		/// </summary>
		Unknown,

		/// <summary>
		/// Indicates that the status of the host is online.
		/// </summary>
		Online,

		/// <summary>
		/// Indicates that the status of the host is offline.
		/// </summary>
		Offline,

		/// <summary>
		/// Indicates that the status of the host transitioned from online to offline.
		/// </summary>
		WentOffline
	}
}