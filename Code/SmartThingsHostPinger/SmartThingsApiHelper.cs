using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using SmartThingsHostPinger.Options;

namespace SmartThingsHostPinger
{
	/// <summary>
	/// Class <see cref="SmartThingsApiHelper"/>.
	/// </summary>
	/// <seealso cref="SmartThingsHostPinger.ISmartThingsApiHelper" />
	internal class SmartThingsApiHelper : ISmartThingsApiHelper
	{
		/// <summary>
		/// The options
		/// </summary>
		private ISmartThingsAppConfig options;

		/// <summary>
		/// Initializes a new instance of the <see cref="SmartThingsApiHelper"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public SmartThingsApiHelper(ISmartThingsAppConfig options)
		{
			this.options = options;
		}

		/// <summary>
		/// Delegate UpdateRequestEventHandler
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="UpdateRequestEventArgs"/> instance containing the event data.</param>
		internal delegate void UpdateRequestEventHandler(object sender, UpdateRequestEventArgs e);

		/// <summary>
		/// Occurs after an update request is sent.
		/// </summary>
		internal event UpdateRequestEventHandler UpdateRequestEvent;

		/// <summary>
		/// Send an update to the Smart Things SmartApp asynchronously.
		/// </summary>
		/// <param name="hostNameOrAddress">The host name or address.</param>
		/// <param name="online">A value indicating the online status of the host or address.</param>
		/// <returns>A value indicating if the request was sent successfully (true) or not (false).</returns>
		public Task<bool> SendUpdateAsync(string hostNameOrAddress, bool online)
		{
			string endpoint = this.ConstructEndpointUrl(hostNameOrAddress, online);

			return this.SendGetRequestAsync(endpoint);
		}

		/// <summary>
		/// Constructs the endpoint URL.
		/// </summary>
		/// <param name="hostNameOrAddress">The host name or address.</param>
		/// <param name="online">A value indicating the online status of the host or address.</param>
		/// <returns>The endpoing URL.</returns>
		private string ConstructEndpointUrl(string hostNameOrAddress, bool online)
		{
			StringBuilder endpoint = new StringBuilder();
			if (online)
			{
				endpoint.Append(this.options.OnlineEndpoint);
			}
			else
			{
				endpoint.Append(this.options.OfflineEndpoint);
			}

			// Add the GET parameters to the request.
			endpoint.Append("?access_token=" + WebUtility.UrlEncode(this.options.AccessToken));
			endpoint.Append("&ipadd=" + WebUtility.UrlEncode(hostNameOrAddress));

			return endpoint.ToString();
		}

		/// <summary>
		/// Handles the <see cref="UpdateRequestEvent" /> event.
		/// </summary>
		/// <param name="args">The <see cref="UpdateRequestEventArgs"/> instance containing the event data.</param>
		private void OnRaiseSendGetRequestEvent(UpdateRequestEventArgs args)
		{
			var handler = this.UpdateRequestEvent;

			if (handler != null)
			{
				this.UpdateRequestEvent(this, args);
			}
		}

		/// <summary>
		/// Send a get request as an asynchronous operation.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>A value indicating if the request was sent successfully (true) or not (false).</returns>
		private async Task<bool> SendGetRequestAsync(string url)
		{
			bool sentResult = false;

			try
			{
				Log.Debug("SendGetRequest: {Url}", url);
				string response = null;

				if (!this.options.Simulate)
				{
					using (HttpClient client = new HttpClient())
					{
						response = await client.GetStringAsync(url);
					}
				}
				else
				{
					response = "Simulated Success";
				}

				sentResult = true;

				Log.Debug("Result: {Response}", response);

				this.OnRaiseSendGetRequestEvent(
					new UpdateRequestEventArgs { Response = response, Success = sentResult, Url = url });
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to SendGetRequest: {Message}", ex.Message);

				sentResult = false;

				this.OnRaiseSendGetRequestEvent(
					new UpdateRequestEventArgs { Error = ex, Success = sentResult, Url = url });
			}

			return sentResult;
		}
	}

	/// <summary>
	/// Class <see cref="UpdateRequestEventArgs"/>.
	/// </summary>
	/// <seealso cref="System.EventArgs" />
	internal class UpdateRequestEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the error.
		/// </summary>
		public Exception Error { get; set; }

		/// <summary>
		/// Gets or sets the response.
		/// </summary>
		public string Response { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the request was successful.
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		public string Url { get; set; }
	}
}