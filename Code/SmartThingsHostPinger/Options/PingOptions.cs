namespace SmartThingsHostPinger.Options
{
	public class PingOptions : IPingOptions
	{
		/// <summary>
		/// The default ping hosts concurrently setting.
		/// </summary>
		internal const bool DefaultPingHostsConcurrently = true;

		/// <summary>
		/// The default ping interval seconds setting.
		/// </summary>
		internal const int DefaultPingIntervalSeconds = 30;

		/// <summary>
		/// The default retry attempts setting.
		/// </summary>
		internal const int DefaultRetryAttempts = 5;

		/// <summary>
		/// The default timeout milliseconds setting.
		/// </summary>
		internal const int DefaultTimeoutMilliseconds = 1000;

		/// <summary>
		/// Gets a value indicating that pinging hosts concurrently is enabled.
		/// </summary>
		/// <value><c>true</c> if to enable pinging of hosts concurrently; otherwise, <c>false</c>.</value>
		public bool PingHostsConcurrently { get; set; } = DefaultPingHostsConcurrently;

		/// <summary>
		/// Gets the ping interval in seconds.
		/// </summary>
		/// <value>The ping interval in seconds.</value>
		public int PingIntervalSeconds { get; set; } = DefaultPingIntervalSeconds;

		/// <summary>
		/// Gets the number of retry attempts.
		/// </summary>
		/// <value>The number of retry attempts.</value>
		public int RetryAttempts { get; set; } = DefaultRetryAttempts;

		/// <summary>
		/// Gets the timeout value in milliseconds.
		/// </summary>
		/// <value>The timeout value in milliseconds.</value>
		public int TimeoutMilliseconds { get; set; } = DefaultTimeoutMilliseconds;
	}
}