using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Battlesnake.PostGen {
	
	public static class Quote {

		public const char format_spec = '%';
		public const char identifier = 'I';
		public const char literal = 'L';
		public const char verbatim = 's';

		static readonly Regex format_expression_matcher = new Regex(
			                                                  Regex.Escape(format_spec.ToString()) + ".?",
			                                                  RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.Singleline
		                                                  );

		static readonly Regex escape_literal = new Regex(@"['\\]", RegexOptions.Compiled | RegexOptions.ECMAScript);

		public class FormatException: Exception {
			public readonly string format;
			public readonly dynamic[] args;

			public FormatException(string format, dynamic[] args, string message)
				: base(message) {
				this.format = format;
				this.args = args;
			}
		}

		public static string Identifer(string s) {
			/* postgres:/src/bin/pg_upgrade/util.c:quote_identifier */
			return "\"" + s.Replace("\"", "\"\"") + "\"";
		}

		public static string Identifier<T>(T s) {
			return Identifier(s?.ToString());
		}

		public static string Literal(string s) {
			/* postgres:/src/backend/utils/adt/quote.c:quote_literal_internal */
			return s == null ? null : (s.IndexOf('\\') >= 0 ? "E" : "") + escape_literal.Replace(s, m => m.Value + m.Value);
		}

		public static string Literal<T>(T s) {
			return Literal(s?.ToString());
		}

		public static string Nullable(string s) {
			/* postgres:/src/backend/utils/adt/quote.c:quote_nullable */
			return s == null ? "NULL" : Literal(s);
		}

		public static string Nullable<T>(T s) {
			return Nullable(s?.ToString());
		}

		public static string Verbatim(string s) {
			return s ?? "";
		}

		public static string Verbatim<T>(T s) {
			return Verbatim(s?.ToString());
		}

		public static string Format(string format, params object[] args) {
			/* Postgres 9.6 manual §9.4.1 - but does not support position|flags|width */
			var values = new Queue<object>(args);
			return format_expression_matcher.Replace(format, match => {
					if (match.Length == 1) {
						throw new FormatException(format, args, "Trailing % (did you mean %%)".Replace('%', format_spec));
					}
					if (values.Count == 0) {
						throw new FormatException(
							format,
							args,
							string.Format(
								"Not enough ({0}) values provided for format",
								args.Length
							)
						);
					}
					string value = values.Dequeue()?.ToString();
					char spec = match.Value[1];
					switch (spec) {
					case identifier:
						return Identifer(value);
					case literal:
						return Nullable(value);
					case verbatim:
						return Verbatim(value);
					case format_spec:
						return format_spec.ToString();
					default:
						throw new FormatException(format, args, String.Format("Unknown format specifier: {0}", spec));
					}
				});
		}
	
	}
}

