using System;
using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.Language {

	public partial class Table {
	
		public class Index : Tags.TopLevel {

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
			public List<Column> columns;
			public Expression where;

			public Index(
				string name,
				Table table,
				IEnumerable<Column> columns,
				bool unique = false,
				Expression where = null,
				Method method = Method.B_tree
			) {
				this.name = name;
				this.unique = unique;
				this.method = method;
				this.table = table;
				this.columns = columns.ToList();
				this.where = where;
			}
		}
	}
}