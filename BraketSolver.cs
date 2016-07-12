using System;
using System.Collections.Generic;
using MathParser.DataTypes;
using MathParser.DataTypes.DynamicDataTypes;

namespace MathParser
{
	/// <summary>
	/// Braket solver.
	/// This solver is connected to DMAS too... the
	/// It Solves the Brakets.
	/// </summary>

	public class BraketSolver : ISolver
	{
		Dictionary<string, MathParserExpression> theData;
		List<string> ExpressionElements;
		MathParserExpression solution;
		bool Processed = true;

		public MathParserExpression getSolution() => solution;
		public bool isProcessed() => Processed;

		public BraketSolver (){}
		public BraketSolver (List<string>theExpressionElement, Dictionary<string, MathParserExpression>Data)
		{
			theData = Data;
			ExpressionElements = theExpressionElement;


			Solve ();
		}

		void Solve()
		{
			if (!(ExpressionElements.Contains ("(") || ExpressionElements.Contains (")"))) {
				DMASSolver sol = new DMASSolver (ExpressionElements, theData);
				sol.Solve ();
				if (sol.isProcessed ()) {
					solution = sol.getSolution ();
				} else {
					Processed = false;
					throw new MathParserException ("The expression could not be solved");
				}
			} else {
				bool intake = false;
				int bstart = 0;
				int belements = 1;
				List<string> theBraketList = new List<string>();
				for (int c = 0; c < ExpressionElements.Count; c++) {
					string Element = ExpressionElements [c];
					if (!ExpressionElements.Contains ("(") && !ExpressionElements.Contains("-(")) {
						break;
					}
					if (Element == ")") {
						intake = false;
						DMASSolver sol = new DMASSolver (theBraketList, theData);
						sol.Solve ();
						if (sol.isProcessed ()) {
							MathParserExpression ANS = sol.getSolution ();
							if (ExpressionElements [bstart].Contains ("-")) {
								if (ANS.Type.Contains ("Number")) {
									Number g = ANS.Data;
									g = g * new Number ((double)-1);
									ANS = new MathParserExpression (g);
								} else if (ANS.Type.Contains ("Matrix")) {
									Matrix g = ANS.Data;
									g = g * -1;
									ANS = new MathParserExpression (g);
								}
							}
							string name = autoNamer ();
							theData.Add (name, ANS);
							ExpressionElements [bstart] = name; 
							ExpressionElements.RemoveRange (bstart+1, belements+1);
							c = -1;
						} else {
							Processed = false;
							throw new MathParserException ("Error");
						}
						belements = 0;
					}
					if (intake) {
						theBraketList.Add (Element);
						belements++;
					}
					if (Element.Contains("(")) {
						theBraketList = new List<string> ();
						intake = true;
						bstart = c;
						belements = 0;
					} 
				}
				BraketSolver bsol = new BraketSolver (ExpressionElements, theData);
				if (bsol.isProcessed ()) {
					solution = bsol.getSolution ();
				} else {
					Processed = false;
					throw new MathParserException ("Error");
				}
			}
		}

		int ncount = 0;
		string autoNamer()
		{
			ncount++;
			string name = "BODMASS_thinkgy_9pobosx__" + ncount.ToString ();
			if (theData.ContainsKey (name)) {
				return (autoNamer());
			} else {
				return name;
			}
		}
	}
}

