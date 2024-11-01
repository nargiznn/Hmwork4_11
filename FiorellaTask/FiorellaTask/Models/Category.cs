using System;
using System.ComponentModel.DataAnnotations;

namespace FiorellaTask.Models
{
	public class Category:BaseEntity
	{
        [Required]
        public string Name { get; set; }
        //public ICollection<Course> Courses { get; set; }
    }
}

