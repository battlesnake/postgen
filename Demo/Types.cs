using System.Collections.Generic;
using Battlesnake.PostGen.Language;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	public static class Types {
		public static IEnumerable<Type.Composite> Data() {
			return new[] {
				new Type.Composite ("some_composite",
					new Type.Composite.Field ("name", new Type.Basic ("TEXT")),
					new Type.Composite.Field ("id", new Type.Basic ("UUID"))
			   ),
				new Type.Composite ("some_other_composite",
					new Type.Composite.Field ("name", new Type.Basic ("TEXT")),
					new Type.Composite.Field ("id", new Type.Basic ("UUID")),
					new Type.Composite.Field ("created", new Type.Basic ("TIMESTAMP"))
			   )
			};
		}
	}
}