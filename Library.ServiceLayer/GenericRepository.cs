using Library.Core.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Library.ServiceLayer
{
    public abstract class GenericRepository<TEntity, TContext> : IRepository<TEntity> where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _context;

        /// constructor dependency injection
        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {

            return _context.Set<TEntity>().ToList();
        }


        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {

            return _context.Set<TEntity>().Where(predicate).ToList();
        }


        public void Insert(TEntity entity)
        {

            _context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {

            _context.Set<TEntity>().Remove(entity);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }
    }
}
