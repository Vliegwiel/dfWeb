using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OnlineFortress.TelnetClient;

namespace OnlineFortress.Site.Connection {
   
    public class TerminalConnection {

        private static Terminal _singleton = null;
        private static Object _lock = new Object();

        public static Terminal GetSingleton() {
            if (_singleton == null) {
                lock (_lock) {
                    if (_singleton == null) {
                        _singleton = new Terminal("localhost", 8000, 10, 80, 50); // hostname, port, timeout [s], width, height
                        _singleton.Connect();
                    }
                }
            }
            return _singleton;
        }

    }
}