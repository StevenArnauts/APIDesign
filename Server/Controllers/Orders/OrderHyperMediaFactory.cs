//using System.Linq;
//using System.Net.Http;
//using AutoMapper;
//using Server.Domain;
//using Utilities;

//namespace Server.Controllers {

//	public abstract class DomainHyperMediaFactory<TDomainType, TRepresentation> : HyperMediaFactory where TRepresentation : Representation {

//		private readonly TDomainType _domainObject;

//		public DomainHyperMediaFactory(TDomainType domainObject, HttpRequestMessage request) : base(request) {
//			this._domainObject = domainObject;
//		}

//		public static TRepresentation CreateRepresentation(TDomainType order, IMapper mapper, HttpRequestMessage request) 
//		{
//			TRepresentation representation = mapper.Map<TRepresentation>(order);
//			representation.HyperMedia.Factory = this.CreateFactory(order, request).Setup;
//			return representation;
//		}

//		protected abstract HyperMediaFactory CreateFactory(TDomainType domainObject, HttpRequestMessage request);

//		protected TDomainType DomainObject {
//			get { return(this._domainObject); }
//		}

//	}

//	public class OrderHeaderHyperMediaFactory : DomainHyperMediaFactory<Order, OrderRepresentation> {

//		public OrderHeaderHyperMediaFactory(Order domainObject, HttpRequestMessage request) : base(domainObject, request) {}

//		protected override HyperMediaFactory CreateFactory(Order domainObject, HttpRequestMessage request) {
//			return(new OrderHeaderHyperMediaFactory(domainObject, request));
//		}

	

//	}

//	public class OrderHyperMediaFactor : OrderHeaderHyperMediaFactory {

//		public OrderHyperMediaFactor(HttpRequestMessage request, Order order) : base(request, order) {}

//		public override void Setup(Representation representation) {
//			base.Setup(representation);
//			representation.Links.AddSafe("invoices", new LinkCollection(this._order.Invoices.Select(i => new Link(this.Link("INVOICES_GET", new {
//				invoiceId = i.Id
//			})))));
//		}

//	}

//}