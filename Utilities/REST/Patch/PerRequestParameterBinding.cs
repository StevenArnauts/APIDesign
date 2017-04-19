using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Utilities {

	internal class PerRequestParameterBinding : HttpParameterBinding {

		private readonly IEnumerable<MediaTypeFormatter> _formatters;

		public PerRequestParameterBinding(HttpParameterDescriptor descriptor, IEnumerable<MediaTypeFormatter> formatters) : base(descriptor) {
			if(formatters == null) throw new ArgumentNullException("formatters"); 
			this._formatters = formatters;
		}

		public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken) {
			List<MediaTypeFormatter> perRequestFormatters = new List<MediaTypeFormatter>();
			foreach(MediaTypeFormatter formatter in _formatters) {
				MediaTypeFormatter perRequestFormatter = formatter.GetPerRequestFormatterInstance(Descriptor.ParameterType, actionContext.Request, actionContext.Request.Content.Headers.ContentType);
				perRequestFormatters.Add(perRequestFormatter);
			}
			HttpParameterBinding innerBinding = CreateInnerBinding(perRequestFormatters);
			Contract.Assert(innerBinding != null);
			return innerBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
		}

		protected virtual HttpParameterBinding CreateInnerBinding(IEnumerable<MediaTypeFormatter> perRequestFormatters) {
			return Descriptor.BindWithFormatter(perRequestFormatters);
		}

	}

}