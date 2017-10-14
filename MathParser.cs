using System;
using MathParser.DataTypes;
using System.Collections.Generic;

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


	public delegate void UnitFunctionExtentionDelegte(string command, MathParserExpression value, ref MathParserExpression solution, ref bool Processed);

	public class MathParserException: Exception
	{
		public MathParserException(){}
		public MathParserException(string Message): base (Message){}
	}
}
