using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Server.Domain;

namespace Server.Controllers {

	[RoutePrefix("shipments")]
	public class ShipmentsController : ApiController {

		private readonly IShipmentRepository _shipmentRepository;
		private readonly IMapper _mapper;

		public ShipmentsController(IShipmentRepository shipmentRepository, IMapper mapper) {
			this._mapper = mapper;
			this._shipmentRepository = shipmentRepository;
		}

		[Route("")]
		[HttpGet]
		public IEnumerable<ShipmentRepresentation> All() {
			return (this._shipmentRepository.All().Select(this._mapper.Map<ShipmentRepresentation>));
		}

		[Route("{shipmentId}")]
		[HttpGet]
		public ShipmentRepresentation Get(Guid shipmentId) {
			return (this._mapper.Map<ShipmentRepresentation>(this._shipmentRepository.Get(shipmentId)));
		}

	}

}
