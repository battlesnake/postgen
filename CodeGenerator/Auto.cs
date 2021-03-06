﻿using System.Linq;
using System.Collections.Generic;
using Battlesnake.PostGen.Language.Tags;

namespace Battlesnake.PostGen.CodeGenerator {

	public class Auto {

		private List<TopLevel> elements = new List<TopLevel>();

		public Auto() {
		}

		public static Auto operator +(Auto self, IEnumerable<TopLevel> items) {
			self.elements.AddRange(items);
			return self;
		}

		public static Auto operator +(Auto self, TopLevel item) {
			self.elements.Add(item);
			return self;
		}

		public Block Generate() {
			return Block.Concat(
				from element in elements
				select (Dispatch(element as dynamic) as Block),
				";",
				";",
				true
			);
		}

		private static Block Dispatch(Language.Table table) {
			return Table.Generate(table);
		}

		private static Block Dispatch(Language.Function function) {
			return Function.Generate(function);
		}

		private static Block Dispatch(Language.Table.Index trigger) {
			return TableIndex.Generate(trigger);
		}

		private static Block Dispatch(Language.Trigger trigger) {
			return Trigger.Generate(trigger);
		}

		private static Block Dispatch(Language.Type.Composite type) {
			return Type.Generate(type);
		}

	}

}