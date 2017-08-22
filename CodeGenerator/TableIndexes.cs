using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class TableIndexes {

		public static Block Generate(IEnumerable<Language.Table.Index> indexes) {
			return Block.Concat(from index in indexes
				                   select TableIndex.Generate(index), ";", ";", true);
		}

	}
}

