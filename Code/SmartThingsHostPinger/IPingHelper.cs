using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartThingsHostPinger
{
	internal interface IPingHelper
	{
		/// <summary>
		/// Pings the hosts asynchronously.
		/// </summary>
		/// <param name="hosts">The hosts.</param>
		/// <returns>The ping results.</returns>
		Task<IEnumerable<IPingResult>> PingHostsAsync(IEnumerable<string> hosts);
	}
}