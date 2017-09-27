using System;
using MathParser.DataTypes.DynamicDataTypes;
using MathParser;

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
						throw (new MathParserException("Matrix string could not be processed."));
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
						throw new MathParserException("Not similar matrix.");
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
						throw new MathParserException("Not similar matrix.");
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
						throw new MathParserException("No valid matrix sequences.");
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

				public Matrix RowEchelonForm()
				{
					DataTypeSpace.Matrix mat = dataTypeSpaceMatrixMaker (this);
					mat.GaussElimination ();
					return (mathParserMatrixMaker (mat));
				}

				public Matrix ReducedRowEchelonForm()
				{
					DataTypeSpace.Matrix mat = dataTypeSpaceMatrixMaker (this);
					mat.GaussJordan ();
					return (mathParserMatrixMaker (mat));
				}

				public Number Determinant()
				{
					DataTypeSpace.Matrix mat = dataTypeSpaceMatrixMaker (this);
					double d = mat.getdetreminant (mat);
					DataTypes.DynamicDataTypes.Number det = new Number (d);
					return det;
				}

				public Matrix Adjoint()
				{
					DataTypeSpace.Matrix mat = dataTypeSpaceMatrixMaker (this);
					return(mathParserMatrixMaker(mat.getAdjoint ()));
				}

				public Matrix Transpose()
				{
					DataTypeSpace.Matrix mat = dataTypeSpaceMatrixMaker (this);
					return (mathParserMatrixMaker (mat.getTranspose ()));
				}

				public Number Rank()
				{
					DataTypeSpace.Matrix mat = dataTypeSpaceMatrixMaker (this);
					double r = mat.rank (mat);
					DataTypes.DynamicDataTypes.Number rank = new Number (r);
					return rank;
				}

				public Matrix Inverse()
				{
					DataTypeSpace.Matrix mat = dataTypeSpaceMatrixMaker (this);
					Matrix inv = mathParserMatrixMaker (mat.Inverse1 (mat));
					return inv;
				}



				static public DataTypeSpace.Matrix dataTypeSpaceMatrixMaker(Matrix theMat)
				{
					DataTypeSpace.Matrix mat = new DataTypeSpace.Matrix (theMat.Rows, theMat.Columns);
					for (int c = 0; c < theMat.Rows; c++) {
						for (int c1 = 0; c1 < theMat.Columns; c1++) {
							mat.setElement ((double)theMat[c,c1],c+1,c1+1);
						}
					}
					mat.setTag (theMat.Tag);
					return mat;
				}

				static public Matrix mathParserMatrixMaker(DataTypeSpace.Matrix theMat)
				{
					DataTypes.DynamicDataTypes.Matrix mat = new DataTypes.DynamicDataTypes.Matrix (theMat.getRows (), theMat.getColumns (), theMat.getTag ());
					for (int c = 0; c < theMat.getRows (); c++) {
						for (int c1 = 0; c1 < theMat.getColumns (); c1++) {
							mat [c, c1] = theMat.getElement (c + 1, c1 + 1);
						}
					}
					mat.Tag = "";
					return mat;
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



// Start namespace DataTypesSpace

namespace DataTypeSpace
{
	public class Matrix  //matrix class starts
	{
		protected double[,] theMatrix;      //the basic matrix;
		protected int mRows, mCols;
		private OperationsClass theOperations =  new OperationsClass();
		private static OperationsClass staticOperations =  new OperationsClass();
		private string tag;

		// code for magic matrix

		private Matrix Matrixbuilder(Matrix m1, Matrix m2, Matrix m3, Matrix m4)
		{
			int rows = m1.getRows();
			int columns = m1.getColumns();
			Matrix temp = new Matrix(2 * rows, 2 * columns);
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					temp.setElement(m1.getElement(i + 1, j + 1), i + 1, j + 1);
				}
			}
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					temp.setElement(m2.getElement(i + 1, j + 1), i + rows + 1, j + 1);
				}
			}
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					temp.setElement(m3.getElement(i + 1, j + 1), i + 1, j + columns + 1);
				}
			}
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					temp.setElement(m4.getElement(i + 1, j + 1), i + rows + 1, j + columns + 1);
				}
			}
			return temp;
		}


		public Matrix magic(int size)
		{
			Matrix temp = new Matrix(size, size);
			if (size % 2 == 1)
			{
				if (size == 1)
				{
					temp.setElement(1, 1, 1);
					return temp;
				}
				bool donefor = false;
				int r = 0;
				int c = (size - 1) / 2;
				temp.setElement(1, r + 1, c + 1);
				for (int i = 2; i <= size * size; i++)
				{
					while (donefor == false)
					{
						r--;
						c++;
						if (r < 0) { r += size; }
						else if (r >= size) r -= size;
						if (c >= size) c -= size;
						else if (c < 0) c += size;
						if (temp.getElement(r + 1, c + 1) != 0)
						{
							r += 2;
							c--;
						}
						if (r < 0) { r += size; }
						else if (r >= size) r -= size;
						if (c >= size) c -= size;
						else if (c < 0) c += size;
						if (temp.getElement(r + 1, c + 1) == 0) donefor = true;
					}
					temp.setElement(i, r + 1, c + 1);
					donefor = false;
				}

			}
			else if (size % 4 == 0)// for matrices having number of rows divisible by 4
			{
				int k = 1;
				Matrix x1 = new Matrix(size, size);
				Matrix x2 = new Matrix(size, size);
				for (int i = 0; i < size; i++)
				{
					for (int j = 0; j < size; j++, k++)
					{
						x1.setElement(k, i + 1, j + 1);
					}
				}
				k = size * size;
				for (int i = 0; i < size; i++)
				{
					for (int j = 0; j < size; j++)
					{
						x2.setElement(k, i + 1, j + 1);
						k--;
					}
				}
				//Console.WriteLine("");
				for (int i = 0; i < size; i++)
				{
					for (int j = 0; j < size; j++)
					{
						if (i == j)
							temp.setElement(1, i + 1, j + 1);
						else if (i == size - j - 1) temp.setElement(1, i + 1, j + 1);
						else temp.setElement(0, i + 1, j + 1);
					}
				}
				//Console.WriteLine("");
				for (int i = 0; i < size; i++)
				{
					for (int j = 0; j < size; j++)
					{
						if (temp.getElement(i + 1, j + 1) == 1) temp.setElement(x1.getElement(i + 1, j + 1), i + 1, j + 1);
						else temp.setElement(x2.getElement(i + 1, j + 1), i + 1, j + 1);
					}
				}
			}
			else // for magic matrices for number of even rows not divisible by 4
			{
				if (size == 2)
				{
					//Console.WriteLine("Magic Matrix does not exist");
					return temp;
				}
				int r = size / 2;
				Matrix m1 = new Matrix(r, r);
				Matrix m2 = new Matrix(r, r);
				Matrix m3 = new Matrix(r, r);
				Matrix m4 = new Matrix(r, r);
				m1 = m1.magic(r);
				Matrix i1 = new Matrix(r, r);
				for (int i = 0; i < r; i++)
				{
					for (int j = 0; j < r; j++)
					{
						i1.setElement(r * r, i + 1, j + 1);
					}
				}
				m2 = m1 + i1;
				m3 = m1 + i1 + i1;
				m4 = m1 + i1 + i1 + i1;
				temp = Matrixbuilder(m1, m4, m3, m2);
				for (int i = 0; i < r; i++)
				{
					for (int j = 0; j < (r + 1) / 2; j++)
					{
						if (i == (r - 1) / 2 && j == 0)
						{
							continue;
						}
						else if (i != (r - 1) / 2 && j == (r - 1) / 2)
						{
							continue;
						}
						else
						{
							double tempnum = temp.getElement(i + 1, j + 1);
							temp.setElement(temp.getElement(i + r + 1, j + 1), i + 1, j + 1);
							temp.setElement(tempnum, i + r + 1, j + 1);
						}
					}
					for (int j = 2 * r - (r - 3) / 2; j < 2 * r; j++)
					{
						double tempnum = temp.getElement(i + 1, j + 1);
						temp.setElement(temp.getElement(i + r + 1, j + 1), i + 1, j + 1);
						temp.setElement(tempnum, i + r + 1, j + 1);
					}
				}
			}
			return temp;
		}



		private void numberswap(double x1, double x2)
		{
			double temp = x1;
			x1 = x2;
			x2 = temp;
		}


		//code for magice matrix
		// code to find the rank of the matrix
		public int rank(Matrix m)
		{
			int r = 0;
			int numofrows = m.getRows();
			int numofcols = m.getColumns();
			Matrix temp = m;
			bool rowvalue = false;
			temp.GaussJordan();
			for (int i=0;i<numofrows;i++)
			{
				for (int j=0;j< numofcols;j++)
				{
					if (temp.getElement(i + 1, j + 1) == 0) continue;
					rowvalue = true;
					r++;
					break;
				}
				if (rowvalue == false) return r;
			}
			return r;
		}

		public Matrix(Matrix theOther)
		{
			mRows = theOther.mRows;
			mCols = theOther.mCols;
			theMatrix = new double[mRows,mCols];
			for (int c = 0; c < mRows; c++) {
				for (int c1 = 0; c1 < mCols; c1++) {
					theMatrix [c, c1] = theOther.getElement (c+1, c1+1);
				}
			}
			tag = theOther.tag;
		}

		public Matrix(string thetag, double[,] matrix,int row = 1, int column = 1 )
		{
			mRows = row;mCols = column;
			theMatrix = matrix;
			tag = thetag;
		}

		public Matrix(int rows = 01, int columns = 01)
		{
			mRows = rows; mCols = columns;
			theMatrix = new double[mRows, mCols];
			tag = " ";
		}

		public Matrix(double[,] mat, int rows , int cols)
		{
			mRows = rows; mCols = cols;
			theMatrix = mat;
			tag = " ";
		}

		public void Zero()
		{
			for (int c = 0; c < mRows; c++) {
				for (int c1 = 0; c1 < mCols; c1++) {
					theMatrix [c, c1] = 0;
				}
			}
		}

		public double getdetreminant(Matrix theMatrix)    //getdeterminant interface
		{
			double[,] matrix = theMatrix.getMatrixArray();
			int mRows = theMatrix.getRows();
			int mCols = theMatrix.getColumns();
			if (mRows != mCols) {
				throw new MathParserException ("Not a square matrix.");
			}

			return determinant1(matrix, mRows, mCols);
		}

		private Matrix rowaddition(Matrix m, int row1, int row2, double factor)
		{
			int col = m.getColumns();
			for (int i = 0; i < col; i++)
			{
				m.setElement(m.getElement(row2 + 1, i + 1) + factor * m.getElement(row1 + 1, i + 1), row2 + 1, i + 1);
			}
			return m;
		}
		private Matrix rowdivision(double factor, Matrix m, int rownum, int col)
		{
			for (int i = 0; i < col; i++)
			{
				double tempvalue = m.getElement(rownum + 1, i + 1);
				tempvalue /= factor;
				m.setElement(tempvalue, rownum + 1, i + 1);
			}
			return m;
		}

		private void rowswap(Matrix m, int row1, int row2)
		{
			int c = m.getColumns();
			for (int i = 0; i < c; i++)
			{
				double num1, num2;
				num1 = m.getElement(row1 + 1, i + 1);
				num2 = m.getElement(row2 + 1, i + 1);
				m.setElement(num1, row2 + 1, i + 1);
				m.setElement(num2, row1 + 1, i + 1);
			}
		}
		private double determinant1(double[,] matrix, int rows, int cols)   //determinant implementation 
		{
			if (rows == cols)
			{
				if (rows == 1)
				{
					return matrix[0, 0];
				}
				else if (rows == 2)
				{
					return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
				}
				else
				{
					double multiplier = 1;
					bool rowvalue = false;
					Matrix temp = new Matrix(rows, cols);
					for (int j = 0; j < cols; j++)
					{
						for (int i = 0; i < rows; i++)
						{
							temp.setElement(matrix[i, j], i + 1, j + 1);
						}
					}
					for (int j = 0; j < cols; j++)
					{
						for (int i = 0; i < rows; i++)
						{
							if (temp.getElement(i + 1, j + 1) == 0) continue;
							rowvalue = true;
							double first = temp.getElement(i + 1, j + 1);
							multiplier *= first;
							temp = rowdivision(first, temp, i, rows);
							//Console.WriteLine("");
							for (int k = i + 1; k < rows; k++)
								temp = rowaddition(temp, i, k, -1 * temp.getElement(k + 1, j + 1));
							if (i != 0)
							{
								rowswap(temp, i, 0);
								multiplier *= -1;
							}
							if (rows == 3)
							{
								return multiplier * (temp.getElement(2, 2) * temp.getElement(3, 3) - temp.getElement(2, 3) * temp.getElement(3, 2));
							}
							else
							{
								double[,] newarray = new double[rows - 1, rows - 1];
								for (int z = 0; z < rows - 1; z++)
								{
									for (int s = 0; s < rows - 1; s++)
									{
										newarray[z, s] = temp.getElement(z + 2, s + 2);
									}
								}
								return multiplier * determinant1(newarray, rows - 1, rows - 1);
							}
						}
						if (rowvalue == false) return 0;
					}
				}
			}
			else
			{
				//Console.WriteLine("The determinant of the matrix doesnot exist.Sorry.");
				return 0;
			}
			return 0;
		}

		// vector solving code
		public double dotproduct(Matrix m1, Matrix m2)
		{
			if (m1.getColumns() != m2.getColumns() || m1.getRows() != 1 || m2.getRows() != 1)
			{
				//Console.WriteLine("Sorry dot product not possible");
				return 0;
			}
			double dotp = 0;
			for (int i = 0; i < m1.getColumns(); i++)
			{
				dotp += m1.getElement(1, i+1) * m2.getElement(1,i+ 1);
			}
			return dotp;
		}
		public Matrix crossproduct(Matrix m1, Matrix m2)
		{
			Matrix m = new Matrix(m1.getRows(), 1);
			if (m1.getRows() != m2.getRows() || m1.getColumns() != 1 || m2.getColumns() != 1 || m1.getRows() != 3)
			{
				return m;
			}
			m.setElement(m1.getElement(2, 1) * m2.getElement(3, 1) - m2.getElement(2, 1) * m1.getElement(3, 1), 1, 1);
			m.setElement(m1.getElement(1, 1) * m2.getElement(3, 1) - m1.getElement(3, 1) * m2.getElement(1, 1), 2, 1);
			m.setElement(m1.getElement(1, 1) * m2.getElement(2, 1) - m1.getElement(2, 1) * m2.getElement(1, 1), 3, 1);
			return m;
		}
		public Matrix crossproduct(double vector1x, double vector1y, double vector1z, double vector2x, double vector2y, double vector2z)
		{
			Matrix m = new Matrix(3, 1);
			m.setElement(vector1y * vector2z - vector1z * vector2y, 1, 1);
			m.setElement(vector1x * vector2z - vector1z * vector2x, 2, 1);
			m.setElement(vector1x * vector2y - vector2x * vector1y, 3, 1);
			return m;
		}
		// vector solving code.


		public Matrix Inverse1(Matrix m)
		{

			if (getdetreminant(m) == 0)
			{
				//Console.WriteLine("The inverse does not exist.Returning original value");
				return m;
			}
			else
			{
				int r = m.getRows();
				int c = m.getColumns();
				Matrix temp = m;
				Matrix temp2 = new Matrix(r, c);
				Matrix Bigmat = new Matrix(r, 2 * r);
				for (int i = 0; i < r; i++)
				{
					for (int j = 0; j < c; j++)
					{
						if (i == j) temp2.setElement(1, i + 1, j + 1);
						else temp2.setElement(0, i + 1, j + 1);
					}
				}
				for (int i = 0; i < r; i++)
				{
					for (int j = 0; j < r; j++)
					{
						Bigmat.setElement(temp.getElement(i + 1, j + 1), i + 1, j + 1);
					}
				}
				for (int i = 0; i < r; i++)
				{
					for (int j = r; j < 2 * r; j++)
					{
						Bigmat.setElement(temp2.getElement(i + 1, j - r + 1), i + 1, j + 1);
					}
				}
				Bigmat.GaussJordan();
				for (int i = 0; i < r; i++)
				{
					for (int j = 0; j < r; j++)
					{
						temp2.setElement(Bigmat.getElement(i + 1, j + r + 1), i + 1, j + 1);
					}
				}
				return temp2;
			}
		}
		public void Identity()
		{
			for (int c = 0; c < mRows; c++) {
				for (int c1 = 0; c1 < mCols; c1++) {
					if (c == c1)
						theMatrix [c, c1] = 1;
					else
						theMatrix [c, c1] = 0;
				}
			}
		}

		public double this [int aRow, int aCol] => theMatrix[aRow, aCol];

		public void setMatrixArray (double[,] mat)
		{
			theMatrix = mat;
		}

		public string getTag ()
		{
			return tag;
		}

		public Matrix getAdjoint ()
		{
			return theOperations.getAdjoint (this);
		}

		public Matrix getTranspose ()
		{
			return theOperations.getTranspose (this);
		}

		public double determinant ()
		{
			return theOperations.getdetreminant (this);
		}

		public Matrix addTo (Matrix otherMat)
		{
			return theOperations.addMatrix (this, otherMat);
		}

		public Matrix subtractFrom (Matrix otherMat)
		{
			return theOperations.subtractMatrix (otherMat, this);
		}

		// Helper functions and function for guass Jordan.

		private void Swap(string rowOrColumn , int row1 , int row2)   //function for swaping for rows and columns.
		{
			if (rowOrColumn == "row") {   //start if case for row swapping.
				if (row1 > mRows || row1 < 0 || row2 > mRows || row2 < 0)
				{
				//	Console.WriteLine("Sorry, the row swapping ain't possible");
					return;
				}
				int column = this.mCols;
				double temp = 0;
				for (int i = 0; i<column; i++)
				{
					temp = theMatrix[row1 - 1,i];
					theMatrix[row1 - 1,i] = theMatrix[row2 - 1,i];
					theMatrix[row2 - 1,i] = temp;
				} 
			}   //end else case for row swaping.
			else {    //start case for column swaping.
				if (row1 > mCols || row1 < 0 || row2 > mCols || row2<0)
				{
				//	Console.WriteLine ("Sorry, the swaping ain't gonna happen"); 
					return;
				}
				double temp = 0;
				int row = this.mRows;
				for (int i = 0; i<row; i++)
				{
					temp = theMatrix[i,row1 - 1];
					theMatrix[i,row1 - 1] = theMatrix[i,row2 - 1];
					theMatrix[i,row2 - 1] = temp;
				}
			}   // end else case.
		}    //end swap function .

		public Matrix getInverse(double det)
		{
			Matrix adj = this.getAdjoint ();
			adj = adj.getTranspose ();
			adj = adj / det;
			return (new Matrix (adj));
		}


		private void rowaddition(int row1, int row2, double factor)  // function for row addition with a factor.
		{
			for (int i = 0; i < mCols; i++)
			{
				theMatrix[row2,i] += factor*(theMatrix[row1,i]);
			}
		}    //end row addition.

		private void columnaddition(int column1, int column2, double factor) //function for column addtion for factors.
		{
			for (int i = 0; i < mRows; i++)
			{
				theMatrix[i,column2] += factor*(theMatrix[i,column1]);
			}
		}    // end column addition.

		private void divisionofrows(double factor, int rownum)  // function for division of rows
		{
			if (factor == 0)
				return;
			for (int i = 0; i < mCols; i++)
			{
				theMatrix[rownum,i] /= factor;
			}
		}   // end division of rows with a factor

		private void divisionofcolumns(double factor, int colnum)  // function for the division of the columns with a factor
		{
			if (factor == 0)
				return;
			for (int i = 0; i < mRows; i++)
			{
				theMatrix[i,colnum] /= factor;
			}
		}     // end function of division of the columns

		public void GaussElimination()     // method for the guass Elimination. // the echelon form.
		{
			int j, i;
			//bool calculation = false;
			for (j = 0; j < mCols; j++)
			{
				for (i = 0; i < mRows; i++)
				{

					if (theMatrix[i,j] == 0)continue;
					//calculation = true;
					divisionofrows(theMatrix[i,j], i);
					for (int k = i + 1; k < mRows; k++)
					{
						rowaddition(i, k, -1 * theMatrix[k,j]);
						theMatrix[k,j] = 0;
					}
					Swap("row", 1, i + 1);
					int h = 1;
					for (j = j + 1; j < mCols; j++, h++)
					{
						for (i = h; i < mRows; i++)
						{
							if (theMatrix[i,j] == 0)continue;
							divisionofrows(theMatrix[i,j], i);
							for (int k = i + 1; k < mRows; k++)
							{
								rowaddition(i, k, -1 * theMatrix[k,j]);
								theMatrix[k,j] = 0;
							}
							Swap("row", h + 1, i + 1);
						}
					}
				}
			}
		}    // end method of guass Elimination.

		public void GaussJordan()
		{
			int j, i;
			//bool calculation = false;
			for (j = 0; j < mCols; j++)
			{
				for (i = 0; i < mRows; i++)
				{
					if (theMatrix[i,j] == 0)continue;
					//calculation = true;
					divisionofrows(theMatrix[i,j], i);
					for (int k = i+1; k < mRows; k++)
					{
						rowaddition(i, k, -1*theMatrix[k,j]);
						theMatrix[k,j] = 0;
					}
					Swap("row", 1, i + 1);
					int h = 1;
					for ( j = j + 1; j < mCols; j++, h++)
					{
						for ( i = h; i < mRows; i++)
						{
							if (theMatrix[i,j] == 0)continue;
							divisionofrows(theMatrix[i,j], i);
							for (int k = 0; k < mRows; k++)
							{
								if (k == i)continue;
								rowaddition(i, k, -1 * theMatrix[k,j]);
								theMatrix[k,j] = 0;
							}
							Swap("row", h + 1, i + 1);
						}
					}
					return;
				}
			}

		}

		//helper functions and functinos for guass jordan.
		public int getRows ()
		{
			return mRows;
		}

		public int getColumns ()
		{
			return mCols;
		}

		public double  getElement (int row, int col)
		{
			return theMatrix [row - 1, col - 1];
		}

		public double[,] getMatrixArray ()=>theMatrix;

		public void setTag (string theMatTag)
		{
			tag = theMatTag;
		}

		public void setElement (double number, int rows, int col)
		{
			theMatrix [rows - 1, col - 1] = number;
		}

		static public Matrix operator+ (Matrix lhs, Matrix rhs)
		{
			return staticOperations.addMatrix (lhs, rhs);
		}

		static public Matrix operator- (Matrix lhs, Matrix rhs)
		{
			return staticOperations.subtractMatrix (lhs, rhs);
		}

		static public Matrix operator* (Matrix lhs, Matrix rhs)
		{
			return staticOperations.multiplyMatrix (lhs, rhs);
		}

		static public Matrix operator* (Matrix lhs, double num)
		{
			return staticOperations.mConst (lhs, num);
		}

		static public Matrix operator* (double num, Matrix rhs)
		{
			return staticOperations.mConst (rhs, num);
		}

		static public Matrix operator/ (Matrix lhs, double num)
		{
			return staticOperations.mDivide (lhs, num);
		}


	}
	//the basic matrix class ends


	class OperationsClass     //start operations class
	{

		public Matrix mDivide (Matrix mat, double num)
		{
			int r = mat.getRows ();
			int c = mat.getColumns ();
			double[,] m = mat.getMatrixArray ();
			for (int c1 = 0; c1 < r; c1++) {
				for (int c2 = 0; c2 < c; c2++) {
					m [c1, c2] /= num;
				}
			}
			Matrix ans = new Matrix (mat.getTag (), m, r, c);
			return ans;
		}

		public Matrix mConst (Matrix mat, double num)
		{
			int r = mat.getRows ();
			int c = mat.getColumns ();
			double[,] m = mat.getMatrixArray ();
			for (int c1 = 0; c1 < r; c1++) {
				for (int c2 = 0; c2 < c; c2++) {
					m [c1, c2] *= num;
				}
			}
			Matrix ans = new Matrix (mat.getTag (), m, r, c);
			return ans;
		}


		public Matrix addMatrix (Matrix mat, Matrix mat1)    //Matrix Addition
		{ 
			if (mat.getRows () == mat1.getRows () && mat.getColumns () == mat1.getColumns ()) {
				Matrix result = new Matrix (mat.getRows (), mat.getColumns ());
				for (int c = 1; c <= mat.getRows (); c++) {
					for (int c1 = 1; c1 <= mat1.getColumns (); c1++) {
						result.setElement (mat.getElement (c, c1) + mat1.getElement (c, c1), c, c1);
					}
				}
				return result;
			} else {
				Matrix result = new Matrix (1, 1);
				result.setElement (0, 1, 1);
				//Console.WriteLine ("The Matrices are not similar... cannot be added.Sorry.");
				return result;
			}
		}
		//Matrix Addition Ends

		public Matrix subtractMatrix (Matrix mat, Matrix mat1)   //Matrix Subtraction
		{
			if (mat.getRows () == mat1.getRows () && mat.getColumns () == mat1.getColumns ()) {
				Matrix result = new Matrix (mat.getRows (), mat.getColumns ());
				for (int c = 1; c <= mat.getRows (); c++) {
					for (int c1 = 01; c1 <= mat1.getColumns (); c1++) {
						result.setElement (mat.getElement (c, c1) - mat1.getElement (c, c1), c, c1);
					}
				}
				return result;
			} else {
				Matrix result = new Matrix (1, 1);
				result.setElement (0, 1, 01);
				//Console.WriteLine ("The Matrices are not similar... cannot be subtracted.Sorry.");
				return result;
			}
		}
		//End Matrix Subtration

		public Matrix multiplyMatrix (Matrix lhs, Matrix rhs)
		{
			if (lhs.getColumns () == rhs.getRows ()) {
				Matrix result = new Matrix (lhs.getRows (), rhs.getColumns ());
				for (int c = 1; c <= lhs.getRows (); c++) {
					for (int c1 = 1; c1 <= rhs.getColumns (); c1++) {
						for (int c2 = 1; c2 <= lhs.getColumns (); c2++) {
							if (c2 == 0)
								result.setElement (lhs.getElement (c, c2) * rhs.getElement (c2, c1), c, c1);
							else
								result.setElement (result.getElement (c, c1) + (lhs.getElement (c, c2) * rhs.getElement (c2, c1)), c, c1);
						}
					}
				}
				return result;
			} else {
				Matrix result = new Matrix (1, 1);
				result.setElement (0, 1, 1);
				//Console.WriteLine ("The matrices cannot be multiplied.Sorry.");
				return result;
			}
		}

		public double getdetreminant (Matrix theMatrix)    //getdeterminant interface
		{
			double[,] matrix = theMatrix.getMatrixArray ();
			int mRows = theMatrix.getRows ();
			int mCols = theMatrix.getColumns ();
			return determinant (matrix, mRows, mCols);
		}
		//getdetermiant ends here.

		private double determinant (double[,] matrix, int rows, int cols)   //determinant implementation 
		{
			if (rows == cols) {
				if (rows == 1 && cols == 1) {
					return matrix [0, 0];
				} else if (rows == 2 && cols == 2) {
					return matrix [0, 0] * matrix [1, 1] - matrix [0, 1] * matrix [1, 0];
				} else {
					double det = 0;
					double[] store = new double[cols];
					int newRows = rows - 1;
					int newCols = cols - 1;
					double[,] newMatrix = new double[newRows, newCols];

					for (int ncolumn = 0; ncolumn < cols; ncolumn++) {
						int newrow = 0;
						int newcolumn = 0;
						for (int row = 0; row < rows; row++) {
							for (int column = 0; column < cols; column++) {
								if (row != 0) {
									if (column != ncolumn) {
										newMatrix [newrow, newcolumn] = matrix [row, column];
										newcolumn++;
										if (newcolumn == newCols) {
											newcolumn = 0;
											newrow++;
										}//end
									}//end
								}//end
							}//end for
						}//end for
						int sign = (int)Math.Pow (-1, ncolumn);
						store [ncolumn] = sign * determinant (newMatrix, newRows, newCols);
					}//end for
					det = 0;
					for (int c = 0; c < cols; c++) {
						det += (matrix [0, c] * store [c]);
					}
					return det;
				}//end else

			} else {
				//Console.WriteLine ("The determinant of the matrix doesnot exist.Sorry.");
				return 0;
			}
		}
		//end determinant measurement function ends here

		public Matrix getTranspose (Matrix mat)
		{
			double[,] matrix = mat.getMatrixArray ();
			int rows = mat.getRows ();
			int cols = mat.getColumns ();
			return Transpose (matrix, rows, cols);
		}

		private Matrix Transpose (double[,] matrix, int rows, int cols)
		{
			Matrix t = new Matrix (cols, rows);
			for (int c = 01; c <= rows; c++) {
				for (int c1 = 01; c1 <= cols; c1++) {
					t.setElement (matrix [c - 1, c1 - 1], c1, c);
				}
			}
			return t;
		}

		public Matrix getAdjoint (Matrix matrix)
		{
			if (matrix.getdetreminant (matrix) == 0) {
				throw new MathParserException ("Determinant is Zero. Cannot compute Adjoint"); 
			}
			double[,] mat = matrix.getMatrixArray ();
			int rows = matrix.getRows ();
			int cols = matrix.getColumns ();
			return Adjoint (mat, rows, cols);
		}

		private Matrix Adjoint (double[,] matrix, int rows, int cols)   //function for adjoint
		{
			if (rows == cols) {
				if (rows == 1 && cols == 1) {
					Matrix result = new Matrix (1, 1);
					result.setElement (matrix [0, 0], 1, 1);
					return result;
				} else {
					double[,] newMatrix = new double[rows, cols];
					double[,] tobeAdjoint = new double[rows, cols];
					int newCol = cols - 1;
					int newRow = rows - 1;
					for (int nrow = 0; nrow < rows; nrow++) {
						for (int ncol = 0; ncol < cols; ncol++) {
							int newrow = 0;
							int newcol = 0;
							for (int c = 0; c < rows; c++) {
								for (int c1 = 0; c1 < cols; c1++) {
									if (nrow != c) {
										if (ncol != c1) {
											newMatrix [newrow, newcol] = matrix [c, c1];
											newcol++;
											if (newcol == newCol) {
												newcol = 0;
												newrow++;
											}
										}
									}
								}
							}
							int sign = 0;
							int sum = ncol + nrow;
							sign = (int)Math.Pow (-1, sum);
							tobeAdjoint [nrow, ncol] = sign * determinant (newMatrix, newRow, newCol);
						}
					}
					return Transpose (tobeAdjoint, rows, cols);
				}
			} else {
				throw new MathParserException("Not a square matrix. Cannot comopute adjoint");
				//Console.WriteLine ("The Adjoint of this matrix doesnot exist. Sorry.");
				//Matrix result = new Matrix (1, 1);
				//result.Zero ();
				//return result;
			}
		}
		//end function for adjoint.

	}
	//end operations class.


	public class Number
	{
		// the class for numeric numbers
		private double number;
		private string tag;

		public double getNumber ()
		{
			return number;
		}

		public string getTag ()
		{
			return tag;
		}

		public void setTag (string theTag)
		{
			tag = theTag;
		}

		public void setNumber (double theNumber)
		{
			number = theNumber;
		}

		public Number ()
		{
			number = 0;
			tag = "";
		}

		public Number (Number obj)
		{
			number = obj.number;
			tag = obj.tag;
		}

	}
	//end class for numbers;


	//Expression class Stores all the datatypes and manages them in the list and manage their printing.

	public class Expression
	{
		Matrix theMatrix;
		Number theNumber;
		string theStatement;

		public void setStatement(string statement)
		{
			theStatement = statement;
		}

		public string getStatement()
		{
			return theStatement;
		}

		string theTag = "Exp";
		int expType = 0;

		public Expression ()
		{
		}

		public Expression (Matrix mat)
		{
			theMatrix = mat;
			theTag = theMatrix.getTag ();
			expType = 1;
		}

		public Expression (Number num)
		{
			theNumber = num;
			theTag = theNumber.getTag ();
			expType = 2;
		}

		public Expression (Expression exp)
		{

			expType = exp.expType;
			theTag = exp.theTag;
			theStatement = exp.theStatement;
			if (expType == 1) {
				theMatrix = new Matrix(exp.theMatrix);
			}
			if (expType == 2) {
				theNumber = new Number(exp.theNumber);
			}
		}
		public void setTag (string tag)
		{
			theTag = tag;
		}
		public void setEntireTag(string tag)
		{
			theTag = tag;
			if (this.expType == 1) {
				this.theMatrix.setTag (tag);
			} else if (this.expType == 2) {
				this.theNumber.setTag (tag);
			}
		}

		public Matrix getMatrix () => theMatrix;

		public Number getNumber () => theNumber;

		public string getTag () => theTag;

		public int getExpType () => expType;

		public void setExpType (int type) => expType = type;
	}

	//   This class is responsible of printing the Expression.
	public class ExpressionPrinter
	{
		Expression theExpression;

		public ExpressionPrinter ()
		{
		}

		public ExpressionPrinter (Expression exp)
		{
			theExpression = exp;
		}

		public void Print ()
		{
		//	theAndroidInterface.AndroidInterface.AndroidExpressionPrinter (theExpression);
		}

	}



}


// End DataTypeSpace.