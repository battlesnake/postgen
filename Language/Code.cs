using System.Collections.Generic;

namespace Battlesnake.PostGen.Language {

	public class Code {
	
		public Dictionary<string, string> file_ext_map = new Dictionary<string, string> {
			{ "sql", "sql" },
			{ "psql", "plpgsql" },
			{ "js", "plv8" },
			{ "pl", "plperl" },
		};

		public string language;
		public string code;

		public Code(string language, string code) {
			this.language = language;
			this.code = code;
		}

	}
}