using System;
using System.Collections.Generic;
using MathParser.DataTypes;
using MathParser.DataTypes.DynamicDataTypes;


namespace MathParser
{
	/// <summary>
	/// DMAS solver.
	/// This class deals with the solving of the mathematical expression.
	/// it follows the CBODMAS Rule of the mathmatics.
	/// Command.
	/// Brakket.
	/// Order.
	/// Division.
	/// Multiplication.
	/// Addition.
	/// Subtraction.
	/// </summary>


	public class DMASSolver : ISolver
	{
		List<string> Expressions;
		Dictionary<string, MathParserExpression> theData;
		MathParserExpression solution;
		bool Processed = true;
		public MathParserExpression getSolution ()
		{
			return solution;
		}

		public bool isProcessed() => Processed;

		public DMASSolver() { }
		public DMASSolver(List<string> theExpression, Dictionary<string, MathParserExpression> TheData)
		{
			Expressions = theExpression;
			theData = TheData;
		}

		public void Solve()
		{
			if (Expressions.Count == 0) {
				Processed = false;
				throw new MathParserException ("No Data to processes");
			}

			else if (Expressions.Count == 1)
			{
				if (Expressions[0].Contains("-"))
				{
					MathParserExpression hold = theData[Expressions[0].Trim("-".ToCharArray())];
					if (hold.Type.Contains("Number"))
					{
						Number ans = new Number((double)-1) * hold.Data;
						solution = new MathParserExpression(ans);
					}
					else if (hold.Type.Contains("Matrix"))
					{
						Matrix ans = -1 * hold.Data;
						solution = new MathParserExpression(ans);
					}
				}
				else {
					solution = theData[Expressions[0].Trim("-".ToCharArray())];
				}
			}

			else
			{
				if(Unit_Intake_Command()){
				if(Order()){
				if(Division ()){
							if (Multiplication ()) {
								if (Addition ()) {
									solution = new MathParserExpression (theData [Expressions [0].Trim ("-".ToCharArray ())]);
								}
							}
						}
					}
				}
			}


		}

		public delegate bool OperatorActionDelegate (MathParserExpression lhs, MathParserExpression rhs, Dictionary<string,MathParserExpression>theData, List<string>ExpressionList, string AnswerName, int currentPos);
		public static OperatorActionDelegate onAddition;    // Delegate for extending the addition operation.

