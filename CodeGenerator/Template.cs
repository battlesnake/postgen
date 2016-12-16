namespace Battlesnake.PostGen {

	public class Template {

		readonly string template;

		public Template(string template) {
			this.template = template;
		}

		public string this [params object[] args] { get { return Quote.Format(template, args); } }

	}
}

