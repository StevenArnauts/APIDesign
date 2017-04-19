using System.Text;

namespace Utilities.WebApi {

	public static class DefaultProtocol {

		static DefaultProtocol() {
			Encoding = Encoding.UTF8;
			Serializer = new DefaultSerializer();
		}

		public static Encoding Encoding { get; private set; }
		public static DefaultSerializer Serializer { get; private set; }

	}

}
