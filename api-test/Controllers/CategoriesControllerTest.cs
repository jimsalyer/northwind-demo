using Microsoft.Extensions.Options;
using NorthwindApi.Controllers;
using Sieve.Models;
using Sieve.Services;

namespace NorthwindApiTest.Controllers
{
    public class CategoriesControllerTest
    {
        private CategoriesController _controller;
        private ISieveProcessor _sieveProcessor;

        public CategoriesControllerTest()
        {
            _sieveProcessor = new SieveProcessor(Options.Create(new SieveOptions()));
        }
    }
}
