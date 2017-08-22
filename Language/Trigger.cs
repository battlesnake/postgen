using System;

namespace Battlesnake.PostGen.Language {

	public class Trigger : Tags.TopLevel {

		public enum When {
			Before,
			After,
			InsteadOf
		}

		public enum Events {
			Insert = 1,
			Update = 2,
			Delete = 4,
			Truncate = 8
		}

		public enum Scope {
			Statement,
			Row
		}

		public bool constraint;
		public string name;
		public When when;
		public Events events;
		public Table.Column update_column;
		public Table table;
		public Table from;
		public Scope scope;
		public Expression where;
		public Function function;

		public Trigger(
			string name,
			When when,
			Events events,
			Table table,
			Scope scope,
			Expression where,
			Function function,
			Table.Column update_column = null,
			bool constraint = false,
			Table from = null
		) {
			this.constraint = constraint;
			this.name = name;
			this.when = when;
			this.events = events;
			this.update_column = update_column;
			this.table = table;
			this.from = from;
			this.scope = scope;
			this.where = where;
			this.function = function;
		}

	}
}