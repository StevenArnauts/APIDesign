//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http;
//using Application;
//using Application.UBL;
//using AutoMapper;
//using DataTypes;
//using Domain;
//using Domain.Services;
//using Portal.Api;
//using Portal.Areas.OnlineInvoicing;
//using Utilities;
//using Utilities.Translation;
//using Document = Domain.Document;
//using Link = Domain.Link;

//namespace Portal {

//	[HandleErrors]
//	public class SalesDocumentsController : ApiController, ISalesDocumentsController {

//		private readonly CustomerRepository _customerRepository;
//		private readonly IDocumentStoreService _documentStoreService;
//		private readonly LinkFactory _linkFactory;
//		private readonly IMessageBus _messageBus;
//		private readonly SalesDocumentPdfService _pdfService;
//		private readonly SalesDocumentFactory _salesDocumentFactory;
//		private readonly SalesDocumentRepository _salesDocumentRepository;
//		private readonly SettingsRepository _settingsRepository;
//		private readonly IUblService _ublService;
//		private readonly IUriHelper _uriHelper;
//		private readonly IUnitOfWork _unitOfWork;
//		private readonly SalesDocumentSequenceService _sequenceService;
//		private readonly NotificationFactory _notificationFactory;
		
//		public SalesDocumentsController(IUriHelper uriHelper, IUnitOfWork unitOfWork, IRuntimeContext runtimeContext, IMessageBus messageBus, ITranslationService translationService, IDocumentStoreService documentStoreService) {
//			this._unitOfWork = unitOfWork;
//			this._messageBus = messageBus;
//			this._customerRepository = new CustomerRepository(unitOfWork);
//			this._pdfService = new SalesDocumentPdfService(unitOfWork, translationService, documentStoreService);
//			this._ublService = new UblService(unitOfWork, runtimeContext, translationService);
//			this._linkFactory = new LinkFactory(unitOfWork);
//			this._uriHelper = uriHelper;
//			this._salesDocumentFactory = new SalesDocumentFactory(unitOfWork, runtimeContext, translationService);
//			this._salesDocumentRepository = new SalesDocumentRepository(unitOfWork, runtimeContext, translationService);
//			this._settingsRepository = new SettingsRepository(unitOfWork);
//			this._documentStoreService = documentStoreService;
//			this._sequenceService = new SalesDocumentSequenceService(unitOfWork);
//			this._notificationFactory = new NotificationFactory(unitOfWork);
//		}
		
//		[HttpGet]
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}", Name = RouteNames.SALESDOCUMENTS_GET)]
//		public SalesDocument Get(int tenantId, int invoiceId) {
//			SalesDocumentBase invoice = this._salesDocumentRepository.Get(invoiceId);
//			var result = Mapper.Map<SalesDocument>(invoice);
//			result.SendMethod = invoice.SendingMethod.Code; // TODO [MM] Get this working in MapperConfig
			
//			result.HypermediaFactory = CreateInvoiceLinks(result, Security.CurrentTenantId, this._uriHelper, this.Request);
//			result.HypermediaFactory.Invoke(result); // TODO [SAR] Fix this properly
//			return (result);
//		}

		
//		[HttpGet]
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/invoices", Name = RouteNames.SALESDOCUMENTS_QUERY)]
//		public IEnumerable<SalesDocument> Query(int tenantId, decimal? minAmount = null, decimal? maxAmount = null, DateTimeOffset? from = null, DateTimeOffset? until = null, string number = null, string customer = null, int? customerId = null, bool? unPublished = null, bool? published = null, bool? overdue = null, bool? paid = null, bool? unBooked = null, bool? booked = null) {
//			var criteria = new SalesDocumentCriteria { MinAmount = minAmount, MaxAmount = maxAmount, From = from, Until = until, Number = number, Customer = customer, CustomerId = customerId, UnPublished = unPublished, Published = published, Overdue = overdue, Paid = paid, UnBooked = unBooked, Booked = booked};

//			IQueryable<Entities.SalesDocument> salesDocuments = this._unitOfWork.TenantContext.SalesDocuments.Where(criteria.Build());
//			IDbSet<Entities.Customer> customers = _unitOfWork.TenantContext.Customers;

