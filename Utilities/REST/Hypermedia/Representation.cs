using System;
using System.Collections.Generic;
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
		
		public bool ShouldSerializeLinks() {
			return (this.HyperMedia.Include && this.Links.Count > 0);
		}

		[JsonIgnore]
		public HyperMediaSettings HyperMedia { get; set; }
		
		public void AddActionLink(string name, Link link) {
			//LinkCollection actionLinks = new LinkCollection();
			//if (!this.Links.ContainsKey("actions")) {
			//	this.Links["actions"] = actionLinks;
			//}
			this.Links.AddSafe(name, link);
		}

	}

}