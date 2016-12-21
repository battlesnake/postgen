using System;
using System.Linq;
using System.Collections.Generic;

namespace Battlesnake.PostGen.CodeGenerator {

	public static class ColumnList {
	
		public static string Generate(IEnumerable<Language.Table.Column> columns) {
			return String.Join(", ", from column in columns
				                        select Quote.Identifier(column.name));
		}

	}
}

