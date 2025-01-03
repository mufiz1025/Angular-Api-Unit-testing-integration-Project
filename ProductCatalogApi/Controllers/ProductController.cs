using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.Data;
using ProductCatalogApi.DTOs.Product;
using ProductCatalogApi.Mappers.product;
using ProductCatalogApi.Models;
using ProductCatalogApi.Repository;

namespace ProductCatalogApi.Controllers
{

    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
      private readonly IProductRepository _ProductRepo;
       public ProductController(IProductRepository ProductRepo)
       {
          _ProductRepo = ProductRepo;
       }

       [HttpGet]
       public async  Task<IActionResult> GetAll()
      {
         if(!ModelState.IsValid)
                return BadRequest(ModelState);
        var products = await _ProductRepo.GetAllAsync();
 
        if (products == null || !products.Any())
        {
          return NotFound(" There are currently No products available.");
        }
         var ProductDto = products.Select(s => s.ToProductDto()).ToList();
        return Ok(products);
        
      }
      [HttpGet("{id:int}")]
      public async Task<IActionResult> GetById([FromRoute] int id)
      {
           if(!ModelState.IsValid)
                return BadRequest(ModelState);
          
          var product = await _ProductRepo.GetByIdAsync(id);
          if (product == null)
          {
            return NotFound("No Product Found!");
          }

          return Ok(product.ToProductDto());
      }
      [HttpPost]
      public async Task<IActionResult> AddProduct([FromBody] CreateProduct ProductDto )
      {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
          
           var productModel = ProductDto.ToProductDtofromCreateProductDto();
           await _ProductRepo.CreateAsync(productModel);

           return CreatedAtAction(nameof(GetById), new { id = productModel.Id} , productModel.ToProductDto());

      }
      [HttpPut]
      [Route("{id:int}")]
    public async Task<IActionResult> UpdateProduct([FromRoute]int id , [FromBody] CreateProduct ProductDto)
    {
       if(!ModelState.IsValid)
                return BadRequest(ModelState);
          
          var productModel =await _ProductRepo.UpdateAsync(id , ProductDto);
          if (productModel == null)
          {
            return NotFound();
          }
       return Ok(productModel);
    }
      
      [HttpDelete]

      public async Task<IActionResult> DeleteProduct(int id)
      {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
         var product = await _ProductRepo.DeleteAsync(id);
         if (product == null)
         {
          return NotFound("The product you are looking for doesn't resides in the database.");
         }
         return NoContent();  
      }

    }
}