		bool Addition()
		{
			bool done = true;
			if (Expressions.Contains ("+"))
			{
				for (int c = 0; c < Expressions.Count; c++)
				{

					if (Expressions[c] == "+")
					{
						MathParserExpression rhs = theData[Expressions[c + 1].Trim("-".ToCharArray())];
						MathParserExpression lhs = theData[Expressions[c - 1].Trim("-".ToCharArray())];

						if (rhs.Type == lhs.Type) {
							if (rhs.Type.Contains ("Number")) {
								if (Expressions [c + 1].Contains ("-")) {
									rhs = new MathParserExpression (rhs.Data * new Number (-1));
									Expressions [c + 1].Replace ("-", "");
								}
								if (Expressions [c - 1].Contains ("-")) {
									lhs = new MathParserExpression (lhs.Data * new Number (-1));
									Expressions [c - 1].Replace ("-", "");
								}

								Number ans = rhs.Data + lhs.Data;
								string name = autoNamer ();
								Expressions [c - 1] = name;
								theData.Add (name, new MathParserExpression (ans));
							} else if (rhs.Type.Contains ("Matrix")) {
								if (Expressions [c + 1].Contains ("-")) {
									rhs = new MathParserExpression ((Matrix)rhs.Data * -1);
									Expressions [c + 1].Replace ("-", "");
								}
								if (Expressions [c - 1].Contains ("-")) {
									lhs = new MathParserExpression ((Matrix)lhs.Data * -1);
									Expressions [c - 1].Replace ("-", "");
								}

								Matrix mrhs = rhs.Data;
								Matrix mlhs = lhs.Data;

								if (mrhs.Rows == mlhs.Rows && mrhs.Columns == mlhs.Columns) {
									Matrix ans = rhs.Data + lhs.Data;
									string name = autoNamer ();
									Expressions [c - 1] = name;
									theData.Add (name, new MathParserExpression (ans));
								} else {
									Processed = false;
									done = false;
									throw new Exception ("Invalid row and column matrix addition.");
								}
							} else {
								if (onAddition != null && onAddition (lhs, rhs, theData, Expressions, autoNamer (),c)) {
								} else {
									Processed = false;
									done = false;
									throw new Exception ("Invalid data types addition.");
								}
							}
						} else {
							if (onAddition != null && onAddition (lhs, rhs, theData, Expressions, autoNamer (),c)) {
							} else {
								Processed = false;
								done = false;
								throw new Exception ("Invalid data types addition.");
							}
						}
						Expressions.RemoveRange(c,2);
						c = 0;
					}  // end if
				}  // end for loop.
			}
			return done;
		}    // The function deals with the addition operations.
		public static OperatorActionDelegate onMuliplication;
		bool Multiplication()
		{
			bool done = true;
			string multiply_cross = new string (((char)215), 1);
			if (Expressions.Contains ("*") || Expressions.Contains (multiply_cross)){
				for (int c = 0; c < Expressions.Count; c++)
				{
					if (Expressions[c] == "*" || Expressions[c] == multiply_cross)
					{
						MathParserExpression rhs = theData[Expressions[c + 1]];
						MathParserExpression lhs = theData[Expressions[c - 1]];

						if (rhs.Type == lhs.Type) {
							if (rhs.Type.Contains ("Number")) {
								if (Expressions [c + 1].Contains ("-")) {
									rhs = new MathParserExpression (rhs.Data * new Number (-1));
									Expressions [c + 1].Replace ("-", "");
								}
								if (Expressions [c - 1].Contains ("-")) {
									lhs = new MathParserExpression (lhs.Data * new Number (-1));
									Expressions [c - 1].Replace ("-", "");
								}

								Number ans = rhs.Data * lhs.Data;
								string name = autoNamer ();
								Expressions [c - 1] = name;
								theData.Add (name, new MathParserExpression (ans));
							} else if (rhs.Type.Contains ("Matrix")) {
								if (Expressions [c + 1].Contains ("-")) {
									rhs = new MathParserExpression (rhs.Data * -1);
									Expressions [c + 1].Replace ("-", "");
								}
								if (Expressions [c - 1].Contains ("-")) {
									lhs = new MathParserExpression (lhs.Data * -1);
									Expressions [c - 1].Replace ("-", "");
								}

								Matrix mrhs = rhs.Data;
								Matrix mlhs = lhs.Data;

								if (mrhs.Rows == mlhs.Columns) {
									Matrix ans = rhs.Data * lhs.Data;
									string name = autoNamer ();
									Expressions [c - 1] = name;
									theData.Add (name, new MathParserExpression (ans));
								} else {
									Processed = false;
									done = false;
									throw new Exception ("Invalid matrix multiplication.");
								}
							} else if (onMuliplication != null && onMuliplication (lhs, rhs, theData, Expressions, autoNamer (),c)) {
							} else {
								Processed = false;
								done = false;
								throw new Exception ("Invalid multiplication.");
							}
						} else if (lhs.Type.Contains ("Number") && rhs.Type.Contains ("Matrix")) {
							if (Expressions [c + 1].Contains ("-")) {
								rhs = new MathParserExpression (rhs.Data * -1);
								Expressions [c + 1].Replace ("-", "");
							}
							if (Expressions [c - 1].Contains ("-")) {
								lhs = new MathParserExpression (lhs.Data * new Number(-1));
								Expressions [c - 1].Replace ("-", "");
							}


							Number l = lhs.Data;
							Matrix r = rhs.Data;
							Matrix ans = (double)l.Data * r;
							string name = autoNamer ();
							Expressions [c - 1] = name;
							theData.Add (name, new MathParserExpression (ans));
						} else if (rhs.Type.Contains ("Number") && lhs.Type.Contains ("Matrix")) {
							if (Expressions [c + 1].Contains ("-")) {
								rhs = new MathParserExpression (rhs.Data * new Number(-1));
								Expressions [c + 1].Replace ("-", "");
							}
							if (Expressions [c - 1].Contains ("-")) {
								lhs = new MathParserExpression (lhs.Data * -1);
								Expressions [c - 1].Replace ("-", "");
							}

							Number r = rhs.Data;
							Matrix l = lhs.Data;

							Matrix ans = l * (double)r.Data;
							string name = autoNamer ();
							Expressions [c - 1] = name;
							theData.Add (name, new MathParserExpression (ans));
						} else {
							if (onMuliplication != null && onMuliplication (lhs, rhs, theData, Expressions, autoNamer (),c)) {
							} else {
								Processed = false;
								done = false;
								throw new Exception ("Not a valid multiplication.");
							}
						}
						Expressions.RemoveRange(c, 2);
						c = 0;
					}  // end if
				}  // end for loop.
			}
			return done;
		}  // The function deals with the multiplication operations.
		public static OperatorActionDelegate onDivision;
		bool Division()
		{
			bool done = true;
			if (Expressions.Contains ("÷") || Expressions.Contains ("/")) {
				for (int c = 0; c < Expressions.Count; c++) {
					string Element = Expressions [c];
					if (Element == "/" || Element == "÷") {
						MathParserExpression lhs = theData [Expressions [c - 1].Trim ("-".ToCharArray ())];
						MathParserExpression rhs = theData [Expressions [c + 1].Trim ("-".ToCharArray ())];

						if (lhs.Type.Contains ("Number") && rhs.Type.Contains ("Number")) {
							if (Expressions [c + 1].Contains ("-")) {
								rhs = new MathParserExpression (rhs.Data * new Number(-1));
								Expressions [c + 1].Replace ("-", "");
							}
							if (Expressions [c - 1].Contains ("-")) {
								lhs = new MathParserExpression (lhs.Data * new Number(-1));
								Expressions [c - 1].Replace ("-", "");
							}

							Number l = lhs.Data;
							Number r = rhs.Data;
							Number ans = new Number ((double)l.Data /
								(double)r.Data);
							string name = autoNamer ();
							theData.Add (name, new MathParserExpression (ans));
							Expressions [c - 1] = name;
						} else if (lhs.Type.Contains ("Matrix") && rhs.Type.Contains ("Number")) {
							if (Expressions [c + 1].Contains ("-")) {
								rhs = new MathParserExpression (rhs.Data * new Number(-1));
								Expressions [c + 1].Replace ("-", "");
							}
							if (Expressions [c - 1].Contains ("-")) {
								lhs = new MathParserExpression (lhs.Data * -1);
								Expressions [c - 1].Replace ("-", "");
							}


							Matrix l = lhs.Data;
							Number r = rhs.Data;
							Matrix ans = l / ((double)r.Data);
							string name = autoNamer ();
							theData.Add (name, new MathParserExpression (ans));
							Expressions [c - 1] = name;
						} else {
							if (onDivision != null && onDivision (lhs, rhs, theData, Expressions, autoNamer (), c)) {
							} else {
								Processed = false;
								done = false;
								throw new Exception ("Invalid Division.");
							}
						}

						Expressions.RemoveRange (c,2);
						c = 0;
					}
				}
			}
			return done;
		}  // The function deals with the division operation.
		public static OperatorActionDelegate onOrder;
		bool Order()
		{
			bool done = true;
			if (Expressions.Contains ("^")) {
				for (int c = Expressions.Count-1; c >=0; c--) {
					string Element = Expressions [c];
					if (Element == "^") {
						MathParserExpression lhs = theData [Expressions [c - 1].Trim ("-".ToCharArray ())];
						MathParserExpression rhs = theData [Expressions [c + 1].Trim ("-".ToCharArray ())];

						if (lhs.Type.Contains ("Number") && rhs.Type.Contains ("Number")) {
							if (Expressions [c + 1].Contains ("-")) {
								rhs = new MathParserExpression (rhs.Data * new Number (-1));
								Expressions [c + 1].Replace ("-", "");
							}

							Number ans = new Number (Math.Pow ((double)((Number)lhs.Data).Data, (double)((Number)rhs.Data).Data));
						
							if (Expressions [c - 1].Contains ("-")) {
								ans = ans * new Number (-1);
							}

							string name = autoNamer ();
							Expressions [c - 1] = name;
							theData.Add (name, new MathParserExpression (ans));
						} else {
							if (onOrder != null && onOrder (lhs, rhs, theData, Expressions, autoNamer (), c)) {
							} else {
								Processed = false;
								done = false;
								throw new Exception ("Invalid base and exponent.");
							}
						}
						Expressions.RemoveRange (c,2);
						c = Expressions.Count;
					} 
				}
			}
			return done;
		}    // The function deals with the Order/power operation.

