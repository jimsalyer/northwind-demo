using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NorthwindApi;
using NorthwindApi.Models;
using Xunit;

namespace NorthwindApiTests.IntegrationTests.Controllers
{
    public class SuppliersControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private readonly SupplierDto _testSupplierDto = new()
        {
            CompanyName = "Test company",
            ContactName = "Test contact",
            ContactTitle = "Test title",
            Address = "1234 Test St",
            City = "Test City",
            PostalCode = "12345",
            Country = "US",
            Phone = "(123) 456-7890"
        };

        public SuppliersControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsSupplierList()
        {
            var response = await _client.GetAsync("/api/suppliers");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var suppliers = JsonConvert.DeserializeObject<IEnumerable<Supplier>>(
                await response.Content.ReadAsStringAsync()
            );
            suppliers.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsSupplier()
        {
            var response = await _client.GetAsync("/api/suppliers/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var supplier = JsonConvert.DeserializeObject<Supplier>(
                await response.Content.ReadAsStringAsync()
            );
            supplier.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.GetAsync("/api/suppliers/999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_WithValidInput_ReturnsCreatedSupplier()
        {
            (HttpResponseMessage Response, Supplier Supplier) result = await CreateTestSupplier(_testSupplierDto);
            result.Response.StatusCode.Should().Be(HttpStatusCode.Created);
            await DeleteTestSupplier(result.Supplier.SupplierId);

            result.Response.Headers.Location.PathAndQuery.Should().Be($"/api/suppliers/{result.Supplier.SupplierId}");
            result.Supplier.CompanyName.Should().Be(_testSupplierDto.CompanyName);
            result.Supplier.ContactName.Should().Be(_testSupplierDto.ContactName);
            result.Supplier.ContactTitle.Should().Be(_testSupplierDto.ContactTitle);
            result.Supplier.Address.Should().Be(_testSupplierDto.Address);
            result.Supplier.City.Should().Be(_testSupplierDto.City);
            result.Supplier.PostalCode.Should().Be(_testSupplierDto.PostalCode);
            result.Supplier.Country.Should().Be(_testSupplierDto.Country);
            result.Supplier.Phone.Should().Be(_testSupplierDto.Phone);
        }

        [Fact]
        public async Task Create_WithInvalidInput_ReturnsBadRequest()
        {
            var supplierDto = _testSupplierDto.Clone();
            supplierDto.CompanyName = null;

            (HttpResponseMessage Response, Supplier Supplier) createResult = await CreateTestSupplier(supplierDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_WithValidIdAndInput_ReturnsUpdatedSupplier()
        {
            var supplierDto = _testSupplierDto.Clone();

            (HttpResponseMessage Response, Supplier Supplier) createResult = await CreateTestSupplier(supplierDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            supplierDto.CompanyName = "Updated company";
            supplierDto.ContactName = "Updated contact";

            var response = await _client.PutAsJsonAsync($"/api/suppliers/{createResult.Supplier.SupplierId}", supplierDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var supplier = JsonConvert.DeserializeObject<Supplier>(
                await response.Content.ReadAsStringAsync()
            );

            await DeleteTestSupplier(supplier.SupplierId);
            supplier.SupplierId.Should().Be(createResult.Supplier.SupplierId);
            supplier.CompanyName.Should().Be(supplierDto.CompanyName);
            supplier.ContactName.Should().Be(supplierDto.ContactName);
            supplier.ContactTitle.Should().Be(supplierDto.ContactTitle);
            supplier.Address.Should().Be(supplierDto.Address);
            supplier.City.Should().Be(supplierDto.City);
            supplier.PostalCode.Should().Be(supplierDto.PostalCode);
            supplier.Country.Should().Be(supplierDto.Country);
            supplier.Phone.Should().Be(supplierDto.Phone);
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.PutAsJsonAsync("/api/suppliers/999", _testSupplierDto);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_WithInvalidInput_ReturnsBadRequest()
        {
            var supplierDto = _testSupplierDto.Clone();
            supplierDto.CompanyName = null;

            var response = await _client.PutAsJsonAsync("/api/suppliers/999", supplierDto);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsDeletedSupplier()
        {
            var supplierDto = _testSupplierDto.Clone();

            (HttpResponseMessage Response, Supplier Supplier) createResult = await CreateTestSupplier(supplierDto);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await DeleteTestSupplier(createResult.Supplier.SupplierId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var supplier = JsonConvert.DeserializeObject<Supplier>(
                await response.Content.ReadAsStringAsync()
            );

            supplier.SupplierId.Should().Be(createResult.Supplier.SupplierId);
            supplier.CompanyName.Should().Be(supplierDto.CompanyName);
            supplier.ContactName.Should().Be(supplierDto.ContactName);
            supplier.ContactTitle.Should().Be(supplierDto.ContactTitle);
            supplier.Address.Should().Be(supplierDto.Address);
            supplier.City.Should().Be(supplierDto.City);
            supplier.PostalCode.Should().Be(supplierDto.PostalCode);
            supplier.Country.Should().Be(supplierDto.Country);
            supplier.Phone.Should().Be(supplierDto.Phone);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFoundResponse()
        {
            var response = await _client.DeleteAsync("/api/suppliers/999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<(HttpResponseMessage, Supplier)> CreateTestSupplier(SupplierDto supplierDto)
        {
            var response = await _client.PostAsJsonAsync("/api/suppliers", supplierDto);
            Supplier supplier = null;

            if (response.IsSuccessStatusCode)
            {
                supplier = JsonConvert.DeserializeObject<Supplier>(
                    await response.Content.ReadAsStringAsync()
                );
            }
            return (response, supplier);
        }

        private async Task<HttpResponseMessage> DeleteTestSupplier(int supplierId)
        {
            var response = await _client.DeleteAsync($"/api/suppliers/{supplierId}");
            return response;
        }
    }
}
