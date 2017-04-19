using System;
using System.Net.Http.Headers;
using Utilities.Logging;

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
			Logger.Debug("can read " + type.FullName + " : " + canRead);
			return (canRead);
		}

		public override bool CanWriteType(Type type) {
			bool isListOfRepresentationOrSubtype = type.IsListOf(typeof(Representation));
			bool isTypeOrSubTypeOfRepresentation = type.IsSubclassOf(typeof(Representation));
			bool canWrite = isListOfRepresentationOrSubtype || isTypeOrSubTypeOfRepresentation;
			Logger.Debug("can write " + type.FullName + " : " + canWrite);
			return (canWrite);
		}

	}

}
