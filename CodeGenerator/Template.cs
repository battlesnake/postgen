/* Provided for you to copy to other units, for convenience */
using Template1 = Battlesnake.PostGen.CodeGenerator.Template<string>;
using Template2 = Battlesnake.PostGen.CodeGenerator.Template<string, string>;
using Template3 = Battlesnake.PostGen.CodeGenerator.Template<string, string, string>;
using Template4 = Battlesnake.PostGen.CodeGenerator.Template<string, string, string, string>;
using Template5 = Battlesnake.PostGen.CodeGenerator.Template<string, string, string, string, string>;
using Template6 = Battlesnake.PostGen.CodeGenerator.Template<string, string, string, string, string, string>;

namespace Battlesnake.PostGen.CodeGenerator {

	public abstract class AbstractTemplate {

		public readonly string template;

		protected AbstractTemplate(string template) {
			this.template = template;
		}

		protected string format(params object[] args) {
			return Quote.Format(template, args);
		}

	}

	public class Template : AbstractTemplate {
		public Template(string template)
			: base(template) {
		}

		/* Since we can't call operator[] without arguments */
		public static implicit operator string(Template template) {
			return Quote.Format(template.template);
		}

	}

	public class Template<T> : AbstractTemplate {
		public Template(string template)
			: base(template) {
		}

		public static implicit operator Template<T>(Template source) {
			return new Template<T>(source.template);
		}

		public string this [T arg1] { get { return format(arg1); } }
	}

	public class Template<T, U> : AbstractTemplate {
		public Template(string template)
			: base(template) {
		}

		public string this [T arg1, U arg2] { get { return format(arg1, arg2); } }

		public static implicit operator Template<T, U>(Template source) {
			return new Template<T, U>(source.template);
		}
	}

	public class Template<T, U, V> : AbstractTemplate {
		public Template(string template)
			: base(template) {
		}

		public string this [T arg1, U arg2, V arg3] { get { return format(arg1, arg2, arg3); } }

		public static implicit operator Template<T, U, V>(Template source) {
			return new Template<T, U, V>(source.template);
		}
	}

	public class Template<T, U, V, W> : AbstractTemplate {
		public Template(string template)
			: base(template) {
		}

		public string this [T arg1, U arg2, V arg3, W arg4] { get { return format(arg1, arg2, arg3, arg4); } }

		public static implicit operator Template<T, U, V, W>(Template source) {
			return new Template<T, U, V, W>(source.template);
		}
	}

	public class Template<T, U, V, W, X> : AbstractTemplate {
		public Template(string template)
			: base(template) {
		}

		public string this [T arg1, U arg2, V arg3, W arg4, X arg5] { get { return format(arg1, arg2, arg3, arg4, arg5); } }

		public static implicit operator Template<T, U, V, W, X>(Template source) {
			return new Template<T, U, V, W, X>(source.template);
		}
	}

	public class Template<T, U, V, W, X, Y> : AbstractTemplate {
		public Template(string template)
			: base(template) {
		}

		public string this [T arg1, U arg2, V arg3, W arg4, X arg5, Y arg6] { get { return format(
				arg1,
				arg2,
				arg3,
				arg4,
				arg5,
				arg6
			); } }

		public static implicit operator Template<T, U, V, W, X, Y>(Template source) {
			return new Template<T, U, V, W, X, Y>(source.template);
		}
	}

}