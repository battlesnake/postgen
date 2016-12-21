using Battlesnake.PostGen.Language;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	public static class Functions {
	
		public static Block Make() {
			var func = new Function(
				           "potato",
				           new Type.Basic("TEXT"),
				           new Function.Arguments(
					           new Function.Argument(
						           "input",
						           new Type.Basic("TEXT")
					           ),
					           new Function.Argument(
						           "other",
						           new Type.Basic("TEXT")
					           )
				           ),
				           new Code("sql", "select (input || other);"),
				           Function.Security.Definer,
				           new [] { "extensions", "public" },
				           true,
				           Function.Volatility.Immutable,
				           Function.Parallel.Safe
			           );
			return Battlesnake.PostGen.CodeGenerator.Functions.Generate(new [] { func });
		}

	}
}

