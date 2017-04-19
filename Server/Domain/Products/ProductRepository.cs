using System;
using System.Collections.Generic;
using Server.Persistence;
using Utilities.Entities;
using ProductEntity = Server.Persistence.Product;

namespace Server.Domain {

	public interface IProductRepository {

		Product Get(Guid id);
		IEnumerable<Product> All();

	}

	public class ProductRepository : DomainRepository<Product, ProductEntity>, IProductRepository {

		public ProductRepository(IUnitOfWork<IDatabaseContext> unitOfWork) : base(unitOfWork.Context.Products) {}

	}

}