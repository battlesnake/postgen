using System;
using System.Collections.Generic;
using System.Linq;

using Template = Battlesnake.PostGen.CodeGenerator.Template;
using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;
using Template3 = Battlesnake.PostGen.CodeGenerator.Template<string, string, string>;
using System.Configuration;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class Trigger {
	
		public static readonly Template2 create = new Template("CREATE %s %I");
		public static readonly Template3 stimulus = new Template("%s %s ON %I");
		public static readonly Template events_delimiter = new Template(" OR ");
		public static readonly Template1 update_of = new Template(" OF %I");
		public static readonly Template1 from = new Template("FROM %I");
		public static readonly Template1 scope = new Template("FOR EACH %s");
		public static readonly Template1 condition = new Template("WHEN (%s)");
		public static readonly Template1 action = new Template("EXECUTE PROCEDURE %I()");

		public static readonly Dictionary<bool, string> trigger_type = new Dictionary<bool, string> {
			{ false, "TRIGGER" },
			{ true, "CONSTRAINT TRIGGER" }
		};
		public static readonly Dictionary<Language.Trigger.When, string> stimulus_type = new Dictionary<Language.Trigger.When, string> {
			{ Language.Trigger.When.Before, "BEFORE" },
			{ Language.Trigger.When.After, "AFTER" },
			{ Language.Trigger.When.InsteadOf, "INSTEAD OF" }
		};
		public static readonly Dictionary<Language.Trigger.Events, string> event_type = new Dictionary<Language.Trigger.Events, string> {
			{ Language.Trigger.Events.Insert, "INSERT" },
			{ Language.Trigger.Events.Update, "UPDATE" },
			{ Language.Trigger.Events.Delete, "DELETE" },
			{ Language.Trigger.Events.Truncate, "TRUNCATE" }
		};
		public static readonly Dictionary<Language.Trigger.Scope, string> scope_type = new Dictionary<Language.Trigger.Scope, string> {
			{ Language.Trigger.Scope.Row, "ROW" },
			{ Language.Trigger.Scope.Statement, "STATEMENT" }
		};

		public static Block Generate(Language.Trigger trigger) {
			var res = new Block();
			var blk = new Block();
			res += create[trigger_type[trigger.constraint], trigger.name];
			blk += stimulus[
				stimulus_type[trigger.when],
				GetEventsString(trigger.events, trigger.update_column),
				trigger.table.name
			];
			if (trigger.from != null) {
				blk += from[trigger.from.name];
			}
			blk += scope[scope_type[trigger.scope]];
			if (trigger.where != null) {
				blk += condition[Expression.Generate(trigger.where)];
			}
			blk += action[trigger.function.name];
			res %= blk;
			return res;
		}

		public static string GetEventsString(Language.Trigger.Events events, Language.Table.Column update_column) {
			var strings = new List<string>();
			var values = typeof(Language.Trigger.Events).GetEnumValues();
			if (update_column != null && (events & Battlesnake.PostGen.Language.Trigger.Events.Update) == 0) {
				throw new Exception("Update column specified for trigger but update event is not specified");
			}
			for (int i = 0; i < values.GetLength(0); i++) {
				var value = (Language.Trigger.Events)values.GetValue(i);
				if ((events & value) != 0) {
					string str = event_type[value];
					if (value == Battlesnake.PostGen.Language.Trigger.Events.Update && update_column != null) {
						str += update_of[update_column.name];
					}
					strings.Add(str);
				}
			}
			return String.Join(events_delimiter, strings);
		}
	
	}

}