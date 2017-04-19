using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities {

	public class Hasher {

		public const string ALPHABET_ONLY = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

		public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt) {
			HashAlgorithm algorithm = new SHA256Managed();
			byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];
			for (int i = 0; i < plainText.Length; i++) {
				plainTextWithSaltBytes[i] = plainText[i];
			}
			for (int i = 0; i < salt.Length; i++) {
				plainTextWithSaltBytes[plainText.Length + i] = salt[i];
			}
			return algorithm.ComputeHash(plainTextWithSaltBytes);
		}

		public static string GenerateSaltedHash(string plainText, string salt) {
			return (Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(salt))));
		}

		public static string GenerateRandomString(int length) {
			StringBuilder builder = new StringBuilder();
			Random random = new Random(Environment.TickCount);
			for (int i = 0; i < length; i++) {
				int x = random.Next(0, 128);
				char c = (char) x;
				builder.Append(c);
			}
			return (builder.ToString());
		}

		public static string GenerateRandomString(int length, string characters) {
			StringBuilder builder = new StringBuilder();
			Random random = new Random(Guid.NewGuid().GetHashCode());
			for (int i = 0; i < length; i++) {
				int pos = random.Next(0, characters.Length);
				char c = characters[pos];
				builder.Append(c);
			}
			return (builder.ToString());
		}

	}

}
