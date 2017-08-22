using System;
using System.Linq;
using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;

namespace Battlesnake.PostGen.CodeGenerator
{

	public static class TypeName
	{

		public static readonly Template1 array_type = new Template ("%s[]");
		public static readonly Template1 setof_type = new Template ("SETOF %s");
		public static readonly Template2 reference_type = new Template ("%I.%I%%TYPE");

		public static readonly Template1 composite_type = new Template ("TABLE (%s)");
		public static readonly Template2 composite_type_field = new Template ("%I %I");

		public static string Generate (Language.Type type)
		{
			return Dispatch (type as dynamic);
		}

		private static string Dispatch (Language.Type.Basic type)
		{
			return type.name;
		}

		private static string Dispatch (Language.Type.Reference type)
		{
			return reference_type [type.table.name, type.column.name];
		}

		private static string Dispatch (Language.Type.Array type)
		{
			return array_type [Generate (type.type)];
		}

		private static string Dispatch (Language.Type.SetOf type)
		{
			return setof_type [Generate (type.type)];
		}

		private static string Dispatch (Language.Type.Composite type)
		{
			if (type.name != null) {
				return type.name;
			} else {
				return composite_type [String.Join (", ", from field in type.fields select composite_type_field [field.name, Generate (field.type)])];
			}
		}

	}

}