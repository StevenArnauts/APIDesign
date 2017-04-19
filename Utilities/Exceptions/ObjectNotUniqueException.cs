namespace Utilities {

	/// <summary>
	/// Is thrown when more than one item in the collection statisfied the condition
	/// </summary>
	public class ObjectNotUniqueException : QueryException {

		public ObjectNotUniqueException() {}
		public ObjectNotUniqueException(string message) : base(message) {}

	}

}