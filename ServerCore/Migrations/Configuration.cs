using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Base.IdentityConfig;

namespace ServerCore.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Base.DatabaseContext.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Base.DatabaseContext.Context context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new InsideUserManager(new UserStore<User>(context));

            var user = userManager.FindByName("admin");
            var role = roleManager.FindByName("Admin");
            if (role == null)
            {
                role = new IdentityRole("Admin");
                roleManager.Create(role);
            }

            if (user == null)
            {
                var newUser = new User()
                {
                    Email = "admin@inside.com",
                    UserName = "admin",
                    FirstName = "admin",
                    LastName = "admin",
                    Balance = 1000000,
                    EmailConfirmed = true,
                    UserStatus = UserStatus.Active,

                };
                userManager.Create(newUser, "admin");
            }

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
