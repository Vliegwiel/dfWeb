// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED!
//
// IF YOU FIND ERRORS OR POSSIBLE IMPROVEMENTS, PLEASE LET ME KNOW.
// MAYBE TOGETHER WE CAN SOLVE THIS.
//
// YOU MAY USE THIS CODE: HOWEVER THIS GRANTS NO FUTURE RIGHTS.

using Telnet;
using System;
using System.Text;

namespace Telnet.Demo
{
	/// <summary>
	/// Some Test methods for the screen class
	/// </summary>
	public class VirtualScreenTest
	{

		/// <summary>
		/// Fill the screen with a pattern
		/// </summary>
		/// <param name="vs">Virtual screen</param>
		public static void FillScreen(VirtualScreen vs) 
		{
			char w = (char)48; // 48=0 57=9
			for (int y = 0; y < vs.Height; y++) 
			{
				w = (char)48; 
				for (int x = 0; x < vs.Width; x++) 
				{
					vs.WriteCharacter(new ConsoleChar(w));
					if (w<57)
						w++;
					else
						w = (char)48;
				}
			}
		}

        ///// <summary>
        ///// Write a byte block and strings
        ///// </summary>
        ///// <param name="vs">Virtual screen</param>
        //public static void WriteBlockOperation(VirtualScreen vs) {
        //    vs.CleanScreen();
        //    string tb = "This is a test output";
        //    vs.WriteLine("New screen with test output:");
			
        //    for (int i=0; i<10; i++) {
        //        vs.WriteCharacter(tb.ToCharArray());
        //    }

        //    vs.WriteLine("\n");
        //    for (int i=0; i<10; i++) 
        //    {
        //        vs.WriteLine("Line output");
        //    }
        //}

		/// <summary>
		/// Test cursor movements
		/// </summary>
		/// <param name="vs">Virtual screen</param>
		public static void WriteBigX(VirtualScreen vs) 
		{
			vs.CleanScreen();
			for (int i=0; i<vs.Height; i++) 
			{
				vs.Write('X'); // already moves cursor about one
				vs.MoveCursor(vs.Width); // test overflow
			}
			vs.Write('\r'); // beginning of last line
			for (int i=0; i<vs.Height; i++) 
			{
				vs.Write('X'); // already moves cursor about one
				vs.MoveCursor(-vs.Width); // test overflow
			}
		}

	} // class
} // namespace