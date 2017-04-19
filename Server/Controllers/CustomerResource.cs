//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;
//using System.Linq;
//using System.Net.Http;
//using System.Web.Http;
//using AutoMapper;
//using Domain;
//using Domain.Services;
//using Portal.Api;
//using Utilities;
//using Utilities.Translation;
//using Link = Utilities.Link;

//namespace Portal {

//	/// <summary>
//	///     Manages customers.
//	/// </summary>
//	[HandleErrors]
//	public class CustomersController : ApiController, ICustomersController {

//		private readonly CustomerFactory _customerFactory;
//		private readonly CustomerRepository _customerRepository;
//		private readonly SalesDocumentRepository _salesDocumentRepository;
//		private readonly SettingsRepository _settingsRepository;
//		private readonly IUriHelper _uriHelper;
//		private readonly IUnitOfWork _unitOfWork;

//		/// <summary>
//		///     Initializes a new instance of the <see cref="CustomersController" /> class.
//		/// </summary>
//		public CustomersController(IUnitOfWork unitOfWork, IUriHelper uriHelper, IRuntimeContext runtimeContext, ITranslationService translationService) {
//			this._unitOfWork = unitOfWork;
//			this._salesDocumentRepository = new SalesDocumentRepository(unitOfWork, runtimeContext, translationService);
//			this._uriHelper = uriHelper;
//			this._customerRepository = new CustomerRepository(unitOfWork);
//			this._customerFactory = new CustomerFactory(unitOfWork);
//			this._settingsRepository = new SettingsRepository(unitOfWork);
//		}

//		/// <summary>
//		///     Returns an empty specification (with sensible defaults where appropriate, i.e. a template) that can be used by a
//		///     client of the API to create a new instance.
//		/// </summary>
//		/// <returns></returns>
//		[HttpGet]
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/customers/template", Name = RouteNames.CUSTOMERS_TEMPLATE)]
//		public CustomerSpecification Template(int tenantId) {
//			Domain.Settings settings = this._settingsRepository.GetSettings();
//			var customerSpecification = new Domain.CustomerSpecification {
//				Address = new DataTypes.Address {
//					Country = DataTypes.Country.Parse(settings.CustomerCountryCode)
//				},
//				LegislationCode = DataTypes.Legislation.Normaal.Code,
//				SendMethodCode = DataTypes.SendMethod.Email.Code,
//				Email = "",
//				Phone = "",
//				DueDateCalculationAmount = settings.CustomerDueDateCalculationAmount,
//				DueDateCalculationMethodCode = settings.CustomerDueDateCalculationMethodCode,
//				DueDateCalculationPeriodCode = settings.CustomerDueDateCalculationPeriodCode,
//				LanguageCode = settings.CustomerLanguageCode,
//				CustomerType = DataTypes.CustomerType.Company.Code,
//				LegalFormCode = "",
//			};
//			var result = Mapper.Map<CustomerSpecification>(customerSpecification);
//			return (result);
//		}


//		[Route("api/tenants/{tenantId:int}/customers", Name = RouteNames.CUSTOMERS_CREATE)]
//		[CheckAntiForgeryToken]
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[HttpPost]
//		public Customer Create(int tenantId, CustomerSpecification customerSpecification) {
//			if (!string.IsNullOrEmpty(customerSpecification.CompanyNumber)) {
//				//remove spaces from company number
//				customerSpecification.CompanyNumber = new string(customerSpecification.CompanyNumber.Where(char.IsLetterOrDigit).ToArray());
//			}
//			var customer = Mapper.Map<Domain.CustomerSpecification>(customerSpecification);
//			Domain.Customer newCustomer = this._customerFactory.Create(customer);
//			this._customerFactory.Flush();
//			var result = Mapper.Map<Customer>(newCustomer);
//			result.HypermediaFactory = CreateCustomerLinks(newCustomer.Id, tenantId, this._uriHelper, this.Request, this._salesDocumentRepository);
//			result.HypermediaFactory.Invoke(result);
//			return (result);
//		}


//		[Route("api/tenants/{tenantId:int}/customers/{customerId:int}", Name = RouteNames.CUSTOMERS_GET)]
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[HttpGet]
//		public Customer Get(int tenantId, int customerId) {
//			Domain.Customer customer = this._customerRepository.Get(customerId);
//			var result = Mapper.Map<Customer>(customer);
//			result.HypermediaFactory = CreateCustomerLinks(customerId, tenantId, this._uriHelper, this.Request, this._salesDocumentRepository);
//			result.HypermediaFactory.Invoke(result);
//			return (result);
//		}


