using ServerCore.Base.DatabaseContext.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Base.Interfaces
{
    public interface IContext
    {
        Database Database { get; }
        IDbSet<User> Users { set; get; }
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet Set(Type type);
        int SaveChanges();
    }
}
