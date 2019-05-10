using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViewModel.General;

namespace ServiceLayer.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SaniDBEntities Context;
        public GenericRepository(SaniDBEntities context) => Context = context;
        #region select
        public T Single(Expression<Func<T, bool>> predicates) => Context.Set<T>().FirstOrDefault(predicates);
        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicates) => await Context.Set<T>().FirstOrDefaultAsync(predicates);
        public bool SingleAny(Expression<Func<T, bool>> predicates) => Context.Set<T>().Any(predicates);
        public async Task<bool> SingleAnyAsync(Expression<Func<T, bool>> predicates) => await Context.Set<T>().AnyAsync(predicates);
        public long GetCount() => Context.Set<T>().Count();
        public long GetCount(Expression<Func<T, bool>> predicates) => Context.Set<T>().Count(predicates);
        public async Task<long> GetCountAsync() => await Context.Set<T>().CountAsync();
        public async Task<long> GetCountAsync(Expression<Func<T, bool>> predicates) => await Context.Set<T>().CountAsync(predicates);
        public T SingleById(Guid id) => Context.Set<T>().Find(id);
        public async Task<T> SingleByIdAsync(Guid id) => await Context.Set<T>().FindAsync(id);
        public IEnumerable<T> GetAll() => Context.Set<T>().ToList();
        public async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicates) => Context.Set<T>().Where(predicates).ToList();
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicates) => await Context.Set<T>().Where(predicates).ToListAsync();
        #endregion
        #region add
        public void Add(T entity) => Context.Set<T>().Add(entity);
        public void AddRange(IEnumerable<T> entity) => Context.Set<T>().AddRange(entity);
        #endregion
        #region delete
        public void Remove(T entity) => Context.Set<T>().Remove(entity);
        public void RemoveRange(IEnumerable<T> entitys) => Context.Set<T>().RemoveRange(entitys);
        #endregion
        #region update
        public void Update(T entity) => Context.Entry(entity).State = EntityState.Modified;
        public void UpdateRange(IEnumerable<T> entitys) => Context.Entry(entitys).State = EntityState.Modified;
        #endregion
    }
}
