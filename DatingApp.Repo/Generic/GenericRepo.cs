using DatingApp.Contracts.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Repo.Generic
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        public GenericRepo(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<T> Add(T entity)
        {
            var savedEntity = _unitOfWork.Context.Set<T>().Add(entity);
            await _unitOfWork.CommitAsync();
            return savedEntity.Entity;
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
           _unitOfWork.Context.Set<T>().AddRange(entities);
           await _unitOfWork.CommitAsync();
        }

        public async Task Delete(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            _unitOfWork.Context.Set<T>().RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }

       

        public async Task<T> Edit(T entity)
        {
            var savedEntity = _unitOfWork.Context.Set<T>().Update(entity);
            await _unitOfWork.CommitAsync();
            return savedEntity.Entity;
        }

        public async Task EditRange(IEnumerable<T> entities)
        {
            _unitOfWork.Context.Set<T>().UpdateRange(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<T>> Find(Expression<Func<T, bool>> predicate)
        {
            var query = await _unitOfWork.Context.Set<T>().Where(predicate).ToListAsync();
            return query;
        }

        public async Task<IQueryable<T>> GetAll()
        {
            IQueryable<T> query = null;
            await Task.Run(() =>
            {
                query = _unitOfWork.Context.Set<T>();
            });
            return query;
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _unitOfWork.Context.Set<T>().FindAsync(id);
            return entity;
        }

        public T ReloadEntity(T entity)
        {
            _unitOfWork.Context.Entry<T>(entity).Reload();
            return entity;
        }

        private bool disposed = false;

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    _unitOfWork.Context.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
