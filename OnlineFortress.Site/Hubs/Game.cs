﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;

using SignalR;
using SignalR.Hubs;
using SignalR.Infrastructure;

using OnlineFortress.TelnetClient;
using OnlineFortress.Site.Connection;

namespace OnlineFortress.Site.Hubs {

    [HubName("Game")]    
    public class Game : Hub, IConnected, IDisconnect {

        private static int _gameLoopRunning;
        private const int FPSLIMIT = 24;

        public Task Connect() {

            EnsureGameLoop();

            return Clients.connectCallback(Context.ConnectionId);
        }

        [HubMethodName("Chat")]
        public void Chat(string name, string message) {
            Caller.sendCallBack(message);
            Clients.addMessage(message);
        }

        [HubMethodName("SendKey")]
        public void SendKey(byte characterWich, byte keycode, bool hasAlt, bool hasCntrl, bool hasShift) {
            Terminal tn = TerminalConnection.GetSingleton();
            tn.SendResponse(Helpers.KeyBinder.ParsKeypress(characterWich, keycode, hasAlt, hasCntrl, hasShift));
        }

        public void EnsureGameLoop() {
            if (Interlocked.Exchange(ref _gameLoopRunning, 1) == 0) {
                new Thread(_ => GameLoop()).Start();
            }
        }

        [HubMethodName("DrawFullScreen")]
        public void DrawFullScreen() {
            Terminal tn = TerminalConnection.GetSingleton();
            
            var matrix = new Dictionary<int, Dictionary<int, Tile>>();
            var screen = tn.GetScreenSafe().GetCurrentScreen();

            for (int y = 0; y < screen.GetLength(1); y++) {
                var xaxis = new Dictionary<int, Tile>();
                for (int x = 0; x < screen.GetLength(0); x++) {
                    //ConsoleChar point = Screen[x, y];
                    if (screen[x, y] != null) {
                        xaxis.Add(x, screen[x, y]);
                    }
                }
                matrix.Add(y, xaxis);
            }
            Caller.ScreenUpdate(matrix);
        }


        private static void GameLoop() {

            int ticksPerFrame = (int)Math.Round(1000.0 / FPSLIMIT);
            int lastUpdate = 0;

            var context = GlobalHost.ConnectionManager.GetHubContext<Game>();
            Terminal tn = TerminalConnection.GetSingleton();

            var dict = new Dictionary<int, int>();

            while (tn.IsOpenConnection()) {
                var startTicks = Environment.TickCount;
                
                if (tn.HasUpdate()) {
                    var matrix = new Dictionary<int, Dictionary<int, Tile>>();
                    var Screen = tn.GetScreenSafe().GetScreenUpdate();

                    for (int y = 0; y < Screen.GetLength(1); y++) {
                        var xaxis = new Dictionary<int, Tile>();
                        for (int x = 0; x < Screen.GetLength(0); x++) {
                            //ConsoleChar point = Screen[x, y];
                            if (Screen[x, y] != null) {
                                xaxis.Add(x, Screen[x, y]);
                            }
                        }
                        matrix.Add(y, xaxis);
                    }
                    context.Clients.ScreenUpdate(matrix);
                    
                }

                int timeTicksLeft = (startTicks + ticksPerFrame) - Environment.TickCount;
                if (timeTicksLeft > 0) {
                    //System.Diagnostics.Debug.WriteLine("{2} Sec {0} sleeping {1}ms", DateTime.Now.Second, timeTicksLeft, Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(timeTicksLeft);
                }

                
            }
        }

        public Task Reconnect(IEnumerable<string> groups) {
            EnsureGameLoop();
            return null;
        }

        public Task Disconnect() {
            return null;
        }

    }
}