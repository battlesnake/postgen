using System;
using System.Linq;
using System.Collections.Generic;
using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;
using Template3 = Battlesnake.PostGen.CodeGenerator.Template<string, string, string>;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class Function {

		private static readonly Template2 function_open = new Template("CREATE FUNCTION %I(%s)");
		private static readonly Template1 function_returns = new Template("RETURNS %s");
		private static readonly Template1 function_language = new Template("LANGUAGE %s");
		private static readonly Template function_strict = new Template("STRICT");
		private static readonly Template1 function_volatile = new Template("%s");
		private static readonly Template1 function_parallel = new Template("PARALLEL %s");
		private static readonly Template1 function_security = new Template("SECURITY %s");
		private static readonly Template2 function_config = new Template("SET %s = %s");
		private static readonly Template function_code_open = new Template("AS $postgen$");
		private static readonly Template function_code_close = new Template("$postgen$");
		private static readonly Dictionary<Language.Function.Volatility, string> function_volatility = new Dictionary<Battlesnake.PostGen.Language.Function.Volatility, string> {
			{ Language.Function.Volatility.Immutable, "IMMUTABLE" },
			{ Language.Function.Volatility.Stable, "STABLE" },
			{ Language.Function.Volatility.Volatile, "VOLATILE" },
		};
		private static readonly Dictionary<Language.Function.Parallel, string> function_parallels = new Dictionary<Battlesnake.PostGen.Language.Function.Parallel, string> {
			{ Language.Function.Parallel.Restricted, "RESTRICTED" },
			{ Language.Function.Parallel.Safe, "SAFE" },
			{ Language.Function.Parallel.Unsafe, "UNSAFE" },
		};
		private static readonly Dictionary<Language.Function.Security, string> function_securities = new Dictionary<Battlesnake.PostGen.Language.Function.Security, string> {
			{ Language.Function.Security.Definer, "DEFINER" },
			{ Language.Function.Security.Invoker, "INVOKER" },
		};

		public static Block Generate(Language.Function function) {
			var res = new Block();
			res += function_open[function.name, Arguments.Generate(function.arguments)];
			var attrs = new Block();
			attrs += function_returns[function.type.name];
			attrs += function_language[function.code.language];
			if (function.strict) {
				attrs += function_strict;
			}
			attrs += function_volatile[function_volatility[function.volatility]];
			attrs += function_parallel[function_parallels[function.parallel]];
			attrs += function_security[function_securities[function.security]];
			if ((function.search_path?.Count).GetValueOrDefault(0) > 0) {
				attrs += function_config["search_path", Quote.Array(", ", Quote.Specifier.Identifier, function.search_path)];
			}
			res %= attrs;
			res += function_code_open;
			res %= Block.From(function.code.code);
			res += function_code_close;
			return res;
		}

		public static class Arguments {

			// Analysis disable once MemberHidesStaticFromOuterClass
			public static string Generate(Language.Function.Arguments arguments) {
				return String.Join(", ", from argument in arguments.list
										 select Argument.Generate(argument));
			}

		}

		public static class Argument {

			private static readonly Template3 function_argument = new Template("%s %s %s");
			private static readonly Dictionary<Language.Function.Argument.Direction, string> function_argument_direction = new Dictionary<Battlesnake.PostGen.Language.Function.Argument.Direction, string> {
				{ Language.Function.Argument.Direction.In, "IN" },
				{ Language.Function.Argument.Direction.Out, "OUT" },
				{ Language.Function.Argument.Direction.InOut, "INOUT" },
			};

			// Analysis disable once MemberHidesStaticFromOuterClass
			public static string Generate(Language.Function.Argument argument) {
				return function_argument[function_argument_direction[argument.direction], argument.name, argument.type.name];
			}

		}

	}
}