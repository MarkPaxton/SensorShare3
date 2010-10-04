namespace SensorShare.Compact
{
   partial class PrepareNoteForm
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.serverTitle = new System.Windows.Forms.Label();
         this.timeLabel = new System.Windows.Forms.Label();
         this.sendButton = new System.Windows.Forms.Button();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.cancelButton = new System.Windows.Forms.Button();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // serverTitle
         // 
         this.serverTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.serverTitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
         this.serverTitle.Location = new System.Drawing.Point(0, 0);
         this.serverTitle.Name = "serverTitle";
         this.serverTitle.Size = new System.Drawing.Size(320, 23);
         this.serverTitle.Text = "Server Name";
         // 
         // timeLabel
         // 
         this.timeLabel.Location = new System.Drawing.Point(186, 23);
         this.timeLabel.Name = "timeLabel";
         this.timeLabel.Size = new System.Drawing.Size(131, 16);
         this.timeLabel.Text = "Server time";
         // 
         // sendButton
         // 
         this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sendButton.Location = new System.Drawing.Point(216, 189);
         this.sendButton.Name = "sendButton";
         this.sendButton.Size = new System.Drawing.Size(101, 22);
         this.sendButton.TabIndex = 3;
         this.sendButton.Text = "Send";
         this.sendButton.Click += new System.EventHandler(this.saveButton_Click);
         // 
         // textBox1
         // 
         this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox1.Location = new System.Drawing.Point(0, 56);
         this.textBox1.Multiline = true;
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(320, 100);
         this.textBox1.TabIndex = 1;
         // 
         // cancelButton
         // 
         this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.cancelButton.Location = new System.Drawing.Point(3, 189);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(101, 22);
         this.cancelButton.TabIndex = 2;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
         // 
         // label3
         // 
         this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.label3.Location = new System.Drawing.Point(0, 39);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(320, 14);
         this.label3.Text = "Write your note here:";
         // 
         // label4
         // 
         this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.label4.Location = new System.Drawing.Point(0, 159);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(320, 26);
         this.label4.Text = "Your note will be saved at the time you click \"Send\"";
         // 
         // label5
         // 
         this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.label5.Location = new System.Drawing.Point(3, 23);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(177, 16);
         this.label5.Text = "Current time:";
         this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
         // 
         // PrepareNoteForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(320, 214);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.serverTitle);
         this.Controls.Add(this.timeLabel);
         this.Controls.Add(this.sendButton);
         this.Controls.Add(this.textBox1);
         this.Controls.Add(this.cancelButton);
         this.Controls.Add(this.label3);
         this.Name = "PrepareNoteForm";
         this.Text = "Create A Note";
         this.Closing += new System.ComponentModel.CancelEventHandler(this.PrepareNoteForm_Closing);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label serverTitle;
      private System.Windows.Forms.Label timeLabel;
      private System.Windows.Forms.Button sendButton;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
   }
}