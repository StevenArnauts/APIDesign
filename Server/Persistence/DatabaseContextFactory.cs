using System.Configuration;
using System.Data.Entity.Infrastructure;

namespace Server.Persistence {

	/// <summary>
	/// Use update-database -StartUpProjectName "Api" -ConnectionStringName "OrderDb" to run migrations on the database!
	/// </summary>
	public class DatabaseContextFactory : IDbContextFactory<DatabaseContext> {

		public DatabaseContext Create() {
			ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings["OrdersDb"];
			if(connectionStringSetting == null) throw new ConfigurationErrorsException("Connection string not configured");
			var connectionString = connectionStringSetting.ConnectionString;
			return new DatabaseContext(connectionString);
		}
	}

}