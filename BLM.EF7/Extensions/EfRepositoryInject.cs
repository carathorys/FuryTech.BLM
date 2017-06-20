using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLM.EF7.Extensions
{
    public static class EfRepositoryInject
    {
        public static void AddAllBLMRepositories<TDbContext>(this IServiceCollection s) where TDbContext : DbContext
        {
            var efType = typeof(EfRepository<>);
            var dummyService = new { dummy = true };
            TDbContext dbContext = null;

            foreach (var srv in s)
            {
                if (srv.ImplementationType == typeof(TDbContext))
                {
                    dbContext = (TDbContext)srv.ImplementationInstance;
                    break;
                }
            }

            if (dbContext != null)
            {
                foreach (var t in dbContext.Model.GetEntityTypes())
                {
                    var type = t.ClrType;
                    var genericType = efType.MakeGenericType(type);
                    s.AddTransient(factory => Activator.CreateInstance(genericType, factory.GetService<TDbContext>()));
                }
            }
        }
        public static void AddBLMRepository<TDbContext, TEntity>(this IServiceCollection s)
            where TDbContext : DbContext
            where TEntity : class, new()
        {
            s.AddTransient(typeof(EfRepository<TEntity>), f =>
            {
                var context = f.GetService<TDbContext>();
                return new EfRepository<TEntity>(context);
            });
        }
    }
}
