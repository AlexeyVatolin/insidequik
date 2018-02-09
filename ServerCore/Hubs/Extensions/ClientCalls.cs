using System.Threading.Tasks;
using Common.Helpers;
using Microsoft.AspNet.SignalR;

namespace ServerCore.Hubs.Extensions
{
    public static class ClientCalls
    {
        //TODO: возможно стоит перенести этот код в BaseHub
        // Client call for logout
        public static Task Logout(this IHubContext self, string id, object message)
        {
            return self == null ? TaskHelper.Empty : self.Clients.Client(id).disconnect(message);
        }
    }
}
