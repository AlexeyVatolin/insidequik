using System;
using System.Data.Entity;
using System.Linq;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Hubs.Exceptions;

namespace ServerCore.Extexntions
{
    public static class UserExtension
    {
        public static User GetUserLoggedIn(this IDbSet<User> users, string login, string password)
        {
            //var hash = new PasswordHasher();
            var user = users.FirstOrDefault(a => a.UserName.Equals(login, StringComparison.InvariantCultureIgnoreCase)
                                                 && a.PasswordHash == password);
            if (user == null)
            {
                throw new WrongUsernameOrPasswordException();
            }
            return user;
        }

        public static void CheckBotStatus(this User user)
        {
            if (user.UserStatus != UserStatus.Active)
            {
                throw new UserIsNotActiveException();
            }
        }
    }
}