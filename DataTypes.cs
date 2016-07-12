using System;
using MathParser.DataTypes.DynamicDataTypes;

namespace MathParser
{

	namespace DataTypes
	{
		public abstract class BaseDataType
		{
			public abstract void Print();
			public string Tag = "";
		}
		/// <summary>
		/// Dynamic Data Types are held here which configure their internal dataTypes on the run time.
		/// </summary>
		namespace DynamicDataTypes
		{

			/// <summary>
			/// The DataType to hold the numeric types.
			/// The data stord in it is dynamic.
			/// </summary>

			public class Number : BaseDataType
			{
				dynamic data;
				string tag;
				public delegate void PrinterFunction(Number number);    ///Don't use it outside the class.
				public PrinterFunction OnPrint = null;    // delegate that gets called when Print function is called.
				public static PrinterFunction staticOnPrint = null;  //delegate that gets called in Print. It is statically defined and gets called only when 
																	 //the OnPrint is not defined i.e null 
				public Number() { }

				public Number(dynamic Data)
				{
					data = Data;
					tag = "";
				}

				public static Number Parse(string NumberString, string dataType = "double")
				{
					Number num = null;
					if (dataType.ToLowerInvariant() == "double")
						num = new Number(double.Parse(NumberString));
					else if (dataType.ToLowerInvariant() == "decimal")
						num = new Number(decimal.Parse(NumberString));
					else if (dataType.ToLowerInvariant() == "int")
						num = new Number(int.Parse(NumberString));
					else if (dataType.ToLowerInvariant() == "float")
						num = new Number(float.Parse(NumberString));
					else
					{
						num = new Number(double.Parse(NumberString));
					}
					return num;
				}

				public Number(dynamic Data, string dataTag)
				{
					data = Data;
					tag = dataTag;
				}

				public dynamic Data
				{
					get { return data; }
					set { data = value; }
				}

				public new string Tag
				{
					get { return tag; }
					set { tag = value; }
				}

				public Number(Number number)
				{
					data = number.Data;
					tag = number.Tag;
					OnPrint = number.OnPrint;
				}

				/// <summary>
				/// Print this instance.
				/// Prints the Number as defined by the OnPrint function delegate;
				/// </summary>

				public override void Print()    // Prints the Number as defined by the OnPrint function delegate;
				{
					if (OnPrint != null)
					{
						OnPrint(this);
					}
					else {
						staticOnPrint(this);
					}
				}

				public static Number operator +(Number lhs, Number rhs)
				{
					return (new Number(lhs.Data + rhs.Data));
				}


				public static Number operator *(Number lhs, Number rhs)
				{
					return (new Number(lhs.Data * rhs.Data));
				}


				public static Number operator -(Number lhs, Number rhs)
				{
					return (new Number(lhs.Data - rhs.Data));
				}


				public static Number operator /(Number lhs, Number rhs)
				{
					return (new Number(lhs.Data / rhs.Data));
				}

			}

			// End Number class.

			/// <summary>
			/// The DataType to hold the Matrices types.
			/// The data stord in it is dynamic.
			/// </summary>

			public class Matrix : BaseDataType
			{
				public dynamic[,] data
				{
					private set;
					get;
				}
				public int Rows
				{
					private set;
					get;
				} = 0;
				public int Columns
				{
					private set;
					get;
				} = 0;

				//public string tag;

				public new string Tag
				{
					set;
					get;
				}

				public delegate void PrinterFunction(Matrix matrix);    // don't use this outside the class.
				public PrinterFunction OnPrint = null;   // this Function gets called matrix is printed by calling Print() function.
				public static PrinterFunction staticOnPrint = null;  //delegate that gets called in Print. It is statically defined and gets called only when 
																	 //the OnPrint is not defined i.e null 

				public Matrix() { }

				public Matrix(dynamic[,] Data, int Rows, int Columns, string tag = " ")
				{
					data = Data;
					Tag = tag;
					this.Rows = Rows;
					this.Columns = Columns;
				}

				public Matrix(int Rows, int Columns, string tag = " ")
				{
					Tag = tag;
					this.Rows = Rows; this.Columns = Columns;
					data = new dynamic[this.Rows, this.Columns];
					for (int c = 0; c < this.Rows; c++)
					{
						for (int c1 = 0; c1 < this.Columns; c1++)
						{
							data[c, c1] = 0;
						}
					}
				}

