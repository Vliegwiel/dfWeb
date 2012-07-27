using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineFortress.Console {
    class Program {

        /// <summary>
        /// The main entry point for the application.
        /// Can be used to test the programm and run it from command line.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            if (args.Length > 0) {
                System.Console.WriteLine("Trying to connect to: " + args[0] + "on port 8000");
                TerminalDemo td = new TerminalDemo(args[0]);
            } else {
                TerminalDemo td = new TerminalDemo("localhost");
            }



        }
    }
}
