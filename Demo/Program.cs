using System;
using Battlesnake.PostGen.Language;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	class MainClass {

		public static Block MakeFunction() {
			var func = new Function(
				           "potato",
				           new BasicType("TEXT"),
				           new Function.Arguments(
					           new Function.Argument(
						           "input",
						           new BasicType("text")
					           ),
					           new Function.Argument(
						           "other",
						           new BasicType("text")
					           )
				           ),
				           new Code("sql", "select (input || other);"),
				           Function.Security.Definer,
				           new [] { "extensions", "public" },
				           true,
				           Function.Volatility.Immutable,
				           Function.Parallel.Safe
			           );
			return Battlesnake.PostGen.CodeGenerator.Function.Generate(func);
		}

		public static Block MakeTable() {
			var types = new Table(
				            "potato_type",
				            new [] {
					new Table.Column(
						"type",
						new BasicType("TEXT"),
						new Table.Column.Constraint [] {
							new Table.Column.Constraint.PrimaryKey(null)
						}
					)
				});
			var table = new Table("potatoes",
				            new [] {
					new Table.Column("potato_id", new BasicType("UUID"), new Table.Column.Constraint[] {
							new Table.Column.Constraint.Default(null, new Expression.Raw("uuid_generate_v4()"))
						}),
					new Table.Column("potato_type", new BasicType("TEXT"), new Table.Column.Constraint[] {
							new Table.Column.Constraint.ForeignKey(
								null,
								types,
								types.columns[0],
								Table.Column.Constraint.ForeignKey.Action.Cascade
							)
						}),
					new Table.Column("potato_weight", new BasicType("FLOAT"))
				},
				            new Table.Constraint[] {
					new Table.Constraint.Check(null, new Expression.Raw("potato_weight > 0.8"))
				});
			table.constraints.Add(new Table.Constraint.PrimaryKey("pk", table.columns[0]));
			return Battlesnake.PostGen.CodeGenerator.Tables.Generate(new [] { types, table });
		}

		public static void Main(string[] args) {
			Func<Block> subject = MakeTable;
			Console.WriteLine(subject().ToString("    "));
			Console.ReadLine();
		}
	}
}