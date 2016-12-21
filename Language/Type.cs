using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.Language {

	public abstract class Type {

		public class Basic: Type {
			public string name;

			public Basic(string name) {
				this.name = name;
			}
		}

		public class Array: Type {
			public Basic type;

			public Array(Basic type) {
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
				public Basic type;

				public Field(string name, Basic type) {
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

}