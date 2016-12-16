using System.Collections.Generic;
using System.Linq;
using Battlesnake.PostGen;

namespace Battlesnake.PostGen.CodeGenerator {
	
	public static class Table {

		public static readonly Template create_table = new Template("CREATE TABLE %I (");

		public static Block Generate(Language.Table table) {
			var block = new Block();
			var columns = table.columns.Select(column => TableColumn.Generate(column));
			var constraints = table.constraints.Select(constraint => TableConstraint.Generate(constraint));
			block.Add(create_table[table.name]);
			block.Indent(Block.ConcatList(columns.Concat(constraints)));
			block.Add(");");
			return block;
		}
	}

}