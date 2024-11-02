using System;
using FiorellaTask.Models;

namespace FiorellaTask.ViewModels
{
	public class HomeVM
	{
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductCategories> ProductCategories { get; set; }
        public List<ProductImages> ProductImages { get; set; }
        public List<Slider> Sliders { get; set; }
        public SliderWords SliderWords { get; set; }
    }
}

