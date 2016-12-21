using System;
using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.CodeGenerator {

	public class Block {

		private struct Line {
			public int level;
			public string text;

			public Line(int level, string text) {
				this.level = level;
				this.text = text;
			}

			public string ToString(string indent) {
				return string.IsNullOrEmpty(text) ? "" : String.Concat(Enumerable.Repeat(indent, level)) + text;
			}

			public override string ToString() {
				return ToString("\t");
			}

			// Analysis disable once MemberHidesStaticFromOuterClass
			public static Line From(string line, string indent = "\t") {
				int indent_length = indent.Length;
				int level = 0;
				int start = 0;
				while (line.Length >= start + indent_length && line.Substring(start, indent_length) == indent) {
					level++;
					start += indent_length;
				}
				return new Line(level, line.Substring(start));
			}
		}

		private readonly List<Line> lines = new List<Line>();

		/* Append to last line */
		private void Append(string s) {
			var last = lines[lines.Count - 1];
			last.text += s;
		}

		private void Add(int level, string line) {
			if (line != null) {
				lines.Add(new Line(level, line));
			}
		}

		/* Concatenate a series of blocks */
		public static Block Concat(IEnumerable<Block> blocks) {
			var res = new Block();
			foreach (var block in blocks) {
				res.lines.AddRange(block.lines);
			}
			return res;
		}

		/* Concatenate a series of blocks, delimiting them and terminating the last one */
		public static Block ConcatList(
			IEnumerable<Block> blocks,
			string delimiter,
			string terminator,
			bool blank_lines = false
		) {
			var res = new Block();
			Block[] ar = blocks.Where(block => block != null).ToArray();
			if (ar.Length == 0) {
				return res;
			}
			int last = ar.Length - 1;
			for (int i = 0; i <= last; i++) {
				if (blank_lines && i > 0) {
					res++;
				}
				Block block = ar[i];
				res.lines.AddRange(block.lines);
				int last_line = res.lines.Count - 1;
				Line editing = res.lines[last_line];
				editing.text += (i == last ? terminator : delimiter);
				res.lines[last_line] = editing;
			}
			return res;
		}

		public static Block From(IEnumerable<string> lines, string indent = "\t") {
			return new Block(from line in lines
				                select Line.From(line, indent));
		}

		private Block(IEnumerable<Line> lines) {
			this.lines.AddRange(lines.ToList());
		}

		public Block(IEnumerable<string> lines) {
			AddRange(lines);
		}

		public Block(params string[] lines)
			: this(lines.AsEnumerable()) {
		}

		public static implicit operator Block(string line) {
			return new Block(line);
		}

		public void Add(string line) {
			Add(0, line);
		}

		public void AddRange(IEnumerable<string> range) {
			foreach (var line in range) {
				Add(line);
			}
		}

		/* IS THIS USEFUL vs ToList?  Appends delimiter to all lines except the last, which has terminator appended instead */
		private void AddList(bool ignored, IEnumerable<string> items, string delimiter = ",", string terminator = "") {
			string[] ar = items.ToArray();
			int last = ar.Length - 1;
			for (int i = 0; i <= last; i++) {
				Add(ar[i] + (i == last ? terminator : delimiter));
			}
		}

		/* IS THIS USEFUL vs ConcatList?  Like AddList, but treats indented lines as continuation of the previous line */
		public static Block ToList(Block block, string delimiter = ",", string terminator = "") {
			var res = new Block();
			Line[] ar = block.lines.ToArray();
			if (ar.Length == 0) {
				return res;
			}
			int last = ar.Length - 1;
			for (int i = 0; i <= last; i++) {
				Line line = ar[i];
				string s = line.text;
				if (i == last) {
					s += terminator;
				} else if (line.level == 0 && ar[i + 1].level == 0) {
					s += delimiter;
				}
				res.Add(line.level, line.text + s);
			}
			return res;
		}

		/* Append a block, increasing the indent level of the block's lines */
		public void Indent(Block block, int level = 1) {
			foreach (var line in block.lines) {
				Add(line.level + level, line.text);
			}
		}

		/* Add operators (mutates LHS) */
		public static Block operator +(Block self, string line) {
			self.Add(line);
			return self;
		}

		public static Block operator +(Block self, IEnumerable<string> range) {
			self.AddRange(range);
			return self;
		}

		public static Block operator %(Block self, Block child) {
			self.Indent(child);
			return self;
		}

		public static Block operator ++(Block self) {
			self.lines.Add(new Line(0, null));
			return self;
		}

		public string ToString(string indent, string newline = "\n") {
			return String.Join(newline, lines.Select(line => line.ToString(indent)));
		}

		public string ToCommaList() {
			return ToString("", ", ");
		}

		public override string ToString() {
			return ToString("\t", "\n");
		}

		public static implicit operator string(Block Block) {
			return Block.ToString();
		}

	}
}