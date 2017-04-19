using System;
using System.Collections.Generic;
using Server.Persistence;
using Utilities.Entities;
using BackOrderEntity = Server.Persistence.BackOrder;

namespace Server.Domain {

	public interface IBackOrderRepository {

		BackOrder Get(Guid id);
		IEnumerable<BackOrder> All();

	}

	public class BackOrderRepository : DomainRepository<BackOrder, BackOrderEntity>, IBackOrderRepository {

		public BackOrderRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.BackOrders) {}

	}

}
