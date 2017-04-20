using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Server.Domain;
using Utilities;

namespace Server.Controllers {

	[RoutePrefix("orders")]
	public class OrdersController : BaseController {

		private readonly IMapper _mapper;
		private readonly OrderConfirmationService _orderConfirmationService;

		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		
		public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository, OrderConfirmationService orderConfirmationService, IMapper mapper) {
			this._orderConfirmationService = orderConfirmationService;
			this._productRepository = productRepository;
			this._mapper = mapper;
			this._orderRepository = orderRepository;
		}

		[Route("", Name = "ORDERS_ALL")]
		[HttpGet]
		public IEnumerable<OrderRepresentation> All() {
			return (this._orderRepository.All().Select(o => {
				OrderRepresentation representation = this._mapper.Map<OrderRepresentation>(o);
				representation.HyperMedia.Factory = new OrderHyperMediaFactory(this.Request, o).Setup;
				return representation;
			}));
		}

		[Route("{orderId}", Name = "ORDERS_GET")]
		[HttpGet]
		public OrderDetailRepresentation Get(Guid orderId) {
			Order order = this._orderRepository.Get(orderId);
			OrderDetailRepresentation representation = this._mapper.Map<OrderDetailRepresentation>(order);
			representation.HyperMedia.Factory = new OrderHyperMediaFactory(this.Request, order).Setup;
			return representation;
		}

		[Route("{orderId}/lines")]
		[HttpGet]
		public IEnumerable<OrderLineRepresentation> GetLines(Guid orderId) {
			Order order = this._orderRepository.Get(orderId);
			return (order.Lines.Select(this._mapper.Map<OrderLineRepresentation>));
		}

		[Route("{orderId}/lines/{orderLineId}")]
		[HttpGet]
		public OrderLineRepresentation GetLine(Guid orderId, Guid orderLineId) {
			Order order = this._orderRepository.Get(orderId);
			OrderLine line = order.Lines.Get(l => l.Id == orderLineId);
			return (this._mapper.Map<OrderLineRepresentation>(line));
		}

		[Route("{orderId}/lines")]
		[HttpPost]
		public OrderLineRepresentation AddLine(Guid orderId, OrderLineSpecification specification) {
			Order order = this._orderRepository.Get(orderId);
			Product product = this._productRepository.Get(specification.ProductId);
			OrderLine line = order.AddLine(product, specification.Quantity);
			return (this._mapper.Map<OrderLineRepresentation>(line));
		}

		[Route("{orderId}/confirm")]
		[HttpPost]
		public OrderDetailRepresentation Confirm(Guid orderId) {
			Order order = this._orderRepository.Get(orderId);
			this._orderConfirmationService.ConfirmOrder(order);
			return (this._mapper.Map<OrderDetailRepresentation>(order));
		}

	}

	public class OrderHyperMediaFactory : HyperMediaFactory {

		private readonly Order _order;

		public OrderHyperMediaFactory(HttpRequestMessage request, Order order) : base(request) {
			this._order = order;
		}
 
		public void Setup(Representation representation) {
			representation.Links.AddSafe("self", new Link {
				Href = this.Link("ORDERS_GET", new { orderId = this._order.Id })
			});
			LinkCollection invoiceLinks = new LinkCollection();
			this._order.Invoices.ForEach(i => invoiceLinks.AddLast(new Link {
				Href = this.Link("INVOICES_GET", new {
					invoiceId = this._order.Invoices.First().Id
				})
			}));
			representation.Links.AddSafe("invoices", invoiceLinks);
		}

	}


}
