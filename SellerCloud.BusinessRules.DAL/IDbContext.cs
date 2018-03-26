using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SellerCloud.BusinessRules.DAL
{
    public interface IDbContext : IDisposable
    {
        DbChangeTracker ChangeTracker { get; }
        Database Database { get; }
        DbContextConfiguration Configuration { get; }
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
