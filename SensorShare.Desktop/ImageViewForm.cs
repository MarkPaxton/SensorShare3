using System;
using System.Drawing;
using System.Windows.Forms;

namespace SensorShare.Desktop
{
    public partial class ImageViewForm : Form
    {
       Bitmap image;

       public ImageViewForm(Bitmap image)
       {
          InitializeComponent();
          this.image = image;
          pictureBox.Image = JpegImage.GetThumbnail(image, pictureBox.Size);
       }

       protected void DoCloseInvoke()
       {
          if (this.InvokeRequired)
          {
             this.Invoke(new EventHandler(DoClose));
          }
          else
          {
             this.Close();
          }
       }

       protected void DoClose(object sender, EventArgs args)
       {
          this.Close();
       }

       private void pictureBox_Click(object sender, EventArgs e)
       {
          DoCloseInvoke();
       }


       private void ImageViewForm_Resize(object sender, EventArgs e)
       {
          pictureBox.Image = JpegImage.GetThumbnail(image, pictureBox.Size);
       }
    }
}