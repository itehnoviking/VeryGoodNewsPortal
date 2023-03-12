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

        public virtual async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }


        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }


        public async virtual Task Dispose()
        {
            await Db.DisposeAsync();
            GC.SuppressFinalize(this);
        }





        public virtual IQueryable<T> Get()
        {
            return DbSet;
        }


        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
             return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async virtual Task<T?> GetByIdWithIncludes(Guid id, params Expression<Func<T, object>>[] includes)
        {
            if (includes.Any())
            {
                return await includes.Aggregate
                    (DbSet.Where(entity => entity.Id.Equals(id)),
                    (current, include) => current.Include(include)).FirstOrDefaultAsync();
            }

            return await GetByIdAsync(id);
        }

        public async virtual Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> searchExpression,
            params Expression<Func<T, object>>[] includes)
        {
            var result = DbSet.Where(searchExpression);

            if (includes.Any())
            {
                result = includes.Aggregate(result, (current, include) =>
                current.Include(include));
            }
            return result;
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


        public virtual void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public virtual async Task RemoveRange(Expression<Func<T, bool>> predicate)
        {
            var entities = DbSet.Where(predicate);

            DbSet.RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }
}