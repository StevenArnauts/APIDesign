using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Server {

	public class ChatHub : Hub {

		public void Send(string name, string text) {
			Console.WriteLine(this.Context.ConnectionId + " sent " + name + ": " + text);
			this.Clients.All.receive(name, text);
		}

		public override Task OnConnected() {
			Console.WriteLine(this.Context.ConnectionId + " connected");
			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled) {
			Console.WriteLine(this.Context.ConnectionId + " disconnected, stop = " + stopCalled);
			return base.OnDisconnected(stopCalled);
		}

		public override Task OnReconnected() {
			Console.WriteLine(this.Context.ConnectionId + " reconnected");
			return base.OnReconnected();
		}

	}

}
