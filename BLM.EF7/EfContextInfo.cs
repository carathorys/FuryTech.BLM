using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using BLM.NetStandard;
using BLM.NetStandard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLM.EF7
{
    public class EfContextInfo : IContextInfo
    {

        private readonly DbContext _dbcontext;

        public EfContextInfo(IPrincipal principal, DbContext ctx)
        {
            _dbcontext = ctx;
            Principal = principal;
        }

        public IPrincipal Principal { get; }
        public IQueryable<T> GetFullEntitySet<T>() where T : class
        {
            return _dbcontext.Set<T>();
        }

        public IQueryable<T> GetAuthorizedEntitySet<T>() where T : class
        {
            return Authorize.Collection(_dbcontext.Set<T>(), new EfContextInfo(Principal, _dbcontext));
        }

        public async Task<IQueryable<T>> GetAuthorizedEntitySetAsync<T>() where T : class
        {
            return await Authorize.CollectionAsync(_dbcontext.Set<T>(), new EfContextInfo(Principal, _dbcontext));
        }
    }
}
