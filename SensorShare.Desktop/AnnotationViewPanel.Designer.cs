namespace SensorShare.Desktop
{
    partial class AnnotationView
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
            this.timeLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.textLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.timeLabel.Location = new System.Drawing.Point(0, 0);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(131, 300);
            this.timeLabel.TabIndex = 6;
            this.timeLabel.Text = "time";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 20);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(195, 109);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            this.pictureBox.Visible = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // textLabel
            // 
            this.textLabel.Location = new System.Drawing.Point(0, 132);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(195, 74);
            this.textLabel.TabIndex = 4;
            this.textLabel.Text = "text";
            this.textLabel.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(137, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 20);
            this.button1.TabIndex = 3;
            this.button1.Text = "Show";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AnnotationView
            // 
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.timeLabel);
            this.Size = new System.Drawing.Size(195, 300);
            this.Resize += new System.EventHandler(this.AnnotationView_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label textLabel;
        private System.Windows.Forms.Button button1;
    }
}
