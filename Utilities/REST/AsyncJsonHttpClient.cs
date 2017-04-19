using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.WebApi {

	public class AsyncJsonHttpClient {

		private readonly HttpClient _httpClient;
		private readonly IJsonHttpClientSerializer _serializer;

		public AsyncJsonHttpClient(Uri serverUrl, HttpMessageHandler messageHandler = null, IJsonHttpClientSerializer serializer = null) {
			this._serializer = serializer ?? DefaultProtocol.Serializer;
			HttpMessageHandler handler = messageHandler ?? new WebRequestHandler();
			this._httpClient = new HttpClient(handler) {
				BaseAddress = serverUrl
			};
		}

		public async Task<string> GetAsync(Uri uri, CancellationToken cancellationToken) {
			Console.WriteLine("Requesting GET " + uri.AbsoluteUri + "...");
			return await this.DoStringRequest(uri, cancellationToken);
		}

		public async Task<string> GetAsync(string path, CancellationToken cancellationToken) {
			Uri uri = new Uri(this._httpClient.BaseAddress, path);
			return await this.GetAsync(uri, cancellationToken);
		}

		public async Task<TResponse> GetAsync<TResponse>(string path, CancellationToken cancellationToken) {
			Uri uri = new Uri(this._httpClient.BaseAddress, path);
			return await this.GetAsync<TResponse>(uri, cancellationToken);
		}

		public async Task<TResponse> GetAsync<TResponse>(Uri uri, CancellationToken cancellationToken) {
			Console.WriteLine("Requesting GET " + uri.AbsoluteUri + "...");
			return await this.Parse<TResponse>(this._httpClient.GetAsync(uri, cancellationToken));
		}

		public async Task<byte[]> DownloadAsync(string path, CancellationToken cancellationToken) {
			Uri uri = new Uri(this._httpClient.BaseAddress, path);
			Console.WriteLine("Requesting GET " + uri.AbsoluteUri + "...");
			return await this.DoByteRequest(uri, cancellationToken);
		}

		public async Task DownloadAsync(string url, string localPath, CancellationToken cancellationToken) {
			Task<Stream> downloadTask = this._httpClient.GetStreamAsync(url);
			using (FileStream stream = new FileStream(localPath, FileMode.Create, FileAccess.Write)) {
				await downloadTask;
				await downloadTask.Result.CopyToAsync(stream, 4096, cancellationToken);
				Console.WriteLine("Downloaded " + url + " to " + localPath);
			}
		}

		public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest content, CancellationToken cancellationToken) {
			try {
				HttpContent message = new StringContent(string.Empty);
				if (content != null) {
					string requestJson = this._serializer.Serialize(content);
					Console.WriteLine("Posting " + requestJson + " to " + path + "...");
					message = new StringContent(requestJson, DefaultProtocol.Encoding, "application/json");
					message.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				}
				TResponse response = await this.Parse<TResponse>(this._httpClient.PostAsync(path, message, cancellationToken));
				return (response);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task PostAsync<TRequest>(string path, TRequest content, CancellationToken cancellationToken) {
			try {
				string requestJson = this._serializer.Serialize(content);
				Console.WriteLine("Posting " + requestJson + " to " + path + "...");
				HttpContent message = new StringContent(requestJson, DefaultProtocol.Encoding, "application/json");
				message.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				await this._httpClient.PostAsync(path, message, cancellationToken);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public void Dispose() {
			this._httpClient.Dispose();
		}

		private async Task<TResponse> Parse<TResponse>(Task<HttpResponseMessage> request) {
			string body = await request.Result.Content.ReadAsStringAsync();
			Console.WriteLine("Parsing response " + body);
			TResponse result = string.IsNullOrEmpty(body) ? default(TResponse) : this._serializer.Deserialize<TResponse>(body);
			return (result);
		}

		private async Task<string> DoStringRequest(Uri uri, CancellationToken cancellationToken) {
			while (true) {
				Task<HttpResponseMessage> request = this._httpClient.GetAsync(uri, cancellationToken);
				HttpResponseMessage headers = await this.GetHeaders(request, cancellationToken);
				if (headers == null) {
					continue;
				}
				string body = await request.Result.Content.ReadAsStringAsync();
				LogResponse(headers, body);
				return (body);
			}
		}

		private static void LogResponse(HttpResponseMessage headers, object body) {
			string resultText = body is byte[] ? ((byte[]) body).Length.ToString(CultureInfo.InvariantCulture) : body.ToString();
			Console.WriteLine("Server responded " + (int) headers.StatusCode + ", date = " + (headers.Headers.Date == null ? "(not set)" : headers.Headers.Date.Value.ToString()) + ", etag = " + (headers.Headers.ETag == null ? "(not set)" : headers.Headers.ETag.Tag) + ", max-age = " + (headers.Headers.CacheControl != null && headers.Headers.CacheControl.MaxAge.HasValue ? headers.Headers.CacheControl.MaxAge.Value.ToString() : "(not set)") + ", body = " + resultText);
		}

		/// <summary>
		/// Executes the task to get the response headers. Should return them if all went to plan.
		/// If it returns null, the operation will be retried until a result is returned, or an exception is thrown.
		/// </summary>
		private async Task<HttpResponseMessage> GetHeaders(Task<HttpResponseMessage> request, CancellationToken cancellationToken) {
			await request;
			if (!request.Result.IsSuccessStatusCode) {
				Console.WriteLine("Request failed, status code = " + request.Result.StatusCode + ", reason = " + request.Result.ReasonPhrase);
			}
			return (request.Result);
		}

		private async Task<byte[]> DoByteRequest(Uri uri, CancellationToken cancellationToken) {
			while (true) {
				Task<HttpResponseMessage> request = this._httpClient.GetAsync(uri, cancellationToken);
				HttpResponseMessage headers = await this.GetHeaders(request, cancellationToken);
				if (headers == null) {
					continue;
				}
				Task<byte[]> body = request.Result.Content.ReadAsByteArrayAsync();
				body.Wait(cancellationToken);
				LogResponse(headers, body);
				return (body.Result);
			}
		}

	}

}
