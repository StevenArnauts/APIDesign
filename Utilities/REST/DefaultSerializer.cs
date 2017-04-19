using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Utilities.WebApi {

	public class DefaultSerializer : IJsonHttpClientSerializer {

		private readonly JsonSerializer _serializer;

		public DefaultSerializer() {
			this._serializer = JsonSerializer.Create(DefaultSerializerSettings.Settings);
		}

		public T Deserialize<T>(string content) {
			using (StringReader reader = new StringReader(content)) {
				return (this.Deserialize<T>(reader));
			}
		}

		public string Serialize(object content) {
			StringBuilder builder = new StringBuilder();
			using (StringWriter writer = new StringWriter(builder)) {
				this._serializer.Serialize(writer, content);
			}
			return (builder.ToString());
		}

		public T Deserialize<T>(Stream stream) {
			StreamReader reader = new StreamReader(stream);
			return (this.Deserialize<T>(reader));
		}

		public T Deserialize<T>(TextReader reader) {
			return ((T) this._serializer.Deserialize(reader, typeof(T)));
		}

		public void Populate(Stream stream, object target) {
			StreamReader reader = new StreamReader(stream);
			this.Populate(reader, target);
		}

		public void Populate(TextReader reader, object target) {
			this._serializer.Populate(reader, target);
		}

		public void Serialize(object content, Stream stream) {
			using (StreamWriter writer = new StreamWriter(stream)) {
				this._serializer.Serialize(writer, content);
			}
		}

	}

}
