using System;
namespace FiorellaTask.Models
{
	public class ProductImages:BaseEntity
	{
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }

    }
}

