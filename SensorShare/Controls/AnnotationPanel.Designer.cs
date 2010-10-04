using System.Windows.Forms;
namespace SensorShare
{
   partial class AnnotationPanel : UserControl
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
         this.timeLabel = new System.Windows.Forms.Label();
         this.textData = new System.Windows.Forms.Label();
         this.answerData = new System.Windows.Forms.Label();
         this.pictureBox = new System.Windows.Forms.PictureBox();
         this.SuspendLayout();
         // 
         // timeLabel
         // 
         this.timeLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
         this.timeLabel.Location = new System.Drawing.Point(3, 3);
         this.timeLabel.Name = "timeLabel";
         this.timeLabel.Size = new System.Drawing.Size(234, 20);
         this.timeLabel.Text = "Waiting for data...";
         // 
         // AnnotationPanel
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.Controls.Add(this.timeLabel);
         this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.Name = "AnnotationPanel";
         this.Size = new System.Drawing.Size(240, 271);
         this.ResumeLayout(false);

      }

      #endregion

      private Label timeLabel;
   }
}
