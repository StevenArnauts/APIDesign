using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Server.Persistence {

	public class EntityMap<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : Entity {

		public EntityMap() {
			this.Property(x => x.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			this.Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
			this.Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
			this.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsRequired();
			this.Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsRequired();
			this.HasKey(x => x.Id);
		}

	}

}