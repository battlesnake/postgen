namespace Battlesnake.PostGen.Language {

	public partial class Table {

		public partial class Column {

			public abstract class Constraint {
				public string name;

				public Constraint(string name) {
					this.name = name;
				}

				public class Check: Constraint {
					public Expression expression;

					public Check(string name, Expression expression)
						: base(name) {
						this.expression = expression;
					}
				}

				public class NotNull: Constraint {
					public NotNull(string name)
						: base(name) {
					}
				}

				public class Unique: Constraint {
					public Unique(string name)
						: base(name) {
					}
				}

				public class Default: Constraint {
					public Expression expression;

					public Default(string name, Expression expression)
						: base(name) {
						this.expression = expression;
					}
				}

				public class PrimaryKey: Constraint {
					public PrimaryKey(string name)
						: base(name) {
					}
				}

				public class ForeignKey: Constraint {
					public Table reftable;
					public Column refcolumn;
					public Action on_update;
					public Action on_delete;

					public enum Action {
						Restrict = 0,
						Cascade,
						Ignore
					}

					public ForeignKey(
						string name,
						Table reftable,
						Column refcolumn,
						Action on_update = Action.Restrict,
						Action on_delete = Action.Restrict
					)
						: base(name) {
						this.reftable = reftable;
						this.refcolumn = refcolumn;
						this.on_update = on_update;
						this.on_delete = on_delete;
					}

				}

			}
		}
	}
}

