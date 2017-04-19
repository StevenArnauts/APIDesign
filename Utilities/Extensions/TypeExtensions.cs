using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Extensions {

	public static class TypeExtensions {

		public static bool Implements(this Type source, Type @interface) {
			if (@interface == null) return (false);
			if (!@interface.IsInterface) return (false);
			return (source.GetInterfaces().Contains(@interface));
		}

		public static IEnumerable<Type> BaseTypesAndSelf(this Type type) {
			while (type != null) {
				yield return type;
				type = type.BaseType;
			}
		}

	}

}
