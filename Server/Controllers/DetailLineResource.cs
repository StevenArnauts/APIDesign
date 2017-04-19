//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using AutoMapper;
//using Domain;
//using Domain.Services;
//using Portal.Api;
//using Utilities;
//using Link = Utilities.Link;

//namespace Portal {

//	[HandleErrors]
//	public class DetailLinesController : ApiController, IDetailLinesController {

//		private readonly SalesDocumentRepository _salesDocumentRepository;
//		private readonly IUriHelper _uriHelper;

//		public DetailLinesController(SalesDocumentRepository salesDocumentRepository, IUriHelper uriHelper) {
//			this._uriHelper = uriHelper;
//			this._salesDocumentRepository = salesDocumentRepository;
//		}

		
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/lines", Name = RouteNames.SALESDOCUMENTLINES_QUERY)]
//		[HttpGet]
//		public IEnumerable<DetailLine> Query(int tenantId, int invoiceId) {
//			SalesDocumentBase salesDocument = this._salesDocumentRepository.Get(invoiceId);
//			return salesDocument.DetailLines.Select(line => this.CreateDetailLineRepresentation(tenantId, invoiceId, line, this.Request));
//		}

		
//		[CheckClaim(Type = Claims.TENANCY, Key = "tenantId")]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/lines/{lineId:int}", Name = RouteNames.SALESDOCUMENTLINES_GET)]
//		[HttpGet]
//		public DetailLine Get(int tenantId, int invoiceId, int lineId) {
//			SalesDocumentBase salesDocument = this._salesDocumentRepository.Get(invoiceId);
//			Domain.DetailLine line = salesDocument.DetailLines.Get(l => l.Id == lineId);
//			return (Mapper.Map<DetailLine>(line));
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/lines", Name = RouteNames.SALESDOCUMENTLINES_CREATE)]
//		[HttpPost]
//		public HttpResponseMessage Create(int tenantId, int invoiceId, DetailLineSpecification detailLineSpecification) {
//			DraftSalesDocument parentDocument = this._salesDocumentRepository.Get(invoiceId) as DraftSalesDocument;
//			if (parentDocument == null) {
//				return (this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Adding detail lines to a published sales document is not allowed"));
//			}
//			Domain.DetailLineSpecification specification = Mapper.Map<Domain.DetailLineSpecification>(detailLineSpecification);
//			Domain.DetailLine newDetailLine = parentDocument.AddDetailLine(specification);
//			this._salesDocumentRepository.Flush();

//			DetailLine result = this.CreateDetailLineRepresentation(tenantId, invoiceId, newDetailLine, this.Request);
//			result.HypermediaFactory.Invoke(result);

//			HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, result);
//			string location = this._uriHelper.Link(RouteNames.SALESDOCUMENTLINES_GET, new { tenantId, invoiceId, lineId = newDetailLine.Id }, this.Request);
//			response.Headers.Location = new Uri(location);
//			return (response);
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/lines/{lineId:int}", Name = RouteNames.SALESDOCUMENTLINES_SAVE)]
//		[HttpPut]
//		public HttpResponseMessage Save(int tenantId, int invoiceId, int lineId, DetailLineSpecification detailLine) {
//			DraftSalesDocument parentDocument = this._salesDocumentRepository.GetWithDetailLines(invoiceId) as DraftSalesDocument;
//			if (parentDocument == null) {
//				return (this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Changing detail lines on a published sales document is not allowed"));
//			}
//			DataTypes.Money mappedPrice = Mapper.Map<DataTypes.Money>(detailLine.Price);
//			Domain.DetailLine updatedDetailLine = parentDocument.UpdateDetailLine(lineId, detailLine.Description, detailLine.Sequence, detailLine.Discount, detailLine.Quantity, detailLine.VatRateCode, detailLine.ProductId, detailLine.ProductCode, mappedPrice, detailLine.TypeCode);

//			DetailLine result = this.CreateDetailLineRepresentation(tenantId, invoiceId, updatedDetailLine, this.Request);
//			result.HypermediaFactory.Invoke(result);

//			HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, result);
//			string location = this._uriHelper.Link(RouteNames.SALESDOCUMENTLINES_GET, new { tenantId, invoiceId, lineId }, this.Request);
//			response.Headers.Location = new Uri(location);
//			return response;
//		}

		
//		[CheckClaim(Type = Claims.OI_MODIFYTENANT, Key = "tenantId")]
//		[CheckAntiForgeryToken]
//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/lines/{lineId:int}", Name = RouteNames.SALESDOCUMENTLINES_DELETE)]
//		[HttpDelete]
//		public void Delete(int tenantId, int invoiceId, int lineId) {
//			DraftSalesDocument parentDocument = this._salesDocumentRepository.Get(invoiceId) as DraftSalesDocument;
//			if (parentDocument == null) {
//				throw new NotAllowedException("Removing detail lines from a published sales document is not allowed");
//			}
//			parentDocument.RemoveDetailLine(lineId);
//		}

//		[Route("api/tenants/{tenantId:int}/invoices/{invoiceId:int}/lines/template", Name = RouteNames.SALESDOCUMENTLINES_TEMPLATE)]
//		[HttpGet]
//		public DetailLineSpecification Template(int tenantId, int invoiceId) {
//			DetailLineSpecification template = new DetailLineSpecification { Price = new Money(0, "EUR"), VatRateCode = DataTypes.VatRate.Normal.Code, TypeCode = DetailLineType.ProductLine.Code };
//			return (template);
//		}

//		private DetailLine CreateDetailLineRepresentation(int tenantId, int invoiceId, Domain.DetailLine line, HttpRequestMessage request) {
//			DetailLine representation = Mapper.Map<DetailLine>(line);
//			representation.HypermediaFactory = (r => {
//				r.Links.AddSafe("self", new Link { Href = this._uriHelper.Link(RouteNames.SALESDOCUMENTLINES_GET, new { tenantId, invoiceId, lineId = line.Id }, request) });
//				if (line.ProductId.HasValue) {
//					r.Links.AddSafe("product", new Link { Href = this._uriHelper.Link(RouteNames.PRODUCTS_GET, new { tenantId, productId = line.ProductId.Value }, request) });
//				}
//				if (!r.Links.ContainsKey("actions")) {
//					LinkCollection actionLinks = new LinkCollection();
//					actionLinks.AddLast(new Link { Name = "save", Href = this._uriHelper.Link(RouteNames.SALESDOCUMENTLINES_SAVE, new { tenantId, invoiceId, lineId = line.Id }, request) });
//					actionLinks.AddLast(new Link { Name = "delete", Href = this._uriHelper.Link(RouteNames.SALESDOCUMENTLINES_DELETE, new { tenantId, invoiceId, lineId = line.Id }, request) });
//					r.Links["actions"] = actionLinks;
//				}
//			});
//			return representation;
//		}

//	}

//}