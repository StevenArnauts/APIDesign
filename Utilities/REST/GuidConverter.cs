using System;
using System.Linq;
using Newtonsoft.Json;

namespace Utilities.REST {

	public class GuidConverter : JsonConverter {

		private readonly Type[] _convertableTypes = new[] { typeof (Guid), typeof (Guid?) };

		public override bool CanRead {
			get { return false; }
		}

		public override bool CanWrite {
			get { return true; }
		}

		public override bool CanConvert(Type objectType) {
			return(this._convertableTypes.Contains(objectType));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value == null) return;
			Guid guid = value as Guid? ?? ((Guid?)value).Value;
			writer.WriteValue(guid.ToString("N").ToUpper());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			// the default deserialization works fine, 
			// but otherwise we'd handle that here
			throw new NotImplementedException();
		}

	}

}