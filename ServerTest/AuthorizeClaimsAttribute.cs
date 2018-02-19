using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace ServerTest
{
    public class AuthorizeClaimsAttribute : AuthorizeAttribute
    {
        protected override bool UserAuthorized(System.Security.Principal.IPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return true;
            if (user is ClaimsPrincipal principal)
            {
                Claim authenticated = principal.FindFirst(ClaimTypes.Authentication);
                if (authenticated != null && authenticated.Value == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
