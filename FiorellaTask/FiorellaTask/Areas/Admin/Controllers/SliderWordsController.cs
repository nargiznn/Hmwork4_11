using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorellaTask.Data;
using FiorellaTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FiorellaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderWordsController : Controller
    {
        private readonly AppDbContext _context;
        public SliderWordsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SliderWords sliders = await _context.SliderWords.FirstOrDefaultAsync();
            return View(sliders);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            SliderWords sliderImage = await _context.SliderWords.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderImage is null) return NotFound();

            return View(sliderImage);
        }
    }
}

