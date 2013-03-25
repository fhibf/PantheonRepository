using System;
namespace GenericRepository
{
    interface IPantheonRepository<TEntity>
     where TEntity : class
    {
        void Delete(TEntity entity);
        System.Collections.Generic.IEnumerable<TEntity> GetAll(params string[] includeProperties);
        System.Collections.Generic.IEnumerable<TEntity> Query(Func<TEntity, bool> query);
        System.Collections.Generic.IEnumerable<TEntity> Query(Func<TEntity, bool> query, params string[] includeProperties);
        TEntity Save(System.Collections.Generic.IEnumerable<TEntity> entities);
        System.Collections.Generic.IEnumerable<TEntity> Save(System.Collections.Generic.IEnumerable<TEntity> entities, bool saveNestedProperties);
        TEntity Save(TEntity entity);
        TEntity Save(TEntity entity, bool saveNestedProperties);
    }
}
