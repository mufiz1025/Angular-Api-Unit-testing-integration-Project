using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalogApi.DTOs.Product;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Mappers.product
{
    public static  class ProductMapper
    {
        public static ProductDto ToProductDto( this Product ProductDto)
        {
            return new ProductDto{

               ProductName = ProductDto.ProductName,
               Id = ProductDto.Id,
               ProductDescription = ProductDto.ProductDescription,
               ProductPrice =ProductDto.ProductPrice,
               ProductStatus = ProductDto.ProductStatus

            };
        }
        public static Product ToProductDtofromCreateProductDto(this CreateProduct ProductDto)
        {
            return new Product{
               ProductName = ProductDto.ProductName,
               
               ProductDescription = ProductDto.ProductDescription,
             
               ProductPrice =ProductDto.ProductPrice,
               
               ProductStatus = ProductDto.ProductStatus
            };
        }
        
    }
}