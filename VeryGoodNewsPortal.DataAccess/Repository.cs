using FirstMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VeryGoodNewsPortal.Core.Data;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;
using System.Data;

namespace VeryGoodNewsPortal.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseEntities
    {

        protected readonly VeryGoodNewsPortalContext Db;
        protected readonly DbSet<T> DbSet;

        public Repository(VeryGoodNewsPortalContext context)
        {
            Db = context;
            DbSet = Db.Set<T>();
        }

        public async virtual Task Add(T obj)
        {
            await DbSet.AddAsync(obj);
        }


        public async virtual Task AddRange(IEnumerable<T> obj)
        {
            await DbSet.AddRangeAsync(obj);
        }


        public async virtual Task Dispose()
        {
            await Db.DisposeAsync();
            GC.SuppressFinalize(this);
        }


        public async virtual Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includes)
        {
            var result = DbSet.Where(predicate);

            if (includes.Any())
            {
                result = includes.Aggregate(result, (current, include) => current.Include(include));
            }

            return result;
        }


        public virtual IQueryable<T> Get()
        {
            return DbSet;
        }


        public async virtual Task<T> GetById(Guid id)
        {
             return await DbSet.AsNoTracking()
                 .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }


        public async virtual Task<T> GetByIdWithIncludes(Guid id, params Expression<Func<T, object>>[] includes)
        {
            if (includes.Any())
            {
                return await includes.Aggregate
                    (DbSet.Where(entity => entity.Id.Equals(id)),
                    (current, include) => current.Include(include)).FirstOrDefaultAsync();
            }

            return await GetById(id);
        }


        public async virtual Task PatchAsync(Guid id, List<PatchModel> patchDtos)
        {
            var model = await DbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id));

            var nameValuePairProperties = patchDtos
                .ToDictionary(a => a.PropertyName, a => a.PropertyValue);

            var dbEntityEntry = Db.Entry(model);
            dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
            dbEntityEntry.State = EntityState.Modified;
        }


        public async virtual Task Remove(Guid id)
        {
            DbSet.Remove(await DbSet.FindAsync(id));
        }


        public async virtual Task Update(T obj)
        {
            DbSet.Update(obj);
        }
    }
}