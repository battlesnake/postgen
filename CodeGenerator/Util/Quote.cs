using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Battlesnake.PostGen {
	
	public static class Quote {

		public const char format_spec = '%';
		public const char identifier = 'I';
		public const char literal = 'L';
		public const char verbatim = 's';

		public enum Specifier {
			Identifier = identifier,
			Literal = literal,
			Verbatim = verbatim
		}

		static readonly Regex format_expression_matcher = new Regex(
			                                                  Regex.Escape(format_spec.ToString()) + ".?",
			                                                  RegexOptions.Compiled | RegexOptions.ECMAScript
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

			public FormatException(string message)
				: this(null, null, message) {
			}
		}

		public static string Identifier(string s) {
			/* postgres:/src/bin/pg_upgrade/util.c:quote_identifier */
			return "\"" + s.Replace("\"", "\"\"") + "\"";
		}

		public static string Literal(string s) {
			/* postgres:/src/backend/utils/adt/quote.c:quote_literal_internal */
			return s == null ? null : (s.IndexOf('\\') >= 0 ? "E" : "") + escape_literal.Replace(s, m => m.Value + m.Value);
		}

		public static string Nullable(string s) {
			/* postgres:/src/backend/utils/adt/quote.c:quote_nullable */
			return s == null ? "NULL" : Literal(s);
		}

		public static string Verbatim(string s) {
			return s ?? "";
		}

		/* Untyped versions */
		private static string Identifier<T>(T s) {
			return Identifier(s?.ToString());
		}

		private  static string Literal<T>(T s) {
			return Literal(s?.ToString());
		}

		private static string Nullable<T>(T s) {
			return Nullable(s?.ToString());
		}

		private static string Verbatim<T>(T s) {
			return Verbatim(s?.ToString());
		}

		private static readonly Dictionary<char, Specifier> specifiers = new Dictionary<char, Specifier> {
			{ 'L', Specifier.Literal },
			{ 'I', Specifier.Identifier },
			{ 's', Specifier.Verbatim }
		};

		private static string Token(Specifier specifier, object value) {
			switch (specifier) {
			case Specifier.Identifier:
				return Identifier(value);
			case Specifier.Literal:
				return Nullable(value);
			case Specifier.Verbatim:
				return Verbatim(value);
			default:
				throw new FormatException("Invalid format specifier");
			}
		}

		public static string Array(string separator, Specifier specifier, IEnumerable<object> items) {
			return String.Join(separator, from item in items
				                             select Token(specifier, item));
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
					char specifier_char = match.Value[1];
					if (specifier_char == format_spec) {
						return format_spec.ToString();
					}
					Specifier specifier;
					if (!specifiers.TryGetValue(specifier_char, out specifier)) {
						throw new FormatException(format, args, String.Format("Unknown format specifier: {0}", specifier_char));
					}
					object value = values.Dequeue();
					if (value == null || !value.GetType().IsArray) {
						return Token(specifier, value?.ToString());
					} else {
						throw new FormatException(format, args, "Formatter does not accept arrays yet");			
					}
				});
		}
	
	}
}

