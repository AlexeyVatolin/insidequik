using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Common.Helpers
{
    public static class HubHelper
    {
        private static readonly object SyncRoot = new object();
        private static CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public static void StartCallBacks(this IHubProxy hub)
        {
            lock (SyncRoot)
            {
                CreateTask();
                _tokenSource = new CancellationTokenSource();
            }
        }
        private static Task _currentTask;

        private static void CreateTask()
        {
            _currentTask = Task.Factory.StartNew(() => { }, _tokenSource.Token);
        }

        public static void CancelCallbacks(this IHubProxy hub)
        {
            lock (SyncRoot)
            {
                _tokenSource.Cancel();
                _currentTask = null;
            }
        }

        public static async Task<TOutput> Invoke<TOutput>(this IHubProxy hub, string method, params object[] args)
        {
            return await hub.Invoke<TOutput>(method, args).ConfigureAwait(false);
        }

        public static void RegisterCallback<TInput>(this IHubProxy hub, Action<TInput> action)
        {
            var name = action.Method.Name;
            hub.On<TInput>(name, data =>
            {
                // Run tasks by turn
                lock (SyncRoot)
                {
                    _currentTask = _currentTask?.ContinueWith(t => OnCallback(action, data), _tokenSource.Token);
                }
            });
        }

        private static void OnCallback<TInput>(Action<TInput> action, TInput data)
        {
            if (_currentTask != null)
            {
                action(data);
            }
        }

    }
}
