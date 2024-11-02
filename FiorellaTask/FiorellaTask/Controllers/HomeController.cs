using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorellaTask.Data;
using FiorellaTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FiorellaTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/

        public async Task<IActionResult> Index()
        {
            return View(new HomeVM
            {
                Categories = await _context.Categories.ToListAsync(),
                Products = await _context.Products.ToListAsync(),
                ProductImages = await _context.ProductImages.ToListAsync(), 
                ProductCategories = await _context.ProductCategories.ToListAsync(),
                Sliders = await _context.Sliders.ToListAsync(),
                SliderWords = await _context.SliderWords.OrderByDescending(sw => sw.Id).FirstOrDefaultAsync()
            });
        }

    }
}

