using System;
using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.Language {
	
	public partial class Table {
		
		public abstract class Constraint {
			public string name;

			public Constraint(string name = null) {
				this.name = name;
			}

			public class Check: Constraint {
				public Expression expression;

				public Check(string name, Expression expression)
					: base(name) {
					this.expression = expression;
				}
			}

			public class Unique: Constraint {
				public List<Column> columns;

				public Unique(string name, IEnumerable<Column> columns)
					: base(name) {
					this.columns = columns.ToList();
				}

				public Unique(string name, params Column[] columns)
					: this(name, columns.AsEnumerable()) {
				}
			}

			public class PrimaryKey: Constraint {
				public List<Column> columns;

				public PrimaryKey(string name, IEnumerable<Column> columns)
					: base(name) {
					this.columns = columns.ToList();
				}

				public PrimaryKey(string name, params Column[] columns)
					: this(name, columns.AsEnumerable()) {
				}
			}

			public class Exclude: Constraint {
				public class Element {
					public Column column;
					public BinaryOperator binary_operator;

					public Element(Column column, BinaryOperator binary_operator) {
						this.column = column;
						this.binary_operator = binary_operator;
					}
				}

				public Index.Method index_method;
				public List<Element> elements;
				public Expression where;

				public Exclude(Index.Method index_method, IEnumerable<Element> elements, Expression where = null) {
					this.index_method = index_method;
					this.elements = elements.ToList();
					this.where = where;
				}

				public Exclude(Index.Method index_method, Expression where, params Element[] elements)
					: this(
						index_method,
						elements,
						where
					) {
				}
			}

			public class ForeignKey: Constraint {
				public List<Column> columns;
				public Table reftable;
				public List<Column> refcolumns;
				public Action on_update;
				public Action on_delete;

				public enum Action {
					Restrict = 0,
					Cascade,
					Ignore
				}

				public ForeignKey(
					string name,
					IEnumerable<Column> columns,
					Table reftable,
					IEnumerable<Column> refcolumn,
					Action on_update = Action.Restrict,
					Action on_delete = Action.Restrict
				)
					: base(name) {
					this.columns = columns.ToList();
					this.reftable = reftable;
					this.refcolumns = refcolumns.ToList();
					this.on_update = on_update;
					this.on_delete = on_delete;
				}

				public ForeignKey(
					string name,
					Table reftable,
					IEnumerable<Tuple<Column, Column>> mappings,
					Action on_update = Action.Restrict,
					Action on_delete = Action.Restrict
				)
					: this(
						name,
						from map in mappings
						select map.Item1,
						reftable,
						from map in mappings
						select map.Item2,
						on_update,
						on_delete
					) {
				}

			}
		}
	}
}

