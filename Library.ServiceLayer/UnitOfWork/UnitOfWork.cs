using Library.Core.Context;
using Library.ServiceLayer.Interfaces;
using Library.ServiceLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ServiceLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        
        protected readonly LibraryDbContext _context;


        public UnitOfWork(LibraryDbContext context)
        {

            //all db's
            _context = context;



            LibraryUser = new UserRepository(_context);

            Book = new BookRepository(_context);

            BookRezervation = new BookRezervationRepository(_context);

           
        }


        public IUserRepository LibraryUser { get; private set; }

        public IBookRepository Book { get; private set; }

        public IBookRezervationRepository BookRezervation { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }


        public void Dispose()
        {
            _context.Dispose();

        }
    }
}
