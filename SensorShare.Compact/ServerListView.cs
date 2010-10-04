using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using mcp;
using mcp.Compact;

namespace SensorShare.Compact
{
   public class ServerListView : ListView
   {
      protected Hashtable serverDataList = null;
      protected ImageList serverImageList = null;

      private ServerData serverToAdd = null;

      private ServerDelegate addInvokeHandler = null;
      private VoidDelegate clearInvokeHandler = null;
      private GuidDelegate removeInvokeHandler = null;

      public ServerListView()
         : base()
      {
         this.serverDataList = new Hashtable(10);  // Will expact a capacity of no more than 10
         this.serverImageList = new ImageList();
         this.serverImageList.ImageSize = new Size(64, 64);
         this.LargeImageList = serverImageList;

         this.addInvokeHandler = new ServerDelegate(InvokedAdd);
         this.clearInvokeHandler = new VoidDelegate(ClearItems);
         this.removeInvokeHandler = new GuidDelegate(InvokedRemove);
         this.Activation = ItemActivation.OneClick;
         this.ItemActivate += new EventHandler(ServerListView_ItemActivate);
      }

      void ServerListView_ItemActivate(object sender, EventArgs a)
      {
         if (this.SelectedIndices.Count > 0)
         {
            ListViewItem selectedItem = this.Items[this.SelectedIndices[0]];
            if (itemsAndServerData.ContainsKey(selectedItem))
            {
               FireOnServerSelected((ServerData)itemsAndServerData[selectedItem]);
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
         if (serverDataList.ContainsKey(server.id))
         {
            serverDataList[server.id] = serverToAdd;
            RefreshList();
         }
         else
         {
            serverDataList[server.id] = serverToAdd;
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
         Bitmap bmp = JpegImage.LoadFromBytes(server.pictureBytes);
         serverImageList.Images.Add(JpegImage.GetThumbnail(bmp, serverImageList.ImageSize));
         newItem.ImageIndex = serverImageList.Images.Count - 1;
         itemsAndServerData[newItem] = server;

         this.Items.Add(newItem);
      }

      public void Remove(ServerData server)
      {
         this.Remove(server.id);
      }

      public void Remove(Guid id)
      {
         if (this.InvokeRequired)
         {
            this.Invoke(removeInvokeHandler, id);
         }
         else
         {
            InvokedRemove(id);
         }
      }

      private void InvokedRemove(Guid id)
      {
         serverDataList.Remove(id);
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
            ClearItems();
         }
      }

      private void ClearItems()
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
