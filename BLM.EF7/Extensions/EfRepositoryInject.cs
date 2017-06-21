using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BLM.EF7.Extensions
{
    public static class EfRepositoryInject
    {
        public static void AddAllBLMRepositories<TDbContext>(this IServiceCollection s, bool disposeDbContextOnDispose = true) where TDbContext : DbContext
        {
            var ctxType = typeof(TDbContext);
            var efType = typeof(EfRepository<>);
            var props = ctxType.GetProperties();
            Parallel.ForEach(props, prop =>
            {
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                {
                    var args = prop.PropertyType.GetGenericArguments()[0];
                    var genericType = efType.MakeGenericType(args);

                    s.AddScoped(genericType, f =>
                    {
                        var dbContext = f.GetService<TDbContext>();
                        return Activator.CreateInstance(genericType, dbContext, disposeDbContextOnDispose);
                    });
                }
            });
        }
    }
}
