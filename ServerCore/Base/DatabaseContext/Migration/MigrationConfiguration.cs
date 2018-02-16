using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Base.IdentityConfig;

namespace ServerCore.Base.DatabaseContext.Migration
{
    /// <summary>
    /// Class wich response for migrate
    /// </summary>
    public sealed class MigrationConfiguration : DbMigrationsConfiguration<Context>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context context)
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
                userManager.Create(newUser, "admin123");
            }
            base.Seed(context);
        }
    }
}
