using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.SignalR;

namespace ServerTest
{
    public class RemoteLogin : Hub
    {
        public string Login(string username, string password)
        {
            string sessionId = Guid.NewGuid().ToString();
            SessionStorage.Add(sessionId, username);
            return sessionId;
            bool result = Membership.ValidateUser(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
        }

        
    }
}
