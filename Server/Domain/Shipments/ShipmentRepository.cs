using System;
using System.Collections.Generic;
using Server.Persistence;
using Utilities.Entities;
using ShipmentEntity = Server.Persistence.Shipment;

namespace Server.Domain {

	public interface IShipmentRepository {

		IEnumerable<Shipment> All();
		Shipment Get(Guid id);

	}

	public class ShipmentRepository : DomainRepository<Shipment, ShipmentEntity>, IShipmentRepository {

		public ShipmentRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.Shipments) {}

	}

}