				public dynamic this[int indexRows, int indexCols]
				{
					set { data[indexRows, indexCols] = value; }

					get { return (data[indexRows, indexCols]); }
				}

				public Matrix(Matrix matrix)    // copy constructor.
				{
					data = new dynamic[matrix.Rows, matrix.Columns];
					for (int c = 0; c < matrix.Rows; c++)
					{
						for (int c1 = 0; c1 < matrix.Columns; c1++)
						{
							data[c, c1] = matrix[c, c1];
						}
					}
					Rows = matrix.Rows;
					Columns = matrix.Columns;
					Tag = matrix.Tag;
					OnPrint = matrix.OnPrint;
				}

				public static Matrix Parse(string matrixString)
				{
					MathParser.MatrixBuilder build = new MatrixBuilder(matrixString);
					build.SyncFlags();
					build.Parse();
					Matrix result = build.getMatrix();
					if (!build.isProcessed())
					{
						throw (new Exception("Matrix string could not be processed."));
					}
					return result;
				}

				/// <summary>
				/// Print this instance.
				/// Function prints the matrix data as defined by the delegate function of ' OnPrint '. 
				/// </summary>

				public override void Print()     // function prints the matrix data as defined by the delegate function of ' OnPrint '. 
				{
					if (OnPrint != null)
					{
						OnPrint(this);
					}
					else
					{
						staticOnPrint(this);
					}
				}

				public static Matrix operator +(Matrix lhs, Matrix rhs)
				{
					Matrix ans;
					if ((lhs.Rows != rhs.Rows) || (lhs.Columns != rhs.Columns))
					{
						throw new Exception("Not similar matrix.");
					}
					else {
						ans = new Matrix(lhs.Rows, rhs.Columns);
						for (int c = 0; c < lhs.Rows; c++)
						{
							for (int c1 = 0; c1 < rhs.Columns; c1++)
							{
								ans[c, c1] = lhs[c, c1] + rhs[c, c1];
							}
						}
					}
					return ans;
				}

				public static Matrix operator -(Matrix lhs, Matrix rhs)
				{
					Matrix ans;
					if ((lhs.Rows != rhs.Rows) || (lhs.Columns != rhs.Columns))
					{
						throw new Exception("Not similar matrix.");
					}
					else {
						ans = new Matrix(lhs.Rows, rhs.Columns);
						for (int c = 0; c < lhs.Rows; c++)
						{
							for (int c1 = 0; c1 < rhs.Columns; c1++)
							{
								ans[c, c1] = lhs[c, c1] - rhs[c, c1];
							}
						}
					}
					return ans;
				}

				public static Matrix operator *(Matrix lhs, Matrix rhs)
				{
					Matrix result = new Matrix(lhs.Rows, rhs.Columns);
					if (lhs.Columns == rhs.Rows)
					{
						for (int c = 0; c < lhs.Rows; c++)
						{
							for (int c1 = 0; c1 < rhs.Columns; c1++)
							{
								for (int c2 = 0; c2 < lhs.Columns; c2++)
								{
									if (c2 == 0)
										result[c, c1] = lhs[c, c2] * rhs[c, c2];
									else
										result[c, c1] = (result[c, c1] + (lhs[c, c2] * rhs[c2, c1]));
								}
							}
						}
					}
					else {
						throw new Exception("No valid matrix sequences.");
					}
					return result;
				}

				static public Matrix operator /(Matrix lhs, double num)
				{
					Matrix ans = new Matrix(lhs.Rows, lhs.Columns);
					for (int c = 0; c < lhs.Rows; c++)
					{
						for (int c1 = 0; c1 < lhs.Columns; c1++)
						{
							ans[c, c1] = lhs[c,c1]/num;
						}
					}
					return ans;
				}

				static public Matrix operator *(Matrix lhs, double num)
				{
					Matrix ans = new Matrix(lhs.Rows, lhs.Columns);
					for (int c = 0; c < lhs.Rows; c++)
					{
						for (int c1 = 0; c1 < lhs.Columns; c1++)
						{
							ans[c, c1] = lhs[c,c1]*num;
						}
					}
					return ans;
				}

