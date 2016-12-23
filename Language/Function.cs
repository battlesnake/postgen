using System;
using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.Language {

	public class Function : Tags.TopLevel {
		
		public class Argument {
			public enum Direction {
				In = 0,
				Out,
				InOut
			}

			public string name;
			public Type.Basic type;
			public Direction direction;

			public Argument(string name, Type.Basic type, Direction direction = Direction.In) {
				this.name = name;
				this.type = type;
				this.direction = direction;
			}
		}

		public class Arguments {
			public List<Argument> list = new List<Argument>();

			public Arguments(IEnumerable<Argument> value) {
				list.AddRange(value);
			}

			public Arguments(params Argument[] value)
				: this(value.AsEnumerable()) {
			}
		}

		public enum Volatility {
			Volatile = 0,
			Immutable,
			Stable,
		}

		public enum Security {
			Invoker = 0,
			Definer,
		}

		public enum Parallel {
			Unsafe = 0,
			Restricted,
			Safe,
		}

		public string name;
		public Arguments arguments = new Arguments();
		public Type.Basic type;
		public Code code;
		public Security security;
		public List<string> search_path;
		public Boolean strict;
		public Volatility volatility;
		public Parallel parallel;

		public Function(
			string name,
			Type.Basic type,
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
			this.arguments = arguments ?? new Arguments();
			this.code = code;
			this.security = security;
			this.search_path = search_path?.ToList();
			this.strict = strict;
			this.volatility = volatility;
			this.parallel = parallel;
		}
	}
}