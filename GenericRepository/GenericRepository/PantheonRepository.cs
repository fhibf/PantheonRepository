using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pantheon
{
    public class PantheonRepository<TEntity, TDbContext> : IRepository<TEntity> 
            where TEntity : class
            where TDbContext : class
    {
        private enum RepositoryAction
        {
            None,
            Updating,
            Inserting,
            Deleting,
            Searching
        }

        private RepositoryAction _currentAction;

        /// <summary>
        /// Create new context do manipulate data.
        /// </summary>
        /// <returns>Return new instance of the context.</returns>
        private DbContext CreateContext()
        {
            return (DbContext)Activator.CreateInstance(typeof(TDbContext));
        }

        /// <summary>
        /// Save data into database.
        /// </summary>
        /// <param name="entity">Entity to save.</param>
        /// <returns>Entity saved.</returns>
        public virtual TEntity Save(TEntity entity)
        {
            return Save(entity, false);
        }

        /// <summary>
        /// Save data into database.
        /// </summary>
        /// <param name="entities">Entities to save.</param>
        /// <returns>Entity saved.</returns>
        public virtual TEntity Save(IEnumerable<TEntity> entities)
        {
            return Save(entities, false).FirstOrDefault();
        }

        /// <summary>
        /// Save data into database.
        /// </summary>
        /// <param name="entity">Entity to save.</param>c
        /// <param name="saveNestedProperties">Indicated repopsitory needs update nested properties</param>
        /// <returns>Entity saved.</returns>
        public virtual TEntity Save(TEntity entity, bool saveNestedProperties)
        {
            return Save(new TEntity[] { entity }, saveNestedProperties).FirstOrDefault();
        }

        /// <summary>
        /// Save data into database.
        /// </summary>
        /// <param name="entities">Entities to save.</param>c
        /// <param name="saveNestedProperties">Indicated repopsitory needs update nested properties</param>
        /// <returns>Entity saved.</returns>
        public virtual IEnumerable<TEntity> Save(IEnumerable<TEntity> entities, bool saveNestedProperties)
        {
            using (var context = CreateContext())
            {
                foreach (var entity in entities)
                {
                    // Identity primary key value.
                    int primaryKeyValue = IdentityPrimaryKeyValue(entity, context);

                    // Identify current action
                    this._currentAction = primaryKeyValue == 0 ? RepositoryAction.Inserting : RepositoryAction.Updating;

                    // Save entity to database.
                    SaveEntity(entity, context, primaryKeyValue);

                    // Update nested properties
                    UpdateNestedProperties(entity, context, saveNestedProperties);
                }

                // Commit changes to database
                context.SaveChanges();
            }

            return entities;
        }

        private void UpdateNestedProperties(TEntity entity, DbContext context, bool saveNestedProperties)
        {
            Type typeGeneric = entity.GetType();

            foreach (var property in typeGeneric.GetProperties())
            {
                if (IsEntityProperty(property))
                {
                    // Como identificar se uma entidade é um tipo complexo...





                    if (IsArray(entity, property))
                    {
                        IEnumerable<object> itensOfArray = (IEnumerable<object>)property.GetValue(entity);

                        if (itensOfArray != null)
                        {
                            foreach (var itemOfArray in itensOfArray)
                            {
                                SetStateOfEntity(itemOfArray, context, saveNestedProperties, entity);
                            }
                        }
                    }
                    else
                    {
                        var nestedPropertyToUpdate = property.GetValue(entity);

                        if (nestedPropertyToUpdate != null)
                        {
                            SetStateOfEntity(nestedPropertyToUpdate, context, saveNestedProperties, entity);
                        }
                    }
                }
            }
        }

        private bool IsArray(object entity, PropertyInfo property)
        {
            bool returnValue = false;

            returnValue = (property.PropertyType.IsArray ||
                           property.GetValue(entity, null) is IEnumerable<object>)
                           && property.PropertyType != typeof(byte[]);

            return returnValue;
        }

        private bool IsEntityProperty(PropertyInfo property)
        {
            return property.PropertyType.IsClass && 
                   property.PropertyType != typeof(string) && 
                   property.PropertyType != typeof(byte[]);
        }

        private void SetNestedPropertiesStatus(object entity, DbContext context, EntityState state, object parent)
        {
            Type typeGeneric = entity.GetType();

            foreach (var property in typeGeneric.GetProperties())
            {
                if (IsEntityProperty(property))
                {
                    if (property.PropertyType.IsArray || property.GetValue(entity, null) is IEnumerable<object>)
                    {
                        IEnumerable<object> itensOfArray = (IEnumerable<object>)property.GetValue(entity, null);

                        if (itensOfArray != null)
                        {
                            foreach (object itemOfArray in itensOfArray)
                            {
                                int nestedPropertyPrimaryKeyValue = IdentityPrimaryKeyValue(itemOfArray, context);

                                if (itemOfArray != parent)
                                {
                                    var currentStatus = context.Entry(itemOfArray).State;

                                    if (!(nestedPropertyPrimaryKeyValue == 0 && currentStatus == EntityState.Added))
                                    {
                                        context.Entry(itemOfArray).State = state;
                                        SetNestedPropertiesStatus(itemOfArray, context, state, entity);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var nestedPropertyToUpdate = property.GetValue(entity, null);

                        if (nestedPropertyToUpdate != null)
                        {
                            if (nestedPropertyToUpdate != parent)
                            {
                                context.Entry(nestedPropertyToUpdate).State = state;

                                SetNestedPropertiesStatus(nestedPropertyToUpdate, context, state, entity);
                            }
                        }
                    }
                }
            }
        }
        
        private void SetStateOfEntity(object entity, DbContext context, bool saveNestedProperties, object parent)
        {
            EntityState targetState = EntityState.Added;

            // Capture ID value from nested property
            int nestedPropertyPrimaryKeyValue = IdentityPrimaryKeyValue(entity, context);

            // Actions during insert
            if (this._currentAction == RepositoryAction.Inserting)
            {
                // Verify if property needs to be added or stay as unchanged
                if (nestedPropertyPrimaryKeyValue == 0)
                    targetState = saveNestedProperties ? EntityState.Added : EntityState.Unchanged;
                else
                    targetState = EntityState.Unchanged;
            }
            // Actions during update
            else if (this._currentAction == RepositoryAction.Updating)
            {
                // If nestedPropertyPrimaryKeyValue equals zero, so this instance must be added
                if (nestedPropertyPrimaryKeyValue == 0)
                    targetState = saveNestedProperties ? EntityState.Added : EntityState.Unchanged;
                else
                    targetState = saveNestedProperties ? EntityState.Modified : EntityState.Unchanged;
            }

            context.Entry(entity).State = targetState;

            if (targetState == EntityState.Unchanged)
                SetNestedPropertiesStatus(entity, context, targetState, parent);
        }

        private void SaveEntity(TEntity entity, DbContext context, int currentPrimaryKeyValue)
        {
            if (currentPrimaryKeyValue == 0)
                context.Set<TEntity>().Add(entity);
            else
                context.Entry(entity).State = EntityState.Modified;
        }

        private int IdentityPrimaryKeyValue(object entity, DbContext context)
        {
            int returnValue = 0;
            var primaryKeyProperties = GetKeyNames(context);

            Type typeGeneric = entity.GetType();

            var primaryKeyProperty = typeGeneric.GetProperties().Where(p => string.Compare(p.Name, primaryKeyProperties[0], true) == 0).FirstOrDefault();

            if (primaryKeyProperty != null)
            {
                returnValue = (int)primaryKeyProperty.GetValue(entity);
            }

            return returnValue;
        }

        private string[] GetKeyNames(DbContext context)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            ObjectSet<TEntity> set = objectContext.CreateObjectSet<TEntity>();
            IEnumerable<string> keyNames = set.EntitySet.ElementType
                                                        .KeyMembers
                                                        .Select(k => k.Name);

            return keyNames.ToArray();
        }

        public void Delete(TEntity entity)
        {
            this._currentAction = RepositoryAction.Deleting;

            using (var context = CreateContext())
            {
                context.Set<TEntity>().Attach(entity);
                context.Set<TEntity>().Remove(entity);

                context.SaveChanges();
            }
        }

        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            this._currentAction = RepositoryAction.Searching;

            return Query(query, null);
        }

        public IEnumerable<TEntity> GetAll(params string[] includeProperties)
        {
            this._currentAction = RepositoryAction.Searching;

            IEnumerable<TEntity> result = null;

            // Create database context
            using (var context = CreateContext())
            {
                // Create instance of DbSet
                var dbSet = context.Set<TEntity>();

                DbQuery<TEntity> currentQuery = null;

                // Verify if the user wants to use includes
                if (includeProperties != null && includeProperties.Count() > 0)
                {
                    foreach (string propertyInclude in includeProperties)
                    {
                        // Add includes to query...
                        if (currentQuery == null)
                            currentQuery = dbSet.Include(propertyInclude);
                        else
                            currentQuery.Include(propertyInclude);
                    }

                    result = currentQuery;
                }
                else
                {
                    result = dbSet;
                }

                result = result.ToList();
            }

            return result;
        }

        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> query, params string[] includeProperties)
        {
            this._currentAction = RepositoryAction.Searching;

            IEnumerable<TEntity> result = null;

            // Create database context
            using (var context = CreateContext())
            {
                // Create instance of DbSet
                var dbSet = context.Set<TEntity>();

                DbQuery<TEntity> currentQuery = null;

                // Verify if the user wants to use includes
                if (includeProperties != null && includeProperties.Count() > 0)
                {
                    foreach (string propertyInclude in includeProperties)
                    {
                        // Add includes to query...
                        if (currentQuery == null)
                            currentQuery = dbSet.Include(propertyInclude);
                        else
                            currentQuery.Include(propertyInclude);
                    }
                }

                // Execute query
                if (currentQuery == null)
                    result = dbSet.Where(query);
                else
                    result = currentQuery.Where(query);

                result = result.ToList();
            }

            return result;
        }
    }
}
