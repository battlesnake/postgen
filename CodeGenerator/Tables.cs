using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class Tables {
	
		public static Block Generate(IEnumerable<Language.Table> tables) {
			return Block.ConcatList(from table in tables
				                       select Table.Generate(table), ";", ";", true);
		}

	}
}