using System;
using MathParser.DataTypes;

namespace MathParser
{

	/// <summary>
	/// The interface for the al the class related to solving in the 
	/// MathParser.
	/// </summary>
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
