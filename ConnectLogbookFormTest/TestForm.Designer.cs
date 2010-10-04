namespace ConnectLogbookFormTest
{
   partial class TestForm
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
         this.button1 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(35, 23);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(146, 28);
         this.button1.TabIndex = 0;
         this.button1.Text = "Test it";
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(102, 250);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(116, 32);
         this.button2.TabIndex = 1;
         this.button2.Text = "Exit";
         this.button2.Click += new System.EventHandler(this.button2_Click);
         // 
         // TestForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(240, 294);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.button1);
         this.Name = "TestForm";
         this.Text = "Test Form";
         this.Load += new System.EventHandler(this.TestForm_Load);
         this.Closed += new System.EventHandler(this.TestForm_Closed);
         this.Closing += new System.ComponentModel.CancelEventHandler(this.TestForm_Closing);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
   }
}

