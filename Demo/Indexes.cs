using System.Collections.Generic;
using System.Linq;

using Battlesnake.PostGen.Language;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	public static class Indexes {

		public static IEnumerable<Table.Index> Data() {
			var table = Tables.Data().Skip(1).First();
			return new [] { 
				new Table.Index(
					null,
					table,
					new [] { table.columns.Skip(2).First() },
					false,
					new Expression.Raw("type == 'maris piper'")
				),
				new Table.Index(
					"potato_weight_index",
					table,
					new [] { table.columns.Skip(1).First() },
					false,
					null,
					Table.Index.Method.Hash
				)
			};
		}

		public static Block Make() {
			return Battlesnake.PostGen.CodeGenerator.TableIndexes.Generate(Data());
		}

	}
}

