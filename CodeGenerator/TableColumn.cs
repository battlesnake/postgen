using System.Linq;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class TableColumn {

		public static readonly Template2 column_name_type = new Template("%I %s");

		public static Block Generate(Language.Table.Column column) {
			var block = new Block();
			block += column_name_type[column.name, TypeName.Generate(column.type)];
			block %= from constraint in column.constraints
					 select TableColumnConstraint.Generate(constraint);
			return block;
		}

	}
}

