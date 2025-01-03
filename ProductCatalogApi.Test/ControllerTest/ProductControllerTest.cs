using NUnit.Framework;
using Moq;
using ProductCatalogApi.Repository;
using ProductCatalogApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApi.Models;

using ProductCatalogApi.DTOs.Product;

namespace ProductCatalogApi.Test.ControllerTest
{
    [TestFixture]
   
    public class ProductControllerTest
    {
        private   Mock<IProductRepository> _productRepoMock = null!;
        private  ProductController _ControllerMock = null!;
        [SetUp]
        public  void setUp()
        {
             _productRepoMock = new Mock<IProductRepository>();
             _ControllerMock = new ProductController(_productRepoMock.Object);

        }
        [Test]
        public async Task GetAll_WhenModelStateIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ControllerMock.ModelState.AddModelError("Key", "Error message");

            // Act
            var result = await _ControllerMock.GetAll();

            // Assert
            Assert.That(result ,Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.StatusCode , Is.EqualTo(400));
        }
         [Test]
         public async Task GetAll_WhenNoProductExists_ReturnsNotFound(){

            //Arrange 
           _productRepoMock.Setup(repo => repo.GetAllAsync())
           .ReturnsAsync( new List<Product>());


             //Act
             var result = await _ControllerMock.GetAll();

             //Assert
              Assert.That(result , Is.InstanceOf<NotFoundObjectResult>());
             var notFoundResult = result as NotFoundObjectResult;
             Assert.That(notFoundResult?.Value , Is.EqualTo(" There are currently No products available."));
         }

        [Test]
        public async Task GetAll_WhenProductsExist_ReturnsOkWithProducts(){
        
        //Arrange
              var products = new List<Product>
        {
            new Product { Id = 1, ProductName = "Product 1" },
            new Product { Id = 2, ProductName = "Product 2" }
        };
        _productRepoMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _ControllerMock.GetAll();

        // Assert
        Assert.That(result , Is.InstanceOf<OkObjectResult>()) ;
        var okResult = result as OkObjectResult;
        Assert.That( okResult?.StatusCode , Is.EqualTo(200));
        var returnedProducts = okResult?.Value as List<Product>;
        Assert.That( returnedProducts?.Count , Is.EqualTo(2));
        Assert.That(returnedProducts?[0].ProductName , Is.EqualTo("Product 1"));
        Assert.That( returnedProducts?[1].ProductName ,Is.EqualTo("Product 2"));

        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(0)]
        public async Task GetById_WhenModelStateIsInvalid_ReturnsBadRequest(int id){

            //Arrange
             _ControllerMock.ModelState.AddModelError("Key" , "Error message");
            //Act
            var result = await _ControllerMock.GetById(id);
            //Assert
            Assert.That( result , Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.StatusCode , Is.EqualTo(400));
        }

        [TestCase(1)]
        public async Task GetById_WhenProductNotFound_ReturnsNotFound(int id){

            //Arrange
             _productRepoMock.Setup(repo => repo.GetByIdAsync(id))
             .ReturnsAsync((Product?)null);
            //Act
             var result = await _ControllerMock.GetById(id);
            //Assert 
              Assert.That(result , Is.InstanceOf<NotFoundObjectResult>());
              var notFoundResult = result as NotFoundObjectResult;
              Assert.That(notFoundResult?.Value , Is.EqualTo("No Product Found!"));

        }
               
         [TestCase(2)]
         [TestCase(3)]
         public async Task GetById_WhenProductIsReturnedByID_ReturnsOkWithProduct(int id){
            //Arrange
            var products = new List<Product>
                {
                    new Product { Id = 2, ProductName = "Product2" , ProductDescription = "ProductDescription2" , ProductPrice = 1255 , ProductStatus = "In Stock"},
                    new Product { Id = 3, ProductName = "Product3" , ProductDescription = "ProductDescription3" , ProductPrice = 1499 , ProductStatus = "In Stock"}
                };
                _productRepoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(products.FirstOrDefault(p => p.Id == id));
            //Act
                var result = await _ControllerMock.GetById(id);
            //Assert
               
               Assert.That(result , Is.InstanceOf<OkObjectResult>());
               var Okresult = result as OkObjectResult;
               Assert.That( Okresult?.StatusCode , Is.EqualTo(200));
               var returnedProduct = Okresult?.Value as ProductDto;
               Assert.That(returnedProduct ,Is.Not.Null , "ProductDto should not be null");  
               Assert.That(returnedProduct?.ProductName , Is.EqualTo($"Product{id}"));
               Assert.That(returnedProduct?.ProductDescription , Is.EqualTo($"ProductDescription{id}"));
               Assert.That(returnedProduct?.ProductStatus , Is.EqualTo($"In Stock"));

                
         }

