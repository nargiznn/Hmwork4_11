using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorellaTask.Models
{
	public class ProductImages:BaseEntity
	{
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ProductPhoto { get; set; } 
        public bool IsMain { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }

    }
}

