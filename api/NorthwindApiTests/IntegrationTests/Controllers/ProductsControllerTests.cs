using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NorthwindApi;
using NorthwindApi.Models;
using Xunit;

namespace NorthwindApiTests.IntegrationTests.Controllers
{
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private readonly ProductDto _testProductDto = new()
        {
            SupplierId = 1,
            CategoryId = 1,
            ProductName = "Test product",
            QuantityPerUnit = "each",
            UnitPrice = 1,
            UnitsInStock = 1
        };

        public ProductsControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsListOfProducts()
        {
            var response = await _client.GetAsync("/api/products");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            products.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsProduct()
        {
            var response = await _client.GetAsync("/api/products/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var product = await response.Content.ReadFromJsonAsync<Product>();
            product.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.GetAsync("/api/products/999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_WithValidInput_ReturnsCreatedProduct()
        {
            (HttpResponseMessage Response, Product Product) result = await CreateTestProduct(_testProductDto);
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);
            await DeleteTestProduct(result.Product.ProductId);

            result.Response.Headers.Location.PathAndQuery.Should().Be($"/api/products/{result.Product.ProductId}");
            result.Product.SupplierId.Should().Be(_testProductDto.SupplierId);
            result.Product.CategoryId.Should().Be(_testProductDto.CategoryId);
            result.Product.ProductName.Should().Be(_testProductDto.ProductName);
            result.Product.QuantityPerUnit.Should().Be(_testProductDto.QuantityPerUnit);
            result.Product.UnitPrice.Should().Be(_testProductDto.UnitPrice);
            result.Product.UnitsInStock.Should().Be(_testProductDto.UnitsInStock);
            result.Product.UnitsOnOrder.Should().Be(_testProductDto.UnitsOnOrder);
            result.Product.ReorderLevel.Should().Be(_testProductDto.ReorderLevel);
            result.Product.Discontinued.Should().Be(_testProductDto.Discontinued);
        }

        [Fact]
        public async Task Create_WithInvalidInput_ReturnsBadRequest()
        {
            var productDto = _testProductDto.Clone();
            productDto.ProductName = null;

            (HttpResponseMessage Response, Product Product) createResult = await CreateTestProduct(productDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_WithValidIdAndInput_ReturnsUpdatedProduct()
        {
            var productDto = _testProductDto.Clone();

            (HttpResponseMessage Response, Product Product) createResult = await CreateTestProduct(productDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            productDto.ProductName = "Updated product";
            productDto.UnitsInStock = 1;

            var response = await _client.PutAsJsonAsync($"/api/products/{createResult.Product.ProductId}", productDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var product = await response.Content.ReadFromJsonAsync<Product>();
            await DeleteTestProduct(product.ProductId);

            product.ProductId.Should().Be(createResult.Product.ProductId);
            product.SupplierId.Should().Be(productDto.SupplierId);
            product.CategoryId.Should().Be(productDto.CategoryId);
            product.ProductName.Should().Be(productDto.ProductName);
            product.QuantityPerUnit.Should().Be(productDto.QuantityPerUnit);
            product.UnitPrice.Should().Be(productDto.UnitPrice);
            product.UnitsInStock.Should().Be(productDto.UnitsInStock);
            product.UnitsOnOrder.Should().Be(productDto.UnitsOnOrder);
            product.ReorderLevel.Should().Be(productDto.ReorderLevel);
            product.Discontinued.Should().Be(productDto.Discontinued);
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.PutAsJsonAsync("/api/products/999", _testProductDto);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_WithInvalidInput_ReturnsBadRequest()
        {
            var productDto = _testProductDto.Clone();
            productDto.ProductName = null;

            var response = await _client.PutAsJsonAsync("/api/products/999", productDto);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsDeletedProduct()
        {
            var productDto = _testProductDto.Clone();

            (HttpResponseMessage Response, Product Product) createResult = await CreateTestProduct(productDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await DeleteTestProduct(createResult.Product.ProductId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var product = await response.Content.ReadFromJsonAsync<Product>();
            product.ProductId.Should().Be(createResult.Product.ProductId);
            product.SupplierId.Should().Be(productDto.SupplierId);
            product.CategoryId.Should().Be(productDto.CategoryId);
            product.ProductName.Should().Be(productDto.ProductName);
            product.QuantityPerUnit.Should().Be(productDto.QuantityPerUnit);
            product.UnitPrice.Should().Be(productDto.UnitPrice);
            product.UnitsInStock.Should().Be(productDto.UnitsInStock);
            product.UnitsOnOrder.Should().Be(productDto.UnitsOnOrder);
            product.ReorderLevel.Should().Be(productDto.ReorderLevel);
            product.Discontinued.Should().Be(productDto.Discontinued);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.DeleteAsync("/api/products/999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<(HttpResponseMessage, Product)> CreateTestProduct(ProductDto productDto)
        {
            var response = await _client.PostAsJsonAsync("/api/products", productDto);
            Product product = null;

            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadFromJsonAsync<Product>();
            }
            return (response, product);
        }

        private async Task<HttpResponseMessage> DeleteTestProduct(int productId)
        {
            var response = await _client.DeleteAsync($"/api/products/{productId}");
            return response;
        }
    }
}
