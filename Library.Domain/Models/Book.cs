using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
    public class Book : BaseEntity
    {
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(128, ErrorMessage = "Maksimum 128 karakter minimum 2", MinimumLength = 2)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(128, ErrorMessage = "Yazar 128 karakter minimum 2", MinimumLength = 2)]
        public string Author { get; set; }
        public int StockCount { get; set; } = 0;
        public bool IsAvailable { get; set; } = true;

        public ICollection<BookRezervation> Rezervations { get; set; }
    }
}
