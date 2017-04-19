using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Utilities.Specifications {

	/// <summary>
	/// Helper for rebinder parameters without use Invoke method in expressions 
	/// ( this methods is not supported in all linq query providers, 
	/// for example in Linq2Entities is not supported)
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ParametersRebinder : ExpressionVisitor {

		private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

		/// <summary>
		/// Default construcotr
		/// </summary>
		/// <param name="map">Map specification</param>
		public ParametersRebinder(Dictionary<ParameterExpression, ParameterExpression> map) {
			this._map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
		}

		/// <summary>
		/// Replate parameters in expression with a Map information
		/// </summary>
		/// <param name="map">Map information</param>
		/// <param name="exp">Expression to replace parameters</param>
		/// <returns>Expression with parameters replaced</returns>
		public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp) {
			return new ParameterRebinder(map).Visit(exp);
		}

		/// <summary>
		/// Visit pattern method
		/// </summary>
		/// <param name="node">A Parameter expression</param>
		/// <returns>New visited expression</returns>
		protected override Expression VisitParameter(ParameterExpression node) {
			ParameterExpression replacement;
			if(this._map.TryGetValue(node, out replacement)) {
				node = replacement;
			}
			return base.VisitParameter(node);
		}

	}

}
