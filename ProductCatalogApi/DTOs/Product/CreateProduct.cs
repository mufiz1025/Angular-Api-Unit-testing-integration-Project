using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.DTOs.Product
{
    public class CreateProduct
    {
        
      [Required]
      public string ProductName { get; set; } = string.Empty;

      [Required]
      [MaxLength(50, ErrorMessage ="descrption should be maximum of 50 charcters.")]
      [MinLength(3 , ErrorMessage = "descrption should be Minimun of 3 charcters.")]
      public string? ProductDescription { get; set; }
      
      public int ProductPrice { get; set; }
       [Required]
      public string ProductStatus { get; set; } = string.Empty;
    }
}