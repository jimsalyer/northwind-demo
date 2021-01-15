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
    public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private readonly CategoryDto _testCategoryDto = new()
        {
            CategoryName = "Test",
            Description = "Test category"
        };

        public CategoriesControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsListOfCategories()
        {
            var response = await _client.GetAsync("/api/categories");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var categories = await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
            categories.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsCategory()
        {
            var response = await _client.GetAsync("/api/categories/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var category = await response.Content.ReadFromJsonAsync<Category>();
            category.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.GetAsync("/api/categories/999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_WithValidInput_ReturnsCreatedCategory()
        {
            (HttpResponseMessage Response, Category Category) result = await CreateTestCategory(_testCategoryDto);
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);
            await DeleteTestCategory(result.Category.CategoryId);

            result.Response.Headers.Location.PathAndQuery.Should().Be($"/api/categories/{result.Category.CategoryId}");
            result.Category.CategoryName.Should().Be(_testCategoryDto.CategoryName);
            result.Category.Description.Should().Be(_testCategoryDto.Description);
        }

        [Fact]
        public async Task Create_WithInvalidInput_ReturnsBadRequest()
        {
            var categoryDto = _testCategoryDto.Clone();
            categoryDto.CategoryName = null;

            (HttpResponseMessage Response, Category Category) createResult = await CreateTestCategory(categoryDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_WithValidIdAndInput_ReturnsUpdatedCategory()
        {
            var categoryDto = _testCategoryDto.Clone();

            (HttpResponseMessage Response, Category Category) createResult = await CreateTestCategory(categoryDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            categoryDto.CategoryName = "Updated Test";
            categoryDto.Description = "Updated test category";

            var response = await _client.PutAsJsonAsync($"/api/categories/{createResult.Category.CategoryId}", categoryDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var category = await response.Content.ReadFromJsonAsync<Category>();
            await DeleteTestCategory(category.CategoryId);

            category.CategoryId.Should().Be(createResult.Category.CategoryId);
            category.CategoryName.Should().Be(categoryDto.CategoryName);
            category.Description.Should().Be(categoryDto.Description);
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.PutAsJsonAsync("/api/categories/999", _testCategoryDto);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_WithInvalidInput_ReturnsBadRequest()
        {
            var categoryDto = _testCategoryDto.Clone();
            categoryDto.CategoryName = null;

            var response = await _client.PutAsJsonAsync("/api/categories/999", categoryDto);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsDeletedCategory()
        {
            var categoryDto = _testCategoryDto.Clone();

            (HttpResponseMessage Response, Category Category) createResult = await CreateTestCategory(categoryDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await DeleteTestCategory(createResult.Category.CategoryId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var category = await response.Content.ReadFromJsonAsync<Category>();
            category.CategoryId.Should().Be(createResult.Category.CategoryId);
            category.CategoryName.Should().Be(categoryDto.CategoryName);
            category.Description.Should().Be(categoryDto.Description);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.DeleteAsync("/api/categories/999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetProducts_WithValidId_ReturnsListOfProductsInCategory()
        {
            var response = await _client.GetAsync("/api/categories/1/products");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            products.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetProducts_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.GetAsync("/api/categories/999/products");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<(HttpResponseMessage, Category)> CreateTestCategory(CategoryDto categoryDto)
        {
            var response = await _client.PostAsJsonAsync("/api/categories", categoryDto);
            Category category = null;

            if (response.IsSuccessStatusCode)
            {
                category = await response.Content.ReadFromJsonAsync<Category>();
            }
            return (response, category);
        }

        private async Task<HttpResponseMessage> DeleteTestCategory(int categoryId)
        {
            var response = await _client.DeleteAsync($"/api/categories/{categoryId}");
            return response;
        }
    }
}
