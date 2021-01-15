using AutoMapper;
using Sieve.Models;
using Sieve.Services;

namespace NorthwindApi.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly NorthwindContext _context;
        protected readonly IMapper _mapper;
        protected readonly ISieveProcessor _sieveProcessor;

        protected SieveModel CreateSieveModel(string filters, string sorts, string defaultSorts)
        {
            var sieveModel = new SieveModel
            {
                Filters = filters,
                Sorts = !string.IsNullOrWhiteSpace(sorts) ? sorts : defaultSorts
            };
            return sieveModel;
        }

        public RepositoryBase(NorthwindContext context, IMapper mapper, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
        }
    }
}
