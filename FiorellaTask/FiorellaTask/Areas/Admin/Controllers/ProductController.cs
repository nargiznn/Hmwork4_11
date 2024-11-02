using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiorellaTask.Data;
using FiorellaTask.Models;
using FiorellaTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FiorellaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = await _context.Products
                .OrderByDescending(m => m.Id)
                .Include(m => m.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(m => m.Images) 
                .ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var product = await _context.Products
                .Include(m => m.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(m => m.Images)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (product is null) return NotFound();
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product request, List<int> selectedCategories, List<IFormFile> productPhotos)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View();
            }


            if (productPhotos == null || !productPhotos.Any())
            {
                ModelState.AddModelError("Photos", "Please upload at least one image.");
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View();
            }


            List<ProductImages> images = new List<ProductImages>();
            foreach (var photo in productPhotos)
            {
                if (photo != null && photo.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                    using (FileStream stream = new(path, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    images.Add(new ProductImages
                    {
                        Image = fileName,
                        IsMain = images.Count == 0  
                    });
                }
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Images = images,
                ProductCategories = selectedCategories.Select(categoryId => new ProductCategories
                {
                    CategoryId = categoryId
                }).ToList()
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            foreach (var image in product.Images)
            {
                string imagePath = Path.Combine(_env.WebRootPath, "assets/img", image.Image);
                DeleteFile(imagePath);
            }

            _context.ProductCategories.RemoveRange(product.ProductCategories);
            _context.ProductImages.RemoveRange(product.Images);
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var product = await _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product is null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync(); 
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Product request, List<IFormFile> productPhotos, List<int> selectedCategories, List<string> removeImages)
        {
            if (id is null) return BadRequest();
            Product product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product is null) return NotFound();
            product.Name = request.Name;
            product.Price = request.Price;
            if (productPhotos != null && productPhotos.Count > 0)
            {
                foreach (var file in productPhotos)
                {
                    if (file != null && file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                        using (FileStream stream = new(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var productImage = new ProductImages
                        {
                            Image = fileName,
                            IsMain = false,
                            ProductId = product.Id
                        };
                        product.Images.Add(productImage); 
                    }
                }
            }

            if (removeImages != null && removeImages.Count > 0)
            {
                foreach (var imageToRemove in removeImages)
                {
                    var productImage = product.Images.FirstOrDefault(img => img.Image == imageToRemove);
                    if (productImage != null)
                    {
                        string imagePath = Path.Combine(_env.WebRootPath, "assets/img", productImage.Image);
                        DeleteFile(imagePath);
                        product.Images.Remove(productImage);
                    }
                }
            }
            product.ProductCategories = selectedCategories.Select(categoryId => new ProductCategories
            {
                CategoryId = categoryId,
                ProductId = product.Id
            }).ToList();

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

