using System;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Base.DatabaseContext.Migration;
using Inside.Core.Base.DatabaseContext.Entities;
using ServerCore.Base.Interfaces;

namespace ServerCore.Base.DatabaseContext
{
    public class Context : IdentityDbContext<User>, IContext, IDispose
    {
        static Context()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, MigrationConfiguration>());
        }

        public Context() : base("InsideDB")
        {
#if DEBUG
            Database.Log = text => Debug.WriteLine(text);
#endif
        }

        //TODO:посмотреть нужны ли они
        //public DbSet<Event> Events { set; get; }
        public DbSet<OrderHistory> OrderHistories { set; get; }

        /// <summary>
        /// Create instance of database class
        /// </summary>
        /// <returns>New database instance</returns>
        public static Context Create()
        {
            var context = new Context();
            return context;
        }

        public static void Init()
        {
            Create().Database.Initialize(true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IsDisposed = true;
        }
        /// <summary>
        /// Is context now disposed
        /// </summary>
        public bool IsDisposed { get; set; }
    }
}
