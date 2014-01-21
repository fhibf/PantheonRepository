using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pantheon
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entity);
        
        IEnumerable<TEntity> GetAll(params string[] includeProperties);
        
        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> query);

        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> query, params string[] includeProperties);
        
        TEntity Save(IEnumerable<TEntity> entities);
        
        IEnumerable<TEntity> Save(IEnumerable<TEntity> entities, bool saveNestedProperties);
        
        TEntity Save(TEntity entity);

        TEntity Save(TEntity entity, bool saveNestedProperties);
    }
}
