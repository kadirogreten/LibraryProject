using Library.Core.Context;
using Library.Domain.Models;
using Library.ServiceLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Library.ServiceLayer.Repositories
{
    public class BookRezervationRepository : GenericRepository<BookRezervation, LibraryDbContext>, IBookRezervationRepository
    {

        public BookRezervationRepository(LibraryDbContext context) : base(context)
        {

        }
        public LibraryDbContext context { get { return _context as LibraryDbContext; } }

        public IEnumerable<BookRezervation> GetBookRezervations(Expression<Func<BookRezervation, bool>> predicate)
        {
            return context.BookRezervation.Where(predicate).Include(a => a.Book).Include(a => a.LibraryUser).ToList();
        }
    }
}
