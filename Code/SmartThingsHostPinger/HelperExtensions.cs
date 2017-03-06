using System;

namespace SmartThingsHostPinger
{
	internal static class HelperExtensions
	{
		/// <summary>
		/// Convert a <see cref="PingStatus"/> to a <see cref="HostStatus"/>.
		/// </summary>
		/// <param name="pingStatus">The ping status.</param>
		/// <param name="previouslyOnline">Indicates that the host was previously online.</param>
		/// <returns>The equivalent <see cref="HostStatus"/>.</returns>
		public static HostStatus ToHostStatus(this PingStatus pingStatus, bool previouslyOnline = false)
		{
			switch (pingStatus)
			{
				case PingStatus.Offline:
					return previouslyOnline ? HostStatus.WentOffline : HostStatus.Offline;

				case PingStatus.Online:
					return HostStatus.Online;

				case PingStatus.Unknown:
					return HostStatus.Unknown;

				default:
					throw new NotSupportedException($"Unsupported {nameof(PingStatus)}");
			}
		}
	}
}