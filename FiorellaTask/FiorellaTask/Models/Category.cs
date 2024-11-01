using System;
using System.ComponentModel.DataAnnotations;
using FiorellaTask.Models;

namespace FiorellaTask.Models
{
	public class Category:BaseEntity
	{
        [Required]
        public string Name { get; set; }
        public ICollection<ProductCategories> ProductCategories { get; set; }

        //public ICollection<Course> Courses { get; set; }
    }
}



