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
            Terminal tn = new Terminal("localhost", 8000, 10, 80, 50); // hostname, port, timeout [s], width, height
            tn.Connect();

            Console.SetWindowSize(80, 50);
            
            do {
                //string response = "vliegwiel";
                if (tn.WaitForChangedScreen()) {
                    Console.Clear();
                    Console.Write(tn.VirtualScreen.Hardcopy().Trim());
                }
                if (Console.KeyAvailable || true) {
                    ConsoleKeyInfo name = Console.ReadKey();
                        
                    if (name.Key.ToString().StartsWith("F")) {
                        string keystring = name.Key.ToString();
                        int fkey = int.Parse(keystring.Replace("F", ""));
                        tn.SendResponseFunctionKey(fkey);
                    }
                    var mod = name.Modifiers;
                    tn.SendResponse(Convert.ToString(name.KeyChar), false);
                }

            } while (true);

            tn.Close(); 
        }
    }
}