//			var query = from sd in salesDocuments join c in customers on sd.CustomerInfo.Id equals c.Id into groupJoin from subcustomer in groupJoin.DefaultIfEmpty() select new { sd.Id, CustomerId = subcustomer != null ? subcustomer.Id : 0, Customer_FirstName = sd.IsPublished ? (sd.CustomerInfo.FirstName) : (subcustomer == null ? "" : subcustomer.FirstName), Customer_LastName = sd.IsPublished ? (sd.CustomerInfo.LastName) : (subcustomer == null ? "" : subcustomer.LastName), Customer_CompanyName = sd.IsPublished ? (sd.CustomerInfo.CompanyName) : (subcustomer == null ? "" : subcustomer.CompanyName), Customer_TypeCode = (subcustomer == null ? "" : subcustomer.CustomerTypeCode), Customer_SendingMethod = (subcustomer == null ? DataTypes.SendMethod.Paper.Code : subcustomer.SendMethodCode), sd.DueDate, IsFulFilled = sd.Fulfilled, sd.IsPublished, sd.IsValid, sd.Reference, sd.TotalWithoutPaymentDiscount, sd.TotalWithPaymentDiscount, sd.Currency, sd.Number, sd.Date, sd.Type };

//			IEnumerable<SalesDocument> queryResults = query.ToList().Select(sd => {
//				var result = new SalesDocument {
//					CustomerId = sd.CustomerId,
//					CustomerLabel = sd.Customer_TypeCode == DataTypes.CustomerType.PrivatePerson.Code ? sd.Customer_LastName + " " + sd.Customer_FirstName : sd.Customer_CompanyName,
//					Id = sd.Id,
//					Fulfilled = sd.IsFulFilled,
//					IsPublished = sd.IsPublished,
//					IsValid = sd.IsValid,
//					Reference = sd.Reference,
//					SendMethod = sd.Customer_SendingMethod,
//					Totals = new SalesDocumentTotals { TotalWithoutPaymentDiscount = new Money(sd.TotalWithoutPaymentDiscount, sd.Currency), TotalWithPaymentDiscount = new Money(sd.TotalWithPaymentDiscount, sd.Currency) },
//					Number = sd.Number,
//					Date = sd.Date,
//					Type = sd.Type,
//					//TODO this is duplicate code from the domain (in the query model, we pass the domain layer to get straight to the entities). How to resolve this?
//					Category = !sd.IsPublished ? SalesDocumentCategory.NotPublished : (sd.IsFulFilled ? SalesDocumentCategory.Paid : (DateTimeFactory.CurrentOffset > sd.DueDate ? SalesDocumentCategory.Overdue : SalesDocumentCategory.Published))
//				};
//				result.HypermediaFactory = CreateInvoiceLinks(result, Security.CurrentTenantId, this._uriHelper, this.Request);
//				return result;
//			});
//			return queryResults.ToList();
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices", Name = RouteNames.SALESDOCUMENTS_CREATE)]
//		[HttpPost]
//		public SalesDocument Create(int tenantId, SalesDocumentSpecification specification) {
//			var domainSpecification = Mapper.Map<Domain.SalesDocumentSpecification>(specification);
//			DraftSalesDocument salesDocument = this._salesDocumentFactory.Create(domainSpecification);
//			this._salesDocumentFactory.Flush();
//			var result = Mapper.Map<SalesDocument>(salesDocument);
//			return (result);
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}", Name = RouteNames.SALESDOCUMENTS_SAVE)]
//		[HttpPut]
//		public SalesDocument Save(int tenantId, int invoiceId, SalesDocumentSpecification specification) {
//			SalesDocumentBase document = this._salesDocumentRepository.Get(invoiceId);
//			Mapper.Map(specification, document);
//			SalesDocument respresentation = Mapper.Map<SalesDocument>(document);
//			respresentation.SendMethod = document.SendingMethod.Code; // TODO [MM] Get this working in MapperConfig
//			respresentation.HypermediaFactory = CreateInvoiceLinks(respresentation, Security.CurrentTenantId, this._uriHelper, this.Request);
//			respresentation.HypermediaFactory.Invoke(respresentation); // TODO [SAR] Fix this properly
//			return respresentation;
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}", Name = RouteNames.SALESDOCUMENTS_DELETE)]
//		[HttpDelete]
//		public void Delete(int tenantId, int invoiceId) {
//			this._salesDocumentRepository.Delete(invoiceId);
//			this._salesDocumentFactory.Flush();
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/email", Name = RouteNames.SALESDOCUMENTS_EMAIL)]
//		[HttpPost]
//		public void Email(int tenantId, int invoiceId, bool duplicate = false) {
//			//check prerequisites 
//			SalesDocumentBase document = this._salesDocumentRepository.Get(invoiceId);
//			if (!document.IsValid) throw new InvalidOperationException("Salesdocument " + invoiceId + " cannot be sent because it's not valid");

