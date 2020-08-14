using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
    }
}
