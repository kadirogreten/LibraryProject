using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
    public class BookRezervation : BaseEntity
    {
        public int BookId { get; set; }
        public string UserId { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public Book Book { get; set; }
        [ForeignKey("UserId")]
        public LibraryUser LibraryUser { get; set; }
    }
}
