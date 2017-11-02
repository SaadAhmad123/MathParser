using System;
using MathParser.DataTypes;
using MathParser.DataTypes.DynamicDataTypes;
using System.IO;
using System.Linq;

namespace MathParser
{
	public class UnitFunctionSolver : ISolver
	{
		MathParserExpression solution, value;
		bool Processed = true;
		string command;
		string error = "Invalid function argument.";
		public MathParserExpression getSolution() => solution;
		public bool isProcessed() => Processed;

		public UnitFunctionSolver (){}
		public UnitFunctionSolver (string Command,MathParserExpression Value){
			command = Command.Trim ();
			value = Value;
			Solve ();
		}


		// Helper functions
		bool isMultiple(double basenum,double num )
		{
			bool x = false;
			if (num % basenum == 0) {
				x = true;
			}
			return x;
		}

		bool isCosMultiple(double basenum,double num )
		{
			bool x = false;
			if (num % basenum == 0) {
				double hold = num / basenum;
				if (hold % 2 != 0) {
					x = true;
				} else if (basenum == num) {
					x = true;
				}
			}
			return x;
		}
		// Helper functions.


		void Solve()
		{
			if(command == "degToRad"){
				if(value.Type.Contains("Number"))
				{
					double num = ((Number)value.Data).Data;
					solution = new MathParser.DataTypes.MathParserExpression(new Number((double)num * Math.PI / 180));
				}
				else{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function arguments");
				}
			}
			else if (command == "radToDeg"){
				if(value.Type.Contains("Number"))
				{
					double num = ((Number)value.Data).Data;
					solution = new MathParser.DataTypes.MathParserExpression(new Number((double)num * 180/Math.PI));
				}
				else{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function arguments");
				}
			}
			else if (command == "sin")
			{      // for sin function.
				if (value.Type.Contains("Number"))
				{
					double num = ((Number)value.Data).Data;
					double ans = 0;
					if (num > 0)
					{       // for the number to be positive.
						if (isMultiple(180, num))
						{
							ans = 0;
						}
						else
						{
							ans = Math.Sin(num);
						}
					}
					else if (num < 0)
					{     //for the number to be negitive.
						if (isMultiple(-180, num))
						{
							ans = 0;
						}
						else
						{
							ans = Math.Sin(num);
						}
					}
					else
					{         // for the number to be zero.
						ans = Math.Sin(num);
					}
					solution = new MathParserExpression(new Number((double)ans));  // solution make.
				}
				else
				{   // if not a number.
					Processed = false;
					throw new MathParserException("Invalid function argument.");
				}
			}     // end if for sin.

			else if (command == "cos")
			{    // for cos command.
				if (value.Type.Contains("Number"))
				{
					double radian = ((Number)value.Data).Data;
					double num = ((Number)value.Data).Data;
					double ans = 0;
					if (num > 0)
					{      // for positive number.
						if (isCosMultiple(90, num))
						{
							ans = 0;
						}
						else
						{
							ans = Math.Cos(radian);
						}
					}
					else if (num < 0)
					{       // for negitive number.
						if (isCosMultiple(-90, num))
						{
							ans = 0;
						}
						else
						{
							ans = Math.Cos(radian);
						}
					}
					else
					{         // for zero.
						ans = Math.Cos(radian);
					}
					solution = new MathParserExpression(new Number(ans));    // solution making.
				}
				else
				{    // if not a number.
					Processed = false;
					throw new MathParserException("Invalid function argument.");
				}
			}
			else if (command == "tan")
			{

				if (value.Type.Contains("Number"))
				{     // if there is a number.
					double radian = (double)(((Number)value.Data).Data);
					double num = (double)(((Number)value.Data).Data);
					double ans = 0;
					if (num > 0)
					{    // for positive number
						if (isCosMultiple(90, num))
						{
							dynamic z = (double)0;
							ans = 1 / z;
						}
						else if (isMultiple(180, num))
						{
							ans = 0;
						}
						else
						{
							ans = Math.Tan(radian);
						}
					}
					else if (num < 0)
					{     // for negitive number.  
						if (isCosMultiple(-90, num))
						{
							dynamic z = (double)0;
							ans = -1 / z;
						}
						else if (isMultiple(-180, num))
						{
							ans = 0;
						}
						else
						{
							ans = Math.Tan(radian);
						}
					}
					else
					{     // for zero number.
						ans = Math.Tan(radian);
					}
					solution = new MathParserExpression(new Number(ans));    // solution maker.
				}
				else
				{     // it anyother then number.
					Processed = false;
					throw new MathParserException(error);
				}
			}
			else if (command == "cot")
			{
				if (value.Type.Contains("Number"))
				{
					double radian = (double)(((Number)value.Data).Data);
					double num = (double)(((Number)value.Data).Data);
					double ans = 0;
					if (num > 0)
					{      // for positive number.
						if (isMultiple(180, num))
						{
							dynamic z = (double)0;
							ans = 1 / z;
						}
						else if (isCosMultiple(90, num))
						{
							ans = 0;
						}
						else
						{
							ans = 1 / Math.Tan(radian);
						}
					}
					else if (num < 0)
					{     // for negitive number.
						if (isMultiple(-180, num))
						{
							dynamic z = (double)0;
							ans = -1 / z;
						}
						else if (isCosMultiple(-90, num))
						{
							ans = 0;
						}
						else
						{
							ans = 1 / Math.Tan(radian);
						}
					}
					else
					{     // for zero.
						ans = 1 / Math.Tan(radian);
					}
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException(error);
				}
			}
			else if (command == "sec")
			{
				if (value.Type.Contains("Number"))
				{
					double num = (double)(((Number)value.Data).Data);
					double radian = num;
					double ans = 0;
					if (num > 0)
					{    //for +ive number.
						if (isCosMultiple(90, num))
						{
							dynamic z = (double)0;
							ans = 1 / z;
						}
						else
						{
							ans = 1 / Math.Cos(radian);
						}
					}
					else if (num < 0)
					{     //for -ive number.
						if (isCosMultiple(-90, num))
						{
							dynamic z = (double)0;
							ans = -1 / z;
						}
						else
						{
							ans = 1 / Math.Cos(radian);
						}
					}
					else
					{     //for zero.
						ans = 1 / Math.Cos(radian);
					}
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{    // for non- Numbers.
					Processed = false;
					throw new MathParserException(error);
				}
			}
			else if (command == "cosec" || command == "csc")
			{
				if (value.Type.Contains("Number"))
				{
					double num = ((Number)value.Data).Data;
					double ans = 0;
					if (num > 0)
					{       // for the number to be positive.
						if (isMultiple(180, num))
						{
							dynamic z = (double)0;
							ans = 1 / z;
						}
						else
						{
							ans = 1 / Math.Sin(num);
						}
					}
					else if (num < 0)
					{     //for the number to be negitive.
						if (isMultiple(-180, num))
						{
							dynamic z = (double)0;
							ans = -1 / z;
						}
						else
						{
							ans = 1 / Math.Sin(num);
						}
					}
					else
					{         // for the number to be zero.
						ans = 1 / Math.Sin(num);
					}
					solution = new MathParserExpression(new Number((double)ans));  // solution make.
				}
				else
				{   // if not a number.
					Processed = false;
					throw new MathParserException("Invalid function argument.");
				}
			}
			else if (command == "floor")
			{
				if (value.Type.Contains("Number"))
				{
					double num = ((Number)value.Data).Data;
					double ans = Math.Floor(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments.");
				}
			}
			else if (command == "ceil")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Ceiling(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function argument");
				}
			}
			else if (command == "abs")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Abs(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments");
				}
			}
			else if (command == "sinh")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Sinh(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments.");
				}
			}
			else if (command == "cosh")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Cosh(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments.");
				}
			}
			else if (command == "tanh")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Tanh(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Inalid function arguments");
				}
			}
			else if (command == "ln")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Log(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid Function Arguments.");
				}
			}
			else if (command == "log")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Log10(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid Function Arguments.");
				}
			}
			else if (command == "sqrt" || command == "√")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Sqrt(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid Function Arguments.");
				}
			}
			else if (command == "arcCos")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Acos(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid Function Arguments.");
				}
			}
			else if (command == "arcSin")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Asin(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid Function Arguments.");
				}
			}
			else if (command == "arcTan")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Atan(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid Function Arguments.");
				}
			}
			else if (command == "round")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Round(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments");
				}
			}
			else if (command == "trunc")
			{
				if (value.Type.Contains("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Truncate(num);
					solution = new MathParserExpression(new Number(ans));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments");
				}
			}
			else if (command == "ref")
			{
				if (value.Type.Contains("Matrix"))
				{
					Matrix mat = value.Data;
					solution = new MathParserExpression(new Matrix(mat.RowEchelonForm()));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments");
				}
			}
			else if (command == "rref")
			{
				if (value.Type.Contains("Matrix"))
				{
					Matrix mat = value.Data;
					solution = new MathParserExpression(new Matrix(mat.ReducedRowEchelonForm()));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function arguments");
				}
			}
			else if (command == "adj")
			{
				if (value.Type.Contains("Matrix"))
				{
					Matrix mat = value.Data;
					solution = new MathParserExpression(new Matrix(mat.Adjoint()));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function argument");
				}
			}
			else if (command == "transp")
			{
				if (value.Type.Contains("Matrix"))
				{
					Matrix mat = value.Data;
					solution = new MathParserExpression(new Matrix(mat.Transpose()));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function argument");
				}
			}
			else if (command == "rank")
			{
				if (value.Type.Contains("Matrix"))
				{
					Matrix mat = value.Data;
					solution = new MathParserExpression(new Number(mat.Rank()));
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function argument");
				}
			}
			else if (command == "det")
			{
				if (value.Type.Contains("Matrix"))
				{
					Matrix mat = value.Data;
					if (mat.Rows != mat.Columns)
					{
						throw new MathParserException("Not a square matrix. Cannot, compute determinant.");
					}
					solution = new MathParserExpression(mat.Determinant());
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function argument");
				}
			}
			else if (command == "inv")
			{
				if (value.Type.Contains("Matrix"))
				{
					Matrix mat = value.Data;
					if (mat.Rows != mat.Columns)
					{
						throw new MathParserException("Not a square matrix. Cannot, compute inverse.");
					}
					solution = new MathParserExpression(mat.Inverse());
				}
				else
				{
					Processed = false;
					throw new MathParserException("Invalid function argument");
				}
			}
			else if (command == "rootByBisection_polynomial")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 1)
					{
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++)
						{
							array[c] = value.Data[0, c];
						}
						rootsByBisection_polynomial r = new rootsByBisection_polynomial(array, 3);
						double ans = r.Eval();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(ans));
						Processed = true;
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}
			else if (command == "rootByRFM_polynomial")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 1)
					{
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++)
						{
							array[c] = value.Data[0, c];
						}
						regulaFalsiMethod_polynomial r = new regulaFalsiMethod_polynomial(array);
						double ans = r.Eval();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(ans));
						Processed = true;
					}
					else if (value.Data.Rows == 2)
					{
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++)
						{
							array[c] = value.Data[0, c];
						}
						regulaFalsiMethod_polynomial r = new regulaFalsiMethod_polynomial(array, (double)(value.Data[1, 0] == 0 ? 0.1 : value.Data[1, 0]), (int)(value.Data[1, 1] == 0 ? 0.1 : value.Data[1, 1]));
						double ans = r.Eval();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(ans));
						Processed = true;
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}
			else if (command == "rootByNRM_polynomial")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 1)
					{
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++)
						{
							array[c] = value.Data[0, c];
						}
						newtonRaphsonMethod_polynomial r = new newtonRaphsonMethod_polynomial(array);
						double ans = r.Eval();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(ans));
						Processed = true;
					}
					else if (value.Data.Rows == 2)
					{
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++)
						{
							array[c] = value.Data[0, c];
						}
						newtonRaphsonMethod_polynomial r = new newtonRaphsonMethod_polynomial(array, (double)(value.Data[1, 0] == 0 ? 0.1 : value.Data[1, 0]), (int)(value.Data[1, 1] == 0 ? 0.1 : value.Data[1, 1]));
						double ans = r.Eval();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(ans));
						Processed = true;
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}
			else if (command == "rootBySM_polynomial")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 1)
					{
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++)
						{
							array[c] = value.Data[0, c];
						}
						secantMethod_polynomial r = new secantMethod_polynomial(array);
						double ans = r.Eval();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(ans));
						Processed = true;
					}
					else if (value.Data.Rows == 2)
					{
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++)
						{
							array[c] = value.Data[0, c];
						}
						secantMethod_polynomial r = new secantMethod_polynomial(array, (double)(value.Data[1, 0] == 0 ? 0.1 : value.Data[1, 0]), (int)(value.Data[1, 1] == 0 ? 0.1 : value.Data[1, 1]));
						double ans = r.Eval();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(ans));
						Processed = true;
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}
			else if (command == "linearFit_XY")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 2)
					{
						if(value.Data.Columns < 2){
							Processed = false;
							throw new MathParser.MathParserException("The amount of data is very less");
						}
						Matrix matrix = value.Data;
						linearFit_XY lfxy = new linearFit_XY(ref matrix);
						lfxy.Solve();
						solution = new MathParser.DataTypes.MathParserExpression(lfxy.getSolution());
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
			}
			else if (command == "polynomialFit_XY")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 2)
					{
						if(value.Data.Columns < 2){
							Processed = false;
							throw new MathParser.MathParserException("The amount of data is very less");
						}
						Matrix matrix = value.Data;
						polynomialFit_XY lfxy = new polynomialFit_XY(ref matrix);
						lfxy.Solve();
						solution = new MathParser.DataTypes.MathParserExpression(lfxy.getSolution());
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}
			else if (command == "exponentialFit_XY")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 2)
					{
						if(value.Data.Columns < 2){
							Processed = false;
							throw new MathParser.MathParserException("The amount of data is very less");
						}
						Matrix matrix = value.Data;
						exponentialFit_XY lfxy = new exponentialFit_XY(ref matrix);
						lfxy.Solve();
						solution = new MathParser.DataTypes.MathParserExpression(lfxy.getSolution());
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}
			else if (command == "geometricFit_XY")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows == 2)
					{
						if(value.Data.Columns < 2){
							Processed = false;
							throw new MathParser.MathParserException("The amount of data is very less");
						}
						Matrix matrix = value.Data;
						geometricFit_XY lfxy = new geometricFit_XY(ref matrix);
						lfxy.Solve();
						solution = new MathParser.DataTypes.MathParserExpression(lfxy.getSolution());
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}
			else if (command == "linearFit_ND")
			{
				if (value.Type.Contains("Matrix"))
				{
					if (value.Data.Rows > 1)
					{
						if(value.Data.Columns < 2){
							Processed = false;
							throw new MathParser.MathParserException("The amount of data is very less");
						}
						Matrix matrix = value.Data;
						linearFit_ND lfxy = new linearFit_ND(ref matrix);
						lfxy.Solve();
						solution = new MathParser.DataTypes.MathParserExpression(lfxy.getSolution());
					}
					else
					{
						Processed = false;
						throw new MathParser.MathParserException("Invalid function argument");
					}
				}
				else
				{
					Processed = false;
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}else if(command == "interpolationByNFIM"){
				if(value.Type.Contains("Matrix")){
					if (value.Data.Rows != 4)
					{
						throw new MathParser.MathParserException("4 argument rows expected -> [x_values; y_values; interpolation_index; value_of_X_to_which_Y_is_to_be_found]");
					}
					else {
						Matrix m = value.Data;
						newtonForwardInterpolationMethod nfim = new newtonForwardInterpolationMethod(ref m);
						nfim.Solve();
						solution = new MathParser.DataTypes.MathParserExpression(new Number(nfim.getSolution()));
					}
				}else{
					throw new MathParser.MathParserException("Invalid function argument");
				}
			}else if(command == "factorial"){
				if(value.Type.Contains("Number")){
					double number = value.Data;
					Factorial f = new Factorial(number);
					f.Solve();
					solution = new MathParser.DataTypes.MathParserExpression(new Number(f.getSolution()));
				}else{
					throw new MathParserException("Invalid function arguments");
				}
			}
			else if (Solver.On_Single_Argument_KeyWord_Implement != null)
			{
				Solver.On_Single_Argument_KeyWord_Implement(command, value, ref solution, ref Processed);
			}
			else
			{

				Processed = false;
				throw new MathParserException($"No '{command}' implementation defined.");
			}

		}

	}





	public class linearFit_ND{

		Matrix matrix, ans;
		public linearFit_ND(){}
		public linearFit_ND(ref Matrix matrix){
			this.matrix = matrix;
		}

		public void Solve(){
			int n = matrix.Columns;
			int calRow = matrix.Rows, calCol = matrix.Rows + 1;
			Matrix cal = new Matrix(calRow, calCol);
			cal[0, 0] = (double)n;
			for (int c = 0; c < n; c++) {
				for (int c2 = 0; c2 < matrix.Rows; c2++) {
					cal [0, c2 + 1] = cal [0, c2 + 1] + matrix [c2, c];
					if (c2 != 0) {
						cal [c2, 0] = cal [c2, 0] + matrix [c2 - 1, c];
					}
				}
			}
			for (int i = 0; i < matrix.Columns; i++) {
				int mr = 0;
				for (int r = 1; r < calRow; r++) {
					for (int c = 1; c < calCol; c++) {
						cal [r, c] = cal [r, c] + (matrix [mr, i] * matrix [c - 1, i]);
					}
					mr++;
				}
			}

			cal = cal.ReducedRowEchelonForm ();
			ans = new Matrix (2, calRow);
			for (int c = 1; c < calRow; c++) {
				ans [0, c-1] = cal [c, cal.Columns - 1];
			}
			ans [0, ans.Columns - 1] = cal [0, cal.Columns - 1];

			// Error

			double e = 0;

			for(int i = 0; i < matrix.Columns; i++){
				double err = 0;
				for (int c = ans.Columns - 1; c >= 0; c--) {
					if (c == ans.Columns - 1) {
						err += ans [0, c];
					} else {
						err += ans [0, c] * matrix [c , i];
					}
				}
				err = matrix [matrix.Rows - 1, i] - err;
				e += err * err;
			}


			ans [1, 0] = e;

		}


		public Matrix getSolution(){
			return ans;
		}

	}







	/// <summary>
	/// Geometric fit xy.
	/// This class gets the data set X and Y;
	/// and calculate an geometric Fit of 
	/// that data. The matix must strictly have only
	/// 2 Rows i.e. [x1, x2, ..., xN; y1, y2, ..., yN]
	/// 
	/// It uses the function Solve to initiate
	/// the solution calculation and make result
	/// 	
	/// After the using the getSolution function
	/// we can get matrix [a,b] where y = a*x^b
	/// which is the linear Fit of the given 
	/// data
	/// </summary>
	public class geometricFit_XY{

		Matrix matrix, ans;
		public geometricFit_XY(){}
		public geometricFit_XY(ref Matrix matrix){
			this.matrix = matrix;
		}

		public void Solve(){
			// Here, 
			// c1 = sum : 1 to n -> (log(y) = Y)
			// c2 = sum : 1 to n -> (log(x) = X)
			// c3 = sum : 1 to n -> (log(x) * log(y) = XY)
			// c4 = sum : 1 to n -> (log(x) * log(x) = X^2)
			// Equation :-
			//		c1 = nA + bc2
			//		c3 = Ac2 + bc4
			// Where :-
			//		a = antilog(A)
			double c1 = 0, c2 = 0, c3 = 0, c4 = 0, n = matrix.Columns;
			for (int c = 0; c < n; c++)
			{
				double x = matrix[0, c], y = matrix[1, c];
				c1 += Math.Log10(y);
				c2 += Math.Log10(x);
				c3 += Math.Log10(x) * Math.Log10(y);
				c4 += Math.Log10(x) * Math.Log10(x);
			}

			Matrix m = new Matrix(new dynamic[,] { { n, c2, c1 }, { c2, c4, c3 } }, 2, 3);
			m = m.ReducedRowEchelonForm();
			ans = new Matrix(new dynamic[,] { { Math.Pow(10, m[0, 2]), m[1, 2] } }, 1, 2);
		}

		public Matrix getSolution(){
			return ans;
		}

	}







	/// <summary>
	/// Exponential fit xy.
	/// This class gets the data set X and Y;
	/// and calculate an exponential Fit of 
	/// that data. The matix must strictly have only
	/// 2 Rows i.e. [x1, x2, ..., xN; y1, y2, ..., yN]
	/// 
	/// It uses the function Solve to initiate
	/// the solution calculation and make result
	/// 	
	/// After the using the getSolution function
	/// we can get matrix [a,b] where y = ae^(bx)
	/// which is the linear Fit of the given 
	/// data
	/// </summary>
	public class exponentialFit_XY{
		Matrix matrix, ans;
		public exponentialFit_XY(){}
		public exponentialFit_XY(ref Matrix matrix){
			this.matrix = matrix;
		}

		public void Solve(){
			// Here.
			// c1 = sum : 1 to n -> (log(y) => Y)
			// c2 = sum : 1 to n -> (x)
			// c3 = sum : 1 to n -> (x * log(y) => xY)
			// c4 = sum : 1 to n -> (x^2)
			// Equation :-
			// 		c1 = nA + Bc2
			//		c3 = Ac2 + Bc4
			// Form :- 
			//		Y = A + Bx
			// Now :-
			//		a = antilog(A)
			//		b = B / log(e)
			// Matric :-
			// 		| n		c2 	:	c1 |
			//		| c2	c4	:	c3 |
			double c1 = 0, c2 = 0, c3 = 0, c4 = 0, n = matrix.Columns;
			for (int c = 0; c < n; c++)
			{
				double y = matrix[1, c], x = matrix[0, c];
				c1 += Math.Log10((double)y);
				c2 += x;
				c3 += x * Math.Log10((double)y);
				c4 += x * x;
			}
			Matrix m = new Matrix(new dynamic[,] { { n, c2, c1 }, { c2, c4, c3 } }, 2, 3);
			m = m.ReducedRowEchelonForm();

			ans = new Matrix(new dynamic[,] { { Math.Pow(10, m[0, 2]) ,m[1, 2]/Math.Log10(Math.E)} }, 1, 2);
		}


		public Matrix getSolution(){
			return ans;
		}
	}








	/// <summary>
	/// Polynomial fit xy.
	/// This class gets the data set X,Y and
	/// calculate the polynomial fit for the 
	/// data. The matrix must strictly have only 2
	/// Rows i.e. [x1, x2, ..., xN; y1, y2, ..., yN]
	/// 
	/// It uses the function Solve to initiate
	/// the solution calculation and make result
	/// 
	/// After the using the getSolution function
	/// we can get matrix [a,b,c] where y = ax^2 + bx + c
	/// which is the linear Fit of the given 
	/// data
	/// </summary>
	public class polynomialFit_XY{
		Matrix matrix, ans;
		public polynomialFit_XY(){}
		public polynomialFit_XY(ref Matrix matrix) {
			this.matrix = matrix;
		}

		public void Solve(){
			// Here,
			// c1 = sum : 1 to n -> (x^2*y)
			// c2 = sum : 1 to n -> (x^4)
			// c3 = sum	 : 1 to n -> (x^3)
			// c4 = sum : 1 to n -> (x^2)
			// c5 = sum : 1 to n -> (xy)
			// c6 = sum : 1 to n -> (x)
			// c7 = sum : 1 to n -> )
			// Equations :- 
			// 		c1 = ac2 + bc3 + c*c4
			//		c5 = ac3 + bc4 + c*c6
			// 		c7 = ac4 + bc6 + c*n
			// Matrix :-
			//		| c2	c3	c4	:	c1 |
			//		| c3	c4	c6	:	c5 |
			//		| c4	c6	n	:	c7 |
			double c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0, c6 = 0, c7 = 0, n = matrix.Columns;
			for (int c = 0; c < n; c++)
			{
				double x = matrix.data[0, c], y = matrix.data[1, c];
				c1 += x * x * y;
				c2 += x * x * x * x;
				c3 += x * x * x;
				c4 += x * x;
				c5 += x * y;
				c6 += x;
				c7 += y;
				Matrix m = new Matrix(new dynamic[,] { { c2, c3, c4, c1 }, { c3, c4, c6, c5 }, { c4, c6, n, c7 } }, 3, 4);
				m = m.ReducedRowEchelonForm();
				ans = new Matrix(new dynamic[,] { { m[0, 3], m[1, 3], m[2, 3] } }, 1, 3);
			}

		}

		public Matrix getSolution(){
			return ans;
		}

	}







	/// <summary>
	/// Linear fit xy.
	/// This class uses a matrix and evaluates 
	/// The linear Fit of the data in the matrix
	/// The matrix must have strictly 2 rows in
	/// it i.e.  [x1, x2, ..., xN; y1, y2, ..., yN]
	/// 
	/// It uses the function Solve to initiate
	/// the solution calculation and make result
	/// 
	/// After the using the getSolution function
	/// we can get matrix [a,b] where y = ax + b
	/// which is the linear Fit of the given 
	/// data
	/// </summary>
	public class linearFit_XY{

		Matrix matrix;
		Matrix ans;
		public linearFit_XY(){}
		public linearFit_XY(ref Matrix matrix){
			this.matrix = matrix;
		}

		public void Solve(){
			// Here,
			// c1 = sum : 1 to n -> (xy)
			// c2 = sum : 1 to n -> (x^2)
			// c3 = sum : 1 to n -> (x)
			// c4 = sum : 1 to n -> (y)
			// Equations :-
			// 		c1 = ac2 + bc3
			//		c4 = ac3 + nb
			// Matrix to get a and b via reduced row echelon form :-
			// 		| c2 c3 : c1 |
			//		| c3 n  : c3 |
			double c1 = 0, c2 = 0, c3 = 0, c4 = 0, n = matrix.Columns;
			for (int c = 0; c < n; c++){
				c1 += matrix.data[0, c] * matrix.data[1, c];
				c2 += matrix.data[0, c] * matrix.data[0, c];
				c3 += matrix.data[0, c];
				c4 += matrix.data[1, c];
			}

			Matrix m = new Matrix(new dynamic[,] {{c2,c3,c1},{c3,n,c4}}, 2, 3);	
			m = m.ReducedRowEchelonForm();
			ans = new Matrix(new dynamic[,] { { m.data[0, 2], m.data[1, 2] } }, 1, 2);
		}

		public Matrix getSolution(){
			return ans;
		}


	}



	public class rootsByBisection_polynomial{


		double[] array; int precision;
		public rootsByBisection_polynomial(){}
		public rootsByBisection_polynomial(double[] Array, int Precision){
			this.array = Array;
			this.precision = Precision;
		}

		double evaluateEquation(double x){
			int maxPower = array.Length - 1;
			double ans = 0;
			for (int c = 0; c < array.Length; c++) {
				ans += array [c] * Math.Pow (x, maxPower - c);
			}
			return ans;
		}

		public double Eval(){			
			double[] interval = getRootInterval(new double[] {-100, 100});
			double pow = Math.Pow (20, this.precision);
			double cal = 0;
			double rangeMean = 0;
			for(int c = 0; c <= 10; c++){
				if (c != 0) {
					interval = getRootInterval (interval, (double)
						1 / (10000 * c), 1);
				}
				rangeMean = (interval [0] + interval [1]) / 2;
				double r_val = evaluateEquation (rangeMean);
				if (r_val == 0) {
					return rangeMean;
				}
				double val = Math.Truncate (r_val * pow) / pow;
				double diff = Math.Abs (val - cal);
				cal = val;
				if(diff < 1/pow){
					break;
				}
				if (r_val < 0) {
					interval [0] = rangeMean;
				}else if(r_val > 0){
					interval [1] = rangeMean;
				}
			}
			return rangeMean; 
		}

		double[] getRootInterval(double[] range,double increment = 1.0000000,int iteration = 3){
			double min = range [0]; double max = range [1];
			double[] n_range = new double[2];
			n_range[0] = min;
			n_range [1] = max;
			for (double c = min; c <= max; c += increment) {

				double e = evaluateEquation (c);
				if (e < 0) {
					if (c > n_range [0]) {
						n_range [0] = c;
					}
				} else if (e > 0) {
					if (c < n_range [1]) {
						n_range [1] = c;
					}
				} else {
					n_range [0] = c;
					n_range [1] = c;
					return n_range;
				}
			}

			if (iteration <= 1) {
				return n_range;
			}
			iteration--;
			return getRootInterval (n_range, increment / 10, iteration);
		}


	}


	public class Polynomial_Eval{

		double[] array;
		public Polynomial_Eval(){
			array = null;
		}
		public Polynomial_Eval(double[] arr){
			array = arr;
		}

		/// <summary>
		/// This function evaluates the polynomial array on the given x i.e Eval(x) <=> f(x);
		/// Input (x) double;
		/// Output Eval(x) double;
		/// </summary>
		public double Eval(double x){
			if (array == null) {
				throw new MathParser.MathParserException ("There is no equation, the given array is null.");
			}
			int maxPower = array.Length - 1;
			double ans = 0;
			for (int c = 0; c < array.Length; c++) {
				ans += array [c] * Math.Pow (x, maxPower - c);
			}
			return ans;		
		}
	}

	public class regulaFalsiMethod_polynomial{
		double[] array;
		double precision;
		int max_iteration;
		Polynomial_Eval pe;
		public regulaFalsiMethod_polynomial(){}
		public regulaFalsiMethod_polynomial(double[] array, double precision = 0.10, int max_iteration = 30){
			this.array = array;
			pe = new Polynomial_Eval (this.array);
			this.precision = precision;
			this.max_iteration = max_iteration;
		}


		public double Eval(){
			double a_max = array.Max (x => Math.Abs(x));
			double[] r = getRootInterval(new double[] {a_max*-2,2*a_max});
			double e1 = pe.Eval (r [0]);
			double e2 = pe.Eval (r [1]);
			double x2 = 0;
			double x2_old = 0;
			if ((e1 < 0 && e2 < 0) || (e1 > 0 && e2 > 2)) {
				throw new MathParser.MathParserException ($"The function did not change value in interval {r[0]} to {r[1]}.Most probably the funciton doesnot change its value.");
			}
			for (int i = 0; i < max_iteration; i++) {
				if (i != 0) {
					r = getRootInterval (r,1);		
				}

				Console.WriteLine ($"x0 = {r[0]}, x1 = {r[1]}");

				double a0 = pe.Eval (r [0]);
				double a1 = pe.Eval (r [1]);
				x2 = ((r [0] * a1) - (r [1] * a0)) / (a1 - a0);
				double a2 = pe.Eval (x2);
				Console.WriteLine ($"ans = {x2}");
				if (a2 < 0) {
					r [0] = x2;
				} else if (a2 > 0) {
					r [1] = x2;
				} else {
					return x2;
				}
				double err = Math.Pow((x2 - x2_old)/x2,1);
				Console.WriteLine ($"err = {err}");
				if (err < precision) {
					break;
				}
				x2_old = x2;
			}

			return x2;

		}

		double[] getRootInterval(double[] range, int itrr = 3, int numberOfSteps = 100){
			double max_coeff = range[1];
			double min_coeff = range[0];
			double increment = (max_coeff - min_coeff >= 100)?1.000000000:(max_coeff - min_coeff) / numberOfSteps;
			double lastval = pe.Eval(min_coeff);
			double last_c = min_coeff;

			for (double c = min_coeff + increment; c <= max_coeff; c += increment) {
				double e = pe.Eval (c);
				if ((lastval < 0 && e > 0) || (lastval > 0 && e < 0)) {
					range [0] = last_c;
					range [1] = c;
					break;
				} else {
					last_c = c;
					lastval = e;
				}
			}

			if(itrr <= 1){return range;}
			return getRootInterval (range, itrr - 1, numberOfSteps);
		}


	}


	public class newtonRaphsonMethod_polynomial{

		double[] array;
		double precision;
		int max_iteration;
		Polynomial_Eval pe;
		public newtonRaphsonMethod_polynomial(){}
		public newtonRaphsonMethod_polynomial(double[] array, double precision = 0.1, int max_iteration = 30){
			this.array = array;
			this.precision = precision;
			this.max_iteration = max_iteration;
			pe = new Polynomial_Eval (this.array);
		}

		double[] getEsstimatePointInterval(){
			double a_max = array.Max (x => Math.Abs(x));
			double[] r = getRootInterval (new double[] { a_max * -2, 2 * a_max });
			double e0 = pe.Eval (r [0]);

			if (e0 > 0) {
				return new double[] { r [0], r [0] + Math.Pow(10,-15), -1};
			} else {
				return new double[] { r [1], r [1] - Math.Pow(10,-15), 1 };
			}
		}

		public double getFirstDerviative(double[] interval){
			if (interval [1] > interval [0]) {
				return (pe.Eval (interval [1]) - pe.Eval (interval [0])) / (interval [1] - interval [0]);
			} else {
				return (pe.Eval (interval [0]) - pe.Eval (interval [1])) / (interval [0] - interval [1]);
			}
		}

		double getPoint(double xk, double f, double df){
			return xk - (f / df);	
		}

		double getError(double x , double x_old){
			return Math.Abs ((x - x_old) / x);
		}

		public double Eval(){
			double[] interval = getEsstimatePointInterval ();
			double df = getFirstDerviative (interval);
			double f = pe.Eval (interval [0]);
			double old_xk = 0, xk = 0;
			for (int c = 0; c < max_iteration; c++) {
				xk = getPoint (interval [0], f , df);
				if (getError (xk, old_xk) < precision) {
					return xk;
				}
				interval [0] = xk;
				old_xk = xk;
			}
			return xk;
		}


		double[] getRootInterval(double[] range, int itrr = 3, int numberOfSteps = 100){
			double max_coeff = range[1];
			double min_coeff = range[0];
			double increment = (max_coeff - min_coeff >= 100)?1.000000000:(max_coeff - min_coeff) / numberOfSteps;
			double lastval = pe.Eval(min_coeff);
			double last_c = min_coeff;

			for (double c = min_coeff + increment; c <= max_coeff; c += increment) {
				double e = pe.Eval (c);
				if ((lastval < 0 && e > 0) || (lastval > 0 && e < 0)) {
					range [0] = last_c;
					range [1] = c;
					break;
				} else {
					last_c = c;
					lastval = e;
				}
			}

			if(itrr <= 1){return range;}
			return getRootInterval (range, itrr - 1, numberOfSteps);
		}

	}

	public class secantMethod_polynomial{
		double[] array; double precision; int max_iterration;
		Polynomial_Eval pe;
		public secantMethod_polynomial(){}
		public secantMethod_polynomial(double[] array, double precision = 0.1, int max_iterration = 30){
			this.array = array;
			pe = new Polynomial_Eval (this.array);
			this.precision = precision;
			this.max_iterration = max_iterration;
		}

		double getPoint(double x0, double x1){
			double e0 = pe.Eval (x0);
			double e1 = pe.Eval (x1);
			return (x0 * e1 - x1 * e0) / (e1 - e0);
		}

		double getError(double curr, double old){
			return Math.Abs ((curr - old) / curr);
		}

		public double Eval(){
			double a_max = array.Max (x => Math.Abs (x));
			double[] r = getRootInterval (new double[] { a_max * -2, 2 * a_max });
			double x2 = 0, x2_old = 0;
			x2_old = getPoint (r [0], r [1]);
			r [0] = r [1];
			r [1] = x2_old;
			for (int i = 0; i < max_iterration; i++) {
				x2 = getPoint (r [0], r [1]);
				double err = getError (x2, x2_old);
				if (err < precision) {
					return x2;
				}
				r [0] = x2_old;
				r [1] = x2;
				x2_old = x2;
			}
			return x2;
		}



		double[] getRootInterval(double[] range, int itrr = 3, int numberOfSteps = 100){
			double max_coeff = range[1];
			double min_coeff = range[0];
			double increment = (max_coeff - min_coeff >= 100)?1.000000000:(max_coeff - min_coeff) / numberOfSteps;
			double lastval = pe.Eval(min_coeff);
			double last_c = min_coeff;

			for (double c = min_coeff + increment; c <= max_coeff; c += increment) {
				double e = pe.Eval (c);
				if ((lastval < 0 && e > 0) || (lastval > 0 && e < 0)) {
					range [0] = last_c;
					range [1] = c;
					break;
				} else {
					last_c = c;
					lastval = e;
				}
			}

			if(itrr <= 1){return range;}
			return getRootInterval (range, itrr - 1, numberOfSteps);
		}
	}

	public class Factorial{

		double number,solution;
		public Factorial(){}
		public Factorial(double num){
			number = num;
			solution = 1;
		}

		public void Solve(){

			for (int i = 1; i <= number; i++)
			{
				solution = solution * i;
			}
		}

		public double getSolution() { return solution;}

	}



	/// <summary>
	/// Newton forward interpolation method.
	/// 
	/// This function does the newton forward interpolation in 
	/// two diamensions (X, Y).
	/// 
	/// It takes the matrix with following characteristics :
	/// 
	/// [row1 -> x_values; 
	///  row2 -> y_values; 
	///  row3 -> interpolation_index; 
	///  row4 -> the value of X to find Y]
	/// 
	/// </summary>



	public class newtonForwardInterpolationMethod{

		Matrix matrix;
		Matrix sol_mat;
		double solution;
		public double getSolution(){return solution;}
		public newtonForwardInterpolationMethod(){}
		public newtonForwardInterpolationMethod(ref Matrix mat){
			matrix = mat;
			solution = 0;
		}

		public void Solve(){
			int col = matrix.Columns;
			int rows = matrix.Rows;
			int interpolation_index = (int)matrix[2, 0]; // This parameter defines the polynomial of interpolation e.g. 1 means linear equation, 2 means x^2 + x + c, 3 means x^3 + x^2 + x + c and so on.
														 // This is limited to the number of data points. E.G. if 10 data points (X,Y) are given then polynomial of interpolation can be atmost 10

			if(interpolation_index > col - 1){
				throw new MathParser.MathParserException("Invalid interpolation index -> must be less than or equal to the number of data points");
			}else if(interpolation_index < 2){
				throw new MathParser.MathParserException("Interpolation Index cannot be equal to or less than 1");
			}

			double valueOf_X_toFind = matrix[3, 0];
			sol_mat = new Matrix(col, col);
			for (int c = 0; c < col; c++){
				for (int c1 = 0; c1 < col - c; c1++){
					if(c == 0){
						sol_mat[c1, c] = matrix[1, c1];
					}else if(c1+1 < col){
						sol_mat[c1, c] = - sol_mat[c1, c - 1] + sol_mat[c1 + 1, c - 1];
					}
				}
			}

			double P = (valueOf_X_toFind - matrix[0, 0])/(Math.Abs(matrix[0,1]-matrix[0,0]));

			double yP = sol_mat[0,0];
			for (int c = 1; c <= interpolation_index; c++){
				double new_p = P;
				for (int c1 = 1; c1 < c;c1++){
					new_p = new_p * (P - c1);
				}
				Factorial f = new Factorial(c);
				f.Solve();

				yP = yP + ((new_p * sol_mat[0, c]) / f.getSolution());
			}

			solution = yP;

		}

	}




}
