using FirstMvcApp.Data;
using System.Linq.Expressions;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Core.Data
{
    public interface IRepository<T> where T : BaseEntities
    {
        //Read
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Get();
        
        IQueryable<T> FindBy(Expression<Func<T, bool>> searchExpression,
            params Expression<Func<T, object>>[] includes);

        //Task<T> GetByIdWithIncludes(Guid id, params Expression<Func<T, object>>[] includes);


        //Create
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        
        
        //Update
        void Update(T entity);
        Task PatchAsync(Guid id, List<PatchModel> patchDtos);

        //Delete
        void Remove(T entity);

        //public Task Dispose();
    }
}