using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ProductCatalogApi.Models
{
      [Table("Products")]
    public class Product
    {   
      public int Id { get; set; }   //primary key
      public string ProductName { get; set; } = string.Empty;
      public string? ProductDescription { get; set; }

      public int ProductPrice { get; set; }

      public string ProductStatus { get; set; } = string.Empty;
    }
}