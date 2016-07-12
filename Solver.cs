using System;
using System.Collections.Generic;
using MathParser.DataTypes;
using MathParser.DataTypes.DynamicDataTypes;

namespace MathParser
{
	/// <summary>
	/// Solver.
	/// This is the Entry point of the MathParsing liberay.
	/// </summary>

	/// The other Programmers will use this class as an interface.

	public delegate void SolverOnPrintDelegate<T>(T theType); // Internal delegate. Don't mess with them from outside the namespace MATH_PARSER.
	public delegate void SolverKeyWordsSyncDelegate<T>(ref T theList); // Internal delgate.
	public delegate void SolverMatrixFlagSync(ref string FlagStart, ref string FlagEnd, ref string FlagElementSeperation, ref string FlagRowSeperation);

	public class Solver : ISolver
	{
		string theExpression;
		bool saveHistory = false;
		Dictionary<string, MathParserExpression> History;
		MathParserExpression solution;
		bool Processed = true;
		public SolverOnPrintDelegate<Matrix> OnMatixPrint = delegate { };
		public SolverOnPrintDelegate<Number> OnNumberPrint = delegate { };
		public SolverKeyWordsSyncDelegate<List<string>> OnKeyWordsSync = null;
		public SolverKeyWordsSyncDelegate<Dictionary<string, MathParserExpression>> OnConstantSync = null;
		public SolverMatrixFlagSync OnMatrixFlagSync = null;
		//public List<string>theKeyWordsList = new List<string>();
		//public List<string> 


		public MathParserExpression getSolution()
		{
			return solution;
		}

		public bool isProcessed()
		{
			return Processed;
		}

		void KeyWordsList(ref List<string>theKeyWordList)
		{
			theKeyWordList.Add("sin");
			theKeyWordList.Add("cos");
			theKeyWordList.Add("tan");
			theKeyWordList.Add("cot");
			theKeyWordList.Add("cosec");
			theKeyWordList.Add("csc");
			theKeyWordList.Add("sec");
			theKeyWordList.Add ("floor");
			theKeyWordList.Add ("ceil");
			theKeyWordList.Add ("abs");
			theKeyWordList.Add ("sinh");
			theKeyWordList.Add ("cosh");
			theKeyWordList.Add ("tanh"); 
			theKeyWordList.Add ("ln");
			theKeyWordList.Add ("log");
			theKeyWordList.Add ("arcCos");
			theKeyWordList.Add ("arcSin");
			theKeyWordList.Add ("arcTan");
			theKeyWordList.Add ("sqrt");
			OnKeyWordsSync?.Invoke (ref theKeyWordList);
		}

		void ConstantList(ref Dictionary<string , MathParserExpression> ConstantList)
		{
			ConstantList.Add ("e", new MathParserExpression(new Number((double)Math.E)));
			ConstantList.Add ("pi", new MathParserExpression(new Number((double)Math.PI))); 
			OnConstantSync?.Invoke (ref ConstantList );
		}

		public Solver (){
			Matrix.staticOnPrint = (Matrix matrix) =>{
				OnMatixPrint?.Invoke (matrix);
			};

			MatrixBuilder.staticOnFlagSync = (ref string FlagStart, ref string FlagEnd, ref string FlagElementSeperation, ref string FlagRowSeperation) => {
				OnMatrixFlagSync?.Invoke (ref FlagStart,ref FlagEnd, ref FlagElementSeperation, ref FlagRowSeperation);
			};

			Number.staticOnPrint = (Number number) => {
				OnNumberPrint?.Invoke(number);
			};

			NonEquation.staticOnKeyWordSync += KeyWordsList;
			NonEquation.staticOnConstantSync += ConstantList;
		}

