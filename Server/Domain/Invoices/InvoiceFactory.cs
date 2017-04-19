using System;
using Server.Persistence;
using Utilities.Entities;
using InvoiceEntity = Server.Persistence.Invoice;

namespace Server.Domain {

	public interface IInvoiceFactory : IFactory {

		Invoice Create(Order order, DateTimeOffset date);

	}

	public class InvoiceFactory : BaseFactory<IDatabaseContext>, IInvoiceFactory {

		public InvoiceFactory(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork) {}

		public Invoice Create(Order order, DateTimeOffset date) {
			InvoiceEntity entity = new InvoiceEntity {
				Order = order.Entity,
				Date = date
			};
			this.UnitOfWork.Context.Invoices.Add(entity);
			this.UnitOfWork.Context.SaveChanges();
			return new Invoice(entity);
		}

	}

}