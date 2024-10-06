using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Blog.Models
{
    public class Department
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Department name cannot be longer than 100 characters")]
        public string? DepName { get; set; }

        [Required(ErrorMessage = "CreateDate is required")]
        [Range(2000, 2024, ErrorMessage = "CreateDate must be between the year 2000 and 2024")]
        public string? CreateDate { get; set; }

        public ICollection<Author>? Authors { get; set; }
    }
}