		~Solver()
		{
			NonEquation.staticOnKeyWordSync -= KeyWordsList;
			NonEquation.staticOnConstantSync -= ConstantList;
		}
		public Solver (string theExpression)
		{
			Matrix.staticOnPrint = (Matrix matrix) =>{
				OnMatixPrint?.Invoke (matrix);
			};

			Number.staticOnPrint = (Number number) => {
				OnNumberPrint?.Invoke(number);
			};

			MatrixBuilder.staticOnFlagSync = (ref string FlagStart, ref string FlagEnd, ref string FlagRowSeperation, ref string FlagElementSeperation) => {
				OnMatrixFlagSync?.Invoke (ref FlagStart,ref FlagEnd, ref FlagRowSeperation, ref FlagElementSeperation);
			};

			NonEquation.staticOnKeyWordSync += KeyWordsList;
			NonEquation.staticOnConstantSync += ConstantList;
			this.theExpression	= theExpression.Trim();
			if (this.theExpression.Contains ("`")) {
				Processed = false;
				throw new Exception ("Invalid symbol \" ` \" in the string cannot proceed");
			}	
			if (this.theExpression.Contains ("#")) {
				Processed = false;
				throw new Exception ("Invalid symbol \" # \" in the string cannot proceed");
			}
			if (string.IsNullOrEmpty (this.theExpression) || string.IsNullOrWhiteSpace (this.theExpression)) {
				Processed = false;
				throw new Exception ("The given Expression doen not contain any information.");
			}

		}

		public void setMathExpression(string theExpression)
		{
			Processed = true;
			this.theExpression	= theExpression.Trim();
			if (this.theExpression.Contains ("`")) {
				Processed = false;
				throw new Exception ("Invalid symbol \" ` \" in the string cannot proceed");
			}
			if (this.theExpression.Contains ("#")) {
				Processed = false;
				throw new Exception ("Invalid symbol \" # \" in the string cannot proceed");
			}
			if (string.IsNullOrEmpty (this.theExpression) || string.IsNullOrWhiteSpace (this.theExpression)) {
				Processed = false;
				throw new Exception ("The given Expression doen not contain any information.");
			}
		}

		public void SaveHistory()
		{
			saveHistory = true;
			History = new Dictionary<string, MathParserExpression> ();
		}

		public void ClearHistory()
		{
			History.Clear ();
		}

		public Dictionary<string, MathParserExpression> getHistory () => History;

		public void Solve()
		{
			MathParser.StringObserver sol = new StringObserver (theExpression,History);
			if (sol.isProcessed ()) {
				solution = sol.getSolution ();
			} else {
				Processed = false;
				throw new Exception ("Expression Unsolvable.");
			}

			solution.Statement = theExpression;

			if (saveHistory) {
				if (string.IsNullOrEmpty (solution.Tag) || string.IsNullOrWhiteSpace (solution.Tag)) {
					if (solution.Type.Contains ("Number")) {
						string name = autoNumberNamer ();
						solution.Tag = name;
						solution.setEntireTag (name);
						History.Add (name, solution);
					} else if (solution.Type.Contains ("Matrix")) {
						string name = autoMatrixNamer ();
						solution.Tag = name;
						solution.setEntireTag (name);
						History.Add (name, solution);
					}
				} else {
					string name = solution.Tag;
					if (History.ContainsKey (name)) {
					//	solution.setEntireTag (name);
						History [name] = solution;
					} else {
					//  solution.setEntireTag (name);
						History.Add (name, solution);
					}
				}
			}

		}     // end method for solve

		public void PrintSolution()
		{
			if (solution.Type.Contains ("Number")) {
				((MathParser.DataTypes.DynamicDataTypes.Number)solution.Data).Print ();
			} else if (solution.Type.Contains ("Matrix")) {
				((MathParser.DataTypes.DynamicDataTypes.Matrix)solution.Data).Print ();
			}
		}

		int ncount = 0;
		string autoNumberNamer()
		{
			ncount++;
			string name = "n" + ncount;
			if (!History.ContainsKey (name)) {
				return name;
			} else {
				return autoNumberNamer ();
			}
		}


		int mcount = 0;
		string autoMatrixNamer()
		{
			mcount++;
			string name = "m" + mcount;
			if (!History.ContainsKey (name)) {
				return name;
			} else {
				return autoMatrixNamer ();
			}
		}

	}
}

