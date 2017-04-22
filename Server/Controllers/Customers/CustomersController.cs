using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Server.Domain;

namespace Server.Controllers {

	[RoutePrefix("customers")]
	public class CustomersController : BaseController {

		private readonly ICustomerFactory _customerFactory;
		private readonly ICustomerRepository _customerRepository;
		private readonly IMapper _mapper;
		private readonly IOrderFactory _orderFactory;
		private readonly IOrderRepository _orderRepository;

		public CustomersController(ICustomerRepository customerRepository, ICustomerFactory customerFactory, IOrderFactory orderFactory, IOrderRepository orderRepository, IMapper mapper) {
			this._orderRepository = orderRepository;
			this._mapper = mapper;
			this._customerRepository = customerRepository;
			this._customerFactory = customerFactory;
			this._orderFactory = orderFactory;
		}

		[Route("", Name = "CUSTOMER_ALL")]
		[HttpGet]
		public IEnumerable<CustomerRepresentation> All() {
			return (this._customerRepository.All().Select(c => this._mapper.Map<CustomerRepresentation>(c).SetupHyperMediaFactory(c, this)));
		}

		[Route("{customerId}", Name = "CUSTOMER_GET")]
		[HttpGet]
		public CustomerRepresentation Get(Guid customerId) {
			Customer customer = this._customerRepository.Get(customerId);
			return (this._mapper.Map<CustomerRepresentation>(customer).SetupHyperMediaFactory(customer, this));
		}

		[Route("", Name = "CUSTOMER_ADD")]
		[HttpPost]
		public CustomerRepresentation New(CustomerSpecification spec) {
			Customer customer = this._customerFactory.Create(spec.Name);
			return (this._mapper.Map<CustomerRepresentation>(customer).SetupHyperMediaFactory(customer, this));
		}

		[Route("{customerId}/orders", Name = "CUSTOMER_ORDERS_ADD")]
		[HttpPost]
		public OrderRepresentation AddOrder(Guid customerId, OrderSpecification spec) {
			Customer customer = this._customerRepository.Get(customerId);
			Order order = this._orderFactory.Create(customer, spec.Description, spec.Date);
			return (this._mapper.Map<OrderRepresentation>(order).SetupHyperMediaFactory(order, this));
		}

		[Route("{customerId}/orders", Name = "CUSTOMER_ORDERS_ALL")]
		[HttpGet]
		public IEnumerable<OrderRepresentation> GetOrders(Guid customerId) {
			Customer customer = this._customerRepository.Get(customerId);
			IEnumerable<Order> orders = this._orderRepository.FindByCustomer(customer);
			return (orders.Select(o => this._mapper.Map<OrderRepresentation>(o).SetupHyperMediaFactory(o, this)));
		}

	}

}
