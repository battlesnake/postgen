using System.Linq;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class TableColumn {

		public static readonly Template column_name_type = new Template("%I %s");

		public static Block Generate(Language.Table.Column column) {
			var block = new Block();
			block += column_name_type[column.name, TypeName.Generate(column.type)];
			block %= Block.Concat(column.constraints.Select(constraint => TableColumnConstraint.Generate(constraint)));
			return block;
		}

	}
}

