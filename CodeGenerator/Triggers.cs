using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class Triggers {

		public static Block Generate(IEnumerable<Language.Trigger> triggers) {
			return Block.ConcatList(from trigger in triggers
				                       select Trigger.Generate(trigger), ";", ";", true);
		}

	}

}