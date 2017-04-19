using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Utilities {

	/// <summary>
	/// VERY naive implementation of a patch, e.g. does not support nested properties.
	/// </summary>
	/// <typeparam name="TTargetType">The type of the target type.</typeparam>
	public class Patch<TTargetType> : DynamicObject where TTargetType : class {

		readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			string name = binder.Name.ToCamelCase();
			return _dictionary.TryGetValue(name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value) {
			_dictionary[binder.Name.ToCamelCase()] = value;
			return true;
		}

		/// <summary>
		/// Overwrites the <paramref name="original"/> entity with the changes tracked by this Delta.
		/// <remarks>The semantics of this operation are equivalent to a HTTP PATCH operation, hence the name.</remarks>
		/// </summary>
		/// <param name="original">The entity to be updated.</param>
		public void Apply(TTargetType original) {
			if(original == null) throw new ArgumentNullException("original");
			foreach(string propertyName in _dictionary.Keys) {
				PropertyInfo property = typeof(TTargetType).GetProperty(propertyName.ToPascalCase());
				var type = property.PropertyType;
				property.SetValue(original, Convert.ChangeType(_dictionary[propertyName], type));
			}
		}

	}

}
