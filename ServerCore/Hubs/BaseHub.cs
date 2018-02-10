using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNet.SignalR;
using ServerCore.Base.DatabaseContext;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Extexntions;
using ServerCore.Helpres;
using ServerCore.Hubs.Exceptions;
using ServerCore.Hubs.Extensions;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Models;

namespace ServerCore.Hubs
{
    public class BaseHub<TClient, TClientCallbacks> : Hub<TClientCallbacks>, IBaseHub
        where TClientCallbacks : class, IHubClientCallbacks
        where TClient : class, IClientBase, new()
    {
        /// <summary>
        /// Sync locker
        /// </summary>
        private static readonly object SyncRoot = new object();
        private static readonly ConnectedClients Clients = new ConnectedClients();

        public static IHubContext CurrentContext => GlobalHost.ConnectionManager.GetHubContext<BaseHub<TClient, TClientCallbacks>>();
      
        /// <summary>
        /// Connection Id of current hub object
        /// </summary>
        private string ConnectionId => Context.ConnectionId;

        private TClient _client;
        private TClient Client => _client ?? (_client = (TClient) Clients.GetClientByConnectionId(ConnectionId));

        protected TClient GetCurrentClient()
        {
            var client = Client;
            if (client == null)
            {
                throw new NotLoggedException();
            }

            if (client.UserStatus != UserStatus.Active)
            {
                Clients.Remove(client);
                
                CurrentContext.Logout(client.ConnectionId, null);
                throw new UserIsNotActiveException();
            }

            client.LastAccessDate = DateTime.UtcNow;
            return client;
        }

        public event Action OnLogin;
        public event Action OnDisconect;

        /// <summary>
        /// Method for login new client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual object Login(object request)
        {
            var loginOptionsRequest = JObjectHelper.GetJObject<LoginOptionsRequest, WrongUsernameOrPasswordException>(request);

            User user;
            using (var context = new Context())
            {
                user = context.Users.GetUserLoggedIn(loginOptionsRequest.Login, loginOptionsRequest.Password);
            }

            user.CheckBotStatus();
            lock (SyncRoot)
            {
                var connectionId = ConnectionId;
                var status = user.UserStatus;

                //todo can bot login multiple?

                var client = new TClient
                {
                    ApplicationUserId = user.Id,
                    ConnectionId = connectionId,
                    Name = loginOptionsRequest.Login,
                    LastAccessDate = DateTime.UtcNow,
                    Ip = this.GetConnectionIp(),
                    UserStatus = status
                };
                Clients.Add(client);
            }
            OnLogin?.Invoke();
            return new LoginResponse
            {
                Balance = user.Balance
            };
        }

        /// <summary>
        /// Client disconnect handler
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            // disconnect when client stoped connection
            // do not process diconnect for temporary connection issues
            if (stopCalled)
            {
                Disconnect(true);
            }
            OnDisconect?.Invoke();
            return base.OnDisconnected(stopCalled);
        }


        /// <summary>
        /// Disconnect handler
        /// </summary>
        /// <param name="isDisconnected">Show is user really disconnected or call end method</param>
        private void Disconnect(bool isDisconnected = false)
        {
            try
            {
                Clients.Disconnect(ConnectionId, isDisconnected);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Logout method
        /// </summary>
        public virtual void Logout()
        {
            Disconnect(true);
        }
    }
}
