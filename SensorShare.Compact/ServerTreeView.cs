using System;
using System.Collections;
using System.Net;
using System.Windows.Forms;
using SensorShare;
using SensorShare.Compact;

namespace SensorShare.Compact
{
    public class ServerTreeView : TreeView
    {
        protected Hashtable serverDataList = null;

        private ServerData serverToAdd = null;
        private IPAddress serverIPToRemove = null;

        private ServerDelegate addInvokeHandler = null;
        private EventHandler clearInvokeHandler = null;
        private EventHandler removeInvokeHandler = null;

        public ServerTreeView()
            : base()
        {
            this.serverDataList = new Hashtable(10);  // Will expact a capacity of no more than 10
            
            this.addInvokeHandler = new ServerDelegate(InvokedAdd);
            
            this.clearInvokeHandler = new EventHandler(ClearNodes);
            this.removeInvokeHandler = new EventHandler(InvokeRemove);
            this.AfterSelect += new TreeViewEventHandler(ServerListView_AfterSelect);
        }

        void ServerListView_AfterSelect(object sender, TreeViewEventArgs a)
        {
            if (a.Action == TreeViewAction.ByMouse)
            {
                if (nodesAndServerData.ContainsKey(a.Node))
                {
                    FireOnServerSelected((ServerData)nodesAndServerData[a.Node]);
                }
            }
        }

        public void Add(ServerData server)
        {
            serverToAdd = server;
            if (this.InvokeRequired)
            {
                this.Invoke(addInvokeHandler, server);
            }
            else
            {
                InvokedAdd(server);
            }
        }

        private void InvokedAdd(ServerData server)
        {
            if (serverDataList.ContainsKey(server.id.ToString()))
            {
                serverDataList[server.id.ToString()] = server;
                RefreshTree();
            }
            else
            {
                serverDataList[serverToAdd.id.ToString()] = server;
                addServerNode(server);
            }
        }

        private void RefreshTree()
        {
                this.Nodes.Clear();
                this.nodesAndServerData.Clear();
                foreach (Object serverData in serverDataList.Values)
                {
                    addServerNode((ServerData) serverData);
                }
        }

        private Hashtable nodesAndServerData = new Hashtable(10);

        private void addServerNode(ServerData server)
        {
            TreeNode newNode = new TreeNode(server.name);
            
            TreeNode connectNode = new TreeNode("Connect");
            newNode.Nodes.Add(connectNode);
            nodesAndServerData[connectNode] = server;

            newNode.Nodes.Add(server.description);

            TreeNode sensorsNode = new TreeNode("Sensors");

            newNode.Nodes.Add(sensorsNode);
            this.Nodes.Add(newNode);
        }

        public void Remove(IPAddress serverIP)
        {
            serverIPToRemove = serverIP;
            this.Invoke(removeInvokeHandler);
        }

        private void InvokeRemove(object sender, EventArgs a)
        {
            serverDataList.Remove(serverIPToRemove.ToString());
            serverIPToRemove = null;
            RefreshTree();
        }

        public void Clear()
        {
            this.Invoke(clearInvokeHandler);
        }

        private void ClearNodes(object sender, EventArgs a)
        {
            this.serverDataList.Clear();
            this.Nodes.Clear();
        }
 
        public event ServerSelectedEventHandler ServerSelected;

        private void FireOnServerSelected(ServerData server)
        {
            if (ServerSelected != null)
            {
                ServerSelected(this, new ServerSelectedEventArgs(server));
            }
        }

    }

    public delegate void ServerSelectedEventHandler(object sender, ServerSelectedEventArgs e);

    public class ServerSelectedEventArgs : EventArgs
    {
        private readonly ServerData server;

        public ServerSelectedEventArgs(ServerData serverSelected)
            : base()
        {
            this.server = serverSelected;
        }

        public ServerData Server
        {
            get { return server; }
        }

    }
}