		bool Unit_Intake_Command()  // For the command like sin, cos, tan
		{
			bool done = true;
			if (Expressions.Contains ("`")) {
				for (int c = Expressions.Count-1; c >= 0; c--) {
					string Element = Expressions [c];
					if (Element == "`") {
						MathParserExpression RHS = theData [Expressions [c + 1].Trim ("-".ToCharArray ())];
						if (Expressions [c + 1].Contains ("-")) {
							if (RHS.Type.Contains ("Number")) {
								RHS= new MathParserExpression((Number)RHS.Data * new Number (-1));
								Expressions [c + 1].Replace ("-", "");
							}
							if (RHS.Type.Contains ("Matrix")) {
								RHS = new MathParserExpression((Matrix)RHS.Data * -1);
								Expressions [c + 1].Replace ("-", "");
							}
						}
						UnitFunctionSolver ufs = new UnitFunctionSolver (Expressions [c - 1].Trim ('-'), RHS);
						if (ufs.isProcessed ()) {
							string name = autoNamer ();
							MathParserExpression ans = ufs.getSolution ();
							if (Expressions [c - 1].Contains ("-")) {
								if (ans.Type.Contains ("Number")) {
									ans= new MathParserExpression((Number)ans.Data * new Number (-1));
									Expressions [c + 1].Replace ("-", "");
								}

								if (ans.Type.Contains ("Matrix")) {
									ans = new MathParserExpression((Matrix)ans.Data * -1);
									Expressions [c + 1].Replace ("-", "");
								}
							}
							theData.Add (name, ans);
							Expressions [c - 1] = name;
						} else {
							Processed = false;
							done = false;
							break;
						}
						Expressions.RemoveRange (c,2);
						c = Expressions.Count;
					}
				}
			}
			return done;
		}

		int NamingInteger = 0;
		string autoNamer()
		{
			NamingInteger++;
			string name = "DMASSOLVER_shity_sol___221209_99" + NamingInteger.ToString ();
			if (theData.ContainsKey (name)) {
				return autoNamer ();
			} else {
				return name;
			}
		}



	}  // end class DMASSolver.
}

