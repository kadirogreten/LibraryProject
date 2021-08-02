using Library.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ServiceLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository LibraryUser { get; }

        int Complete();

    }
}
