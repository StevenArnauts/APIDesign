using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities {

	public static class ReflectionHelper {

		public static bool IsListOf(this Type source, Type listType) {
			Type[] genericTypeArguments = source.GenericTypeArguments;
			if (genericTypeArguments != null && genericTypeArguments.Count() == 1) {
				Type genericType = source.GetGenericTypeDefinition();
				bool isList = genericType.IsAssignableFrom(typeof (IEnumerable<>));
				bool hasSingleGenericType = genericTypeArguments.Count() == 1;
				bool isListOfSubtypeOf = genericTypeArguments.ElementAt(0).IsSubclassOf(listType);
				return (isList && hasSingleGenericType && isListOfSubtypeOf);
			}
			return false;
		}

		/// <summary>
		///     Finds all types in the specified assemblies that satisfy the condition.
		/// </summary>
		public static IEnumerable<Type> FindTypes(Predicate<Type> condition, params Assembly[] assemblies) {
			foreach (Assembly assembly in assemblies) {
				foreach (Type type in assembly.GetTypes()) {
					if (condition(type)) yield return (type);
				}
			}
		}

		public static bool Implements(this Type source, Type interfaceToImplement) {
			if (interfaceToImplement == null) throw new ArgumentNullException("interfaceToImplement");
			if (!interfaceToImplement.IsInterface) throw new ArgumentException("argument must be an interface type");
			return (source.GetInterfaces().Contains(interfaceToImplement));
		}

		public static Type GetInterfaceThatImplements(this Type source, Type interfaceToImplement) {
			if (interfaceToImplement == null) throw new ArgumentNullException("interfaceToImplement");
			if (!interfaceToImplement.IsInterface) throw new ArgumentException("argument must be an interface type");
			foreach (Type implementedInterface in source.GetInterfaces()) {
				if (implementedInterface.Implements(interfaceToImplement)) return (implementedInterface);
			}
			return (null);
		}

		public static ConstructorInfo GetConstructorThatAccepts(this Type source, params Type[] argumentTypes) {
			if (argumentTypes == null) throw new ArgumentNullException("argumentTypes");
			foreach (ConstructorInfo constructor in source.GetConstructors()) {
				ParameterInfo[] parameters = constructor.GetParameters();
				if (parameters.Count() != argumentTypes.Length) continue;
				bool parametersMatch = true;
				for (int i = 0; i < argumentTypes.Length; i ++) parametersMatch = parametersMatch && (argumentTypes[i].IsAssignableFrom(parameters[i].ParameterType));
				if (parametersMatch) return (constructor);
			}
			return (null);
		}

		public static string GetMemberName<T>(Expression<Func<T, object>> selection) where T : class {
			return (GetMember(selection).Name);
		}

		public static MemberInfo GetMember<T>(Expression<Func<T, object>> selection) where T : class {
			MemberExpression e = GetMemberInfo(selection);
			return (e.Member);
		}

		private static MemberExpression GetMemberInfo(Expression method) {
			var lambda = method as LambdaExpression;
			if (lambda == null) throw new ArgumentNullException("method");

			MemberExpression memberExpr = null;

			if (lambda.Body.NodeType == ExpressionType.Convert) {
				memberExpr = ((UnaryExpression) lambda.Body).Operand as MemberExpression;
			} else if (lambda.Body.NodeType == ExpressionType.MemberAccess) {
				memberExpr = lambda.Body as MemberExpression;
			}

			if (memberExpr == null) throw new ArgumentException("method");

			return memberExpr;
		}

		public static string GetPropertyName<T>(Expression<Func<T, object>> selection) where T : class {
			return (GetProperty(selection).Name);
		}

		public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> selection) where T : class {
			MemberExpression e = GetPropertyInfo(selection);
			if (e.Member.MemberType == MemberTypes.Property) {
				var f = (PropertyInfo) e.Member;
				return (f);
			}
			throw new ArgumentException("That is not a property but a method");
		}

		private static MemberExpression GetPropertyInfo(Expression method) {
			var lambda = method as LambdaExpression;
			if (lambda == null) throw new ArgumentNullException("method");
			MemberExpression memberExpr = null;
			if (lambda.Body.NodeType == ExpressionType.Convert) {
				memberExpr = ((UnaryExpression) lambda.Body).Operand as MemberExpression;
			} else if (lambda.Body.NodeType == ExpressionType.MemberAccess) {
				memberExpr = lambda.Body as MemberExpression;
			}
			if (memberExpr == null) throw new ArgumentException("method");
			return memberExpr;
		}

	}

}