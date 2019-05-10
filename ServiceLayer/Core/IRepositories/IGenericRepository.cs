using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViewModel.General;

namespace ServiceLayer.Core.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        #region select
        T Single(Expression<Func<T, bool>> predicates);
        Task<T> SingleAsync(Expression<Func<T, bool>> predicates);
        bool SingleAny(Expression<Func<T, bool>> predicates);
        Task<bool> SingleAnyAsync(Expression<Func<T, bool>> predicates);
        T SingleById(Guid id);
        Task<T> SingleByIdAsync(Guid id);
        long GetCount();
        long GetCount(Expression<Func<T, bool>> predicates);
        Task<long> GetCountAsync();
        Task<long> GetCountAsync(Expression<Func<T, bool>> predicates);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicates);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicates);
        #endregion
        #region add
        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        #endregion
        #region delete
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entitys);
        #endregion
        #region update
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entitys);
        #endregion
    }
}
