/*
 * User: JinGen
 * Date: 11/12/2013
 * Time: 6:50 PM
 */
using System;

namespace CCManager
{
	/// <summary>
	/// Description of Log.
	/// </summary>
	public static class Log
	{
		// Most important events should begin from 1
		// Major events at 2
		// Tracing values at 3
		// Debug data at 4
		public static int LogLevel = 2;
		
		public static void Info(string dataToLog, int logLevel)
		{
			if (logLevel <= LogLevel)
			{
				Info(dataToLog);
			}
		}
		
		public static void Info(string dataToLog)
		{
			Console.WriteLine("[INFO]    " + dataToLog);
		}
		
		public static void Warning(string dataToLog)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("[WARNING] " + dataToLog);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
		
		public static void Error(string dataToLog)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("[ERROR]   " + dataToLog);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
		
		public static void Success(string dataToLog)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("[SUCCESS] " + dataToLog);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
