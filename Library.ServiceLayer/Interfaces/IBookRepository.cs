using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ServiceLayer.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
    }
}
