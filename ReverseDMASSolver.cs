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


	public class ReverseDMASSolver : ISolver
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

		public ReverseDMASSolver() { }
		public ReverseDMASSolver(List<string> theExpression, Dictionary<string, MathParserExpression> TheData)
		{
			Expressions = theExpression;
			theData = TheData;
		}

		public void Solve()
		{
			if (Expressions.Count == 1)
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
				if(Division ()){
					if (Multiplication ()) {
						if (Addition ()) {
							solution = new MathParserExpression (theData [Expressions [0].Trim ("-".ToCharArray ())]);
						}
					}
				}
			}


		}


		bool Addition()
		{
			bool done = true;
			if (Expressions.Contains ("+"))
			{
				for (int c = Expressions.Count-1; c >= 0; c--)
				{

					if (Expressions[c] == "+")
					{
						MathParserExpression rhs = theData[Expressions[c - 1].Trim("-".ToCharArray())];
						MathParserExpression lhs = theData[Expressions[c + 1].Trim("-".ToCharArray())];

						if (rhs.Type == lhs.Type) {
							if (rhs.Type.Contains ("Number")) {
								if (Expressions [c - 1].Contains ("-")) {
									rhs = new MathParserExpression (rhs.Data * new Number (-1));
									Expressions [c - 1].Replace ("-", "");
								}
								if (Expressions [c + 1].Contains ("-")) {
									lhs = new MathParserExpression (lhs.Data * new Number (-1));
									Expressions [c + 1].Replace ("-", "");
								}

								Number ans = rhs.Data + lhs.Data;
								string name = autoNamer ();
								Expressions [c - 1] = name;
								theData.Add (name, new MathParserExpression (ans));
							} else if (rhs.Type.Contains ("Matrix")) {
								if (Expressions [c - 1].Contains ("-")) {
									rhs = new MathParserExpression ((Matrix)rhs.Data * -1);
									Expressions [c - 1].Replace ("-", "");
								}
								if (Expressions [c + 1].Contains ("-")) {
									lhs = new MathParserExpression ((Matrix)lhs.Data * -1);
									Expressions [c + 1].Replace ("-", "");
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
							}
						} else {
							Processed = false;
							done = false;
							throw new Exception ("Invalid data types addition.");
						}
						Expressions.RemoveRange(c,2);
						c = Expressions.Count-1;
					}  // end if
				}  // end for loop.
			}
			return done;
		}    // The function deals with the addition operations.
		bool Multiplication()
		{
			bool done = true;
			string multiply_cross = new string (((char)215), 1);
			if (Expressions.Contains ("*") || Expressions.Contains (multiply_cross)){
				for (int c = Expressions.Count-1; c >= 0; c--)
				{
					if (Expressions[c] == "*" || Expressions[c] == multiply_cross)
					{
						MathParserExpression rhs = theData[Expressions[c - 1]];
						MathParserExpression lhs = theData[Expressions[c + 1]];

						if (rhs.Type == lhs.Type) {
							if (rhs.Type.Contains ("Number")) {
								if (Expressions [c - 1].Contains ("-")) {
									rhs = new MathParserExpression (rhs.Data * new Number (-1));
									Expressions [c - 1].Replace ("-", "");
								}
								if (Expressions [c + 1].Contains ("-")) {
									lhs = new MathParserExpression (lhs.Data * new Number (-1));
									Expressions [c + 1].Replace ("-", "");
								}

								Number ans = rhs.Data * lhs.Data;
								string name = autoNamer ();
								Expressions [c - 1] = name;
								theData.Add (name, new MathParserExpression (ans));
							} else if (rhs.Type.Contains ("Matrix")) {
								if (Expressions [c - 1].Contains ("-")) {
									rhs = new MathParserExpression (rhs.Data * -1);
									Expressions [c - 1].Replace ("-", "");
								}
								if (Expressions [c + 1].Contains ("-")) {
									lhs = new MathParserExpression (lhs.Data * -1);
									Expressions [c + 1].Replace ("-", "");
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
							}
						} else if (lhs.Type.Contains ("Number") && rhs.Type.Contains ("Matrix")) {
							Number l = lhs.Data;
							Matrix r = rhs.Data;

							Matrix ans = (double)l.Data * r;
							string name = autoNamer ();
							Expressions [c - 1] = name;
							theData.Add (name, new MathParserExpression (ans));
						} else if (rhs.Type.Contains ("Number") && lhs.Type.Contains ("Matrix")) {
							Number r = rhs.Data;
							Matrix l = lhs.Data;

							Matrix ans = l * (double)r.Data;
							string name = autoNamer ();
							Expressions [c - 1] = name;
							theData.Add (name, new MathParserExpression (ans));
						} else {
							Processed = false;
							done = false;
							throw new Exception ("Not a valid multiplication.");
						}
						Expressions.RemoveRange(c, 2);
						c = Expressions.Count-1;
					}  // end if
				}  // end for loop.
			}
			return done;
		}  // The function deals with the multiplication operations.
		bool Division()
		{
			bool done = true;
			if (Expressions.Contains ("÷") || Expressions.Contains ("/")) {
				for (int c = Expressions.Count-1; c >=0; c--) {
					string Element = Expressions [c];
					if (Element == "/" || Element == "÷") {
						MathParserExpression lhs = theData [Expressions [c - 1].Trim ("-".ToCharArray ())];
						MathParserExpression rhs = theData [Expressions [c + 1].Trim ("-".ToCharArray ())];

						if (lhs.Type.Contains ("Number") && rhs.Type.Contains ("Number")) {
							Number l = lhs.Data;
							Number r = rhs.Data;
							Number ans = new Number ((double)l.Data /
								(double)r.Data);
							string name = autoNamer ();
							theData.Add (name, new MathParserExpression (ans));
							Expressions [c - 1] = name;
						} else if (lhs.Type.Contains ("Matrix") && rhs.Type.Contains ("Number")) {
							Matrix l = lhs.Data;
							Number r = rhs.Data;
							Matrix ans = l / ((double)r.Data);
							string name = autoNamer ();
							theData.Add (name, new MathParserExpression (ans));
							Expressions [c - 1] = name;
						} else {
							Processed = false;
							done = false;
							throw new Exception ("Invalid Division.");
						}

						Expressions.RemoveRange (c,2);
						c = Expressions.Count-1;
					}
				}
			}
			return done;
		}

		bool Order()
		{
			bool done = true;
			if (Expressions.Contains ("^")) {

			}
			return done;
		}

		int NamingInteger = 0;
		string autoNamer()
		{
			NamingInteger++;
			return ("DMASINTERLAN_Solutionsss__" + NamingInteger);
		}



	}  // end class DMASSolver.
}