//		[Route("api/tenants/{tenantId:int}/customers", Name = RouteNames.CUSTOMERS_QUERY)]
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[HttpGet]
//		public IEnumerable<Customer> Query(int tenantId, string filter = null, int numberOfItems = 0) {
//			var result = new List<Customer>();
//			var customers = this._customerRepository.FindQueryable(filter);
//			if (numberOfItems > 0) customers = customers.Take(numberOfItems);

//			Dictionary<Domain.Customer, int> queryResult = (from customer in customers
//															join salesdocument in this._unitOfWork.TenantContext.SalesDocuments on customer.Id equals salesdocument.CustomerInfo.Id into cs
//															select new { Customer = customer, NumberOfSalesDocuments = cs.Count() }).ToDictionary(o => new Domain.Customer(o.Customer), o => o.NumberOfSalesDocuments);
//			ICanCheckIfCustomerHasSalesDocuments counter = new CustomerSalesDocumentsCounter(queryResult.ToDictionary(k => k.Key.Id, v => v.Value));

//			foreach (Domain.Customer customer in queryResult.Keys) {
//				var c = Mapper.Map<Customer>(customer);
//				c.HypermediaFactory = CreateCustomerLinks(customer.Id, tenantId, this._uriHelper, this.Request, counter);
//				c.HypermediaFactory.Invoke(c);
//				result.Add(c);
//			}
//			return (result);
//		}


//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/customers/{customerId:int}", Name = RouteNames.CUSTOMERS_SAVE)]
//		[HttpPut]
//		public Customer Save(int tenantId, int customerId, CustomerSpecification customerRepresentation) {
//			try {
//				Domain.Customer customer = this._customerRepository.Get(customerId);
//				if (String.IsNullOrWhiteSpace(customerRepresentation.Code)) {
//					customerRepresentation.Code = new CustomerSequenceService(this._unitOfWork).GetNext();
//				}
//				//remove spaces from company number
//				if (!string.IsNullOrEmpty(customerRepresentation.CompanyNumber)) {
//					customerRepresentation.CompanyNumber = customerRepresentation.CompanyNumber.Replace(" ", string.Empty);
//				}
//				Mapper.Map(customerRepresentation, customer);
//				this._customerRepository.Flush();
//				return (Mapper.Map<Customer>(customer));
//			} catch (DbEntityValidationException ex) {
//				throw CustomerFactory.ParseDbCustomerValidationExceptions(ex);
//			} catch (DbUpdateException ex) {
//				throw CustomerFactory.ParseDbCustomerUpdateException(ex);
//			}
//		}


//		[Route("api/tenants/{tenantId:int}/customers/{customerId:int}", Name = RouteNames.CUSTOMERS_DELETE)]
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[HttpDelete]
//		public Customer Delete(int tenantId, int customerId) {
//			Domain.Customer customer = this._customerRepository.Delete(customerId);
//			var result = Mapper.Map<Customer>(customer);
//			return (result);
//		}

//		public static Action<Representation> CreateCustomerLinks(int customerId, int tenantId, IUriHelper uriHelper, HttpRequestMessage request, ICanCheckIfCustomerHasSalesDocuments salesDocumentCounter) {
//			return (r => {
//				r.Links.AddSafe("self", new Link { Href = uriHelper.Link(RouteNames.CUSTOMERS_GET, new { tenantId, customerId }, request) });
//				if (!r.Links.ContainsKey("actions")) {
//					var actionLinks = new LinkCollection();
//					r.Links["actions"] = actionLinks;
//					actionLinks.AddLast(new Link { Name = "save", Href = uriHelper.Link(RouteNames.CUSTOMERS_SAVE, new { tenantId, customerId }, request) });
//					if (!salesDocumentCounter.HasSalesDocumentForCustomer(customerId)) {
//						actionLinks.AddLast(new Link { Name = "delete", Href = uriHelper.Link(RouteNames.CUSTOMERS_DELETE, new { tenantId, customerId }, request) });
//					}
//				}
//			});
//		}

//	}

//}