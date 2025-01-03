using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Data;
using ProductCatalogApi.DTOs.Product;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Repository
{
 
   
    public class Productrepository : IProductRepository
    {
        private readonly  ApplicationDbContext _context ;
        public Productrepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> CreateAsync(Product productModel)
        {
            await _context.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;

        }

        public async Task<Product?> DeleteAsync(int id)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct == null)
            {
            return null ;
            }
            _context.Remove(existingProduct);
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<List<Product>> GetAllAsync()
        {
           return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
           return await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
           
        }

        public async Task<Product?> UpdateAsync(int id, CreateProduct productDto)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if(existingProduct == null)
            {
                return null ;
            }
            existingProduct.ProductName = productDto.ProductName;
            existingProduct.ProductDescription = productDto.ProductDescription;
            existingProduct.ProductPrice = productDto.ProductPrice;
            existingProduct.ProductStatus = productDto.ProductStatus;
            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}