using System;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using Server.Persistence;
using Utilities.Entities;
using Utilities.Logging;

namespace Service.Persistence {

	public class UnitOfWork : IUnitOfWork<IDatabaseContext> {

		private readonly IDbContextFactory<DatabaseContext> _factory = new DatabaseContextFactory();
		private readonly object _lock = new object();
		private IDatabaseContext _context;
		private bool _isDisposed;

		public IDatabaseContext Context {
			get {
				if (this._context == null) {
					lock (this._lock) {
						if (this._context == null) {
							DatabaseContext context = this._factory.Create();
							context.Database.Log = this.LogSql;
							this._context = context;
						}
					}
				}
				return (this._context);
			}
		}

		public void SaveChanges() {
			if (this._context != null) {
				this._context.SaveChanges();
			}
			Logger.Debug("Saved " + this.GetType().Name + " " + this.GetHashCode());
		}

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void LogSql(string sql) {
			if (string.IsNullOrWhiteSpace(sql)) {
				return;
			}
			if (sql.StartsWith("--")) {
				return;
			}
			if (sql.StartsWith("Opened")) {
				return;
			}
			if (sql.StartsWith("Closed")) {
				return;
			}
			string txt = Regex.Replace(sql, @"\s+", " ");
			if (!string.IsNullOrWhiteSpace(txt)) {
				Logger.Debug("SQL", txt);
			}
		}

		protected virtual void Dispose(bool disposing) {
			lock (this._lock) {
				if (!this._isDisposed) {
					if (disposing) {
						this.SaveChanges();
						if (this._context != null) {
							this._context.Dispose();
						}
					}
					this._isDisposed = true;
					Logger.Debug("Disposed " + this.GetType().Name + " " + this.GetHashCode() + "...");
				} else {
					Logger.Debug(this.GetType().Name + " " + this.GetHashCode() + " is already disposed");
				}
			}
		}

		public override string ToString() {
			return (this.GetType().Name + " " + this.GetHashCode());
		}

		~UnitOfWork() {
			this.Dispose(false);
		}

	}

}
