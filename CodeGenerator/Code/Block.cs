using System;
using System.Collections.Generic;
using System.Linq;

namespace Battlesnake.PostGen.CodeGenerator {

	public struct Line {
		public readonly Guid block;
		public readonly uint level;
		public readonly string text;
		private readonly bool terminated;

		private Line(Guid block, uint level, string text, bool terminated) {
			this.block = block;
			this.level = level;
			this.text = text;
			this.terminated = terminated;
		}

		public Line(Guid block, uint level, string text)
			: this(block, level, text, false) {
		}

		public static Line operator >>(Line line, int amount) {
			return new Line(line.block, line.level + (uint)amount, line.text, line.terminated);
		}

		public static Line operator +(Line line, string terminator) {
			if (line.terminated) {
				throw new Exception("Attempted to double-terminate line");
			}
			return new Line(line.block, line.level, line.text + terminator, true);
		}

		public string ToString(string indent) {
			return String.Concat(Enumerable.Repeat(indent, (int)level)) + text;
		}

		public override string ToString() {
			return ToString("\t");
		}
	}

	public class Block {
		private Guid id = Guid.NewGuid();
		private readonly List<Line> lines = new List<Line>();

		public static Block From(IEnumerable<string> lines, string indent = "\t") {
			uint indent_length = (uint)indent.Length;
			var res = new Block();
			foreach (var line in lines) {
				uint level = 0;
				uint start = 0;
				while (line.Length >= start + indent_length && line.Substring((int)start, (int)indent_length) == indent) {
					level++;
					start += indent_length;
				}
				res.lines.Add(new Line(res.id, level, line.Substring((int)start)));
			}
			return res;
		}

		public static implicit operator Block(string line) {
			var res = new Block();
			res += line;
			return res;
		}

		/* Deprecate this once we can nest e.g. expressions in-line in blocks */
		public static implicit operator string(Block block) {
			return block.ToString();
		}

		/* Concatenate a series of blocks, delimiting them and terminating the last one, optionally separating blocks with blank lines */
		public static Block Concat(
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
				res.lines[last_line] += i == last ? terminator : delimiter;
			}
			return res;
		}

		/* Indent operator (pure) */
		public static Block operator >>(Block self, int amount) {
			var res = new Block();
			res.lines.AddRange(from line in self.lines
				                  select line >> amount);
			return res;
		}

		/* Add operators (mutates LHS) */
		public static Block operator +(Block self, string line) {
			if (line != null) {
				self.lines.Add(new Line(self.id, 0, line));
			}
			return self;
		}

		public static Block operator +(Block self, IEnumerable<string> range) {
			foreach (var line in range) {
				self += line;
			}
			return self;
		}

		/* Add with indent operators (mutates LHS) */
		public static Block operator %(Block self, string line) {
			if (line != null) {
				self.lines.Add(new Line(self.id, 1, line));
			}
			return self;
		}

		public static Block operator %(Block self, Line line) {
			self.lines.Add(line >> 1);
			return self;
		}

		public static Block operator %(Block self, Block child) {
			foreach (var line in child.lines) {
				self %= line;
			}
			return self;
		}

		public static Block operator %(Block self, IEnumerable<Block> children) {
			foreach (var child in children) {
				self %= child;
			}
			return self;
		}

		/* Append blank line operator */
		public static Block operator ++(Block self) {
			self.lines.Add(new Line(self.id, 0, null));
			return self;
		}

		public string ToString(string indent, string newline = "\n") {
			return String.Join(newline, lines.Select(line => line.ToString(indent)));
		}

		public override string ToString() {
			return ToString("\t", "\n");
		}

	}
}