        [Test]
        public async Task AddProduct_WhenModelStateIsInvalid_ReturnBadRequest(){
            //Arrange
                 var  product = new CreateProduct  
                {
                   ProductName = "" , ProductDescription = "ProductDescription2" , ProductPrice = 1255 , ProductStatus = "In Stock"
                };
              _ControllerMock.ModelState.AddModelError(nameof(CreateProduct.ProductName) , "The ProductName is required");
            //Act
               var result = await _ControllerMock.AddProduct(product);
            //Assert
            Assert.That(result , Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.StatusCode , Is.EqualTo(400));

            Assert.That(badRequestResult?.Value , Is.InstanceOf<SerializableError>());
            var errors = badRequestResult?.Value as SerializableError;
            Assert.That(errors?.ContainsKey("ProductName") , Is.True);
            Assert.That( (errors["ProductName"] as string[])[0],  Is.EqualTo("The ProductName is required"));

        }
        
        [Test]
        public async Task AddProduct_WhenProductIsAdded_ReturnsOkWithProducts(){

            // Arrange
                    var productDto = new CreateProduct
                    {
                        ProductName = "Sample Product",
                        ProductDescription = "Sample Description",
                        ProductPrice = 100,
                        ProductStatus = "In Stock"
                    };

                    var productModel = new Product
                    {
                        
                        ProductName = productDto.ProductName,
                        ProductDescription = productDto.ProductDescription,
                        ProductPrice = productDto.ProductPrice,
                        ProductStatus = productDto.ProductStatus
                    };

                    _ControllerMock.ModelState.Clear(); 

                    // Mocking the repository to simulate product creation
                    _productRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<Product>()))
                                    .ReturnsAsync(productModel); // Simulated return value

                    //Act
                       var result = await _ControllerMock.AddProduct(productDto);
                    //Assert
                    Assert.That(result , Is.InstanceOf<CreatedAtActionResult>());

                    var createdResult = result as CreatedAtActionResult;
                    // Check HTTP status code
                    Assert.That(createdResult?.StatusCode , Is.EqualTo(201));
                    // Validating route values
                    Assert.That(createdResult?.ActionName ,Is.EqualTo(nameof(_ControllerMock.GetById)));
                   
                    // Validating returned product details
                    Assert.That(createdResult?.Value , Is.InstanceOf<ProductDto>());

                    var returnedProduct = createdResult?.Value as ProductDto;
                    Assert.That(returnedProduct?.ProductName, Is.EqualTo(productDto.ProductName));
                    Assert.That(returnedProduct?.ProductDescription, Is.EqualTo(productDto.ProductDescription));
                    Assert.That(returnedProduct?.ProductPrice, Is.EqualTo(productDto.ProductPrice));
                    Assert.That(returnedProduct?.ProductStatus, Is.EqualTo(productDto.ProductStatus));
        }
        
        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteProduct_WhenModelSateIsInvalid_ReturnsBadRequest(int id){

            //Arrange
            _ControllerMock.ModelState.AddModelError("Key" , "Error message");

            //Act
            var result = await _ControllerMock.DeleteProduct(id);
            //Assert
            Assert.That( result , Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.StatusCode , Is.EqualTo(400));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteProduct_WhenProductNotFound_ReturnsNotFound(int id){

            //Arrange
                var products = new List<Product>
                {
                    new Product { Id = 3, ProductName = "Product2" , ProductDescription = "ProductDescription2" , ProductPrice = 1255 , ProductStatus = "In Stock"},
                    new Product { Id = 4, ProductName = "Product3" , ProductDescription = "ProductDescription3" , ProductPrice = 1499 , ProductStatus = "In Stock"}
                };
                 _productRepoMock.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(products.FirstOrDefault(p => p.Id == id));
            //Act
                  var result = await _ControllerMock.DeleteProduct(id);
            //Assert
                Assert.That(result , Is.InstanceOf<NotFoundObjectResult>());
                var notFoundResult = result as NotFoundObjectResult;
                Assert.That(notFoundResult?.Value , Is.EqualTo("The product you are looking for doesn't resides in the database."));
             

        } 

        [TestCase(2)]
        [TestCase(4)]
        public async Task DeleteProduct_WhenProductisDeleted_ReturnsOk(int productId){

              // Arrange
    
                  _productRepoMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                          .ReturnsAsync(new Product { Id = 1, ProductName = "SampleProduct" }); // Simulates successful deletion

                // Act
                var result = await _ControllerMock.DeleteProduct(productId);

                // Assert
                Assert.That(result, Is.InstanceOf<NoContentResult>(), "Expected NoContentResult for successful deletion.");

                var noContentResult = result as NoContentResult;
                Assert.That(noContentResult?.StatusCode, Is.EqualTo(204), "Status code should be 204 for NoContent.");

                _productRepoMock.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == productId)), Times.Once,
                    "DeleteAsync should be called exactly once with the correct product ID.");
              
        }
       
        [TestCase(1)]
        [TestCase(2)]
        public async Task UpdateProduct_WhenModelIsInValid_ReturnsBadRequest(int id){
            //Arrange
               var  product = new CreateProduct  
                {
                   ProductName = "" , ProductDescription = "Pr" , ProductPrice = 1255 , ProductStatus = "In Stock"
                };
             _ControllerMock.ModelState.AddModelError(nameof(CreateProduct.ProductDescription) , "descrption should be Minimun of 3 charcters.");
             _ControllerMock.ModelState.AddModelError(nameof(CreateProduct.ProductName) , "the ProductName is required");

            //Act
             var result = await _ControllerMock.UpdateProduct(id , product );
            //Assert
            Assert.That(result , Is.InstanceOf<BadRequestObjectResult>());

            var BadResult = result as BadRequestObjectResult;
            Assert.That(BadResult?.StatusCode , Is.EqualTo(400));
             
             Assert.That(BadResult?.Value , Is.InstanceOf<SerializableError>());
            var errors = BadResult?.Value as SerializableError;
            
            Assert.That(errors?.ContainsKey("ProductName") , Is.True);
            Assert.That((errors["ProductName"] as string[])[0],  Is.EqualTo("the ProductName is required"));

            Assert.That(errors.ContainsKey("ProductDescription"), Is.True);
            Assert.That((errors["ProductDescription"] as string[])[0] , Is.EqualTo("descrption should be Minimun of 3 charcters."));
        }
    
        [TestCase(1)]
        [TestCase(3)]
        public async Task UpdateProduct_WhenProductNotFound_ReturnsNotFound(int id){
           
                //Arrange           
                     var productDto = new CreateProduct
                    {
                        ProductName = "Updated Product",
                        ProductDescription = "Updated Description",
                        ProductPrice = 1500,
                        ProductStatus = "In Stock"
                    };

                    // Mock UpdateAsync to return null for a non-existent product
                      _productRepoMock?
                                .Setup(repo => repo.UpdateAsync(It.Is<int>(inputId => inputId == id),
                                        It.Is<CreateProduct>(inputDto =>
                                            inputDto.ProductName == "Updated Product" &&
                                            inputDto.ProductDescription == "Updated Description" &&
                                            inputDto.ProductPrice == 1500 &&
                                            inputDto.ProductStatus == "In Stock")))
                                             .ReturnsAsync(null as Product);

                    // Act
                     var result = await _ControllerMock.UpdateProduct(id, productDto);

                    // Assert
                    // Assert that the result is NotFoundResult
                    Assert.That(result, Is.InstanceOf<NotFoundResult>(), "The result should be a NotFoundResult.");
                    var notFoundResult = result as NotFoundResult;
                    Assert.That(notFoundResult?.StatusCode, Is.EqualTo(404), "The status code for NotFoundResult should be 404.");

                    // Verify UpdateAsync was called exactly once with the correct parameters
                    _productRepoMock?.Verify(repo => repo.UpdateAsync(It.Is<int>(inputId => inputId == id), productDto), Times.Once);

                    // Ensure no other methods on the repository were called
                    _productRepoMock?.VerifyNoOtherCalls();
        }
         
        [TestCase(1)]
        [TestCase(3)]
        public async Task UpdateProduct_WhenProductIsUpdated_ReturnsOkProducts(int id){
            //Arrange 
              var products = new List<Product>
                {
                    new Product { Id = 1, ProductName = "Product1" , ProductDescription = "ProductDescription1" , ProductPrice = 1452 , ProductStatus = "In Stock"},
                    new Product { Id = 2, ProductName = "Product2" , ProductDescription = "ProductDescription2" , ProductPrice = 1265 , ProductStatus = "In Stock"},
                    new Product { Id = 3, ProductName = "Product3" , ProductDescription = "ProductDescription3" , ProductPrice = 1255 , ProductStatus = "In Stock"},
                    new Product { Id = 4, ProductName = "Product4" , ProductDescription = "ProductDescription4" , ProductPrice = 1499 , ProductStatus = "In Stock"}
                };
                var productDto = new CreateProduct
                    {
                        ProductName = "Product" + id,
                        ProductDescription = "Description" + id,
                        ProductPrice = 100,
                        ProductStatus = "Out Of Stock"
                    };

                    var productModel = new Product
                    { 
                        ProductName = productDto.ProductName,
                        ProductDescription = productDto.ProductDescription,
                        ProductPrice = productDto.ProductPrice,
                        ProductStatus = productDto.ProductStatus
                    };
                     _productRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<CreateProduct>()))
                     .ReturnsAsync(productModel);
               
            //Act
                var result = await _ControllerMock.UpdateProduct(id , productDto); 
            //Assert
                Assert.That(result , Is.InstanceOf<OkObjectResult>());
                
                var Okresult = result as OkObjectResult;
                Assert.That( Okresult?.StatusCode , Is.EqualTo(200));
                
                var returnedProduct = Okresult?.Value as Product;
                Assert.That(returnedProduct , Is.Not.Null , "Product Should Not be Null");
                Assert.That(returnedProduct?.ProductName , Is.EqualTo($"Product{id}"));
                Assert.That(returnedProduct?.ProductDescription , Is.EqualTo($"Description{id}"));
                Assert.That(returnedProduct?.ProductPrice , Is.EqualTo(100));
                Assert.That(returnedProduct?.ProductStatus , Is.EqualTo("Out Of Stock")); 
        }
    }
}