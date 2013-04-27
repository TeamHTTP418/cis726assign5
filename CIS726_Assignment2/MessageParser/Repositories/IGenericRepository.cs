using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MessageParser.Repositories
{
    /// <summary>
    /// This class acts as an interface for the Repository framework. It is not necessarily needed at this point,
    /// but is a good practice in case it is expanded upon in the future. For right now, the GenericRepository implementation
    /// of this interface is the only one used.
    /// 
    /// Using tutorial from: http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle
    /// Also see http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// </summary>
    /// <typeparam name="T">Class for the repository (usually GenericRepository)</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        T Find(int id);
        void Add(T entity);
        void Remove(T entity);
        void Edit(T entity);
        void UpdateValues(T entity, T item);
        void SaveChanges();
        void Dispose();

        bool RemoveAndSave(T entity);
        int AddAndSave(T entity);
        bool UpdateAndSave(T entity, T item);
        bool EditAndSave(T entity);
    }
}