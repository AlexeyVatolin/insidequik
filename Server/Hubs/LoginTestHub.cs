using ServerCore.Hubs;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Models;

namespace Server.Hubs
{
    public class LoginTestHub : BaseHub<ClientBase, IHubClientCallbacks> 
    {
        public override object Login(object request)
        {
            return base.Login(request);
        }

        public string SayHello()
        {
            return "Hello";
        }
    }
}
