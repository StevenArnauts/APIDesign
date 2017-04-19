using System;

namespace Server.Controllers {

	public class MessageRepresentation {

		public int Sequence { get; set; }
		public DateTime Date { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }

	}

}