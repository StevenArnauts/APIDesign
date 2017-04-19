using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities {

	public static class ObjectExtensions {

		public static IDictionary<string, object> AsMap(this object source, Func<object, object> transformer = null) {
			Dictionary<string, object> result = new Dictionary<string, object>();
			foreach (PropertyInfo prop in source.GetType().GetProperties()) {
				string name = prop.Name;
				object value = prop.GetValue(source, null);
				if (transformer != null) value = transformer(value);
				result.Add(name, value);
			}
			return (result);
		}

	}

}