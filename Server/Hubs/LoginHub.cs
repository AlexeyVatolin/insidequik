using ServerCore.Hubs;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Models;

namespace Server.Hubs
{
    public class LoginHub : BaseHub<ClientBase, IHubClientCallbacks> 
    { }
}
