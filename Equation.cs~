using System;
using MathParser.DataTypes;
using System.Collections.Generic;
using System.IO;

namespace MathParser
{
	public class Equation : ISolver
	{
		MathParserExpression solution;
		bool Processed = true;
		public Dictionary<string, MathParserExpression> theHistory;
		string BasicOperators;
		public MathParserExpression getSolution ()
		{
			return solution;
		}
		public bool isProcessed() => Processed;
		string givenExpression;
		public Equation(){}
		public Equation (string theExpression)
		{
			givenExpression = theExpression.Trim();
		}

		public void Solve()
		{
			string testString = givenExpression.Replace("=","");
			int testLenght = testString.Length;
			if (givenExpression.Length - testLenght <= 1) {   // Declaration statement. a = 2+2
				string Name, Exp;
				{
					string[] parts= givenExpression.Split ('=');
					Name = parts [0].Trim ();
					Exp = parts [1].Trim ();
				}
				NonEquation sol = new NonEquation(Exp);
				sol.History = theHistory;
				sol.Solve ();
				BasicOperators = sol.theBasicOperatorsString;
				string numbers = "0123456789.";
				if (sol.ConstantName.Contains (Name) || sol.theKeyWordsList.Contains (Name) ||  Checker.ifContainOperation (Name,BasicOperators) || numbers.Contains (new string(Name[0],1)) || string.IsNullOrEmpty (Name) || string.IsNullOrWhiteSpace (Name)) {
					Processed = false;
					throw new MathParserException ($"Invalid declaration. '{Name}'.");
				}
				if (sol.isProcessed ()) {
					solution = new MathParserExpression(sol.getSolution ());
					solution.Tag = Name.Trim ();
					//if (string.IsNullOrEmpty (solution.Data.Tag) || string.IsNullOrWhiteSpace (solution.Data.Tag)) {
						solution.setEntireTag (Name.Trim ());
					
				} else {
					Processed = false;
					throw new MathParserException ("Unsolveable Input.");
				}
			}
		}   // end solve().



	}
}

