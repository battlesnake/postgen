using System.Collections.Generic;
using Battlesnake.PostGen.Language;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	public static class Functions {

		public static IEnumerable<Function> Data() {
			return new[] {
				new Function(
					"potato_on_change",
					new Type.Basic("TRIGGER"),
					null,
					new Code(
						"sql",
						new [] {
							"BEGIN",
							"\tPERFORM pg_notify('potato_observer', NEW.potato_id);",
							"END"
						})),
				new Function(
					"potato_on_huge_change",
					new Type.Basic("TRIGGER"),
					null,
					new Code(
						"sql",
						new [] {
							"BEGIN",
							"\tPERFORM pg_notify('potato_observer_huge', NEW.potato_id);",
							"END"
						}))
			};
		}

		public static Block Make() {
			return Battlesnake.PostGen.CodeGenerator.Functions.Generate(Data());
		}

	}
}

