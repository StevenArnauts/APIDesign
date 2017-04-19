using System;
using System.Collections.Generic;
using Server.Persistence;
using Utilities.Entities;
using DeliveryEntity = Server.Persistence.Delivery;

namespace Server.Domain {

	public interface IDeliveryRepository {

		Delivery Get(Guid id);
		IEnumerable<Delivery> All();

	}

	public class DeliveryRepository : DomainRepository<Delivery, DeliveryEntity>, IDeliveryRepository {

		public DeliveryRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.Deliveries) {}

	}

}
