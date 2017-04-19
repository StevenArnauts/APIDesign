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
	public class InvoicesController : ApiController {

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
			return (this._invoiceRepository.All().Select(this._mapper.Map<InvoiceRepresentation>));
		}

		[Route("{invoiceId}", Name = "INVOICES_GET")]
		[HttpGet]
		public InvoiceDetailsRepresentation Get(Guid invoiceId) {
			Invoice invoice = this._invoiceRepository.Get(invoiceId);
			InvoiceDetailsRepresentation representation = this._mapper.Map<InvoiceDetailsRepresentation>(invoice);
			representation.HyperMedia.Factory = new InvoiceHyperMediaFactory(this.Request, invoice).Setup;
			return representation;
		}

		[Route("{invoiceId}/send", Name = "INVOICES_SEND")]
		[HttpPost]
		public InvoiceDetailsRepresentation SendInvoice(Guid invoiceId) {
			Invoice invoice = this._invoiceRepository.Get(invoiceId);
			invoice.Send();
			return (this._mapper.Map<InvoiceDetailsRepresentation>(invoice));
		}

		[Route("{invoiceId}/payments")]
		[HttpPost]
		public InvoiceDetailsRepresentation AddPayment(Guid invoiceId, PaymentSpecification specification) {
			Invoice invoice = this._invoiceRepository.Get(invoiceId);
			this._invoicePaymentService.AddPayment(invoice, specification.Amount, specification.Date);
			return (this._mapper.Map<InvoiceDetailsRepresentation>(invoice));
		}

	}

	public class InvoiceHyperMediaFactory : HyperMediaFactory {

		private readonly Invoice _invoice;

		public InvoiceHyperMediaFactory(HttpRequestMessage request, Invoice invoice) : base(request) {
			this._invoice = invoice;
		}

		public void Setup(Representation representation) {
			representation.Links.AddSafe("self", new Link {
				Href = this.Link("INVOICES_GET", new { invoiceId = this._invoice.Id })
			});
			if(this._invoice.State == InvoiceState.Created) representation.AddActionLink("send", new Link { 
				Href = this.Link("INVOICES_SEND", new { invoiceId = this._invoice.Id })
			});
		}

	}

}
