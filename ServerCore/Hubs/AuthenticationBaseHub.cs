using System.Runtime.CompilerServices;
using Common.Models;
using Microsoft.AspNet.SignalR;
using ServerCore.Base.DatabaseContext;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Extexntions;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Storages;

namespace ServerCore.Hubs
{
    public class AuthenticationBaseHub : Hub, IAuthenticationBaseHub
    {
        private string SessionId => Context.Headers["Session-Id"];

        public LoginResponse Login(string username, string password)
        {
            User user;
            using (var context = new Context())
            {
                user = context.Users.GetUserLoggedIn(username, password);
            }

            user.CheckBotStatus();
            string sessionId = SessionStorage.CreateSession(user);
            var response = new LoginResponse
            {
                Balance = user.Balance,
                SessionId = sessionId
            };
            return response;
        }

        public void Logout()
        {
            SessionStorage.FinishSession(SessionId);
        }
    }
}
