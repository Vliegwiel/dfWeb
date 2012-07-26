using System;
using System.Threading;
using Win32;
using System.Threading.Tasks;


namespace Telnet.Demo {

    /// <summary>
    /// <a href="http://www.klausbasan.de/misc/telnet/index.html">Further details</a>
    /// </summary>
    public class TerminalDemo {

        private Terminal tn;

        /// <summary>
        /// The main entry point for the application.
        /// Can be used to test the programm and run it from command line.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            TerminalDemo td = new TerminalDemo();

        }

        public TerminalDemo() {
            //tn = new Terminal("faf.vliegwiel.org", 8000, 10, 80, 50); // hostname, port, timeout [s], width, height
            tn = new Terminal("localhost", 8000, 10, 80, 50); // hostname, port, timeout [s], width, height
            tn.Connect();

            Console.WindowHeight = 51;
            Console.WindowWidth = 80;
            Console.Title = "DFTerm parsing By Vliegwiel";

            ThreadStart tc1 = new ThreadStart(DrawLoop);
            ThreadStart tc2 = new ThreadStart(Read);

            Thread t1 = new Thread(tc1);
            Thread t2 = new Thread(tc2);

            t1.Start();
            t2.Start();


        }

        public void DrawLoop() {
            ConsoleEx.Cls();
            do {
                if (tn.GetScreenSafe().ChangedScreen) {

                    ConsoleChar[,] Screen = tn.GetScreenSafe().GetScreenUpdate();

                    for (int y = 0; y < Screen.GetLength(1); y++) {
                        for (int x = 0; x < Screen.GetLength(0); x++) {
                            ConsoleChar point = Screen[x, y];

                            if (point != null) {
                                if (point.Character > 8720 && point.Character < 8750) point.Character = (char)1;
                                ConsoleEx.QPrint(point.Character.ToString(), x, y, ConvertColor(point.ForeColor), ConvertColor(point.BackColor));
                            }
                        }
                    }
                }
            } while (true);
            // http://dwarffortresswiki.org/index.php/Character_table
        }


        /// <summary>
        /// Demo for a MS Telnet server
        /// </summary>
        private void Read() {
            while (true) {
                if (Console.KeyAvailable) {
                    ConsoleKeyInfo name = Console.ReadKey();

                    if (name.Key.ToString().StartsWith("F") && name.Key.ToString().Length > 1) {
                        string keystring = name.Key.ToString();
                        int fkey = int.Parse(keystring.Replace("F", ""));
                        tn.SendResponseFunctionKey(fkey);
                    }

                    tn.SendResponse(GetKeyPressBytes(name), false);
                }
            }

            tn.Close();
        }

        private static string GetKeyPressBytes(ConsoleKeyInfo keypress) {
            string ret = "";
            const byte ESC = 27;

            switch (keypress.Key) {
                case ConsoleKey.UpArrow:
                    ret += Convert.ToChar(ESC);
                    ret += "[A";
                    return ret;
                case ConsoleKey.DownArrow:
                    ret += Convert.ToChar(ESC);
                    ret += "[B";
                    return ret;
                case ConsoleKey.LeftArrow:
                    ret += Convert.ToChar(ESC);
                    ret += "[D";
                    return ret;
                case ConsoleKey.RightArrow:
                    ret += Convert.ToChar(ESC);
                    ret += "[C";
                    return ret;
                case ConsoleKey.Home:
                    ret += Convert.ToChar(ESC);
                    ret += "[H";
                    return ret;
                case ConsoleKey.End:
                    ret += Convert.ToChar(ESC);
                    ret += "[F";
                    return ret;
                case ConsoleKey.F:
                    if (keypress.Modifiers == ConsoleModifiers.Control) {
                        return "" + Convert.ToChar(6);
                    }
                    return "F";
                case ConsoleKey.R:
                    if (keypress.Modifiers == ConsoleModifiers.Control) {
                        return "" + Convert.ToChar(18);
                    }
                    return "F";
                case ConsoleKey.Backspace:
                    return "" + Convert.ToChar(127);
            }

            if (keypress.Modifiers == ConsoleModifiers.Alt) {
                ret += Convert.ToChar(ESC);
            }
            if (keypress.Modifiers == ConsoleModifiers.Control) {
                ret += Convert.ToChar(ESC);
                return ret += keypress.KeyChar;
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