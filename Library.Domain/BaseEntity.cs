using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.Domain
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public string ModifiedBy { get; set; }

        public byte Status { get; set; } = 0;


        public BaseEntity()
        {
            Status = 0;
        }
    }
}