//			//check customer
//			Domain.Customer customer = this._customerRepository.Get(document.Customer.Id);
//			if (customer == null) throw new MissingCustomerException();
//			if (string.IsNullOrEmpty(customer.Email))
//			{
//				this._notificationFactory.Create(new NotificationSpecification { MessageKey = "exceptions.missingemail.tocustomer", MessageValueDictionary = new Dictionary<string, string>{{"customername", customer.GetLabel()}}, Type = NotificationType.ALERT.Code });
//				throw new MissingEmailException();
//			}

//			//check settings
//			Domain.Settings settings = this._settingsRepository.GetSettings();
//			var settingsComplete = document.HasMinimumPublishSettings(settings);
//			if (!settings.InvoiceNumberInitialized || !settingsComplete) throw new SettingsIncompleteException(settings.InvoiceNumberInitialized, settingsComplete);

//			//publish sales document
//			if (!duplicate && !document.Status.IsPublished) {
//				document = this.Publish(document);
//			}
//			Domain.SalesDocument publishedDocument = document as Domain.SalesDocument;
//			publishedDocument.MarkAsBeingPublished(customer.Email, duplicate);

//			// create email request
//			var request = new EmailInvoiceRequest { Token = Security.CurrentPrincipal.Tokenize(), InvoiceId = invoiceId, Duplicate = duplicate, BaseUrl = this.Request.RequestUri.GetLeftPart(UriPartial.Authority) };

//			// create and save an unguessable link to the pdf
//			string option = duplicate ? "pdf-duplicate" : "pdf";
//			request.PdfInternalUrl = this._uriHelper.Route(OnlineInvoicingAreaRegistration.DOWNLOAD_SALESDOCUMENT_ROUTE, new { tenantId, invoiceId, option, logEvent = true.ToString() }, this.Request);
			
//			// create and save an unguessable link to the ubl
//			string optionUbl = duplicate ? "ubl-duplicate" : "ubl";
//			request.UblInternalUrl = this._uriHelper.Route(OnlineInvoicingAreaRegistration.DOWNLOAD_SALESDOCUMENT_ROUTE, new { tenantId, invoiceId, option = optionUbl, logEvent = true.ToString() }, this.Request);
//			request.CorrelationId = CorrelationId.Get();
//			this._messageBus.Send(Queues.EMAIL_INVOICE_REQUEST, request);
//			//await this._messageBus.RequestAndWaitAsync<EmailInvoiceRequest, EmailInvoiceResponse>(Queues.EMAIL_INVOICE_REQUEST, request);
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/print", Name = RouteNames.SALESDOCUMENTS_PRINT)]
//		[HttpPost]
//		public HttpResponseMessage Print(int tenantId, int invoiceId, bool duplicate = false) {
//			//check prerequisites 
//			SalesDocumentBase document = this._salesDocumentRepository.Get(invoiceId);
//			if (!document.Status.IsPublished && !duplicate) {
//				Security.RequireClaim(Claims.OI_PUBLISH);
//			}
//			if (!document.IsValid) throw new InvalidOperationException("Salesdocument " + invoiceId + " cannot be printed because it's not valid");

//			// compose download URL based on request
//			string downloadUrl = this._uriHelper.Link(RouteNames.SALESDOCUMENTS_DOWNLOAD, new { id = invoiceId, option = duplicate ? "pdf-duplicate" : "pdf" }, this.Request);
			
//			// publish sales document
//			if (!duplicate) document = this.Publish(document);
//			var publishedDocument = document as Domain.SalesDocument;

//			// Create PDF and UBL documents
//			if (!publishedDocument.DocumentId.HasValue) this.GenerateDocuments(publishedDocument);

