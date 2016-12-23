using System;
using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.Language {

	public partial class Table : Tags.TopLevel {
		public string name;
		public List<Column> columns;
		public List<Constraint> constraints = new List<Constraint>();

		public Column this [string name] {
			get {
				return columns.Find(col => col.name == name);
			}
		}

		public Table(string name, IEnumerable<Column> columns, IEnumerable<Constraint> constraints = null) {
			this.name = name;
			this.columns = columns.ToList();
			if (constraints != null) {
				this.constraints.AddRange(constraints);
			}
		}

		public Table(string name, params Column[] columns)
			: this(name, columns.AsEnumerable()) {
		}

		public Table addConstraints(params Constraint[] constraints) {
			if (this.constraints == null) {
				this.constraints = new List<Constraint>();
			}
			this.constraints.AddRange(constraints);
			return this;
		}

		public static void assertOwnership(Table table, IEnumerable<Column> columns) {
			var diff = (from column in columns.Except(table.columns)
			            select column.name).ToArray();
			if (diff.Length > 0) {
				throw new InvalidOperationException("Table " + table.name + " does not contain columns: " + String.Join(", ", diff));
			}
		}
	}
}