				static public Matrix operator *(double num, Matrix lhs)
				{
					Matrix ans = new Matrix(lhs.Rows, lhs.Columns);
					for (int c = 0; c < lhs.Rows; c++)
					{
						for (int c1 = 0; c1 < lhs.Columns; c1++)
						{
							ans[c, c1] = lhs[c,c1]*num;
						}
					}
					return ans;
				}


			}
			// End Matrix Class.

			/// <summary>
			/// The DataType to hold the data of the complex numbers.
			/// </summary>

			public class ComplexNumber : BaseDataType
			{
				public dynamic RealPart
				{
					private set;
					get;
				}

				public dynamic ImaginaryPart
				{
					private set;
					get;
				}

				public new string Tag
				{
					set;
					get;
				}

				public delegate void PrinterFunction(ComplexNumber complexNumber);   // dont use it outside the class.
				public PrinterFunction OnPrint = null;     // this Function gets called matrix is printed by calling Print() function.
				public static PrinterFunction staticOnPrint = null;        //delegate that gets called in Print. It is statically defined and gets called only when 
																		   //the OnPrint is not defined i.e null  

				public ComplexNumber() { }

				public ComplexNumber(dynamic realData, dynamic imaginaryData, string tag = " ")
				{
					RealPart = realData;
					ImaginaryPart = imaginaryData;
					Tag = tag;
				}

				public ComplexNumber(ComplexNumber complexNumber)
				{
					RealPart = complexNumber.RealPart;
					ImaginaryPart = complexNumber.ImaginaryPart;
					Tag = complexNumber.Tag;
					OnPrint = complexNumber.OnPrint;
				}


				/// <summary>
				/// Print this instance.
				/// Calls OnPrint function delegate. Or staticOnPrint function delegate.
				/// </summary>

				public override void Print()
				{
					if (OnPrint != null)
					{
						OnPrint(this);
					}
					else {
						staticOnPrint(this);
					}
				}

			}

			// end complex number datatype.


			/// <summary>
			/// Vector. To hold vector data.
			/// </summary>

			public class Vector : BaseDataType
			{
				public dynamic i
				{
					private set;
					get;
				} = 0;
				public dynamic j
				{
					private set;
					get;
				} = 0;
				public dynamic k
				{
					private set;
					get;
				} = 0;
				public new string Tag
				{
					set;
					get;
				}

				public delegate void PrinterFunction(Vector vector);   // dont use it outside the class.
				public PrinterFunction OnPrint = null;     // this Function gets called matrix is printed by calling Print() function.
				public static PrinterFunction staticOnPrint = null;        //delegate that gets called in Print. It is statically defined and gets called only when 


				public Vector() { }

				public Vector(dynamic i = null, dynamic j = null, dynamic k = null, string tag = "")
				{
					this.i = i;
					this.j = j;
					this.k = k;
					this.Tag = tag;
				}

				public Vector(Vector vector)
				{
					i = vector.i;
					j = vector.j;
					k = vector.k;
					Tag = vector.Tag;
				}

				public override void Print()
				{
					if (OnPrint != null)
					{
						OnPrint(this);
					}
					else {
						staticOnPrint(this);
					}
				}
			}    //end vector datatype.

		}        // end dynamic datatypes.

		public class MathParserExpression : BaseDataType
		{
			dynamic data;

			public override void Print() { 
				data.Print ();

			}
			public dynamic Data
			{
				get { return data; }
				private set { data = value; }
			}

			//string tag;

			public new string Tag {
				get;
	    		set;
			}

			public void setEntireTag(string name)
			{
				data.Tag = name;
			}

			public string Statement {
				get;
				set;
			}

			public string Type
			{
				get;
				private set;
			}

			public MathParserExpression() { }
			public MathParserExpression(BaseDataType Data)
			{
				this.Data = Data;
				this.Tag = this.data.Tag;
				this.Type = this.data.GetType().Name;
			}

			public MathParserExpression(MathParserExpression expression)
			{
				this.Data = expression.Data;
				this.Tag = expression.Tag;
				this.Type = expression.Type;
				this.Statement = expression.Statement;
			}
		}


	}     //end data type Space

}     // end Math Parser.