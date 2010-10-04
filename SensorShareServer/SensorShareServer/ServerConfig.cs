using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using mcp;
using mcp.Compact;

namespace SensorShare.Compact
{
   public partial class ServerConfig : Form
   {
      internal Bitmap _imageBoxImage;
      private Bitmap _image;
      internal Bitmap image
      {
         get { return _image; }
         set
         {
            // Store the path
            _image = value;
            // Update the image
            _imageBoxImage = JpegImage.GetThumbnail(new Bitmap(_image), this.serverImageBox.Size);
            DoUpdateImageBoxInvoke(_imageBoxImage);
         }
      }

      public ServerConfig()
      {
         InitializeComponent();
         updateImageBoxDelegate = new BitmapDelegate(UpdateImageBox);
      }

      private void newImageLinkLabel_Click(object sender, EventArgs e)
      {
         if (selectImageDialog.ShowDialog() == DialogResult.OK)
         {
            if (File.Exists(selectImageDialog.FileName))
            {
               image = new Bitmap(selectImageDialog.FileName);
            }
            else
            {
               MessageBox.Show("The selected file does not exist.", "Server image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
         }
      }

      #region ImageBox Updater

      private void UpdateImageBox(Bitmap newImage)
      {
         serverImageBox.Image = newImage;
      }

      BitmapDelegate updateImageBoxDelegate;
      private void DoUpdateImageBoxInvoke(Bitmap newImage)
      {
          if (serverImageBox.InvokeRequired)
          {
              serverImageBox.Invoke(updateImageBoxDelegate, newImage);
          }
          else
          {
              UpdateImageBox(newImage);
          }
      }

      #endregion

   }
}