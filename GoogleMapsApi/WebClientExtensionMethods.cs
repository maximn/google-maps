using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
	/// <summary>
	/// Provides asynchronous methods based on the new Async Task pattern. Emulates the new methods added to WebClient in .NET 4.5.
	/// </summary>
	/// <remarks>
	/// The code below uses the guidelines outlined in the MSDN article "Simplify Asynchronous Programming with Tasks" at http://msdn.microsoft.com/en-us/magazine/ff959203.aspx 
	/// under the "Converting an Event-Based Pattern" section.
	/// </remarks>
	public static class WebClientExtensionMethods
	{
		/// <summary>
		/// Download the specified resource as a string asynchronously.
		/// </summary>
		/// <param name="client">The client with which to download the specified resource.</param>
		/// <param name="address">The address of the resource to download.</param>
		/// <param name="token">Optional. A cancellation token that can be used to cancel the pending asynchronous task.</param>
		/// <returns>A Task with a value of the downloaded string.</returns>
		public static Task<string> DownloadStringTaskAsync(this WebClient client, Uri address)
		{
			var tcs = new TaskCompletionSource<string>();

			client.DownloadStringCompleted += (sender, args) =>
			{
				if (args.Error != null)
					tcs.SetException(args.Error);
				else if (args.Cancelled)
					tcs.SetCanceled();
				else tcs.SetResult(args.Result);
			};
			
			client.DownloadStringAsync(address);
			return tcs.Task;
		}
	}
}
