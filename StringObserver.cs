using System;
using MathParser.DataTypes;
using System.Collections.Generic;
namespace MathParser
{
	/// <summary>
	///  The entry point of the data.
	///  Stores all the history and send the string for further processing to different.
	///  Processing classes.
	/// </summary>


	public class StringObserver : ISolver
	{ 
		string givenString;
		bool Processed = true;
		public StringObserver() { }
		MathParserExpression Solution;
		public MathParserExpression getSolution() => Solution;
		public bool isProcessed() => Processed;
		Dictionary<string, MathParserExpression> History;
		Dictionary<string, MathParserExpression> theExpressionListHistory = new Dictionary<string, MathParserExpression> ();

		public StringObserver(string String_To_Interpret, Dictionary<string, MathParserExpression> History) {

			this.History = History;
			givenString = String_To_Interpret.Trim();
			Interpret();
			if (Processed)
			{
				theExpressionListHistory.Add(givenString, Solution);
			}
		}

		private void Interpret()
		{
			if (!givenString.Contains("="))
			{
				NonEquation solve = new NonEquation (givenString);
				solve.History = History;
				solve.Solve ();
				if (solve.isProcessed())
				{
					Solution = solve.getSolution();
				}
				else
				{
					Processed = false;
				}
			}
			else
			{ 

				Equation sol = new Equation (givenString);
				sol.theHistory = History;
				sol.Solve ();
				if (sol.isProcessed ()) {
					Solution = sol.getSolution ();
				} 
				else {
					Processed = false;
				}
			}
		}


	}
}

