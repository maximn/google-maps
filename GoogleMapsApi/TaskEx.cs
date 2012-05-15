using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
	/// <summary>
	/// A partial implementation of the TaskEx class provided in the Async CTP, plus additions.
	/// </summary>
	public static class TaskEx
	{
		static readonly Task _sPreCompletedTask = GetCompletedTask();
		static readonly Task _sPreCanceledTask = GetPreCanceledTask();

		public static Task Delay(TimeSpan dueTime, CancellationToken cancellationToken)
		{
			return Delay((int)dueTime.TotalMilliseconds, cancellationToken);
		}
		public static Task Delay(int dueTimeMs, CancellationToken cancellationToken)
		{
			if (dueTimeMs < -1)
				throw new ArgumentOutOfRangeException("dueTimeMs", "Invalid due time");
			if (cancellationToken.IsCancellationRequested)
				return _sPreCanceledTask;
			if (dueTimeMs == 0)
				return _sPreCompletedTask;

			var tcs = new TaskCompletionSource<object>();
			var ctr = new CancellationTokenRegistration();
			var timer = new Timer(delegate(object self)
			{
				ctr.Dispose();
				((Timer)self).Dispose();
				tcs.TrySetResult(null);
			});
			if (cancellationToken.CanBeCanceled)
				ctr = cancellationToken.Register(delegate
				{
					timer.Dispose();
					tcs.TrySetCanceled();
				});

			timer.Change(dueTimeMs, -1);
			return tcs.Task;
		}

		public static void SetFromTask<T>(this TaskCompletionSource<T> completionSource, Task<T> completedTask)
		{
			if (!completedTask.IsCompleted)
				throw new ArgumentException("Cannot copy the task result from the supplied task since it has not completed.");

			if (completedTask.IsCanceled)
				completionSource.SetCanceled();
			else if (completedTask.IsFaulted)
				completionSource.SetException(completedTask.Exception.InnerException);
			else
				completionSource.SetResult(completedTask.Result);
		}

		private static Task GetPreCanceledTask()
		{
			var source = new TaskCompletionSource<object>();
			source.TrySetCanceled();
			return source.Task;
		}
		private static Task GetCompletedTask()
		{
			var source = new TaskCompletionSource<object>();
			source.TrySetResult(null);
			return source.Task;
		}
	} 

}
