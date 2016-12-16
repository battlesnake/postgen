namespace Battlesnake.PostGen.Language {

	public class Expression {

		public class Raw: Expression {
			public string expression;

			public Raw(string expression) {
				this.expression = expression;
			}
		}

	}
}