//			// return updated salesdocument viewmodel
//			var result = Mapper.Map<SalesDocument>(publishedDocument);
//			result.HypermediaFactory += CreateInvoiceLinks(result, Security.CurrentTenantId, this._uriHelper, this.Request);
//			result.HypermediaFactory.Invoke(result); // TODO [SAR] Fix this properly

//			HttpResponseMessage message = this.Request.CreateResponse(HttpStatusCode.Created, result);

//			// add api download link
//			message.Headers.Location = new Uri(downloadUrl);

//			// update invoice history
//			publishedDocument.MarkAsPrinted(duplicate);

//			Logger.Info(this, "Printed invoice nr " + publishedDocument.Number);

//			return (message);
//		}

		
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/salesdocuments/mergepdf", Name = RouteNames.SALESDOCUMENTS_MERGE_PDF)]
//		[HttpPost]
//		public async Task<HttpResponseMessage> MergePdf(int tenantId, PdfMergeSpecification specification) {
//			IMessageBusResponse<MergePdfRequest, MergePdfResponse> result;
//			var request = new MergePdfRequest { Token = Security.CurrentPrincipal.Tokenize(), SalesDocumentIds = specification.Ids, Duplicate = specification.Duplicate, BaseUrl = this.Request.RequestUri.GetLeftPart(UriPartial.Authority) };
//			result = await this._messageBus.RequestAndWaitAsync<MergePdfRequest, MergePdfResponse>(Queues.MERGE_PDF_REQUEST, request);

//			if (result == null) {
//				Logger.Warn("Received no response");
//				return (this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Background worker seems to be down"));
//			}

//			if (result.Error == null) {
//				HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created);
//				string url = this._uriHelper.Link("Reports.Download", new { tenantId, reportId = result.Response.ReportId }, this.Request);
//				response.Headers.Location = new Uri(url);
//				return (response);
//			}
//			Logger.Warn(this, result.Error, "Creating merged pfd failed");
//			return (this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, result.Error));
//		}

		
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/salesdocuments/{invoiceId:int}/download", Name = RouteNames.SALESDOCUMENTS_DOWNLOAD)]
//		[HttpGet]
//		public async Task<HttpResponseMessage> Download(int tenantId, int invoiceId, string option, string logEvent = "false") {
//			bool shouldLogEvent;
//			bool.TryParse(logEvent, out shouldLogEvent);

//			// Validation
//			if (invoiceId <= 0) throw new ServerValidationException(new Domain.ServerValidationError { ErrorCode = "Invalid", Property = "Id" });
//			if (string.IsNullOrEmpty(option)) throw new ServerValidationException(new Domain.ServerValidationError { ErrorCode = "Invalid", Property = "Option" });
//			DownloadOption download = DownloadOption.Parse(option);
//			if (!download.IsPdf && !download.IsUbl) throw new ServerValidationException(new Domain.ServerValidationError { ErrorCode = "NotSupported", Property = "Option", Message = DataTypes.MultiLanguageString.CreateFromTranslationKey("exceptions.unsupporteddownloadoption", new Dictionary<string, string> { { "option", option } }) });
//			var salesDocument = this._salesDocumentRepository.Get(invoiceId) as Domain.SalesDocument;
//			if (salesDocument == null || !salesDocument.DocumentId.HasValue) {
//				Logger.Warn("User tried to download " + option + " for sales document " + invoiceId + " (tenant " + tenantId + ") but it has no associated document!");
//				// info/error about state "not published"?
//				throw new ServerValidationException(new Domain.ServerValidationError { ErrorCode = "Unavailable", Property = "Document", Message = DataTypes.MultiLanguageString.CreateFromTranslationKey("exceptions.salesdocumentnotfound") });
//			}

//			// Start processing request
//			if (download.IsPdf) {
//			Document pdf = this._documentStoreService.Get(salesDocument.DocumentId.Value, "application/pdf").First();
//				byte[] pdfBytes = download.IsDuplicate ? await this._pdfService.AddWatermarkAsync(pdf.Bytes, salesDocument.Customer.LanguageCode, salesDocument.Type) : pdf.Bytes;

//			Stream stream = pdfBytes.ToStream();
//			stream.Position = 0;

