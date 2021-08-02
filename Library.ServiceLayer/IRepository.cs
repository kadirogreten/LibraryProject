using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Library.ServiceLayer
{
    public interface IRepository<TEntity> where TEntity : class
    {
       
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);       
        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);
    }
}
