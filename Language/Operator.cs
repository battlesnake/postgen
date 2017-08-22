namespace Battlesnake.PostGen.Language {

	public enum UnaryOperator {
		IsNull,
		IsNotNull,
		IsTrue,
		IsNotTrue,
		IsFalse,
		IsNotFalse,
		IsUnknown,
		IsNotUnknown,
		Not,
		SquareRoot,
		CubeRoot,
		Factorial,
		FactorialPrefix,
		AbsoluteValue,
	}

	public enum BinaryOperator {
		IsDistinctFrom,
		IsNotDistinctFrom,
		Equal,
		NotEqual,
		Lesser,
		Grequal,
		Greater,
		Lequal,
		LogicalAnd,
		LogicalOr,
		Addition,
		Subtraction,
		Multiplication,
		Division,
		Modulo,
		Exponentiation,
		BitwiseAnd,
		BitwiseOr,
		BitwiseXor,
		BitwiseNot,
		BitwiseShiftLeft,
		BitwiseShiftRight,
	}

	public enum TernaryOperator {
		Between,
		NotBetween,
	}

}