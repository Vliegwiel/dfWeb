using System;
using System.Threading;
using Win32;

namespace Telnet.Demo {

    /// <summary>
    /// <a href="http://www.klausbasan.de/misc/telnet/index.html">Further details</a>
    /// </summary>
    public class TerminalDemo {

        /// <summary>
        /// The main entry point for the application.
        /// Can be used to test the programm and run it from command line.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            DemoMSTelnetServer(args);
        }

        /// <summary>
        /// Demo for a MS Telnet server
        /// </summary>
        private static void DemoMSTelnetServer(string[] args) {
            //Terminal tn = new Terminal("83.223.1.151", 8000, 10, 80, 50); // hostname, port, timeout [s], width, height
            Terminal tn = new Terminal("faf.vliegwiel.org", 8000, 10, 80, 50); // hostname, port, timeout [s], width, height
            tn.Connect();

            do {
                //string response = "vliegwiel";
                if (tn.WaitForChangedScreen()) {
                    ConsoleEx.Cls();

                    ConsoleChar[,] Screen = tn.VirtualScreen.Screen();
                    for (int y = 0; y < Screen.GetLength(1); y++) {
                        for (int x = 0; x < Screen.GetLength(0); x++) {
                            ConsoleChar point = Screen[x, y];
                            ConsoleEx.QPrint(point.Character.ToString(), 0, 0, ConvertColor(point.ForeColor), ConvertColor(point.BackColor));
                        }
                    }
                }

                if (Console.KeyAvailable || true) {
                    ConsoleKeyInfo name = Console.ReadKey();
                        
                    if (name.Key.ToString().StartsWith("F") && name.Key.ToString().Length >1) {
                        string keystring = name.Key.ToString();
                        int fkey = int.Parse(keystring.Replace("F", ""));
                        tn.SendResponseFunctionKey(fkey);
                    }

                    tn.SendResponse(GetKeyPressBytes(name), false);
                }

            } while (true);

            tn.Close(); 
        }

        private static string GetKeyPressBytes(ConsoleKeyInfo keypress) {
            string ret = "";

            if (keypress.Modifiers == ConsoleModifiers.Alt) {
                ret = "^[";
            }
            if (keypress.Modifiers == ConsoleModifiers.Control)
            {
                ret += "^";
            }
            //if (keypress.Modifiers == ConsoleModifiers.Shift)
            //{
            ret += keypress.KeyChar;
            //}

            return ret;
        }

        private static Win32.ConsoleEx.Colors ConvertColor(ConsoleColor color) {

            switch (color) {
                case ConsoleColor.Black:
                    return Win32.ConsoleEx.Colors.Black; 
                case ConsoleColor.Blue:
                    return Win32.ConsoleEx.Colors.LightBlue;
                case ConsoleColor.Cyan:
                    return Win32.ConsoleEx.Colors.LightCyan;
                case ConsoleColor.DarkBlue:
                    return Win32.ConsoleEx.Colors.Blue;
                case ConsoleColor.DarkCyan:
                    return Win32.ConsoleEx.Colors.Cyan;
                case ConsoleColor.DarkGray:
                    return Win32.ConsoleEx.Colors.Gray;
                case ConsoleColor.DarkGreen:
                    return Win32.ConsoleEx.Colors.Green;
                case ConsoleColor.DarkMagenta:
                    return Win32.ConsoleEx.Colors.Purple;
                case ConsoleColor.DarkRed:
                    return Win32.ConsoleEx.Colors.Red;
                case ConsoleColor.DarkYellow:
                    return Win32.ConsoleEx.Colors.Gold;
                case ConsoleColor.Gray:
                    return Win32.ConsoleEx.Colors.Gray;
                case ConsoleColor.Green:
                    return Win32.ConsoleEx.Colors.LightGreen;
                case ConsoleColor.Magenta:
                    return Win32.ConsoleEx.Colors.LightPurple;
                case ConsoleColor.Red:
                    return Win32.ConsoleEx.Colors.LightRed;
                case ConsoleColor.White:
                    return Win32.ConsoleEx.Colors.White;
                case ConsoleColor.Yellow:
                    return Win32.ConsoleEx.Colors.Yellow;
                default:
                    return Win32.ConsoleEx.Colors.White;
            }

        }

    }
}