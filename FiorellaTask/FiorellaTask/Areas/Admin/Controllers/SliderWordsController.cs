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
        private readonly IWebHostEnvironment _env;

        public SliderWordsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SliderWords> sliders = await _context.SliderWords.OrderByDescending(m => m.Id).ToListAsync();
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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderWords request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (request.SliderPhoto == null)
            {
                ModelState.AddModelError("Photo", "Please upload an image.");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + request.SliderPhoto.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await request.SliderPhoto.CopyToAsync(stream);
            }
            var sliderWords = new SliderWords
            {
                Title = request.Title,  
                Desc = request.Desc,   
                Image = fileName        
            };
            await _context.SliderWords.AddAsync(sliderWords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            SliderWords sliderImage = await _context.SliderWords.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderImage is null) return NotFound();

            return View(sliderImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderWords request) 
        {
            if (id is null) return BadRequest();

            SliderWords sliderImage = await _context.SliderWords.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderImage is null) return NotFound();

            if (request.SliderPhoto != null) 
            {
                string existPath = Path.Combine(_env.WebRootPath, "assets/img", sliderImage.Image);
                DeleteFile(existPath);
                string newFileName = Guid.NewGuid().ToString() + "_" + request.SliderPhoto.FileName;
                string newPath = Path.Combine(_env.WebRootPath, "assets/img", newFileName);

                using (FileStream stream = new(newPath, FileMode.Create))
                {
                    await request.SliderPhoto.CopyToAsync(stream);
                }

                sliderImage.Image = newFileName; 
            }

            sliderImage.Title = request.Title; 
            sliderImage.Desc = request.Desc;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var sliderWords = await _context.SliderWords.FindAsync(id);

            if (sliderWords == null)
            {
                return NotFound();
            }
            string existPath = Path.Combine(_env.WebRootPath, "assets/img", sliderWords.Image);
            DeleteFile(existPath);
            _context.SliderWords.Remove(sliderWords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}

