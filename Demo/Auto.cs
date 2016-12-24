using System.Linq;
using System.Collections.Generic;
using Battlesnake.PostGen.CodeGenerator;

namespace Demo {

	public static class Auto {
	
		public static Block Make() {
			var auto = new Battlesnake.PostGen.CodeGenerator.Auto();
			auto += Tables.Data();
			auto += Functions.Data();
			auto += Indexes.Data();
			auto += Triggers.Data();
			return auto.Generate();
		}

	}

}

