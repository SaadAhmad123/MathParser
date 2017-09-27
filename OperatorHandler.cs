using System;
using System.Collections.Generic;
namespace MathParser
{
	/// <summary>
	/// Operator handler.
	/// Here the Expression List is sent for some operator processing.
	/// The class oversees the correct implementation of the order of operators.
	/// The custom order checking can be done by the delegate 'staticOnOperatorChecking_Advanced' and
	/// 'staticOnOperatorChecking_Basic'.
	/// The Basic Operator checking is also defined in the class itself. 
	/// </summary>

	internal class OperatorHandler   
	{
		List<string> Expressions;
		bool OperatorHandledCorrectly = true;
		public OperatorHandler() { }
		public OperatorHandler(ref List<string> ExpressionBatch)
		{
			Expressions = ExpressionBatch;
		}

		public string BasicOperatorsString = "";

		public delegate bool OnOperatorCheckingDelegate(List<string> ExpressionList);
		public static OnOperatorCheckingDelegate staticOnOperatorChecking_Basic;
		public static OnOperatorCheckingDelegate staticOnOperatorChecking_Advanced;

		public bool isCorrectOperatorSequence () => OperatorHandledCorrectly;

		public void Process()
		{
			staticOnOperatorChecking_Basic?.Invoke(Expressions);
			staticOnOperatorChecking_Advanced?.Invoke(Expressions);
			for (int c = 0; c < Expressions.Count; c++)
			{
				string Element = Expressions[c];
				if (BasicOperatorsString.Contains(Element))
				{
					if (Element == "-")   // For minus operator.
					{
						if (c == 0)    // minus is the first element.
						{
							if (Expressions [c + 1] == "-") {   // --
								Expressions [c + 1] = "+";
								Expressions.RemoveAt (c);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "(") {    // -(
								Expressions [c + 1] = "-" + Expressions [c + 1];
								Expressions.RemoveAt (c);
								c = 0 - 1;
							} else if (!BasicOperatorsString.Contains (Expressions [c + 1])) { // -2
								Expressions [c + 1] = "-" + Expressions [c + 1];
								Expressions.RemoveAt (c);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "+") {    //-+
								Expressions [c + 1] = "-";
								Expressions.RemoveAt (c);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "#") {
								Expressions.RemoveAt (c + 1);
								c = 0-1;
							}
						}    // end case for first minus element.
						else
						{
							if (Expressions [c + 1] == "-" && !BasicOperatorsString.Contains (Expressions [c + 2])) {  // If the second element of curent is a minus too and after that no operator occurs.
								Expressions [c + 1] = "+";
								Expressions.RemoveAt (c);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "+" && !BasicOperatorsString.Contains (Expressions [c + 2])) {   // If the secind element is plus of the current and the third one had no operator.
								Expressions [c + 1] = "-";
								Expressions.RemoveAt (c);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "(") {
								Expressions [c + 1] = "-" + Expressions [c + 1].Trim ();
								Expressions [c] = "+";
							} else if (Expressions [c - 1].Contains ("(") && !BasicOperatorsString.Contains (Expressions [c + 1])) {    // If the next to current operator is a variable the appent minus at its front and replace current opertor with a plus.
								Expressions [c + 1] = "-" + Expressions [c + 1];
								Expressions.RemoveAt (c);
								c = -1;
							} else if (!BasicOperatorsString.Contains (Expressions [c + 1])) {    // If the next to current operator is a variable the appent minus at its front and replace current opertor with a plus.
								Expressions [c + 1] = "-" +
								"" + Expressions [c + 1].Trim ();
								Expressions [c] = "+";
							} else if (Expressions [c + 1] == "#") {
								Expressions.RemoveAt (c + 1);
								c = - 1;
							}

							else   // Bad operator sequence.
							{
								OperatorHandledCorrectly = false;
								break;
							}

						}  // end else.
					}   // end case for minus.



					if (Element == "+")   // For plus operator
					{
						if (c == 0)   // if plus is the first element.   
						{
							Expressions.RemoveAt(c);
							c = 0-1;
						}   // end first plus handling.
						else {
							if (Expressions [c + 1] == "+" && !(BasicOperatorsString.Contains (Expressions [c + 2]))) { // if the is a plus right after the current operator and after that there is no operator.
								Expressions.RemoveAt (c + 1);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "-" && !(BasicOperatorsString.Contains (Expressions [c + 2]))) { //if there is a minus after the current operator and after that there is no operator.
								Expressions.RemoveAt (c);
								c = 0 - 1;
							} else if (!(BasicOperatorsString.Contains (Expressions [c + 1]))) {
								
							} else if (Expressions [c + 1] == "(") {
							} else if (Expressions [c + 1] == "#") {
								Expressions.RemoveAt (c + 1);
								c = -1;
							}
							else {    // bad operator sequence.
								OperatorHandledCorrectly = false;
								break;
							}
						}
					}

					if (Element == "*" || Element == new string((char)215, 1)) // For multiplication operator.
					{
						if (c == 0)   // if multiply is the first element. The bad opertor sequence.
						{
							OperatorHandledCorrectly = false;
							break;
						}
						else
						{
							if (Expressions [c + 1] == "+" && !(BasicOperatorsString.Contains (Expressions [c + 2]))) { // if the is a plus right after the current operator and after that there is no operator.
								Expressions.RemoveAt (c + 1);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "-" && !(BasicOperatorsString.Contains (Expressions [c + 2]))) { //if there is a minus after the current operator and after that there is no operator.
								Expressions [c + 2] = "-" + Expressions [c + 2].Trim ();
								Expressions.RemoveAt (c + 1);
								c = 0 - 1;
							} else if (!(BasicOperatorsString.Contains (Expressions [c + 1]))) {
							} else if (Expressions [c + 1] == "(") {
							} else if (Expressions [c + 1] == "#") {
								Expressions.RemoveAt (c + 1);
								c = -1;
							}
							else {
								OperatorHandledCorrectly = false;
								break;
							}
						}
					}

					if (Element == "÷" || Element == "/") // For dividsion operator.
					{
						if (c == 0)    // if divide is the first element. A bad operator sequence.
						{
							OperatorHandledCorrectly = false;
							break;
						}
						else { 
							if (Expressions [c + 1] == "+" && !(BasicOperatorsString.Contains (Expressions [c + 2]))) { // if the is a plus right after the current operator and after that there is no operator.
								Expressions.RemoveAt (c + 1);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "-" && !(BasicOperatorsString.Contains (Expressions [c + 2]))) { //if there is a minus after the current operator and after that there is no operator.
								Expressions [c + 2] = "-" + Expressions [c + 2].Trim ();
								Expressions.RemoveAt (c + 1);
								c = 0 - 1;
							} else if (!(BasicOperatorsString.Contains (Expressions [c + 1]))) {
							} else if (Expressions [c + 1] == "(") {
							} else if (Expressions [c + 1] == "#") {
								Expressions.RemoveAt(c+1);
								c = -1;
							}
							else {
								OperatorHandledCorrectly = false;
								break;
							}
						}
					}    // end for divide.

					if (Element == "`") {
						if (c == 0) {
							OperatorHandledCorrectly = false;
							break;
						} else {
							if (Expressions [c + 1] == "-" && !BasicOperatorsString.Contains (Expressions [c + 2])) {
								Expressions [c + 2] = "-" + Expressions [c + 2].Trim ();
								Expressions.RemoveAt (c + 1);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "+" && !(BasicOperatorsString.Contains (Expressions [c + 2]))) { // if the is a plus right after the current operator and after that there is no operator.
								Expressions.RemoveAt (c + 1);
								c = 0 - 1;
							} else if (Expressions [c + 1] == "#") {
								Expressions.RemoveAt (c + 1);
								c = -1;
							}
						}
					}

					if (Element == "(")
					{ 
						if (Expressions [c + 1] == "+") {
							Expressions.RemoveAt (c + 1);
							c = c - 1;
						} else if (c != 0 && !(BasicOperatorsString.Contains (Expressions [c - 1]))) {
							Expressions.Insert (c, "*");
							c = -1;
						} else if (Expressions [c + 1] == "#") {
							Expressions.RemoveAt (c + 1);
							c = c-1;
						}
					}

					if (Element == ")") {
						if (!(c >= Expressions.Count - 1) && !BasicOperatorsString.Contains (Expressions [c + 1])) {
							Expressions.Insert (c + 1, "*");
							c = -1;
						} else if (!(c >= Expressions.Count - 1) && Expressions [c + 1] == "(") {
							Expressions.Insert (c + 1, "*");	
							c = -1;
						} else if (!(c >= Expressions.Count - 1) && Expressions [c + 1] == "#") {
							Expressions [c + 1].Replace ("#", "*");
						}
					}

					if (Element == "#") {
						if (c != 0 && !(BasicOperatorsString.Contains (Expressions [c - 1])) && Checker.KeyWords.Contains (Expressions [c + 1])) {
							Expressions [c] = "*";
						} /*else if (c != 0 && Expressions [c - 1] == ")" && Checker.KeyWords.Contains (Expressions [c + !1])) {
							Expressions [c] = "*";
						} else if (c == 0) {
							Expressions.RemoveAt (c);
							c = -1;
						}*/

						else {
							Expressions.RemoveAt (c);
							c = -1;
						}
					}

					if (Element == "^") {
						if (c == 0) {
							OperatorHandledCorrectly = false;
							break;
						} else if (c == Expressions.Count - 1) {
							OperatorHandledCorrectly = false;
							break;
						} else {
							if (Expressions [c + 1] == "-" && !BasicOperatorsString.Contains (Expressions [c + 2])) {
								Expressions [c + 2] = "-" + Expressions [c + 2];	
								Expressions.RemoveAt (c + 1);
								c = -1;
							} else if (Expressions [c + 1] == "-" && Expressions[c+2] == "#" && !BasicOperatorsString.Contains (Expressions [c + 3])) {
								Expressions [c + 3] = "-" + Expressions [c + 3];	
								Expressions.RemoveAt (c + 1);
								Expressions.RemoveAt (c + 1);
								c = -1;
							} else if (Expressions [c + 1] == "+" && !BasicOperatorsString.Contains (Expressions [c + 2])) {
								Expressions.RemoveAt (c + 1);
								c = -1;
							} else if (Expressions [c + 1] == "#" && !BasicOperatorsString.Contains (Expressions [c + 2])) {
								Expressions.RemoveAt (c + 1);
								c = -1;
							} else if (BasicOperatorsString.Contains (Expressions [c + 1])) {
								OperatorHandledCorrectly = false;
								break;
							}
						}
					}

				}    // end if for operators.
			}       // end the for loop.





		}   // end process method.

	}    // end class
}





