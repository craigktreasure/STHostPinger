namespace SmartThingsHostPinger
{
	internal class HostStatusInfo
	{
		/// <summary>
		/// Gets or sets the host name or address.
		/// </summary>
		/// <value>The host name or address.</value>
		public string HostNameOrAddress { get; set; }

		/// <summary>
		/// Indicates that the host status was reported to the
		/// Smart Things SmartApp successfully.
		/// </summary>
		/// <value>The reported.</value>
		public bool Reported { get; set; } = false;

		/// <summary>
		/// Indicates the current status of the host.
		/// </summary>
		/// <value>The status.</value>
		public HostStatus Status { get; set; }

		/// <summary>
		/// Returns a string that represents a <see cref="HostStatusInfo"/> object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return $"{this.HostNameOrAddress}: {this.Status}";
		}
	}
}