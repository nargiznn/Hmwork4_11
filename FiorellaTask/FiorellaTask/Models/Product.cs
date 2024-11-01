using System;
namespace FiorellaTask.Models
{
	public class Product:BaseEntity
	{
        public string Name { get; set; }
        public double Price { get; set; }
        public ICollection<ProductImages> Images { get; set; }
        public ICollection<ProductCategories> ProductCategories { get; set; }
    }
}

