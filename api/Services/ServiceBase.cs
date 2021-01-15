using AutoMapper;
using NorthwindApi.Repositories;
using Sieve.Models;
using Sieve.Services;

namespace NorthwindApi.Services
{
    public abstract class ServiceBase
    {
        protected readonly NorthwindContext _context;
        protected readonly IMapper _mapper;
        protected readonly SieveProcessor _sieveProcessor;

        protected SieveModel CreateSieveModel(string filters, string sorts, string defaultSorts)
        {
            var sieveModel = new SieveModel
            {
                Filters = filters,
                Sorts = !string.IsNullOrWhiteSpace(sorts) ? sorts : defaultSorts
            };
            return sieveModel;
        }

        public ServiceBase(NorthwindContext context, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _context = context;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
        }
    }
}
