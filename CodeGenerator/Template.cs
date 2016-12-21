namespace Battlesnake.PostGen.CodeGenerator {

	public class Template {

		readonly string template;

		public Template(string template) {
			this.template = template;
		}

		public string this [params object[] args] { get { return Quote.Format(template, args); } }

		/* Since we can't call operator[] without arguments */

		public static implicit operator string(Template template) {
			return Quote.Format(template.template);
		}

		public static implicit operator Block(Template template) {
			return (string)template;
		}

	}
}

