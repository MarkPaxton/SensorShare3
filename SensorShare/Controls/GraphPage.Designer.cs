using System.Windows.Forms;
using mcp.Graphs;

namespace SensorShare
{
    partial class GraphPage:UserControl
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ZoomOutButton = new System.Windows.Forms.Button();
            this.ZoomInButton = new System.Windows.Forms.Button();
            this.ZoomLabel = new System.Windows.Forms.Label();
            this.lastReadingLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toNowButton = new System.Windows.Forms.Button();
            this.forwardButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.graphBox = new mcp.Graphs.DateTimeInfoGraphBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(3, 3);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(235, 19);
            this.titleLabel.Text = "Title";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ZoomOutButton);
            this.panel1.Controls.Add(this.ZoomInButton);
            this.panel1.Controls.Add(this.ZoomLabel);
            this.panel1.Controls.Add(this.lastReadingLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.toNowButton);
            this.panel1.Controls.Add(this.forwardButton);
            this.panel1.Controls.Add(this.backButton);
            this.panel1.Controls.Add(this.graphBox);
            this.panel1.Controls.Add(this.titleLabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 271);
            // 
            // ZoomOutButton
            // 
            this.ZoomOutButton.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ZoomOutButton.Location = new System.Drawing.Point(184, 229);
            this.ZoomOutButton.Name = "ZoomOutButton";
            this.ZoomOutButton.Size = new System.Drawing.Size(23, 22);
            this.ZoomOutButton.TabIndex = 16;
            this.ZoomOutButton.Text = "-";
            this.ZoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
            // 
            // ZoomInButton
            // 
            this.ZoomInButton.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ZoomInButton.Location = new System.Drawing.Point(213, 229);
            this.ZoomInButton.Name = "ZoomInButton";
            this.ZoomInButton.Size = new System.Drawing.Size(23, 22);
            this.ZoomInButton.TabIndex = 15;
            this.ZoomInButton.Text = "+";
            this.ZoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
            // 
            // ZoomLabel
            // 
            this.ZoomLabel.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold);
            this.ZoomLabel.Location = new System.Drawing.Point(195, 215);
            this.ZoomLabel.Name = "ZoomLabel";
            this.ZoomLabel.Size = new System.Drawing.Size(43, 16);
            this.ZoomLabel.Text = "Zoom";
            // 
            // lastReadingLabel
            // 
            this.lastReadingLabel.Location = new System.Drawing.Point(109, 21);
            this.lastReadingLabel.Name = "lastReadingLabel";
            this.lastReadingLabel.Size = new System.Drawing.Size(128, 18);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.Text = "Last reading: ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // toNowButton
            // 
            this.toNowButton.Location = new System.Drawing.Point(56, 246);
            this.toNowButton.Name = "toNowButton";
            this.toNowButton.Size = new System.Drawing.Size(92, 22);
            this.toNowButton.TabIndex = 9;
            this.toNowButton.Text = "Back To Now";
            this.toNowButton.Click += new System.EventHandler(this.toNowButton_Click);
            // 
            // forwardButton
            // 
            this.forwardButton.Location = new System.Drawing.Point(99, 220);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(80, 22);
            this.forwardButton.TabIndex = 5;
            this.forwardButton.Text = "See Later >";
            this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(7, 220);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(86, 22);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "< See Earlier";
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // graphBox
            // 
            this.graphBox.BackColor = System.Drawing.SystemColors.Window;
            this.graphBox.FixedY = true;
            this.graphBox.Location = new System.Drawing.Point(0, 39);
            this.graphBox.MaxY = 15;
            this.graphBox.Mean = 0;
            this.graphBox.MinY = 0;
            this.graphBox.Name = "graphBox";
            this.graphBox.ShowMeanLine = false;
            this.graphBox.ShowNegitiveYLables = false;
            this.graphBox.Size = new System.Drawing.Size(240, 159);
            this.graphBox.TabIndex = 11;
            this.graphBox.XAxisMax = new System.DateTime(2007, 7, 21, 22, 27, 24, 169);
            this.graphBox.xMarkIncrement = 5;
            this.graphBox.XWidth = 30;
            this.graphBox.yMarkIncrement = 2;
            this.graphBox.InfoPointSelected += new mcp.Graphs.InfoPointSelectedEventHandler(this.graphBox_InfoPointSelected);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(66, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.Text = "Time of Readings";
            // 
            // GraphPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.panel1);
            this.Name = "GraphPage";
            this.Size = new System.Drawing.Size(240, 271);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private Panel panel1;
        private DateTimeInfoGraphBox graphBox;
        private Label label1;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Button backButton;
        private Button toNowButton;
        private Label label2;
        private Label lastReadingLabel;
        private Button ZoomOutButton;
        private Button ZoomInButton;
        private Label ZoomLabel;
    }
}
