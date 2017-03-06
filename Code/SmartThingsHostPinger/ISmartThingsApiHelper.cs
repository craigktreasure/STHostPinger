using System.Threading.Tasks;

namespace SmartThingsHostPinger
{
	internal interface ISmartThingsApiHelper
	{
		/// <summary>
		/// Send an update to the Smart Things SmartApp asynchronously.
		/// </summary>
		/// <param name="hostNameOrAddress">The host name or address.</param>
		/// <param name="online">A value indicating the online status of the host or address.</param>
		/// <returns>A value indicating if the request was sent successfully (true) or not (false).</returns>
		Task<bool> SendUpdateAsync(string hostNameOrAddress, bool online);
	}
}