using System;
using System.Collections.Generic;
using System.Linq;
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

		[Route("", Name = "ORDER_ALL")]
		[HttpGet]
		public IEnumerable<OrderRepresentation> All() {
			return (this._orderRepository.All().Select(o => this._mapper.Map<OrderRepresentation>(o).SetupHyperMediaFactory(o, this)));
		}

		[Route("{orderId}", Name = "ORDER_GET")]
		[HttpGet]
		public OrderDetailRepresentation Get(Guid orderId) {
			Order order = this._orderRepository.Get(orderId);
			return (this._mapper.Map<OrderDetailRepresentation>(order).SetupHyperMediaFactory(order, this));
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
			return (this._mapper.Map<OrderDetailRepresentation>(order).SetupHyperMediaFactory(order, this));
		}

	}

}
