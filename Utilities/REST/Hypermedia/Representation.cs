using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Utilities {

	public class HyperMediaSettings {

		public Action<Representation> Factory { get; set; }
		public bool Include { get; set; }

	}

	public class Representation {

		public Representation() {
			this.Links = new Dictionary<string, ILinkContainer>();
			this.HyperMedia = new HyperMediaSettings();
		}

		[JsonProperty("_links")]
		public Dictionary<string, ILinkContainer> Links { get; }

		[JsonIgnore]
		public HyperMediaSettings HyperMedia { get; set; }

		public bool ShouldSerializeLinks() {
			return (this.HyperMedia.Include && this.Links.Count > 0);
		}

		public ILinkContainer AddLink(string name, ILinkContainer link) {
			this.Links.Add(name, link);
			return (link);
		}

		public ILinkContainer AddLink(string name, string href) {
			return (this.AddLink(name, href, HttpMethod.Get));
		}

		public ILinkContainer AddLink(string name, string href, HttpMethod method) {
			Link link = new Link(href, method);
			this.Links.AddSafe(name, link);
			return (link);
		}

	}

}