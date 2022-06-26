using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GE.Warehouse.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T>
    {
        T GetById(object id);

        int Count();
        int Count(List<Expression<Func<T, bool>>> expressions);
        int Count(List<Expression<Func<T, bool>>> expressions, string whereClause);

        T FindById(int id);
        T FindById(long id);
        T Find(List<Expression<Func<T, bool>>> expressions);

        ICollection<T> FindAll();
        ICollection<T> FindAll(List<int> list);
        ICollection<T> FindAll(List<long> list);
        ICollection<T> FindAll(string order);
        ICollection<T> FindAll(List<Expression<Func<T, bool>>> expressions);
        ICollection<T> FindAll(List<Expression<Func<T, bool>>> expressions, int limit);
        ICollection<T> FindAll(List<Expression<Func<T, bool>>> expressions, string order);
        ICollection<T> FindAll(List<Expression<Func<T, bool>>> expressions, int limit, string order);
        ICollection<T> Random(List<Expression<Func<T, bool>>> expressions, int limit, string order);
        PagedList<T> Paging(string order, int start, int pageSize);
        PagedList<T> Paging(List<Expression<Func<T, bool>>> expressions, string order, int pageIndex, int pageSize);
        PagedList<T> Paging(List<Expression<Func<T, bool>>> expressions, string whereClause, string order, int pageIndex, int pageSize);
        void Insert(ICollection<T> entities);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
