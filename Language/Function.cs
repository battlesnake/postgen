using System;
using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.Language {

	public class Argument {
		public enum Direction {
			Default = 0,
			In = 1,
			Out = 2,
			InOut = In | Out
		}

		public string name;
		public BasicType type;
		public Direction direction;

		public Argument(string name, BasicType type, Direction direction = Direction.Default) {
			this.name = name;
			this.type = type;
			this.direction = direction;
		}
	}

	public class Arguments {
		public List<Argument> list { get; set; }

		public Arguments(IEnumerable<Argument> value) {
			list = value.ToList();
		}

		public Arguments(params Argument[] value)
			: this(value.AsEnumerable()) {
		}
	}

	public class Function {
		public enum Volatility {
			Volatile,
			Immutable,
			Stable,
		}

		public enum Security {
			Invoker = 0,
			Definer,
		}

		public enum Parallel {
			Unsafe,
			Restricted,
			Safe,
		}

		public string name;
		public Arguments arguments;
		public BasicType type;
		public Code code;
		public Security security;
		public List<string> search_path;
		public Boolean strict;
		public Volatility volatility;
		public Parallel parallel;

		public Function(
			string name,
			BasicType type,
			Arguments arguments,
			Code code,
			Security security = Security.Invoker,
			IEnumerable<string> search_path = null,
			Boolean strict = false,
			Volatility volatility = Volatility.Volatile,
			Parallel parallel = Parallel.Unsafe
		) {
			this.name = name;
			this.type = type;
			this.arguments = arguments;
			this.code = code;
			this.security = security;
			this.search_path = search_path?.ToList();
			this.strict = strict;
			this.volatility = volatility;
			this.parallel = parallel;
		}
	}
}