namespace Server.Controllers {

	public class ProductRepresentation : EntityRepresentation {

		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }

	}

}