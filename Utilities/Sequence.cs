using System.Threading;

namespace Utilities {

	public class Sequence {

		private int _next;

		public Sequence(int initialValue = 0) {
			this._next = initialValue - 1;
		}

		public int Next() {
			return Interlocked.Increment(ref this._next);
		}

	}

}