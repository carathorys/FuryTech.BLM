using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLM.EF7.Extensions
{
    public static class Inject
    {
        public static void AddBLMRepositories<TDbContext>(this IServiceCollection services, bool disposeDbContextOnDispose = true)
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

                    services.AddScoped(genericType, f =>
                    {
                        var dbContext = f.GetService<TDbContext>();
                        return Activator.CreateInstance(genericType, dbContext, disposeDbContextOnDispose);
                    });
                }
            });
        }
    }
}

