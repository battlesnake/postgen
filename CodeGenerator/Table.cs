using System.Collections.Generic;
using System.Linq;
using Battlesnake.PostGen;
using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;

namespace Battlesnake.PostGen.CodeGenerator {
	
	public static class Table {

		public static readonly Template1 create_table_open = new Template("CREATE TABLE %I (");
		public static readonly Template create_table_close = new Template(")");

		public static Block Generate(Language.Table table) {
			var block = new Block();
			var columns = table.columns.Select(column => TableColumn.Generate(column));
			var constraints = table.constraints.Select(constraint => TableConstraint.Generate(constraint));
			block += create_table_open[table.name];
			block %= Block.ConcatList(columns.Concat(constraints), ",", "");
			block += create_table_close;
			return block;
		}
	}

}