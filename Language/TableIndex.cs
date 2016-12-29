using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.Language {

	public partial class Table {
	
		public class Index : Tags.TopLevel {

			public class Layer {

				public enum Order {
					Ascending = 0,
					Descending
				}

				public enum Nulls {
					Last = 0,
					First
				}

				public object what { get; private set; }

				public Expression expression { get { return what as Expression; } set { what = value; } }

				public Column column { get { return what as Column; } set { what = value; } }

				public Order order;
				public Nulls nulls;

				public Layer(Expression expression, Order order = Order.Ascending, Nulls nulls = Nulls.Last) {
					this.expression = expression;
					this.order = order;
					this.nulls = nulls;
				}

				public Layer(Column column, Order order = Order.Ascending, Nulls nulls = Nulls.Last) {
					this.column = column;
					this.order = order;
					this.nulls = nulls;
				}
			}

			public enum Method {
				B_tree = 0,
				Hash,
				GiST,
				SP_GiST,
				GIN,
				BRIN
			}

			public bool unique;
			public string name;
			public Table table;
			public Method method;
			public List<Layer> layers = new List<Layer>();
			public Expression where;

			public Index(
				string name,
				Table table,
				bool unique = false,
				Expression where = null,
				Method method = Method.B_tree
			) {
				this.name = name;
				this.unique = unique;
				this.method = method;
				this.table = table;
				this.where = where;
			}

			public Index(
				string name,
				Table table,
				Column column,
				bool unique = false,
				Expression where = null,
				Method method = Method.B_tree
			)
				: this(name, table, unique, where, method) {
				this.layers.Add(new Layer(column));
			}

			public Index(
				string name,
				Table table,
				IEnumerable<Column> columns,
				bool unique = false,
				Expression where = null,
				Method method = Method.B_tree
			)
				: this(name, table, unique, where, method) {
				this.layers.AddRange(from column in columns
					                    select new Layer(column));
			}

			public Index(
				string name,
				Table table,
				Expression expression,
				bool unique = false,
				Expression where = null,
				Method method = Method.B_tree
			)
				: this(name, table, unique, where, method) {
				this.layers.Add(new Layer(expression));
			}
		
		}
	}
}