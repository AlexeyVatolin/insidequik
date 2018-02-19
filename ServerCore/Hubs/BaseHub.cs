using Microsoft.AspNet.SignalR;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Storages;

namespace ServerCore.Hubs
{
    public class BaseHub<TClient, TClientCallbacks> : Hub<TClientCallbacks>
        where TClientCallbacks : class, IHubClientCallbacks
        where TClient : class, IClientBase, new()
    {
        protected User CurrentUser => SessionStorage.GetUser(Context.Headers["Session-Id"]);

        protected static IHubContext CurrentContext => GlobalHost.ConnectionManager.GetHubContext<BaseHub<TClient, TClientCallbacks>>();

    }
}
