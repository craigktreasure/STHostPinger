namespace SmartThingsHostPinger.Options
{
	public interface IPingOptions
	{
		/// <summary>
		/// Gets a value indicating that pinging hosts concurrently is enabled.
		/// </summary>
		/// <value><c>true</c> if to enable pinging of hosts concurrently; otherwise, <c>false</c>.</value>
		bool PingHostsConcurrently { get; }

		/// <summary>
		/// Gets the ping interval in seconds.
		/// </summary>
		/// <value>The ping interval in seconds.</value>
		int PingIntervalSeconds { get; }

		/// <summary>
		/// Gets the number of retry attempts.
		/// </summary>
		/// <value>The number of retry attempts.</value>
		int RetryAttempts { get; }

		/// <summary>
		/// Gets the timeout value in milliseconds.
		/// </summary>
		/// <value>The timeout value in milliseconds.</value>
		int TimeoutMilliseconds { get; }
	}
}