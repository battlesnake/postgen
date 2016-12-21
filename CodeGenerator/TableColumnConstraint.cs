using System;
using System.Collections.Generic;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class TableColumnConstraint {

		private static readonly Template named = new Template("CONSTRAINT %I");
		private static readonly Template check = new Template("CHECK (%s)");
		private static readonly Template default_ = new Template("DEFAULT %s");
		private static readonly Template foreign = new Template("REFERENCES %I (%I)");
		private static readonly Template foreign_onupdate = new Template("ON UPDATE %s");
		private static readonly Template foreign_ondelete = new Template("ON DELETE %s");
		private static readonly Dictionary<Language.Table.Column.Constraint.ForeignKey.Action, string> foreign_actions = new Dictionary<Battlesnake.PostGen.Language.Table.Column.Constraint.ForeignKey.Action, string> {
			{ Language.Table.Column.Constraint.ForeignKey.Action.Restrict, "RESTRICT" },
			{ Language.Table.Column.Constraint.ForeignKey.Action.Cascade, "CASCADE" },
			{ Language.Table.Column.Constraint.ForeignKey.Action.Ignore, "IGNORE" }
		};
		private static readonly Template not_null = new Template("NOT NULL");
		private static readonly Template primary_key = new Template("PRIMARY KEY");
		private static readonly Template unique = new Template("UNIQUE");

		public static Block Generate(Language.Table.Column.Constraint constraint) {
			var def = Dispatch(constraint as dynamic);
			if (constraint.name == null) {
				return def;
			} else {
				var res = new Block();
				res += named[constraint.name];
				res %= def;
				return res;
			}
		}

		private static Block Dispatch(Language.Table.Column.Constraint.Check constraint) {
			return check[Expression.Generate(constraint.expression)];
		}

		private static Block Dispatch(Language.Table.Column.Constraint.Default constraint) {
			return default_[Expression.Generate(constraint.expression)];
		}

		private static Block Dispatch(Language.Table.Column.Constraint.ForeignKey constraint) {
			var res = new Block(foreign[constraint.reftable.name, constraint.refcolumn.name]);
			var actions = new Block();
			actions += foreign_onupdate[foreign_actions[constraint.on_update]];
			actions += foreign_ondelete[foreign_actions[constraint.on_delete]];
			res %= actions;
			return res;
		}

		private static Block Dispatch(Language.Table.Column.Constraint.NotNull constraint) {
			return not_null;
		}

		private static Block Dispatch(Language.Table.Column.Constraint.PrimaryKey constraint) {
			return primary_key;
		}

		private static Block Dispatch(Language.Table.Column.Constraint.Unique constraint) {
			return unique;
		}

	}
}

