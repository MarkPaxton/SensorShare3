using System;
using System.Collections;
using System.Net;
using System.Windows.Forms;

namespace SensorShare.Desktop
{
    public class ServerListView : ListView
    {
        protected Hashtable serverDataList = null;
        protected ImageList serverImageList = null;

        private ServerData serverToAdd = null;
        private IPAddress serverIPToRemove = null;
        
        private ServerDelegate addInvokeHandler = null;
        private EventHandler clearInvokeHandler = null;
        private EventHandler removeInvokeHandler = null;

        public ServerListView()
            : base()
        {
            this.serverDataList = new Hashtable(10);  // Will expact a capacity of no more than 10
            this.serverImageList = new ImageList();
            this.serverImageList.ImageSize = new System.Drawing.Size(64, 64);
            this.LargeImageList = serverImageList;

            this.addInvokeHandler = new ServerDelegate(InvokedAdd);
            this.clearInvokeHandler = new EventHandler(ClearItems);
            this.removeInvokeHandler = new EventHandler(InvokeRemove);
            this.Activation = ItemActivation.OneClick;
            this.ItemActivate += new EventHandler(ServerListView_ItemActivate);
        }

        void ServerListView_ItemActivate(object sender, EventArgs a)
        {
            ListViewItem selectedItem = this.Items[this.SelectedIndices[0]];
            if (itemsAndServerData.ContainsKey(selectedItem))
            {
                FireOnServerSelected((ServerData)itemsAndServerData[selectedItem]);
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
                serverDataList[server.id.ToString()] = serverToAdd;
                RefreshList();
            }
            else
            {
                serverDataList[server.id.ToString()] = serverToAdd;
                addServerItem(server);
            }
        }

        private void RefreshList()
        {
            this.Items.Clear();
            this.itemsAndServerData.Clear();
            foreach (Object serverData in serverDataList.Values)
            {
                addServerItem((ServerData)serverData);
            }
        }

        private Hashtable itemsAndServerData = new Hashtable(10);

        private void addServerItem(ServerData server)
        {
            ListViewItem newItem = new ListViewItem(server.name);

            serverImageList.Images.Add( JpegImage.GetThumbnail(JpegImage.LoadFromBytes(server.pictureBytes), serverImageList.ImageSize));
            newItem.ImageIndex = serverImageList.Images.Count - 1;

            itemsAndServerData[newItem] = server;

            this.Items.Add(newItem);
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
            RefreshList();
        }

        public new void Clear()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(clearInvokeHandler);
            }
            else
            {
                ClearItems(this, new EventArgs());
            }
        }

        private void ClearItems(object sender, EventArgs a)
        {
            this.serverDataList.Clear();
            this.Items.Clear();
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
}
