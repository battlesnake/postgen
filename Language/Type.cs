using System.Collections.Generic;
using System.Linq;
using Battlesnake.PostGen.Language.Tags;

namespace Battlesnake.PostGen.Language {

	public abstract class Type {

		/* Base / domain (also [ab]use for polymorphic types) */
		public class Basic : Type {
			public string name;

			public Basic(string name) {
				this.name = name;
			}
		}

		/* Reference to column type */
		public class Reference : Type {
			public Table table;
			public Table.Column column;

			public Reference(Table table, Table.Column column) {
				this.table = table;
				this.column = column;
			}
		}

		/* Array of base / domain */
		public class Array : Type {
			public Basic type;

			public Array(Basic type) {
				this.type = type;
			}
		}

		/* Set-of base / domain / composite */
		public class SetOf : Type {
			public Type type;

			public SetOf(Type type) {
				this.type = type;
			}
		}

		/* Composite (explicitly named or anonymous "TABLE" type) */
		public class Composite : Type, TopLevel {

			public class Field {
				public string name;
				public Basic type;

				public Field(string name, Basic type) {
					this.name = name;
					this.type = type;
				}
			}

			public string name;
			public List<Field> fields;

			public Composite(string name, IEnumerable<Field> fields) {
				this.name = name;
				this.fields = fields.ToList();
			}

			public Composite(string name, params Field[] fields)
				: this(name, fields.AsEnumerable()) {
			}

			public Composite(IEnumerable<Field> fields) : this(null, fields) {
			}

			public Composite(params Field[] fields)
				: this(null, fields) {
			}
		}

	}

}