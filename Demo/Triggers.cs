using System.Linq;
using System.Collections.Generic;
using Battlesnake.PostGen.Language;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	public static class Triggers {

		public static IEnumerable<Trigger> Data() {
			var potato = Tables.Data().Skip(1).First();
			var potato_func1 = Functions.Data().First();
			var potato_func2 = Functions.Data().Skip(1).First();
			return new [] {
				new Trigger(
					"potato_observer_notifier",
					Trigger.When.After,
					Trigger.Events.Update | Trigger.Events.Insert,
					potato,
					Trigger.Scope.Row,
					null,
					potato_func1
				),
				new Trigger(
					"potato_observer_huge_notifier",
					Trigger.When.Before,
					Trigger.Events.Update | Trigger.Events.Insert,
					potato,
					Trigger.Scope.Statement,
					new Expression.Raw("weight > 3"),
					potato_func2,
					potato.columns[2]
				)
			};
		}

		public static Block Make() {
			return Block.ConcatList(new [] { Battlesnake.PostGen.CodeGenerator.Triggers.Generate(Data()) }, ";", ";", true);
		}
	}

}

