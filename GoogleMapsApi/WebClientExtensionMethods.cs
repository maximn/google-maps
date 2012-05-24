using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
	/// <summary>
	/// Provides asynchronous methods based on the new Async Task pattern. Emulates the new methods added to WebClient in .NET 4.5 (and has additions).
	/// </summary>
	/// <remarks>
	/// The code below uses the guidelines outlined in the MSDN article "Simplify Asynchronous Programming with Tasks" 
	/// at http://msdn.microsoft.com/en-us/magazine/ff959203.aspx under the "Converting an Event-Based Pattern" section.
	/// </remarks>
	public static class WebClientExtensionMethods
	{
		/// <summary>
		/// Constant. Specified an infinite timeout duration. This is a TimeSpan of negative one (-1) milliseconds.
		/// </summary>
		public static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1);

		/// <summary>
		/// Asynchronously downloads the resource with the specified URI as a Byte array limited by the specified timeout.
		/// </summary>
		/// <param name="client">The client with which to download the specified resource.</param>
		/// <param name="address">The address of the resource to download.</param>
		/// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
		/// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
		/// When a request is aborted due to a timeout the returned task will transition to the Faulted state with a TimeoutException.</param>
		/// <returns>A Task with the future value of the downloaded string.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the client or address parameters.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the value of timeout is neither a positive value or infinite.</exception>
		public static Task<byte[]> DownloadDataTaskAsync(this WebClient client, Uri address, TimeSpan timeout)
		{
			return client.DownloadDataTaskAsync(address, timeout, CancellationToken.None);
		}

		/// <summary>
		/// Asynchronously downloads the resource with the specified URI as a Byte array and allows cancelling the operation. 
		/// Note that this overload specifies an infinite timeout.
		/// </summary>
		/// <param name="client">The client with which to download the specified resource.</param>
		/// <param name="address">The address of the resource to download.</param>
		/// <param name="token">A cancellation token that can be used to cancel the pending asynchronous task.</param>
		/// <returns>A Task with the future value of the downloaded string.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the client or address parameters.</exception>
		public static Task<byte[]> DownloadDataTaskAsync(this WebClient client, Uri address, CancellationToken token)
		{
			return client.DownloadDataTaskAsync(address, TimeSpan.FromSeconds(Timeout.Infinite), token);
		}

		/// <summary>
		/// Asynchronously downloads the resource with the specified URI as a Byte array limited by the specified timeout and allows cancelling the operation.
		/// </summary>
		/// <param name="client">The client with which to download the specified resource.</param>
		/// <param name="address">The address of the resource to download.</param>
		/// <param name="timeout">A TimeSpan specifying the amount of time to wait for a response before aborting the request.
		/// The specify an infinite timeout, pass a TimeSpan with a TotalMillisecond value of Timeout.Infinite.
		/// When a request is aborted due to a timeout the returned task will transition to the Faulted state with a TimeoutException.</param>
		/// <param name="token">A cancellation token that can be used to cancel the pending asynchronous task.</param>
		/// <returns>A Task with the future value of the downloaded string.</returns>
		/// <exception cref="ArgumentNullException">Thrown when a null value is passed to the client or address parameters.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when the value of timeout is neither a positive value or infinite.</exception>
		public static Task<byte[]> DownloadDataTaskAsync(this WebClient client, Uri address, TimeSpan timeout, CancellationToken token)
		{
			if (client == null) throw new ArgumentNullException("client");
			if (address == null) throw new ArgumentNullException("address");
			if (timeout.TotalMilliseconds < 0 && timeout != InfiniteTimeout)
				throw new ArgumentOutOfRangeException("address", timeout, "The timeout value must be a positive value or have a TotalMilliseconds value of Timeout.Infinite.");

			var tcs = new TaskCompletionSource<byte[]>();
			var delayTokenSource = new CancellationTokenSource();

			token.Register(() =>
			{
				delayTokenSource.Cancel();
				client.CancelAsync();
			});

			if (timeout != InfiniteTimeout)
			{
				TaskEx.Delay(timeout, delayTokenSource.Token).ContinueWith(t =>
					{
						tcs.TrySetException(new TimeoutException(string.Format("The request has exceeded the timeout limit of {0} and has been aborted. The requested URI was: {1}", timeout, address)));
						client.CancelAsync();
					}, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled);
			}
			
			client.DownloadDataCompleted += (sender, args) =>
			{
				delayTokenSource.Cancel();

				if (args.Cancelled)
					tcs.TrySetCanceled();
				else if (args.Error != null)
					tcs.TrySetException(args.Error);
				else tcs.TrySetResult(args.Result);
			};
			
			client.DownloadDataAsync(address);
			return tcs.Task;
		}
	}
}
