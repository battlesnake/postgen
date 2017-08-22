using Battlesnake.PostGen.CodeGenerator;

namespace Demo {

	public static class Auto {

		public static Block Make() {
			var auto = new Battlesnake.PostGen.CodeGenerator.Auto();
			auto += Types.Data();
			auto += Tables.Data();
			auto += Functions.Data();
			auto += Indexes.Data();
			auto += Triggers.Data();
			return auto.Generate();
		}

	}

}

