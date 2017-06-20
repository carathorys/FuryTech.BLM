using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BLM.NetStandard.Interfaces
{

    public interface IRepository
    {
        void SaveChanges(IPrincipal userPrincipal);
        Task SaveChangesAsync(IPrincipal userPrincipal);
    }

    public interface IRepository<T> : IRepository<T, T> where T : class
    {

    }

    public interface IRepository<in TInput, TOutput> : IDisposable, IRepository where TInput : class where TOutput : class
    {
        IQueryable<TOutput> Entities(IPrincipal userPrincipal);
        Task<IQueryable<TOutput>> EntitiesAsync(IPrincipal userPrincipal);
        void Add(IPrincipal userPrincipal, TInput newItem);
        Task AddAsync(IPrincipal userPrincipal, TInput newItem);
        void AddRange(IPrincipal userPrincipal, IEnumerable<TInput> newItems);
        Task AddRangeAsync(IPrincipal userPrincipal, IEnumerable<TInput> newItems);
        void Remove(IPrincipal usr, TInput item);
        Task RemoveAsync(IPrincipal usr, TInput item);
        void RemoveRange(IPrincipal usr, IEnumerable<TInput> items);
        Task RemoveRangeAsync(IPrincipal usr, IEnumerable<TInput> items);
        IRepository<T2> GetChildRepositoryFor<T2>() where T2 : class;
    }

}
