using Battlesnake.PostGen.Language;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	public static class Tables {
	
		public static Block Make() {
			var types = new Table(
				            "potato_type",
				            new [] {
					new Table.Column(
						"type",
						new Type.Basic("TEXT"),
						new Table.Column.Constraint [] {
							new Table.Column.Constraint.PrimaryKey(null)
						}
					)
				});
			var table = new Table("potatoes",
				            new [] {
					new Table.Column("potato_id", new Type.Basic("UUID"), new Table.Column.Constraint[] {
							new Table.Column.Constraint.Default(null, new Expression.Raw("uuid_generate_v4()"))
						}),
					new Table.Column("potato_type", new Type.Basic("TEXT"), new Table.Column.Constraint[] {
							new Table.Column.Constraint.ForeignKey(
								null,
								types,
								types.columns[0],
								Table.Column.Constraint.ForeignKey.Action.Cascade
							)
						}),
					new Table.Column("potato_weight", new Type.Basic("FLOAT"))
				},
				            new Table.Constraint[] {
					new Table.Constraint.Check(null, new Expression.Raw("potato_weight > 0.8"))
				});
			table.constraints.Add(new Table.Constraint.PrimaryKey("pk", table.columns[0]));
			return Battlesnake.PostGen.CodeGenerator.Tables.Generate(new [] { types, table });
		}

	}
}