//				if (shouldLogEvent) {
//					salesDocument.MarkAsDownloadedAsPdfCustomer();
//				}

//				// Return PDF Stream
//				HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
//				response.Headers.Add("cache-control", "private, max-age=0");
//				response.Content = new StreamContent(stream);
//				response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
//				response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline") { FileName = pdf.Name };
//				return (response);
//			}
//			if (download.IsUbl) {
//				byte[] ublBytes;

//				// generate downloadlink for pdf
//				string pdfUrl = this.CalculateUnguessableLinkPdfDownload(tenantId, invoiceId, salesDocument.DocumentId.Value, download.IsDuplicate ? "pdf-duplicate" : "pdf");

//				// Get UBL from documentstore
//				Document ublDocument = this._documentStoreService.Get(salesDocument.DocumentId.Value, "application/xml").First();
//				if (download.IsDuplicate) {
//					// Replace PDF bytes with PDF that includes 'duplicate' watermark
//					Document pdf = this._documentStoreService.Get(salesDocument.DocumentId.Value, "application/pdf").First();
//					byte[] pdfBytes = await this._pdfService.AddWatermarkAsync(pdf.Bytes, salesDocument.Customer.LanguageCode, salesDocument.Type);
//					ublBytes = await this._ublService.AddDuplicateAsync(ublDocument.Bytes, pdfBytes, pdfUrl);
//				}
//				else {
//					ublBytes = ublDocument.Bytes;
//				}
				
//				if (shouldLogEvent) {
//					salesDocument.MarkAsDownloadedAsUblCustomer();
//				}

//				HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
//				response.Content = new StringContent(Encoding.UTF8.GetString(ublBytes), Encoding.UTF8, "application/xml");
//				response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = ublDocument.FileName };
//				return (response);
//			}
//			return (this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Download option " + option + " is not supported"));
//		}
		
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/invoices/summary", Name = RouteNames.SALESDOCUMENTS_SUMMARY)]
//		[HttpGet]
//		public SalesDocumentSummary Summary(int tenantId) {
//			var salesDocumentSummary = new SalesDocumentSummary {
//				Unpublished = this.CreateStatistic(new SalesDocumentCriteria { UnPublished = true }),
//				Published = this.CreateStatistic(new SalesDocumentCriteria { Published = true }),
//				Paid = this.CreateStatistic(new SalesDocumentCriteria { Paid = true }),
//				Overdue = this.CreateStatistic(new SalesDocumentCriteria { Overdue = true }),
//				Unbooked = this.CreateStatistic(new SalesDocumentCriteria { UnBooked = true }),
//				HypermediaFactory = delegate(Representation representation) {
//					representation.Links.Add("unpublished", new Utilities.Link(this.Url.Link(RouteNames.SALESDOCUMENTS_QUERY, new { unPublished = true }))); // TODO [SAR] "unpublished"? How is that different from published = false?
//					representation.Links.Add("published", new Utilities.Link(this.Url.Link(RouteNames.SALESDOCUMENTS_QUERY, new { published = true })));
//					representation.Links.Add("overdue", new Utilities.Link(this.Url.Link(RouteNames.SALESDOCUMENTS_QUERY, new { overdue = true })));
//					representation.Links.Add("paid", new Utilities.Link(this.Url.Link(RouteNames.SALESDOCUMENTS_QUERY, new { paid = true })));
//					representation.Links.Add("unbooked", new Utilities.Link(this.Url.Link(RouteNames.SALESDOCUMENTS_QUERY, new { unBooked = true })));// TODO [SAR] "unBooked"? How is that different from booked = false?
//				}
//			};
//			return (salesDocumentSummary);
//		}

