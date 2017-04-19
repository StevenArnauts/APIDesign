using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Utilities.REST;

namespace Utilities.WebApi {

	public class DefaultSerializerSettings {

		private static readonly JsonSerializerSettings jsonSerializerSettings;

		static DefaultSerializerSettings() {
			jsonSerializerSettings = new JsonSerializerSettings {
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = new BaseFirstCamelCaseContractResolver()
			};
			jsonSerializerSettings.Converters.Add(new StringEnumConverter());
			jsonSerializerSettings.Converters.Add(new GuidConverter());
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			jsonSerializerSettings.Formatting = Formatting.Indented;
			jsonSerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
		}

		public static JsonSerializerSettings Settings {
			get { return jsonSerializerSettings; }
		}

	}

}