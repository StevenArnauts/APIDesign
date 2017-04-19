using System.IO;

namespace Utilities {

	public static class StreamHelper {

		public static Stream ToStream(this byte[] source) {
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(source);
			writer.Flush();
			return (stream);
		}

		public static byte[] ToByteArray(this Stream input) {
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream()) {
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

	}

}
