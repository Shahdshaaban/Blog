using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
        public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Post type is required")]
        public string? PostType { get; set; }
               
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

         [Required(ErrorMessage = "CreateDate is required")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; } 
        [Display(Name = "Upload Image")]
        public string? ImagePath { get; set; }

        
        [Required]
    public int AuthorID { get; set; }
    public Author? Author { get; set; }

    [Required]
    public int DepartmentID { get; set; }
    public Department? Department { get; set; }
    }
}
