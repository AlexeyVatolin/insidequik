using Common.Models;

namespace ServerCore.Hubs.Interfaces
{
    public interface IAuthenticationBaseHub
    {
        LoginResponse Login(string username, string password);
        void Logout();
    } 
}
