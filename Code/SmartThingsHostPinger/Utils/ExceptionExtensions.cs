using System;

namespace SmartThingsHostPinger.Utils
{
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Flattens the specified extension.
		/// </summary>
		/// <param name="exception">The exception to flatten.</param>
		/// <returns>A flattened <see cref="Exception"/>.</returns>
		public static Exception Flatten(this Exception exception)
		{
			if (exception.InnerException != null)
			{
				return exception.InnerException.Flatten();
			}
			else
			{
				return exception;
			}
		}
	}
}