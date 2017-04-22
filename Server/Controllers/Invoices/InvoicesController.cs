using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Server.Domain;
using Server.Persistence;
using Utilities;
using Invoice = Server.Domain.Invoice;

namespace Server.Controllers {

	[RoutePrefix("invoices")]
	public class InvoicesController : BaseController {

		private readonly InvoicePaymentService _invoicePaymentService;

		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IMapper _mapper;

		public InvoicesController(IInvoiceRepository invoiceRepository, InvoicePaymentService invoicePaymentService, IMapper mapper) {
			this._invoicePaymentService = invoicePaymentService;
			this._mapper = mapper;
			this._invoiceRepository = invoiceRepository;
		}

		[Route("")]
		[HttpGet]
		public IEnumerable<InvoiceRepresentation> All() {
			return (this._invoiceRepository.All().Select(i => this._mapper.Map<InvoiceRepresentation>(i).SetupHyperMediaFactory(i, this)));
		}

		[Route("{invoiceId}", Name = "INVOICES_GET")]
		[HttpGet]
		public InvoiceDetailsRepresentation Get(Guid invoiceId) {
			Invoice invoice = this._invoiceRepository.Get(invoiceId);
			return (this._mapper.Map<InvoiceDetailsRepresentation>(invoice).SetupHyperMediaFactory(invoice, this));
		}

		[Route("{invoiceId}/send", Name = "INVOICES_SEND")]
		[HttpPost]
		public InvoiceDetailsRepresentation SendInvoice(Guid invoiceId) {
			Invoice invoice = this._invoiceRepository.Get(invoiceId);
			invoice.Send();
			return (this._mapper.Map<InvoiceDetailsRepresentation>(invoice).SetupHyperMediaFactory(invoice, this));
		}

		[Route("{invoiceId}/payments", Name = "INVOICES_PAYMENTS_POST")]
		[HttpPost]
		public InvoiceDetailsRepresentation AddPayment(Guid invoiceId, PaymentSpecification specification) {
			Invoice invoice = this._invoiceRepository.Get(invoiceId);
			this._invoicePaymentService.AddPayment(invoice, specification.Amount, specification.Date);
			return (this._mapper.Map<InvoiceDetailsRepresentation>(invoice).SetupHyperMediaFactory(invoice, this));
		}

	}

}