//		[HttpGet]
//		[Route("api/tenants/{tenantId:int}/invoices/template", Name = RouteNames.SALESDOCUMENTS_TEMPLATE)]
//		public SalesDocumentSpecification Template(int tenantId) {
//			var template = new SalesDocumentSpecification { Date = DateTimeFactory.CurrentOffset, DueDate = DateTimeFactory.CurrentOffset, Customer = new CustomerInfo { Address = new Address(), LanguageCode = Culture.Nederlands.Code }, Type = SalesDocumentType.Invoice };
//			return (template);
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}", Name = RouteNames.SALESDOCUMENTS_PATCH)]
//		[HttpPatch]
//		public SalesDocument Patch(int tenantId, int invoiceId, Patch<SalesDocumentBase> patch) {
//			SalesDocumentBase original = this._salesDocumentRepository.Get(invoiceId);
//			patch.Apply(original);
//			SalesDocument respresentation = Mapper.Map<SalesDocument>(original);
//			respresentation.SendMethod = original.SendingMethod.Code; // TODO [MM] Get this working in MapperConfig
//			return respresentation;
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/book", Name = RouteNames.SALESDOCUMENTS_BOOK)]
//		[HttpPost]
//		public void Book(int tenantId, [FromBody] int[] invoiceIds) {
//			Domain.Settings settings = this._settingsRepository.GetSettings();
//			if (string.IsNullOrEmpty(settings.AccountantEmail)) throw new MissingEmailException();

//			List<SalesDocumentBase> salesDocumentBases = this._salesDocumentRepository.FindByIds(invoiceIds).ToList();

//			// Check salesdocuments 
//			foreach (SalesDocumentBase documentBase in salesDocumentBases) {
//				if (!documentBase.IsPublished) throw new Exception("Sales document " + documentBase.Id + " has not been published");
//				Domain.SalesDocument document = documentBase as Domain.SalesDocument;
//				if (document == null) throw new Exception("Sales document " + documentBase.Id + " has not been correctly published");
//				if (!document.DocumentId.HasValue) throw new Exception("Sales document " + documentBase.Id + " has no document reference ");
//			}

//			List<Domain.SalesDocument> salesDocuments = salesDocumentBases.Cast<Domain.SalesDocument>().ToList();
//			// Mark individual salesdocuments as booked
//			foreach (Domain.SalesDocument document in salesDocuments) {
//				document.MarkAsBeingBooked(settings.AccountantEmail);
//			}

//			var request = new BookInvoiceRequest {
//				Token = Security.CurrentPrincipal.Tokenize(),
//				CorrelationId = CorrelationId.Get(),
//				Ids = invoiceIds,
//				BaseUrl = this.Request.RequestUri.GetLeftPart(UriPartial.Authority)
//			};
//			this._messageBus.Send(Queues.INVOICE_BOOK_REQUEST, request);
//			//await this._messageBus.RequestAndWaitAsync<BookInvoiceRequest, BookInvoiceResponse>(Queues.INVOICE_BOOK_REQUEST, request);
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/copy", Name = RouteNames.SALESDOCUMENTS_COPY)]
//		[HttpPost]
//		public HttpResponseMessage Copy(int tenantId, int invoiceId, SalesDocumentSpecification defaultSpecification) {
//			var specification = Mapper.Map<Domain.SalesDocumentSpecification>(defaultSpecification);
			
//			SalesDocumentBase source = this._salesDocumentRepository.Get(invoiceId);
//			DraftSalesDocument document = this._salesDocumentFactory.Copy(source, specification);
//			this._salesDocumentFactory.Flush();

//			// return updated salesdocument viewmodel
//			var result = Mapper.Map<SalesDocument>(document);
//			result.SendMethod = document.SendingMethod.Code; // TODO [MM] Get this working in MapperConfig
//			result.HypermediaFactory += CreateInvoiceLinks(result, Security.CurrentTenantId, this._uriHelper, this.Request);
//			result.HypermediaFactory.Invoke(result); // TODO [SAR] Fix this properly

//			HttpResponseMessage message = this.Request.CreateResponse(HttpStatusCode.Created, result);

//			return (message);
//		}

//		[Route("api/tenants/{tenantId:int}/invoices/initializeInvoiceNumber", Name = RouteNames.SALESDOCUMENTS_INIT_NUMBER)]
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[HttpPost]
//		public void InitializeInvoiceNumber(int tenantId, [FromBody] int invoiceNumber) {
//			Domain.Settings settings = this._settingsRepository.GetSettings();
//			if (settings.InvoiceNumberInitialized) {
//				throw new InvalidOperationException("Invoice number cannot be set because it is already initialized");
//			}

//			this._sequenceService.Initialize(invoiceNumber);
//			settings.InvoiceNumberInitialized = true;
//			this._settingsRepository.Flush();
//		}


