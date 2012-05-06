using System;
using System.IO;
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
    public static Task<Stream> OpenReadStreamAsync(this WebClient client, Uri address)
    {
      var tcs = new TaskCompletionSource<Stream>();

      client.OpenReadCompleted += (sender, args) =>
      {
        if (args.Error != null)
          tcs.SetException(args.Error);
        else if (args.Cancelled)
          tcs.SetCanceled();
        else tcs.SetResult(args.Result);
      };

      client.OpenReadAsync(address);

      return tcs.Task;
    }
  }
}