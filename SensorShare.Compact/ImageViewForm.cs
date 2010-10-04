using System;
using System.Drawing;
using System.Windows.Forms;
using mcp.Compact;

namespace SensorShare.Compact
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DoCloseInvoke();
        }

    }
}