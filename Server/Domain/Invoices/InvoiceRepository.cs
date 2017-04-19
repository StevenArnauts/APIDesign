using System;
using System.Collections.Generic;
using Server.Persistence;
using Utilities.Entities;
using InvoiceEntity = Server.Persistence.Invoice;

namespace Server.Domain {

	public interface IInvoiceRepository {

		IEnumerable<Invoice> All();
		Invoice Get(Guid id);

	}

	public class InvoiceRepository : DomainRepository<Invoice, InvoiceEntity>, IInvoiceRepository {

		public InvoiceRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.Invoices) {}

	}

}