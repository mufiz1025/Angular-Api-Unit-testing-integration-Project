using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.DTOs.Product
{
    public class ProductDto
    {
      public int Id { get; set; }
      public string ProductName { get; set; } = string.Empty;
      public string? ProductDescription { get; set; }

      public int ProductPrice { get; set; }

      public string ProductStatus { get; set; } = string.Empty;
    }
}