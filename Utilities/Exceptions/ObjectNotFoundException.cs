namespace Utilities {

	/// <summary>
	/// Is thrown when no single item in the collection satisfied the condition
	/// </summary>
	public class ObjectNotFoundException : QueryException {

		public ObjectNotFoundException() {}
		public ObjectNotFoundException(string message) : base(message) {}

	}

}