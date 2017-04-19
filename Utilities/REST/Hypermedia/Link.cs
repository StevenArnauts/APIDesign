using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Utilities {

	public interface ILinkContainer {
		bool ContainsLinkName(string name);
	}

	public class LinkCollection : LinkedList<Link>, ILinkContainer {

		/// <summary>
		/// This method makes the link with the specified name the default link of the collection. This means this link is
		/// set as the first element of the collection.
		/// </summary>
		/// <param name="name"></param>
		public void MakeDefault(string name) {
			var defaultLink = this.Where(link => String.Compare(link.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0).FirstOrDefault();
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

	public class Link : ILinkContainer {

		public Link() { }

		public Link(string href, bool templated = false, string name = null) {
			Href = href;
			Name = name;
			Templated = templated;
			if(href != null)
				Templated = Regex.Match(href, @"{\w+}", RegexOptions.Compiled).Success;
		}

		public string Href { get; set; }
		public string Name { get; set; }
		public bool Templated { get; set; }

		/// <summary>
		/// If this link is templated, you can use this method to make a non templated copy
		/// </summary>
		/// <param name="parameters">The parameters, i.e 'new {id = "1"}'</param>
		/// <returns>A non templated link</returns>
		public Link CreateLink(object parameters) {
			return new Link(this.Href, false, this.CreateUri(parameters).ToString());
		}

		public Uri CreateUri(object parameters) {
			var href = Href;
			foreach(var substitution in parameters.GetType().GetProperties()) {
				var name = substitution.Name;
				var value = substitution.GetValue(parameters, null);
				var substituionValue = value == null ? null : Uri.EscapeDataString(value.ToString());
				href = href.Replace($"{{{name}}}", substituionValue);
			}
			return new Uri(href, UriKind.Relative);
		}

		public bool ContainsLinkName(string name) {
			return (false);
		}

	}

}