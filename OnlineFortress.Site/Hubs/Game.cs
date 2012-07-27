using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SignalR;
using SignalR.Hubs;
using SignalR.Infrastructure;
using System.Threading.Tasks;

namespace OnlineFortress.Site.Hubs {

    [HubName("Game")]    
    public class Game : Hub, IConnected, IDisconnect {

        [HubMethodName("Chat")]
        public void Chat(string name, string message) {
            Caller.sendCallBack(message);
            Clients.addMessage(message);

            //var connectionManager = AspNetHost.DependencyResolver.Resolve<IConnectionManager>();
            //var clients = connectionManager.GetClients<Type of your hub here>();
            //clients[clientId].method();
        }

        public Task Connect() {
            return Clients.addMessage(Context.ConnectionId, DateTime.Now.ToString("g"));
        }

        public Task Reconnect(IEnumerable<string> groups) {
            return Clients.addMessage(Context.ConnectionId, DateTime.Now.ToString("g"));
        }

    }
}