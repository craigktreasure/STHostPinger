namespace SmartThingsHostPinger
{
	internal class PingResult : IPingResult
	{
		/// <summary>
		/// Gets the host name or address.
		/// </summary>
		/// <value>The host name or address.</value>
		public string HostNameOrAddress { get; private set; }

		/// <summary>
		/// Gets the status.
		/// </summary>
		/// <value>The status.</value>
		public PingStatus Status { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PingResult"/> class.
		/// </summary>
		/// <param name="hostNameOrAddress">The host name or address.</param>
		/// <param name="status">The status.</param>
		public PingResult(string hostNameOrAddress, PingStatus status)
		{
			this.HostNameOrAddress = hostNameOrAddress;
			this.Status = status;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return $"{this.HostNameOrAddress}: {this.Status}";
		}
	}
}