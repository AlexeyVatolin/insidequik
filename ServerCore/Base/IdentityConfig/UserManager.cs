using System;
using System.Runtime.InteropServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ServerCore.Base.DatabaseContext.Entities;

namespace ServerCore.Base.IdentityConfig
{
    public class InsideUserManager : UserManager<User>
    {
        public InsideUserManager(IUserStore<User> store) : base(store)
        {
            this.PasswordHasher = new PasswordHasherThatDoingNothing();
        }

        public InsideUserStore UserStore => (InsideUserStore)Store;

        public static InsideUserManager Create(IdentityFactoryOptions<InsideUserManager> options, IOwinContext context)
        {
            var manager = new InsideUserManager(new InsideUserStore(context));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true

            };

            // Configure validation logic for passwords
            // TODO: configure validation use Web.config
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            //Replaces PasswordHasher to store hashes of passwords which are received from customers
            manager.PasswordHasher = new PasswordHasherThatDoingNothing();

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

    }
}
