using System;
using System.Linq;
using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class Type {

		public static readonly Template1 composite_type_open = new Template("CREATE TYPE %I AS (");
		public static readonly Template composite_type_close = new Template(")");
		public static readonly Template2 composite_type_field = new Template("%I %I");

		public static Block Generate(Language.Type.Composite type) {
			var res = new Block();
			res += composite_type_open[type.name];
			res %= Block.Concat(from field in type.fields select GenerateField(field), ", ", "");
			res += composite_type_close;
			return res;
		}

		private static Block GenerateField(Language.Type.Composite.Field field) {
			return composite_type_field[field.name, TypeName.Generate(field.type)];
		}

	}

}