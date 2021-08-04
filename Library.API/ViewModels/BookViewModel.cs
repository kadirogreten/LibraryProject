using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class BookViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(128, ErrorMessage = "Maksimum 128 karakter minimum 2", MinimumLength = 2)]
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Zorunlu Alan")]
        [StringLength(128, ErrorMessage = "Yazar 128 karakter minimum 2", MinimumLength = 2)]
        public string Author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StockCount { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public bool IsAvailable { get; set; } = true;
    }
}
