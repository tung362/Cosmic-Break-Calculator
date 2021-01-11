using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CB.Utils
{
    /// <summary>
    /// Extensions for UniTask
    /// </summary>
    public static class UniAsyncExtensions
    {
        public static UniTask WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (process.HasExited) return UniTask.CompletedTask;

            var tcs = new UniTaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken)) cancellationToken.Register(() => tcs.TrySetCanceled());
            return process.HasExited ? UniTask.CompletedTask : tcs.Task;
        }
    }
}
