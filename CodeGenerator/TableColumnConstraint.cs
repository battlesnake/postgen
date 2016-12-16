using System;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class TableColumnConstraint {

		public static Block Generate(Language.Table.Column.Constraint constraint) {
			return Dispatch(constraint as dynamic);
		}

		private static Block Dispatch(Language.Table.Column.Constraint.Check constraint) {
		}

		private static Block Dispatch(Language.Table.Column.Constraint.Default constraint) {
		}

		private static Block Dispatch(Language.Table.Column.Constraint.ForeignKey constraint) {
		}

		private static Block Dispatch(Language.Table.Column.Constraint.NotNull constraint) {
		}

		private static Block Dispatch(Language.Table.Column.Constraint.PrimaryKey constraint) {
		}

		private static Block Dispatch(Language.Table.Column.Constraint.Unique constraint) {
		}

	}
}

