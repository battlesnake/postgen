using System;

namespace Battlesnake.PostGen.CodeGenerator {
	
	public static class TableConstraint {
		
		public static Block Generate(Language.Table.Constraint constraint) {
			return Dispatch(constraint as dynamic);
		}

		private static Block Dispatch(Language.Table.Constraint.Check constraint) {
		}

		private static Block Dispatch(Language.Table.Constraint.Exclude constraint) {
		}

		private static Block Dispatch(Language.Table.Constraint.ForeignKey constraint) {
		}

		private static Block Dispatch(Language.Table.Constraint.PrimaryKey constraint) {
		}

		private static Block Dispatch(Language.Table.Constraint.Unique constraint) {
		}

	}
}

