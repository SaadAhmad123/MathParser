using System;
using System.Collections.Generic;
using MathParser.DataTypes;


namespace MathParser
{
	internal static class Checker
	{
		/// <summary>
		/// Checks the correct matrix syntax.
		/// </summary>
		/// <returns>The syntax.</returns>
		/// <param name="matrixString">Matrix string.</param>
		/// <param name="FlagStart">Flag start.</param>
		/// <param name="FlagEnd">Flag end.</param>
		/// <param name="FlagElementSeperation">Flag element seperation.</param>
		/// <param name="FlagRowSeperation">Flag row seperation.</param>
		public static bool MatrixSyntax(string matrixString, string FlagStart, string FlagEnd, string FlagElementSeperation, string FlagRowSeperation)
		{
			string givenExpression = matrixString.Trim();
			bool MatrixAlright = true;
			if (givenExpression.Contains(FlagStart) && givenExpression.Contains(FlagEnd))
			{
				if (givenExpression.StartsWith(FlagStart) && givenExpression.EndsWith(FlagEnd))
				{
					int lenghtS = givenExpression.Replace(FlagStart, "").Length;
					int lenghtE = givenExpression.Replace(FlagEnd, "").Length;
					if ((lenghtE == givenExpression.Length - 1) && (lenghtE == givenExpression.Length - 1))
					{ MatrixAlright = true ;}
					else {
						MatrixAlright = false;
					}
				}
				else {
					MatrixAlright = false;
				}
			}
			else {
				MatrixAlright = false;
			}
			return MatrixAlright;
		}    // end matrix syntac checker

		public static List<string> KeyWords;
		public static string OperatorList;
		public static Dictionary<string, MathParserExpression> Constants = new Dictionary<string, MathParserExpression>();
		public static Dictionary<string, MathParserExpression> History = new Dictionary<string, MathParserExpression> ();
		public static List<string> LeftRightFunctionKeywords = new List<string>();
		public static bool ifContainOperation(string Exp, string theBasicOperators)
		{
			string dumy = Exp;
			foreach (char op in theBasicOperators) {
				dumy = dumy.Replace(new String(op,1),"");
			}
			if (dumy.Length != Exp.Length) {
				return true;
			} else {
				return false;
			}
		}


	}
}

