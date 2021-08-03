using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.ServiceLayer.Interfaces
{
    public interface IBookRezervationRepository : IRepository<BookRezervation>
    {
        IEnumerable<BookRezervation> GetBookRezervations(Expression<Func<BookRezervation, bool>> predicate);
    }
}
