using System;
using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.Language {

	public partial class Table {

		public partial class Column {

			public string name;
			public Type type;
			public List<Constraint> constraints;

			public Column(string name, Type type, IEnumerable<Constraint> constraints) {
				this.name = name;
				this.type = type;
				this.constraints = constraints.ToList();
			}

			public Column(string name, Type type, params Constraint[] constraints)
				: this(name, type, constraints.AsEnumerable()) {
			}

		}
	}
}
