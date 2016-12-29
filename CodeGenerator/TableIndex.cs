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
		public static readonly Template1 using_method = new Template("USING %s (");
		public static readonly Template3 layer_expression = new Template("(%s) %s %s");
		public static readonly Template3 layer_column = new Template("%I %s %s");
		public static readonly Template after_layers = new Template(")");
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
		public static readonly Dictionary<Language.Table.Index.Layer.Order, string> layer_order = new Dictionary<Battlesnake.PostGen.Language.Table.Index.Layer.Order, string> {
			{ Language.Table.Index.Layer.Order.Ascending, "ASC" },	
			{ Language.Table.Index.Layer.Order.Descending, "DESC" }
		};
		public static readonly Dictionary<Language.Table.Index.Layer.Nulls, string> layer_nulls = new Dictionary<Battlesnake.PostGen.Language.Table.Index.Layer.Nulls, string> {
			{ Language.Table.Index.Layer.Nulls.First, "NULLS FIRST" },	
			{ Language.Table.Index.Layer.Nulls.Last, "NULLS LAST" }
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
			res %= Block.Concat(from layer in index.layers
				                   select TableIndexLayer.Generate(layer), ", ", "") >> 1;
			res %= after_layers;
			return res;
		}

		public static class TableIndexLayer {
			
			// Analysis disable once MemberHidesStaticFromOuterClass
			public static Block Generate(Language.Table.Index.Layer layer) {
				return Dispatch(layer, layer.what as dynamic);
			}

			private static Block Dispatch(Language.Table.Index.Layer layer, Language.Expression expression) {
				return layer_expression[Expression.Generate(expression), layer_order[layer.order], layer_nulls[layer.nulls]];
			}

			private static Block Dispatch(Language.Table.Index.Layer layer, Language.Table.Column column) {
				return layer_column[column.name, layer_order[layer.order], layer_nulls[layer.nulls]];
			}
		}

	}

}