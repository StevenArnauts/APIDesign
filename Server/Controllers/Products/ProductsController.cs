using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Server.Domain;

namespace Server.Controllers {

	[RoutePrefix("products")]
	public class ProductsController : ApiController {

		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public ProductsController(IProductRepository productRepository, IMapper mapper) {
			this._mapper = mapper;
			this._productRepository = productRepository;
		}

		[Route("")]
		[HttpGet]
		public IEnumerable<ProductRepresentation> All() {
			return (this._productRepository.All().Select(this._mapper.Map<ProductRepresentation>));
		}

	}

}