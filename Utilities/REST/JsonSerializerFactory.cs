using System;
using System.IO;
using Newtonsoft.Json;
using Utilities.WebApi;

namespace Utilities.REST {

	public static class JsonSerializerFactory {

		public const string _garblePrefix = ")]}',\n";

		public static string SerializeObject(this JsonSerializer serializer, object o) {
			using (StringWriter sw = new StringWriter()) {
				serializer.Serialize(sw, o);
				return sw.ToString();
			}
		}

		public static T DeSerializeObject<T>(this JsonSerializer serializer, string value, bool deGarble = false) {
			using (StringReader stringReader = new StringReader(value)) {
				if (deGarble && String.Compare(value.Substring(0, _garblePrefix.Length), _garblePrefix) == 0) {
					//read prefix to advance read pointer
					char[] buffer = new char[_garblePrefix.Length];
					stringReader.ReadBlock(buffer, 0, _garblePrefix.Length);
				}
				return serializer.Deserialize<T>(new JsonTextReader(stringReader));
			}
		}

		public static JsonSerializer Create() {
			JsonSerializer serializer = JsonSerializer.Create(DefaultSerializerSettings.Settings);
			serializer.Converters.Add(new XmlElementJsonConverter());
			return serializer;
		}

	}

}