using System;
using MathParser.DataTypes;
using MathParser.DataTypes.DynamicDataTypes;
using static System.Math;

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
				}

				else {   // if not a number.
					Processed = false;
					throw new Exception ("Invalid function argument.");
				}
			}	  // end if for sin.

			if (command == "cos") {    // for cos command.
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
					throw new Exception ("Invalid function argument.");
				}
			}

			if (command == "tan") {

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
					throw new Exception (error);
				}
			}

			if (command == "cot")
			{
				if (value.Type.Contains ("Number")) {
					double radian = (double)(((Number)value.Data).Data) * Math.PI / 180;
					double num = (double)(((Number)value.Data).Data) ;
					double ans = 0;
					if (num > 0) {      // for positive number.
						if (isMultiple (180, num)) {
							dynamic z = (double)0;
							ans = 1 / z;
						} else if (isCosMultiple (90, num)) {
							ans = 0; 
						}
						else {
							ans = 1 / Math.Tan (radian);
						}
					} else if (num < 0) {     // for negitive number.
						if (isMultiple (-180, num)) {
							dynamic z = (double)0;
							ans = -1 / z;
						} else if (isCosMultiple (-90, num)) {
							ans = 0;
						}
						else {
							ans = 1 / Math.Tan (radian);
						}
					} else {     // for zero.
						ans = 1 / Math.Tan (radian); 
					}
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new Exception (error);
				}
			}

			if (command == "sec") {
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
					throw new Exception (error);
				}
			}

			if (command == "cosec" || command == "csc") {
				if (value.Type.Contains ("Number")) {
					double num = ((Number)value.Data).Data;
					double ans = 0;
					if (num > 0) {       // for the number to be positive.
						if (isMultiple (180, num)) {
							dynamic z = (double)0;
							ans = 1/z;
						} else {
							ans = 1/Math.Sin (num * Math.PI / 180);
						}
					} else if (num < 0) {     //for the number to be negitive.
						if (isMultiple (-180, num)) {
							dynamic z = (double)0;
							ans = -1 / z;
						} else {
							ans = 1/Math.Sin (num * Math.PI / 180);
						}
					} else {         // for the number to be zero.
						ans = 1/Math.Sin (num * Math.PI / 180);
					}
					solution = new MathParserExpression (new Number ((double)ans));  // solution make.
				}

				else {   // if not a number.
					Processed = false;
					throw new Exception ("Invalid function argument.");
				}      	
			}

			if (command == "floor") {
				if (value.Type.Contains ("Number")) {
					double num = ((Number)value.Data).Data;
					double ans = Math.Floor (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new Exception ("Invalid function arguments.");
				}
			}

			if (command == "ceil") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Ceiling (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new Exception ("Invalid function argument");
 	 			}
			}

			if (command == "abs") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Abs (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException("Invalid function arguments");
				}
			}

			if(command == "sinh"){
				if(value.Type.Contains("Number"))	
				{
					double num = value.Data.Data;
					double ans = Math.Sinh (num);
					solution = new MathParserExpression (new Number(ans));
				}
				else{
					Processed = false;
					throw new Exception ("Invalid function arguments.");
				}
			}

			if(command == "cosh")
			{
				if(value.Type.Contains ("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Cosh (num);
					solution = new MathParserExpression(new Number(ans));
				}
				else{
					Processed = false;
					throw new Exception ("Invalid function arguments.");
				}
			}

			if(command == "tanh")
			{
				if(value.Type.Contains ("Number"))
				{
					double num = value.Data.Data;
					double ans = Math.Tanh (num) ;
					solution = new MathParserExpression(new Number(ans));
				}
				else{
					Processed = false ;
					throw new Exception ("Inalid function arguments");
				}
			}

			if (command == "ln") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Log (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			}

			if (command == "log") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Log10 (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			}


			if (command == "sqrt") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Sqrt (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			}

			if (command == "arcCos") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Acos (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			}


			if (command == "arcSin") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Asin (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			}

			if (command == "arcTan") {
				if (value.Type.Contains ("Number")) {
					double num = value.Data.Data;
					double ans = Math.Atan (num);
					solution = new MathParserExpression (new Number (ans));
				} else {
					Processed = false;
					throw new MathParserException ("Invalid Function Arguments.");
				}
			}
			if (command == "det")
			{
				if(value.Type.Contains ("Matrix"))
				{
					Matrix mat = value.Data.Data;

				}
			}

		}

	}
}
