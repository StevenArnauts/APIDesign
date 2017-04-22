using System;
using System.Collections.Generic;
using System.Net.Http;
using Server.Persistence;
using Utilities;
using Invoice = Server.Domain.Invoice;

namespace Server.Controllers {

	public class InvoiceRepresentation : EntityRepresentation {

		public DateTimeOffset Date { get; set; }
		public string State { get; set; }
		public Guid OrderId { get; set; }

		public InvoiceRepresentation SetupHyperMediaFactory(Invoice invoice, IRouteLinker linker) {
			this.HyperMedia.Factory = new InvoiceHyperMediaFactory(invoice, linker).Setup;
			return (this);
		}

		public class InvoiceHyperMediaFactory : HyperMediaFactory {

			private readonly Invoice _invoice;

			public InvoiceHyperMediaFactory(Invoice invoice, IRouteLinker linker) : base(linker) {
				this._invoice = invoice;
			}

			public void Setup(Representation representation) {
				representation.AddLink("self", this.Link("INVOICES_GET", new {
					invoiceId = this._invoice.Id
				}));
				if (this._invoice.State == InvoiceState.Created) {
					representation.AddLink("send", this.Link("INVOICES_SEND", new {
						invoiceId = this._invoice.Id
					}), HttpMethod.Post);
				}
				if (this._invoice.State == InvoiceState.Sent) {
					representation.AddLink("pay", this.Link("INVOICES_PAYMENTS_POST", new {
						invoiceId = this._invoice.Id
					}), HttpMethod.Post);
				}
			}

		}

	}

	public class InvoiceDetailsRepresentation : InvoiceRepresentation {

		public IEnumerable<PaymentRepresentation> Payments { get; set; }

		public new InvoiceDetailsRepresentation SetupHyperMediaFactory(Invoice invoice, IRouteLinker linker) {
			return (base.SetupHyperMediaFactory(invoice, linker) as InvoiceDetailsRepresentation);
		}

	}

}