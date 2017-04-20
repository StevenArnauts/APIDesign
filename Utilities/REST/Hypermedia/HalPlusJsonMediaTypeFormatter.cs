using System;
using System.Net.Http.Headers;

namespace Utilities {

	public class HalPlusJsonMediaTypeFormatter : BaseJsonMediaTypeFormatter {

		public HalPlusJsonMediaTypeFormatter() {
			this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
		}

		protected override void Prepare(Representation config) {
			base.Prepare(config);
			config.HyperMedia.Include = true;
			if (config.HyperMedia.Factory != null) config.HyperMedia.Factory.Invoke(config);
		}

		public override bool CanReadType(Type type) {
			bool canRead = type.IsSubclassOf(typeof(Representation));
			return (canRead);
		}

		public override bool CanWriteType(Type type) {
			bool isListOfRepresentationOrSubtype = type.IsListOf(typeof(Representation));
			bool isTypeOrSubTypeOfRepresentation = type.IsSubclassOf(typeof(Representation));
			bool canWrite = isListOfRepresentationOrSubtype || isTypeOrSubTypeOfRepresentation;
			return (canWrite);
		}

	}

}
