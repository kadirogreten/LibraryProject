using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int StockCount { get; set; } = 0;

        public ICollection<BookRezervation> Rezervations { get; set; }
    }
}
