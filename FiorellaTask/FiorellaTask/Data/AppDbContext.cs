using System;
using FiorellaTask.Models;
using Microsoft.EntityFrameworkCore;

namespace FiorellaTask.Data
{
	public class AppDbContext:DbContext
	{
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }
    }
}

