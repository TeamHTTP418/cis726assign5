using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MessageParser.Models;
using System.Data.Entity;
using System.Data;

namespace MessageParser.Repositories
{
    /// <summary>
    /// This is a concrete implementation of the IStorageContext interface that will actually work
    /// with a given DbContext class (such as CourseDBContext).
    /// 
    /// Using tutorial from: http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle
    /// </summary>
    /// <typeparam name="T">The type of class expected to be stored in this container</typeparam>
    public class StorageContext<T> : IStorageContext<T> where T : IModel
    {
        /// <summary>
        /// The DbContext storing the data.
        /// </summary>
        DbContext context;

        /// <summary>
        /// This will construct a storage context to store data using the given database. 
        /// If you have multiple StorageContexts (and multiple GenericRepositories) in use
        /// in the same controller, they SHOULD all use the same instance of a DbContext to 
        /// make sure the relationships are enforced.
        /// 
        /// </summary>
        /// <param name="a_context">The DbContext to use (Usually CourseDBContext)</param>
        public StorageContext(DbContext a_context)
        {
            context = a_context;
        }

        /// <summary>
        /// This returns the set of all items in the database of the given type T
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Set()
        {
            return context.Set<T>();
        }

        /// <summary>
        /// This will find the item of type T with the given ID field (must be a child of IModel)
        /// </summary>
        /// <param name="anid">The ID to search for</param>
        /// <returns></returns>
        public T FindByID(int anid)
        {
            IQueryable<T> list = context.Set<T>().Where(c => c.ID == anid);
            if(list.Count() > 0){
                return list.First();
            }else{
                return null;
            }
        }

        /// <summary>
        /// This will add an entity to the database
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.Entry(entity).State = System.Data.EntityState.Added;
        }

        /// <summary>
        /// This will remove an entity from the database
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// This will set the state of the entity to "modified" so it will be saved at the 
        /// next call to "SaveChanges"
        /// </summary>
        /// <param name="entity"></param>
        public void Edit(T entity)
        {
            context.Entry(entity).State = System.Data.EntityState.Modified;
        }
        
        /// <summary>
        /// This will save the changes that have been made to the database
        /// </summary>
        public void SaveChanges()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// This will update the values of the first entity to match those of the second. In this 
        /// implementation, it does that directly in the database
        /// </summary>
        /// <param name="entity">The item to be updated</param>
        /// <param name="input">The values it should be updated to</param>
        public void UpdateValues(T entity, T input)
        {
            T attached = this.FindByID(entity.ID);
            context.Entry(attached).CurrentValues.SetValues(input);
        }

        /// <summary>
        /// This simply disposes of the context.
        /// </summary>
        public void Dispose()
        {
            context.Dispose();
        }
  
    }
}