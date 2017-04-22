using System;
using System.Net.Http;
using System.Reflection;

namespace Utilities {

	public class Link : ILinkContainer {

		private readonly HttpMethod _method;
		private readonly string _name;
		private readonly string _href;

		public Link() { }

		public Link(string href): this(href, HttpMethod.Get, null) { }

		public Link(string href, HttpMethod method) : this(href, method, null) { }

		public Link(string href, HttpMethod method, string name) {
			this._href = href;
			this._name = name;
			this._method = method;
		}

		public string Name {
			get { return (this._name); }
		}

		public string Href {
			get { return (this._href); }
		}

		public string Method {
			get { return (this._method.ToString()); }
		}

		/// <summary>
		/// If this link is templated, you can use this method to make a non templated copy
		/// </summary>
		/// <param name="parameters">The parameters, i.e 'new {id = "1"}'</param>
		/// <returns>A non templated link</returns>
		public Link CreateLink(object parameters) {
			return new Link(this.CreateUri(parameters).AbsoluteUri, this._method, this._name);
		}

		public Uri CreateUri(object parameters) {
			string href = this._href;
			foreach(PropertyInfo substitution in parameters.GetType().GetProperties()) {
				string name = substitution.Name;
				object value = substitution.GetValue(parameters, null);
				string substituionValue = value == null ? null : Uri.EscapeDataString(value.ToString());
				href = href.Replace($"{{{name}}}", substituionValue);
			}
			return new Uri(href, UriKind.Relative);
		}

		public bool ContainsLinkName(string name) {
			return (false);
		}

	}

}