//		private Domain.SalesDocument Publish(SalesDocumentBase salesDocument) {
//			Security.RequireClaim(Claims.OI_PUBLISH);

//			var publishedSalesDocument = salesDocument as Domain.SalesDocument;
//			if (salesDocument.GetType() == typeof(DraftSalesDocument)) {
//				var draftSalesDocument = salesDocument as DraftSalesDocument;
//				publishedSalesDocument = draftSalesDocument.Publish();
//			}
//			return publishedSalesDocument;
//		}

//		private void GenerateDocuments(Domain.SalesDocument salesDocument) {
//			byte[] pdfBytes = this._pdfService.CreatePdf(salesDocument);
//			DocumentSpecification pdfDocumentSpecification = this._pdfService.CreateDocumentSpecification(salesDocument, pdfBytes);
//				Document document = this._documentStoreService.CreateDocument(pdfDocumentSpecification);

//				// generate downloadlink for pdf
//				string pdfDownloadUrl = this.CalculateUnguessableLinkPdfDownload(Security.CurrentTenantId, salesDocument.Id, document.Id, "pdf");

//			byte[] ublBytes = this._ublService.CreateUbl(salesDocument, pdfBytes, pdfDownloadUrl, false);
//			DocumentSpecification ublDocumentSpecification = this._ublService.CreateDocumentSpecification(salesDocument, ublBytes);
//				this._documentStoreService.AddDocumentVersion(document.Id, ublDocumentSpecification);

//				this._unitOfWork.DocumentStoreContext.SaveChanges(); // TODO [SAR] Bad idea for something that will be a service once
//			salesDocument.DocumentId = document.Id;
//		}

//		public static Action<Representation> CreateInvoiceLinks(SalesDocument representation, int tenantId, IUriHelper uriHelper, HttpRequestMessage request) {
//			return (r => {
//				r.Links.AddSafe("self", new Utilities.Link { Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_GET, new { tenantId, invoiceId = representation.Id }, request) });
//				r.Links.AddSafe("lines", new Utilities.Link { Href = uriHelper.Link(RouteNames.SALESDOCUMENTLINES_QUERY, new { tenantId, invoiceId = representation.Id }, request) });
//				r.Links.AddSafe("payments", new Utilities.Link { Href = uriHelper.Link(RouteNames.PAYMENTS_QUERY, new { tenantId, invoiceId = representation.Id }, request) });
//				r.Links.AddSafe("events", new Utilities.Link { Href = uriHelper.Link(RouteNames.EVENTS_QUERY, new { tenantId, invoiceId = representation.Id }, request) });
//				if (!r.Links.ContainsKey("actions")) {
//					var actionLinks = new LinkCollection();
//					r.Links["actions"] = actionLinks;
//					if (representation.IsValid) {
//						if (representation.SendMethod == DataTypes.SendMethod.Email.Code) {
//							actionLinks.AddLast(new Utilities.Link { Name = "email", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_EMAIL, new { tenantId, invoiceId = representation.Id }, request) });
//							actionLinks.AddLast(new Utilities.Link { Name = "print", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_PRINT, new { tenantId, invoiceId = representation.Id }, request) });
//						}
//						else {
//							actionLinks.AddLast(new Utilities.Link { Name = "print", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_PRINT, new { tenantId, invoiceId = representation.Id }, request) });
//							actionLinks.AddLast(new Utilities.Link { Name = "email", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_EMAIL, new { tenantId, invoiceId = representation.Id }, request) });
//						}
//						if (representation.IsPublished) {
//							// Booking a salesdocument is allowed for valid and published salesdocuments. Resends are allowed
//							actionLinks.AddFirst(new Utilities.Link { Name = "book", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_BOOK, new { tenantId, invoiceId = representation.Id }, request) });

