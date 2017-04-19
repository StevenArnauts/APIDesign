using System;
using Server.Persistence;
using Utilities.Entities;
using ShipmentEntity = Server.Persistence.Shipment;

namespace Server.Domain {

	public interface IShipmentFactory : IFactory {

		Shipment Create(Order order, DateTimeOffset date);

	}

	public class ShipmentFactory : BaseFactory<IDatabaseContext>, IShipmentFactory {

		public ShipmentFactory(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork) {}

		public Shipment Create(Order order, DateTimeOffset date) {
			ShipmentEntity entity = new ShipmentEntity {
				Order = order.Entity,
				Date = date
			};
			this.UnitOfWork.Context.Shipments.Add(entity);
			this.UnitOfWork.Context.SaveChanges();
			return new Shipment(entity);
		}

	}

}