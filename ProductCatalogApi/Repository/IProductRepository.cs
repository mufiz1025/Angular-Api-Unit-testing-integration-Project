using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalogApi.DTOs.Product;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(); 

        Task<Product?> GetByIdAsync(int id);

        Task<Product?> CreateAsync(Product productModel);

        Task<Product?> UpdateAsync(int id , CreateProduct productDto);

        Task<Product?> DeleteAsync(int id);
    }
}