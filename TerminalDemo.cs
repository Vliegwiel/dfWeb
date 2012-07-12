using System;
using System.Threading;

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
            Terminal tn = new Terminal("faf.vliegwiel.org", 8000, 10, 80, 40); // hostname, port, timeout [s], width, height
            tn.Connect();

            do {
                //string response = "vliegwiel";
                if (tn.WaitForChangedScreen()) {
                    Console.Clear();
                    Console.Write(tn.VirtualScreen.Hardcopy().Trim());
                }
                ConsoleKeyInfo name = Console.ReadKey();
                if (name.KeyChar > 0) {
                    tn.SendResponse(Convert.ToString(name.KeyChar), false);
                }

            } while (true);

            tn.Close(); 
        }
    }
}