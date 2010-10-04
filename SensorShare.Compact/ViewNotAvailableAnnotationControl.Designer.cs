namespace SensorShare
{
    partial class ViewNotAvailableAnnotationControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headingPanel = new System.Windows.Forms.Panel();
            this.headingLabel = new System.Windows.Forms.Label();
            this.infoPicture = new System.Windows.Forms.PictureBox();
            this.detailTextBox = new System.Windows.Forms.TextBox();
            this.headingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headingPanel
            // 
            this.headingPanel.Controls.Add(this.headingLabel);
            this.headingPanel.Controls.Add(this.infoPicture);
            this.headingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headingPanel.Location = new System.Drawing.Point(0, 0);
            this.headingPanel.Name = "headingPanel";
            this.headingPanel.Size = new System.Drawing.Size(238, 33);
            // 
            // headingLabel
            // 
            this.headingLabel.BackColor = System.Drawing.SystemColors.GrayText;
            this.headingLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.headingLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.headingLabel.Location = new System.Drawing.Point(33, 0);
            this.headingLabel.Name = "headingLabel";
            this.headingLabel.Size = new System.Drawing.Size(205, 33);
            this.headingLabel.Text = "Information not available.";
            // 
            // infoPicture
            // 
            this.infoPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoPicture.Location = new System.Drawing.Point(0, 0);
            this.infoPicture.Name = "infoPicture";
            this.infoPicture.Size = new System.Drawing.Size(31, 33);
            this.infoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.infoPicture.Click += new System.EventHandler(this.infoPicture_Click);
            // 
            // detailTextBox
            // 
            this.detailTextBox.Location = new System.Drawing.Point(0, 36);
            this.detailTextBox.Multiline = true;
            this.detailTextBox.Name = "detailTextBox";
            this.detailTextBox.Size = new System.Drawing.Size(238, 79);
            this.detailTextBox.TabIndex = 2;
            this.detailTextBox.Text = "This annotation has not yet been downloaded from the sensor, to view it you need " +
                "to find and connect to that sensor.";
            // 
            // ViewNotAvailableAnnotationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.detailTextBox);
            this.Controls.Add(this.headingPanel);
            this.Name = "ViewNotAvailableAnnotationControl";
            this.Size = new System.Drawing.Size(238, 115);
            this.Resize += new System.EventHandler(this.NotAvailableAnnotationView_Resize);
            this.headingPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headingPanel;
        private System.Windows.Forms.Label headingLabel;
        private System.Windows.Forms.PictureBox infoPicture;
        private System.Windows.Forms.TextBox detailTextBox;

    }
}
