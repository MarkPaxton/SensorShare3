using System;
using System.Drawing;
using System.Windows.Forms;
using mcp;

namespace SensorShare
{
    public partial class ViewNotAvailableAnnotationControl : UserControl
    {
        Boolean showData = false;
        public Boolean ShowData
        {
            get { return showData; }
            set
            {
                showData = value;
                DoUpdateInvoke();
            }
        }

        VoidDelegate updateSizes;
        VoidDelegate updateDel;

        public ViewNotAvailableAnnotationControl(Image infoImage)
        {
            InitializeComponent();

            infoPicture.Image = infoImage;

            this.Height = headingLabel.Height;            

            updateSizes = new VoidDelegate(UpdateSizes);
            updateDel = new VoidDelegate(DoUpdate);
        }


        private void NotAvailableAnnotationView_Resize(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(updateSizes);
            }
            else
            {
                UpdateSizes();
            }
        }

        private void UpdateSizes()
        {
            headingLabel.Size = new Size(Math.Max(this.Width - infoPicture.Width - 5, 0), headingLabel.Height);
            detailTextBox.Size = new Size(this.Width, detailTextBox.Height);
        }

        private void infoPicture_Click(object sender, EventArgs e)
        {
            this.ShowData = !this.ShowData;
        }

        protected void DoUpdateInvoke()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(updateDel);
            }
            else
            {
                DoUpdate();
            }
        }

        protected void DoUpdate()
        {
            if (showData == true)
            {
                this.Size = new Size(this.Width, headingLabel.Height + detailTextBox.Height);
            }
            else
            {
                this.Size = new Size(this.Width, headingLabel.Height);
            }
        }

    }
}
