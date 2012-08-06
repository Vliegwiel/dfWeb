using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using SignalR;
using SignalR.Hubs;
using SignalR.Infrastructure;

using OnlineFortress.TelnetClient;
using OnlineFortress.Site.Connection;

using System.Threading;


namespace OnlineFortress.Site.Hubs {

    [HubName("Game")]    
    public class Game : Hub, IConnected, IDisconnect {

        private Terminal tn;

        public Game() {

        }


        [HubMethodName("Chat")]
        public void Chat(string name, string message) {
            Caller.sendCallBack(message);
            Clients.addMessage(message);
        }

        [HubMethodName("SendKey")]
        public void SendKey(byte characterWich, bool hasAlt, bool hasCntrl, bool hasShift) {
            tn = TerminalConnection.GetSingleton();
            tn.SendResponse(Helpers.KeyBinder.ParsKeypress(characterWich, hasAlt, hasCntrl, hasShift));
        }



        [HubMethodName("DrawFullScreen")]
        public void DrawFullScreen() {
            tn = TerminalConnection.GetSingleton();

            var bln = tn.IsOpenConnection();
            //while (bln) {
            var matrix = new Dictionary<int, Dictionary<int, ConsoleChar>>();
            var Screen = tn.GetScreenSafe().GetCurrentScreen();
            
            for (int y = 0; y < Screen.GetLength(1); y++) {
                var xaxis = new Dictionary<int, ConsoleChar>();
                    for (int x = 0; x < Screen.GetLength(0); x++) {
                    //ConsoleChar point = Screen[x, y];
                    xaxis.Add(x, Screen[x,y]);
                }
                matrix.Add(y, xaxis);
            }
            Caller.ScreenUpdate(matrix);    
        }

        public Task Connect() {
            return Clients.connectCallback(Context.ConnectionId);
        }

        public Task Reconnect(IEnumerable<string> groups) {
            return null;
        }

        public Task Disconnect() {
            return null;
        }

    }
}