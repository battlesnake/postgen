using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class Functions {

		public static Block Generate(IEnumerable<Language.Function> functions) {
			return Block.Concat(from function in functions
				                   select Function.Generate(function), ";", ";", true);
		}

	}
}