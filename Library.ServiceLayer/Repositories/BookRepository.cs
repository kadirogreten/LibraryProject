using Library.Core.Context;
using Library.Domain.Models;
using Library.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ServiceLayer.Repositories
{
    public class BookRepository : GenericRepository<Book, LibraryDbContext>, IBookRepository
    {

        public BookRepository(LibraryDbContext context) : base(context)
        {

        }
        public LibraryDbContext context { get { return _context as LibraryDbContext; } }
    }
}
