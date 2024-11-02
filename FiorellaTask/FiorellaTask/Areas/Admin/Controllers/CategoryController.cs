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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _context.Categories.OrderByDescending(m => m.Id).ToListAsync();
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            bool hasCategory = await _context.Categories.AnyAsync(m => m.Name.Trim() == category.Name.Trim());
            if (hasCategory)
            {
                ModelState.AddModelError("Name", "Category already exists!");
                return View(category);
            }
            await _context.Categories.AddAsync(new Category { Name = category.Name });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category existProduct = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            _context.Categories.Remove(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Category category)
        {
            if (id is null) return BadRequest();
            Category existCategory = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory is null) return NotFound();
            if (!ModelState.IsValid) return View(category);
            bool hasCategory = await _context.Categories.AnyAsync(m => m.Name.Trim() == category.Name.Trim() && m.Id != id);
            if (hasCategory)
            {
                ModelState.AddModelError("Name", "Category already exists!");
                return View(category);
            }
            existCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

