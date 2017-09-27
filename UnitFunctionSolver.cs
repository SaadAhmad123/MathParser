﻿using System;
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
			if (command == "sin") {      // for sin function.
				if (value.Type.Contains ("Number")) {
					double num = ((Number)value.Data).Data;
					double ans = 0;
					if (num > 0) {       // for the number to be positive.
						if (isMultiple (180, num)) {
							ans = 0;
						} else {
							ans = Math.Sin (num * Math.PI / 180);
						}
					} else if (num < 0) {     //for the number to be negitive.
						if (isMultiple (-180, num)) {
							ans = 0;
						} else {
							ans = Math.Sin (num * Math.PI / 180);
						}
					} else {         // for the number to be zero.
						ans = Math.Sin (num * Math.PI / 180);
					}
					solution = new MathParserExpression (new Number ((double)ans));  // solution make.
				} else {   // if not a number.
					Processed = false;
					throw new MathParserException ("Invalid function argument.");
				}
			}	  // end if for sin.

			else if (command == "cos") {    // for cos command.
				if (value.Type.Contains ("Number")) {
					double radian = ((Number)value.Data).Data * Math.PI / 180;
					double num = ((Number)value.Data).Data;
					double ans = 0;
					if (num > 0) {      // for positive number.
						if (isCosMultiple (90, num)) {
							ans = 0;
						} else {
							ans = Math.Cos (radian);	
						}
					} else if (num < 0) {       // for negitive number.
						if (isCosMultiple (-90, num)) {
							ans = 0;
						} else {
							ans = Math.Cos (radian);
						}
					} else {         // for zero.
						ans = Math.Cos (radian);
					}
					solution = new MathParserExpression (new Number (ans));    // solution making.
				} else {    // if not a number.
					Processed = false;
					throw new MathParserException ("Invalid function argument.");
				}
			} else if (command == "tan") {

				if (value.Type.Contains ("Number")) {     // if there is a number.
					double radian = (double)(((Number)value.Data).Data) * Math.PI / 180;
					double num = (double)(((Number)value.Data).Data);
					double ans = 0;
					if (num > 0) {    // for positive number
						if (isCosMultiple (90, num)) {    
							dynamic z = (double)0;
							ans = 1 / z;
						} else if (isMultiple (180, num)) {
							ans = 0;
						} else {
							ans = Math.Tan (radian);
						}
					} else if (num < 0) {     // for negitive number.  
						if (isCosMultiple (-90, num)) {
							dynamic z = (double)0;
							ans = -1 / z;
						} else if (isMultiple (-180, num)) {
							ans = 0;
						} else {
							ans = Math.Tan (radian);
						}
					} else {     // for zero number.
						ans = Math.Tan (radian);
					}
					solution = new MathParserExpression (new Number (ans));    // solution maker.
				} else {     // it anyother then number.
					Processed = false;
					throw new MathParserException (error);
				}
			} else if (command == "cot") {
				if (value.Type.Contains ("Number")) {
					double radian = (double)(((Number)value.Data).Data) * Math.PI / 180;
					double num = (double)(((Number)value.Data).Data);
					double ans = 0;
					if (num > 0) {      // for positive number.
						if (isMultiple (180, num)) {
							dynamic z = (double)0;
							ans = 1 / z;
						} else if (isCosMultiple (90, num)) {
							ans = 0; 
						} else {
							ans = 1 / Math.Tan (radian);
						}
					} else if (num < 0) {     // for negitive number.
						if (isMultiple (-180, num)) {
							dynamic z = (double)0;
							ans = -1 / z;
						} else if (isCosMultiple (-90, num)) {
							ans = 0;
						} else {
							ans = 1 / Math.Tan (radian);
						}
					} else {     // for zero.
						ans = 1 / Math.Tan (radian); 
					}
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException (error);
				}
			} else if (command == "sec") {
				if (value.Type.Contains ("Number")) {
					double num = (double)(((Number)value.Data).Data);
					double radian = num * Math.PI / 180;
					double ans = 0; 
					if (num > 0) {    //for +ive number.
						if (isCosMultiple (90, num)) {
							dynamic z = (double)0;
							ans = 1 / z;
						} else {
							ans = 1 / Math.Cos (radian);
						}
					} else if (num < 0) {     //for -ive number.
						if (isCosMultiple (-90, num)) {
							dynamic z = (double)0;
							ans = -1 / z;
						} else {
							ans = 1 / Math.Cos (radian);
						}
					} else {     //for zero.
						ans = 1 / Math.Cos (radian);
					}
					solution = new MathParserExpression (new Number (ans));
				} else {    // for non- Numbers.
					Processed = false;
					throw new MathParserException (error);
				}
			} else if (command == "cosec" || command == "csc") {
				if (value.Type.Contains ("Number")) {
					double num = ((Number)value.Data).Data;
					double ans = 0;
					if (num > 0) {       // for the number to be positive.
						if (isMultiple (180, num)) {
							dynamic z = (double)0;
							ans = 1 / z;
						} else {
							ans = 1 / Math.Sin (num * Math.PI / 180);
						}
					} else if (num < 0) {     //for the number to be negitive.
						if (isMultiple (-180, num)) {
							dynamic z = (double)0;
							ans = -1 / z;
						} else {
							ans = 1 / Math.Sin (num * Math.PI / 180);
						}
					} else {         // for the number to be zero.
						ans = 1 / Math.Sin (num * Math.PI / 180);
					}
					solution = new MathParserExpression (new Number ((double)ans));  // solution make.
				} else {   // if not a number.
					Processed = false;
					throw new 	MathParserException ("Invalid function argument.");
				}      	
			} else if (command == "floor") {
				if (value.Type.Contains ("Number")) {
					double num = ((Number)value.Data).Data;
					double ans = Math.Floor (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments.");
				}
			} else if (command == "ceil") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Ceiling (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function argument");
				}
			} else if (command == "abs") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Abs (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments");
				}
			} else if (command == "sinh") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Sinh (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments.");
				}
			} else if (command == "cosh") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Cosh (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments.");
				}
			} else if (command == "tanh") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Tanh (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Inalid function arguments");
				}
			} else if (command == "ln") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Log (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			} else if (command == "log") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Log10 (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			} else if (command == "sqrt" || command == "√") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Sqrt (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			} else if (command == "arcCos") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Acos (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			} else if (command == "arcSin") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Asin (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			} else if (command == "arcTan") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Atan (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			} else if (command == "round") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Round (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments");
				}
			} else if (command == "trunc") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Truncate (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments");
				}
			} else if (command == "ref") {
				if (value.Type.Contains ("Matrix")) {
					Matrix mat = value.Data;
					solution = new MathParserExpression (new Matrix (mat.RowEchelonForm ()));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments");
				}
			} else if (command == "rref") {
				if (value.Type.Contains ("Matrix")) {
					Matrix mat = value.Data;
					solution = new MathParserExpression (new Matrix (mat.ReducedRowEchelonForm ()));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function arguments");
				}
			} else if (command == "adj") {
				if (value.Type.Contains ("Matrix")) {
					Matrix mat = value.Data;
					solution = new MathParserExpression (new Matrix (mat.Adjoint ()));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function argument");
				}
			} else if (command == "transp") {
				if (value.Type.Contains ("Matrix")) {
					Matrix mat = value.Data;
					solution = new MathParserExpression (new Matrix (mat.Transpose ()));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function argument");
				}
			} else if (command == "rank") {
				if (value.Type.Contains ("Matrix")) {
					Matrix mat = value.Data;
					solution = new MathParserExpression (new Number (mat.Rank ())); 
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function argument");
				}
			} else if (command == "det") {
				if (value.Type.Contains ("Matrix")) {
					Matrix mat = value.Data;
					if (mat.Rows != mat.Columns) {
						throw new MathParserException ("Not a square matrix. Cannot, compute determinant.");
					}
					solution = new MathParserExpression (mat.Determinant ()); 
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function argument");
				}
			} else if (command == "inv") {
				if (value.Type.Contains ("Matrix")) {
					Matrix mat = value.Data;
					if (mat.Rows != mat.Columns) {
						throw new MathParserException ("Not a square matrix. Cannot, compute inverse.");
					}
					solution = new MathParserExpression (mat.Inverse ());
				} else {
					Processed = false;
					throw new MathParserException ("Invalid function argument");
				}
			} else if (command == "rootByBisection_polynomial") {
				if (value.Type.Contains ("Matrix")) {
					if (value.Data.Rows == 1) {
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++) {
							array [c] = value.Data [0, c];
						}
						rootsByBisection_polynomial r = new rootsByBisection_polynomial (array, 3);
						double ans = r.Eval ();
						solution = new MathParser.DataTypes.MathParserExpression (new Number (ans));
						Processed = true;
					} else {
						Processed = false;
						throw new MathParser.MathParserException ("Invalid function argument");
					}
				} else {
					Processed = false;
					throw new MathParser.MathParserException ("Invalid function argument");
				}
			} else if (command == "rootByRFM_polynomial") {
				if (value.Type.Contains ("Matrix")) {
					if (value.Data.Rows == 1) {
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++) {
							array [c] = value.Data [0, c];
						}
						regulaFalsiMethod_polynomial r = new regulaFalsiMethod_polynomial (array);
						double ans = r.Eval ();
						solution = new MathParser.DataTypes.MathParserExpression (new Number (ans));
						Processed = true;
					} else if (value.Data.Rows == 2) {
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++) {
							array [c] = value.Data [0, c];
						}
						regulaFalsiMethod_polynomial r = new regulaFalsiMethod_polynomial (array, (double)(value.Data [1, 0] == 0 ? 0.1 : value.Data [1, 0]), (int)(value.Data [1, 1] == 0 ? 0.1 : value.Data [1, 1]));
						double ans = r.Eval ();
						solution = new MathParser.DataTypes.MathParserExpression (new Number (ans));
						Processed = true;
					} else {
						Processed = false;
						throw new MathParser.MathParserException ("Invalid function argument");
					}
				} else {
					Processed = false;
					throw new MathParser.MathParserException ("Invalid function argument");
				}
			} else if (command == "rootByNRM_polynomial") {
				if (value.Type.Contains ("Matrix")) {
					if (value.Data.Rows == 1) {
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++) {
							array [c] = value.Data [0, c];
						}
						newtonRaphsonMethod_polynomial r = new newtonRaphsonMethod_polynomial (array);
						double ans = r.Eval ();
						solution = new MathParser.DataTypes.MathParserExpression (new Number (ans));
						Processed = true;
					} else if (value.Data.Rows == 2) {
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++) {
							array [c] = value.Data [0, c];
						}
						newtonRaphsonMethod_polynomial r = new newtonRaphsonMethod_polynomial (array, (double)(value.Data [1, 0] == 0 ? 0.1 : value.Data [1, 0]), (int)(value.Data [1, 1] == 0 ? 0.1 : value.Data [1, 1]));
						double ans = r.Eval ();
						solution = new MathParser.DataTypes.MathParserExpression (new Number (ans));
						Processed = true;
					} else {
						Processed = false;
						throw new MathParser.MathParserException ("Invalid function argument");
					}
				} else {
					Processed = false;
					throw new MathParser.MathParserException ("Invalid function argument");
				}	
			} else if (command == "rootBySM_polynomial") {
				if (value.Type.Contains ("Matrix")) {
					if (value.Data.Rows == 1) {
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++) {
							array [c] = value.Data [0, c];
						}
						secantMethod_polynomial r = new secantMethod_polynomial (array);
						double ans = r.Eval ();
						solution = new MathParser.DataTypes.MathParserExpression (new Number (ans));
						Processed = true;
					} else if (value.Data.Rows == 2) {
						double[] array = new double[value.Data.Columns];
						for (int c = 0; c < value.Data.Columns; c++) {
							array [c] = value.Data [0, c];
						}
						secantMethod_polynomial r = new secantMethod_polynomial (array, (double)(value.Data [1, 0] == 0 ? 0.1 : value.Data [1, 0]), (int)(value.Data [1, 1] == 0 ? 0.1 : value.Data [1, 1]));
						double ans = r.Eval ();
						solution = new MathParser.DataTypes.MathParserExpression (new Number (ans));
						Processed = true;
					} else {
						Processed = false;
						throw new MathParser.MathParserException ("Invalid function argument");
					}
				} else {
					Processed = false;
					throw new MathParser.MathParserException ("Invalid function argument");
				}	
			}





			else if (Solver.On_Single_Argument_KeyWord_Implement != null) {
				Solver.On_Single_Argument_KeyWord_Implement (command, value, ref solution, ref Processed);
			} else {
				
				Processed = false;
				throw new MathParserException ($"No '{command}' implementation defined.");
			}

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
				double a0 = pe.Eval (r [0]);
				double a1 = pe.Eval (r [1]);
				x2 = ((r [0] * a1) - (r [1] * a0)) / (a1 - a0);
				double a2 = pe.Eval (x2);
				if (a2 < 0) {
					r [0] = x2;
				} else if (a2 > 0) {
					r [1] = x2;
				} else {
					return x2;
				}
				double err = Math.Pow((x2 - x2_old)/x2,2);
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



}
