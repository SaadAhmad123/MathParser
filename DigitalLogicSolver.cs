using System;
using MathParser.DataTypes;
using System.Collections.Generic;

namespace MathParser
{

	public delegate void Digital_Logic_KeyWords_Sync_Delegate(ref List<string> theList);


	public class DigitalLogicSolver : ISolver
	{
		MathParserExpression theSolution;
		public MathParserExpression getSolution() => theSolution;

		public static Digital_Logic_KeyWords_Sync_Delegate OnSyncKeyWords = null;

		bool processed = false;
		public bool isProcessed() => processed;

		static List<string> theKeyWordsList = new List<string>();

		public static void syncKeyWords()
		{
			theKeyWordsList.Add ("BIN");
			theKeyWordsList.Add ("HEX");
			theKeyWordsList.Add ("DEC");
			theKeyWordsList.Add ("OCT");
			OnSyncKeyWords?.Invoke (ref theKeyWordsList); 
		}

		public static List<string> getKeyWordsList()
		{
			return theKeyWordsList;
		}

		string theExpression = null;

		public DigitalLogicSolver (string theExpression)
		{
			this.theExpression = theExpression;	
		}


	}
}

