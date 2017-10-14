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


		bool isOverdrive(string exp)
		{
			if (!exp.Contains ("@")) {
				return false;
			}
			string nExp = exp.TrimStart(new char[] { '@' });
			if (exp.Length - nExp.Length == 1)
			{
				if (nExp.Contains("@"))
				{
					throw new MathParser.MathParserException($"Cannot Parse the expression. Make sure to add just one '@' on the " +
						"start of overdrive expression and donot add '@' in any other place in the" +
						"expression. If you don't want to use overdrive do not write '@' in the expression");
				}else{return true;}
			}else{
				throw new MathParser.MathParserException($"Unable to understand expression '{exp}'. For MathParser overdrive " +
					"only add one '@' at the start of the 'overdrive expression'");
			}
		}


		private void Interpret()
		{


			if(isOverdrive(givenString)){
				MathParserOverdrive mpo = new MathParserOverdrive(givenString.TrimStart(new char[] { '@' }));
				mpo.Solve();
				if(isProcessed()){
					Solution = mpo.getSolution();
					Processed = true;
					return;
				}
			}

			else if (!givenString.Contains("="))
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

