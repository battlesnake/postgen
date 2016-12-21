using System;
using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.CodeGenerator {
	
	public static class TableConstraint {

		private static readonly Template named = new Template("CONSTRAINT %I");
		private static readonly Template check = new Template("CHECK (%s)");
		private static readonly Template exclude_one = new Template("EXCLUDE USING %s (%s)");
		private static readonly Template exclude_many_open = new Template("EXCLUDE USING %s (");
		private static readonly Template exclude_many_close = new Template(")");
		private static readonly Template exclude_element = new Template("%I WITH %s");
		private static readonly Template exclude_where = new Template("WHERE (%s)");
		private static readonly Template foreign = new Template("FOREIGN KEY (%s) REFERENCES %I (%s)");
		private static readonly Template foreign_onupdate = new Template("ON UPDATE %s");
		private static readonly Template foreign_ondelete = new Template("ON DELETE %s");
		private static readonly Dictionary<Language.Table.Constraint.ForeignKey.Action, string> foreign_actions = new Dictionary<Battlesnake.PostGen.Language.Table.Constraint.ForeignKey.Action, string> {
			{ Language.Table.Constraint.ForeignKey.Action.Restrict, "RESTRICT" },
			{ Language.Table.Constraint.ForeignKey.Action.Cascade, "CASCADE" },
			{ Language.Table.Constraint.ForeignKey.Action.Ignore, "IGNORE" }
		};
		private static readonly Template primary_key = new Template("PRIMARY KEY (%s)");
		private static readonly Template unique = new Template("UNIQUE (%s)");

		public static Block Generate(Language.Table.Constraint constraint) {
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

		private static Block Dispatch(Language.Table.Constraint.Check constraint) {
			return check[Expression.Generate(constraint.expression)];
		}

		private static string ExcludeElementToString(Language.Table.Constraint.Exclude.Element element) {
			return exclude_element[element.column, element.binary_operator];
		}

		private static Block Dispatch(Language.Table.Constraint.Exclude constraint) {
			var res = new Block();
			var elements = constraint.elements;
			if (elements.Count == 1) {
				res += exclude_one[
					constraint.index_method,
					ExcludeElementToString(elements[0])
				];
			} else {
				var list = new Block(from element in elements
					                    select ExcludeElementToString(element));
				res += exclude_many_open[constraint.index_method];
				res %= list;
				res += exclude_many_close;
			}
			if (constraint.where != null) {
				res += exclude_where[Expression.Generate(constraint.where)];
			}
			return res;
		}

		private static Block Dispatch(Language.Table.Constraint.ForeignKey constraint) {
			var res = new Block(foreign[
					          ColumnList.Generate(constraint.columns),
					          constraint.reftable.name,
					          ColumnList.Generate(constraint.refcolumns)
				          ]);
			var actions = new Block();
			actions += foreign_onupdate[foreign_actions[constraint.on_update]];
			actions += foreign_ondelete[foreign_actions[constraint.on_delete]];
			res %= actions;
			return res;
		}

		private static Block Dispatch(Language.Table.Constraint.PrimaryKey constraint) {
			return primary_key[ColumnList.Generate(constraint.columns)];
		}

		private static Block Dispatch(Language.Table.Constraint.Unique constraint) {
			return unique[ColumnList.Generate(constraint.columns)];
		}

	}
}