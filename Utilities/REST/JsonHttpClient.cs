using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.WebApi {

	public class JsonHttpClient : AsyncJsonHttpClient {

		public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(180);
		private readonly CancellationTokenSource _cts;

		public JsonHttpClient(Uri serverUrl, HttpMessageHandler messageHandler = null, IJsonHttpClientSerializer serializer = null) : base(serverUrl, messageHandler, serializer) {
			this._cts = new CancellationTokenSource(DefaultTimeout);
		}

		public JsonHttpClient(Uri serverUrl, TimeSpan timeout, HttpMessageHandler messageHandler = null, IJsonHttpClientSerializer serializer = null) : base(serverUrl, messageHandler, serializer) {
			this._cts = new CancellationTokenSource(timeout);
		}

		public string Get(Uri uri) {
			Task<string> task = this.GetAsync(uri, this._cts.Token);
			task.Wait(this._cts.Token);
			return (task.Result);
		}

		public string Get(string path) {
			Task<string> task = this.GetAsync(path, this._cts.Token);
			task.Wait(this._cts.Token);
			return (task.Result);
		}

		public TResponse Get<TResponse>(string path) {
			Task<TResponse> task = this.GetAsync<TResponse>(path, this._cts.Token);
			task.Wait(this._cts.Token);
			return (task.Result);
		}

		public TResponse Get<TResponse>(Uri uri) {
			Task<TResponse> task = this.GetAsync<TResponse>(uri, this._cts.Token);
			task.Wait(this._cts.Token);
			return (task.Result);
		}

		public byte[] Download(string path) {
			Task<byte[]> task = this.DownloadAsync(path, this._cts.Token);
			task.Wait(this._cts.Token);
			return (task.Result);
		}

		public void Download(string url, string localPath) {
			Task task = this.DownloadAsync(url, localPath, this._cts.Token);
			task.Wait(this._cts.Token);
		}

		public TResponse Post<TRequest, TResponse>(string path, TRequest content) {
			Task<TResponse> task = this.PostAsync<TRequest, TResponse>(path, content, this._cts.Token);
			task.Wait(this._cts.Token);
			return (task.Result);
		}

		public TResponse Post<TResponse>(string path) {
			Task<TResponse> task = this.PostAsync<object, TResponse>(path, null, this._cts.Token);
			task.Wait(this._cts.Token);
			return (task.Result);
		}

		public void Post<TRequest>(string path, TRequest content) {
			Task task = this.PostAsync(path, content, this._cts.Token);
			task.Wait(this._cts.Token);
		}

	}

}
