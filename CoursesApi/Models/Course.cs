using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        
        [Required]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "Int")]
        public int StudentAmount { get; set; }

        [Required]
        [Column(TypeName = "Int")]
        public int CategoryId { get; set; }
    }
}
