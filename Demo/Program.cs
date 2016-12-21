using System;
using Block = Battlesnake.PostGen.CodeGenerator.Block;

namespace Demo {

	class MainClass {

		public static void WriteSection(string name) {
			Console.WriteLine();
			Console.WriteLine("# " + name);
			Console.WriteLine();
		}

		public static void WriteBlock(Block block) {
			var blk = new Block();
			blk %= block;
			Console.WriteLine(blk.ToString("    "));
		}

		public static void Main(string[] args) {
			WriteSection("Tables");
			WriteBlock(Tables.Make());
			WriteSection("Functions");
			WriteBlock(Functions.Make());
		}

	}

}