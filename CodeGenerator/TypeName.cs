using System;

namespace Battlesnake.PostGen.CodeGenerator {


	public static class TypeName {

		public static string Generate(Language.Type type) {
			return Dispatch(type as dynamic);
		}

		private static string Dispatch(Language.BasicType type) {
			return type.name;
		}

	}

}