using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Utilities.Extensions;

namespace Utilities.REST {

	public class BaseFirstCamelCaseContractResolver : CamelCasePropertyNamesContractResolver {

		static BaseFirstCamelCaseContractResolver() {
			Instance = new BaseFirstCamelCaseContractResolver();
		}

		public static BaseFirstCamelCaseContractResolver Instance { get; }

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
			IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
			IList<JsonProperty> result = properties.Where(p => p.DeclaringType != typeof(Representation)).OrderBy(p => p.DeclaringType.BaseTypesAndSelf().Count()).ToList();
			properties.Where(p => p.DeclaringType == typeof(Representation)).ForEach(p => result.Add(p));
			return result;
		}

	}

}