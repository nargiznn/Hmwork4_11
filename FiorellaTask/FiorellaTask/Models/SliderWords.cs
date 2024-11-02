using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorellaTask.Models
{
	public class SliderWords:BaseEntity
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile SliderPhoto { get; set; }
    }
}

