using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities {

	public class LinkCollection : LinkedList<Link>, ILinkContainer {

		public LinkCollection(IEnumerable<Link> links = null) {
			links?.ForEach(l => this.AddLast(l));
		}

		/// <summary>
		/// This method makes the link with the specified name the default link of the collection. This means this link is
		/// set as the first element of the collection.
		/// </summary>
		/// <param name="name"></param>
		public void MakeDefault(string name) {
			Link defaultLink = this.FirstOrDefault(link => string.Equals(link.Name, name, StringComparison.InvariantCultureIgnoreCase));
			if(defaultLink != null) {
				this.Remove(defaultLink);
				this.AddFirst(defaultLink);
			}
		}

		public void MakeDefault(Link link) {
			if(link != null) {
				this.Remove(link);
				this.AddFirst(link);
			}
		}

		public bool ContainsLinkName(string name) {
			return (this.Any(l => l.Name == name));
		}

	}

}