using System;
using System.Collections.Generic;
using System.Linq;

using Template = Battlesnake.PostGen.CodeGenerator.Template;
using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;
using Template3 = Battlesnake.PostGen.CodeGenerator.Template<string, string, string>;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class TableIndex {

		public static readonly Template1 create = new Template("CREATE %s");
		public static readonly Template2 create_named = new Template("CREATE %s %I");
		public static readonly Template1 on_table = new Template("ON %I");
		public static readonly Template1 using_method = new Template("USING %s");
		public static readonly Template1 on_columns = new Template("(%s)");
		public static readonly Template1 on_column = new Template("%I");
		public static readonly Template on_column_delimiter = new Template(", ");
		public static readonly Dictionary<Boolean, string> index_type = new Dictionary<bool, string> {
			{ false, "INDEX" },
			{ true, "UNIQUE INDEX" }
		};
		public static readonly Dictionary<Language.Table.Index.Method, string> index_method = new Dictionary<Battlesnake.PostGen.Language.Table.Index.Method, string> {
			{ Language.Table.Index.Method.BRIN, "brin" },	
			{ Language.Table.Index.Method.B_tree, "btree" },	
			{ Language.Table.Index.Method.GIN, "gin" },	
			{ Language.Table.Index.Method.GiST, "gist" },	
			{ Language.Table.Index.Method.Hash, "hash" },	
			{ Language.Table.Index.Method.SP_GiST, "spgist" }
		};

		public static Block Generate(Language.Table.Index index) {
			var res = new Block();
			if (index.name == null) {
				res += create[index_type[index.unique]];
			} else {
				res += create_named[index_type[index.unique], index.name];
			}
			res %= on_table[index.table.name];
			res %= using_method[index_method[index.method]];
			res %= on_columns[String.Join(on_column_delimiter,
					from column in index.columns
					select on_column[column.name])];
			return res;
		}

	}

}