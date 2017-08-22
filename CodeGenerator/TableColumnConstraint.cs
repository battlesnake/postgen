using System;
using System.Collections.Generic;

using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class TableColumnConstraint {

		private static readonly Template1 named = new Template("CONSTRAINT %I");
		private static readonly Template1 check = new Template("CHECK (%s)");
		private static readonly Template1 default_ = new Template("DEFAULT %s");
		private static readonly Template2 foreign = new Template("REFERENCES %I (%I)");
		private static readonly Template1 foreign_onupdate = new Template("ON UPDATE %s");
		private static readonly Template1 foreign_ondelete = new Template("ON DELETE %s");
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
			var res = new Block();
			res += foreign[constraint.reftable.name, constraint.refcolumn.name];
			var actions = new Block();
			actions += foreign_onupdate[foreign_actions[constraint.on_update]];
			actions += foreign_ondelete[foreign_actions[constraint.on_delete]];
			res %= actions;
			return res;
		}

		private static Block Dispatch(Language.Table.Column.Constraint.NotNull constraint) {
			return (string)not_null;
		}

		private static Block Dispatch(Language.Table.Column.Constraint.PrimaryKey constraint) {
			return (string)primary_key;
		}

		private static Block Dispatch(Language.Table.Column.Constraint.Unique constraint) {
			return (string)unique;
		}

	}
}

