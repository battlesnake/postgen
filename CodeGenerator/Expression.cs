using System;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class Expression {

		public static Block Generate(Language.Expression expression) {
			return Dispatch(expression as dynamic);
		}

		private static Block Dispatch(Language.Expression.Raw expression) {
			return expression.expression;		
		}

	}
}

