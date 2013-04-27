using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MessageParser.Models;

namespace CIS726_Assignment2.Repositories
{
    /// <summary>
    /// This is a generic repository for data. When created, simply give it an implementation of the IStorageContext interface, and it will
    /// use that interface to store the data.
    /// 
    /// 
    /// Using tutorial from: http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T>
        where T : IModel
    {
        /// <summary>
        /// This creates a generic repository.
        /// 
        /// Generally, this is called as follows (using the Course class as an example)
        /// IGenericRepository<Course> courses = new GenericRepository<Course>(new StorageContext<Course>(context))
        /// where context is a CourseDBContext.
        /// 
        /// For Unit Testing, this is usually called as follows:
        /// IGenericRepository<Course> courses = new GenericRepository<Course>(new FakeStorageContext<Course>())
        /// </summary>
        /// <param name="storage">The IStorageContext used to store the data</param>
        public GenericRepository(IStorageContext<T> storage)
        {
            this._entities = storage;
        }

        //This is the storage context where the data will be stored.
        private IStorageContext<T> _entities;
        public IStorageContext<T> Context
        {
            get { return _entities; }
            set { _entities = value; }
        }

        /// <summary>
        /// This will return all the items in the set of the given type T
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {

            IQueryable<T> query = _entities.Set();
            return query;
        }

        /// <summary>
        /// This will look up items in the set based on the given LINQ query
        /// </summary>
        /// <param name="predicate">The LINQ query to filter the data</param>
        /// <returns></returns>
        public IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = _entities.Set().Where(predicate);
            return query;
        }

        /// <summary>
        /// This will find an item by its ID field (must be named ID using the IModel interface)
        /// </summary>
        /// <param name="anid">The ID to search for</param>
        /// <returns></returns>
        public T Find(int anid)
        {
            return _entities.FindByID(anid);
        }

        /// <summary>
        /// This adds and entity to the storage containter
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// This removes an entity from the storage container
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// This will mark an entry in the storage container as "edited"
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Edit(T entity)
        {
            _entities.Edit(entity);
        }

        /// <summary>
        /// This will tell the storage container to sync all changes and save them
        /// </summary>
        public virtual void SaveChanges()
        {
            _entities.SaveChanges();
        }

        /// <summary>
        /// This will update the values of the first entity item to match those of the second item.
        /// 
        /// The implementation details depends on the storage context
        /// </summary>
        /// <param name="entity">The entity item to be updated</param>
        /// <param name="item">An item containing the new/updated values to use</param>
        public virtual void UpdateValues(T entity, T item)
        {
            _entities.UpdateValues(entity, item);
        }

        /// <summary>
        /// This will dispose the storage context
        /// </summary>
        public virtual void Dispose()
        {
            _entities.Dispose();
        }
    }
}