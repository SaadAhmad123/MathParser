using System;

using MathParser.DataTypes.DynamicDataTypes;
using MathParser.DataTypes;
using System.Collections.Generic;

namespace MathParser
{

	/// <summary>
	/// Matrix builder.
	/// This class actuall makes the matrix out of the given string,
	/// If it is in correct syntax.
	/// The flags for the matrix structure are defined as default to be "[", "Space", ";", "]".
	/// They can be definded by the programmer externally by the static and on static delegates 'OnSyncFlags()' and 'staticOnSyncFlag()'.
	/// The non-static one gets the highest precendence.
	/// 
	/// The parsing is defind by default to convert the elements into the double.
	/// But the parsing methods can be defined by the programmer externally, by the delegate methods which are 
	/// static and non-static. The non-static one gets the highest precedence. 
	/// The delegate is OnParse() and staticOnParse();
	/// 
	/// How to use:
	/// 
	/// MatrixBuilder builder = new MatrixBuilder("[2 2 ; 2 2 ]");
	/// 
	/// builder.OnSyncFlag += (ref string FlagStart,ref string FlagEnd, ref string FlagElementSeperation, ref string FlagRowSeperation) => 
	/// {
	/// // Do Something like;
	/// FlagStart = "[";
	/// FlagEnd = "]";
	/// //....
	/// }; 
	/// 
	/// builder.SyncFlags();
	/// builder.Parse();
	/// Matrix mat = builder.getMatrix();
	/// 
	/// //....
	/// 
	/// </summary>

	public class MatrixBuilder : ISolver
	{
		string givenExpression;
		DataTypes.DynamicDataTypes.Matrix theMatrix;
		bool Processed = true;
		public bool isProcessed() => Processed;
	    string FlagStart = "[", FlagEnd = "]", FlagElementSeperation = " ", FlagRowSeperation = ";";

		public delegate void FlagSyncerDelegate(ref string FlagStart,ref string FlagEnd, ref string FlagElementSeperation, ref string FlagRowSeperation);
		public FlagSyncerDelegate OnFlagSync;
		public static FlagSyncerDelegate staticOnFlagSync;
		public delegate dynamic ParseAlgorithm(string matrixElementString, ref bool Successfully_processed);
		public static ParseAlgorithm staticOnParse;
		public ParseAlgorithm OnParse;

		public MathParserExpression getSolution() {
			MathParserExpression result = new MathParserExpression(theMatrix);
			return result;
		}

		public Matrix getMatrix()
		{
			return theMatrix;
		}

		void DefaultFlagSetter()
		{
			FlagStart = "[";
			FlagEnd = "]";
			FlagRowSeperation = ";";
			FlagElementSeperation = " ";
		}

		public void SyncFlags()
		{
			if (OnFlagSync != null)
			{
				OnFlagSync(ref FlagStart, ref FlagEnd, ref FlagElementSeperation, ref FlagRowSeperation);
			}
			else if (staticOnFlagSync != null)
			{
				staticOnFlagSync(ref FlagStart, ref FlagEnd, ref FlagElementSeperation, ref FlagRowSeperation);
			}
			else
			{ 
			    DefaultFlagSetter();
			}
		}

		public MatrixBuilder(){}
		public MatrixBuilder(string String_To_Parse) {
			givenExpression = String_To_Parse.Trim();
		}


		// Checks the correct syntax of the given Matrix.
		private bool MatrixSyntaxChecker()
		{
			bool MatrixAlright = true;
			if (givenExpression.Contains(FlagStart) && givenExpression.Contains(FlagEnd))
			{
				if (givenExpression.StartsWith(FlagStart) && givenExpression.EndsWith(FlagEnd))
				{
				    int lenghtS = givenExpression.Replace(FlagStart, "").Length;
					int lenghtE = givenExpression.Replace(FlagEnd, "").Length;
					if ((lenghtE == givenExpression.Length - 1) && (lenghtE == givenExpression.Length - 1))
					{ }
					else {
						MatrixAlright = false;
						throw new Exception("Invalid Matrix Syntax.");
					}
				}
				else {
					MatrixAlright = false;
					throw (new Exception("Invalid Matrix Syntax. The given string does not contain the correct sequences of \'{FlagStart}\' and \'{FlagEnd}\'."));
				}
			}
			else {
				MatrixAlright = false;
				throw (new Exception($"Invalid Matrix Syntax. The given string does not contain \'{FlagStart}\' in the Start and \'{FlagEnd}\' in the End."));
			}
			return MatrixAlright;	
		}
		// End matrix syntax checker.

		public void Parse()    // Parse method.
		{
		    bool proceed = MatrixSyntaxChecker();
			if (!proceed)
			{
				Processed = false;
			}
			else {
				string matExpression = givenExpression.TrimStart(FlagStart.ToCharArray());
			    matExpression = matExpression.TrimEnd(FlagEnd.ToCharArray());
				matExpression = matExpression.Trim();
				List<string> theStringRows = new List<string>(matExpression.Split(FlagRowSeperation.ToCharArray()));
				theStringRows.Remove("");
				int rows = theStringRows.Count;
				int cols = 0;
				List<List<string>> theMatrixElements = new List<List<string>>();
				foreach (string row in theStringRows)
				{

					List<string> dumyList = new List<string>(row.Trim().Split(FlagElementSeperation.ToCharArray()));
					dumyList.Remove("");
					if (dumyList.Count > cols)
					{
						cols = dumyList.Count;
					}
					theMatrixElements.Add(dumyList);
				}
				theMatrix = new Matrix(rows, cols, "");
				int irow = 0, icol = 0;
				foreach (List<string> row in theMatrixElements)
				{
					icol = 0;
					foreach (string element in row)
					{
						if (OnParse != null)
						{
							dynamic mElement = OnParse(element.Trim(), ref Processed);
							theMatrix[irow, icol] = mElement;
						}
						else if (staticOnParse != null)
						{
							dynamic mElement = staticOnParse(element.Trim(), ref Processed);
							theMatrix[irow, icol] = mElement;
						}

						else
						{
							NonEquation sol = new NonEquation (element.Trim ());
							sol.History = Checker.History;
							sol.Solve();
							Number a;
							if (sol.isProcessed ()) {
								MathParserExpression ans = sol.getSolution ();
								a = ans.Data;
							} else {
								Processed = false;
								throw new MathParserException ("Invalid Entry.");
							}
							theMatrix[irow, icol] = (double)a.Data;
							Processed = true;
						}
						if (!Processed)
						{
							Processed = false;
							break;
						}
						icol++;
					}
					if (!Processed)
					{
						Processed = false;
						break;
					}
					irow++;
				}
				if (!Processed)
				{
					theMatrix = new Matrix(1, 1);
				}
			}
		}
	}
}

