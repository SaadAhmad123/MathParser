using System;
using MathParser.DataTypes;

namespace MathParser
{
	interface ISolver
	{
		MathParserExpression getSolution();
		bool isProcessed();
	}

	public class MathParserException: Exception
	{
		public MathParserException(){}
		public MathParserException(string Message): base (Message){}
	}
}