//							if (!representation.Fulfilled)
//							{
//								// add payment always comes first
//								actionLinks.AddFirst(new Utilities.Link { Name = "add-payment", Href = uriHelper.Link(RouteNames.PAYMENTS_CREATE, new { tenantId, invoiceId = representation.Id }, request) });
//							}
//							// then the reminder options
//							if (representation.SendMethod == DataTypes.SendMethod.Email.Code) {
//								actionLinks.AddLast(new Utilities.Link { Name = "email-duplicate", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_EMAIL, new { tenantId, invoiceId = representation.Id, duplicate = true }, request) });
//								actionLinks.AddLast(new Utilities.Link { Name = "print-duplicate", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_PRINT, new { tenantId, invoiceId = representation.Id, duplicate = true }, request) });
//							}
//							else {
//								actionLinks.AddLast(new Utilities.Link { Name = "print-duplicate", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_PRINT, new { tenantId, invoiceId = representation.Id, duplicate = true }, request) });
//								actionLinks.AddLast(new Utilities.Link { Name = "email-duplicate", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_EMAIL, new { tenantId, invoiceId = representation.Id, duplicate = true }, request) });
//							}
//						}
//					}
//					if (!representation.IsPublished) {
//						actionLinks.AddLast(new Utilities.Link { Name = "save", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_SAVE, new { tenantId, invoiceId = representation.Id }, request) });
//						actionLinks.AddLast(new Utilities.Link { Name = "create-line", Href = uriHelper.Link(RouteNames.SALESDOCUMENTLINES_CREATE, new { tenantId, invoiceId = representation.Id }, request) });
//						actionLinks.AddLast(new Utilities.Link { Name = "delete", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_DELETE, new { tenantId, invoiceId = representation.Id }, request) });
//					}
//					//credit notes cannot be duplicated or credited
//					if (representation.Type != SalesDocumentType.CreditNote) {
//						actionLinks.AddLast(new Utilities.Link { Name = "copy", Href = uriHelper.Link(RouteNames.SALESDOCUMENTS_COPY, new { tenantId, invoiceId = representation.Id }, request) });
//					}
//				}
//				if (!r.Links.ContainsKey("customer")) {
//					r.Links["customer"] = new Utilities.Link { Name = "customer", Href = uriHelper.Link(RouteNames.CUSTOMERS_GET, new { tenantId, customerId = representation.CustomerId }, request) };
//				}
//			});
//		}

//		private string CalculateUnguessableLinkPdfDownload(int tenantId, int invoiceId, int documentId, string option) {
//			// create and save an unguessable link to the pdf
//			string pdfInternalUrl = this._uriHelper.Route(OnlineInvoicingAreaRegistration.DOWNLOAD_SALESDOCUMENT_ROUTE, new { tenantId, invoiceId, option }, this.Request);

//			// Find unguessable link or create new
//			Link link = this._linkFactory.CreateOrGet(pdfInternalUrl, LinkType.PdfCustomer, Security.CurrentStoreName, Security.CurrentTenantId,
//				Claims.OI_DOWNLOADDOCUMENT, documentId.ToString(CultureInfo.InvariantCulture));

//			return this._uriHelper.Link(RouteConfig.ROUTE_DEFAULT, new { controller = "Links", action = "GoTo", id = link.Id }, this.Request);
//		}

//		private Statistic CreateStatistic(SalesDocumentCriteria criteria) {
//			var result = new Statistic();
//			List<SalesDocumentBase> salesDocuments = this._salesDocumentRepository.Query(criteria).ToList();
//			var invoices = salesDocuments.Where(sd => sd.Type == SalesDocumentType.Invoice).ToList();
//			var creditNotes = salesDocuments.Where(sd => sd.Type == SalesDocumentType.CreditNote).ToList();
//			decimal totalWithPaymentDiscountForinvoices = invoices.Sum(sd => sd.Totals.TotalWithPaymentDiscount);
//			decimal totalWithPaymentDiscountForCreditNotes = creditNotes.Sum(sd => sd.Totals.TotalWithPaymentDiscount);
//			result.AmountWithPaymentDiscount = new Money(totalWithPaymentDiscountForinvoices - totalWithPaymentDiscountForCreditNotes, "EUR");
			
//			decimal totalWithoutPaymentDiscountForinvoices = invoices.Sum(sd => sd.Totals.TotalWithoutPaymentDiscount);
//			decimal totalWithoutPaymentDiscountForCreditNotes = creditNotes.Sum(sd => sd.Totals.TotalWithoutPaymentDiscount);
//			result.AmountWithoutPaymentDiscount = new Money(totalWithoutPaymentDiscountForinvoices - totalWithoutPaymentDiscountForCreditNotes, "EUR");
			
//			result.Number = salesDocuments.Count();
//			return (result);
//		}

//	}

//}