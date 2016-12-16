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
				return String.Concat(Enumerable.Repeat(indent, level)) + text;
			}

			public override string ToString() {
				return ToString("\t");
			}
		}

		private readonly List<Line> lines = new List<Line>();

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
		public static Block ConcatList(IEnumerable<Block> blocks, string delimiter = "\n", string terminator = "") {
			var res = new Block();
			Block[] ar = blocks.Where(block => block != null).ToArray();
			if (ar.Length == 0) {
				return res;
			}
			int last = ar.Length - 1;
			for (int i = 0; i <= last; i++) {
				Block block = ar[i];
				res.lines.AddRange(block.lines);
				int last_line = res.lines.Count;
				Line tmp = res.lines[last_line];
				tmp.text += (i == last ? terminator : delimiter);
				res.lines[last_line] = tmp;
			}
			return res;
		}

		public Block() {
		}

		public void Add(string line) {
			Add(0, line);
		}

		public void AddRange(IEnumerable<string> range) {
			foreach (var line in range) {
				Add(line);
			}
		}

		/* Appends delimiter to all lines except the last, which has terminator appended instead */
		public void AddList(IEnumerable<string> items, string delimiter = ",", string terminator = "") {
			string[] ar = items.ToArray();
			int last = ar.Length - 1;
			for (int i = 0; i <= last; i++) {
				Add(ar[i] + (i == last ? terminator : delimiter));
			}
		}

		/* Like AddList, but understands treats indented lines as continuation of the previous line */
		public Block ToList(Block block, string delimiter = ",", string terminator = "") {
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

		public string ToString(string indent, string newline = "\n") {
			return String.Join(newline, lines.Select(line => line.ToString(indent)));
		}

		public override string ToString() {
			return ToString("\t", "\n");
		}

	}
}