using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using MessageParser.Models;
using CIS726_Assignment2.Repositories;

namespace CIS726_Assignment2.Tests.Fakes
{
    /// <summary>
    /// This is another implementation of the IStorageContext interface that is used
    /// by the Unit Testing framework to store data temporarily. 
    /// 
    /// It simply uses a Generic List to store the data.
    /// 
    /// The concurrency IS NOT GUARANTEED! That means that if you add an item on 
    /// one side of a many-to-many relationship, you have to manually make the link on the 
    /// other side (this WILL NOT do it for you).
    /// 
    /// See the Initialization function in DegreeProgramsControllerTest for a specific example of that.
    /// 
    /// </summary>
    /// <typeparam name="T">The type of item to be stored</typeparam>
    public class FakeStorageContext<T> : IStorageContext<T> where T : IModel
    {
        //This is the storage back-end for this context.
        List<T> storage;

        /// <summary>
        /// This will simply create a new blank list to store the data.
        /// </summary>
        public FakeStorageContext()
        {
            storage = new List<T>();
        }

        /// <summary>
        /// This will return all the items in the list as a queryable object
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Set()
        {
            return storage.AsQueryable<T>();
        }

        /// <summary>
        /// This will find an item in the list using the ID field of IModel. If one is 
        /// not found, it will through an InvalidOperationException to match the
        /// expected functionality of the DbContext class.
        /// </summary>
        /// <param name="anid"></param>
        /// <returns></returns>
        public T FindByID(int anid)
        {
            T now = storage.Find(t => t.ID == anid);
            if (now == null)
            {
                throw new InvalidOperationException();
            }
            return now;
        }

        /// <summary>
        /// This will add an item to the list
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            storage.Add(entity);
        }

        /// <summary>
        /// This will remove an item from the list
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            storage.Remove(entity);
        }

        /// <summary>
        /// This will mark an item as changed. In this implementation, it will
        /// simply replace the matching item in the list with the one given as a parameter
        /// </summary>
        /// <param name="entity"></param>
        public void Edit(T entity)
        {
            int x = storage.FindIndex(item => item.Equals(entity));
            storage[x] = entity;
        }

        /// <summary>
        /// This will update the values of the first given entity to match those
        /// stored in the second. In this implementation, it will simply replace
        /// the first item in the list with the second.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        public void UpdateValues(T entity, T item)
        {
            int x = storage.FindIndex(val => val.Equals(entity));
            storage[x] = item;
        }

        /// <summary>
        /// This will save the changes. In this implementaion, there is nothing to do
        /// here since changes are saved as they happen.
        /// </summary>
        public void SaveChanges()
        {
        }

        /// <summary>
        /// This will dispose of the storage. In this case, it just clears it.
        /// </summary>
        public void Dispose()
        {
            storage.Clear();
        }

    }
}
