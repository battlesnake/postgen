using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.Language {

	public abstract class Type {
	}

	public class BasicType: Type {
		public string name;

		public BasicType(string name) {
			this.name = name;
		}
	}

	public class Array: Type {
		public BasicType type;

		public Array(BasicType type) {
			this.type = type;
		}
	}

	public class SetOf: Type {
		public Type type;

		public SetOf(Type type) {
			this.type = type;
		}
	}

	public class Structure: Type {

		public class Field {
			public string name;
			public BasicType type;

			public Field(string name, BasicType type) {
				this.type = type;
			}
		}

		public string name;
		public List<Field> fields;

		public Structure(string name, IEnumerable<Field> fields) {
			this.name = name;
			this.fields = fields.ToList();
		}

		public Structure(string name, params Field[] fields)
			: this(name, fields.AsEnumerable()) {
		}
	}
}