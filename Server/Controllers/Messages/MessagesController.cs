using System;
using System.Collections.Generic;
using System.Web.Http;
using Utilities;

namespace Server.Controllers {

	[RoutePrefix("messages")]
	public class MessagesController : ApiController {

		private static readonly Sequence sequence = new Sequence(1);
		private static readonly List<MessageRepresentation> messages = new List<MessageRepresentation>();

		static MessagesController() {
			CreateMessage("Steven", "Hello", DateTime.Now.AddDays(-1));
			CreateMessage("Thomas", "Colruyt", DateTime.Now.AddDays(-10));
			CreateMessage("Steven", "Carrefour", DateTime.Now.AddDays(-5));
		}

		[Route("")]
		[HttpGet]
		public IEnumerable<MessageRepresentation> All() {
			return (messages);
		}

		[Route("")]
		[HttpPost]
		public void New(MessageSpecification specification) {
			CreateMessage(specification.Name, specification.Text, DateTime.Now);
		}

		private static void CreateMessage(string name, string text, DateTime date) {
			MessageRepresentation message = new MessageRepresentation {
				Sequence = sequence.Next(), Name = name, Text = text, Date = date
			};
			messages.Add(message);
		}

	}

}
