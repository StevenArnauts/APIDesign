using ProductEntity = Server.Persistence.Product;

namespace Server.Domain {

	public class Product : DomainObject<ProductEntity> {

		public Product(ProductEntity entity) : base(entity) {}

		public string Name {
			get { return (this.Entity.Name); }
		}

		public decimal Price {
			get { return (this.Entity.Price); }
		}

		public int Stock {
			get { return (this.Entity.Stock); }
		}

	}

}
