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
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.OrderByDescending(m => m.Id).ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Slider sliderImage = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

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
        public async Task<IActionResult> Create(Slider request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (request.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please upload an image.");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + request.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await request.Photo.CopyToAsync(stream);
            }
            var slider = new Slider { Image = fileName };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Slider sliderImage = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderImage is null) return NotFound();

            return View(new Slider { Image = sliderImage.Image, Id = sliderImage.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Slider request)
        {
            if (id is null) return BadRequest();

            Slider sliderImage = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderImage is null) return NotFound();

            if (request.Photo != null)
            {
                string existPath = Path.Combine(_env.WebRootPath, "assets/img", sliderImage.Image);
                DeleteFile(existPath);

                string newFileName = Guid.NewGuid().ToString() + "_" + request.Photo.FileName;
                string newPath = Path.Combine(_env.WebRootPath, "assets/img", newFileName);

                using (FileStream stream = new(newPath, FileMode.Create))
                {
                    await request.Photo.CopyToAsync(stream);
                }

                sliderImage.Image = newFileName;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var sliderImage = await _context.Sliders.FindAsync(id);

            if (sliderImage == null)
            {
                return NotFound();
            }

            string existPath = Path.Combine(_env.WebRootPath, "assets/img", sliderImage.Image);
            DeleteFile(existPath);

            _context.Sliders.Remove(sliderImage);
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