using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SensorShare
{
   public partial class AnnotationPanel : UserControl
   {
      public AnnotationPanel(IAnnotation annotationBase)
      {
         InitializeComponent();
         annotationID = annotationBase.ID;
         timeLabel.Text = String.Format("{0} on {1}",
            annotationBase.Date.ToLocalTime().ToShortTimeString(),
            annotationBase.Date.ToLocalTime().ToLongDateString());
         this.textData.Location = new System.Drawing.Point(3, 23);
         this.textData.Name = "textData";
         this.textData.Size = new System.Drawing.Size(234, 245);
         this.textData.Text = String.Format("The {0} message from {1} isn't available yet, please wait...", annotationBase.Type, annotationBase.Author);
         AddIfNotContains(this.textData);
         updateAnotationDelegate = new IAnnotationDelegate(DoUpdateAnnotation);
      }

      private Guid annotationID = Guid.Empty;
      public IAnnotation Annotation
      {
         set
         {
            annotationID = value.ID;
            UpdateAnnotation(value);
         }
      }

      public delegate void IAnnotationDelegate(IAnnotation annotation);
      
      IAnnotationDelegate updateAnotationDelegate;
      private void UpdateAnnotation(IAnnotation annotation)
      {
         if (this.InvokeRequired)
         {
            this.Invoke(updateAnotationDelegate, annotation);
         }
         else
         {
            DoUpdateAnnotation(annotation);
         }
      }

      private Label textData;
      private Label answerData;
      private PictureBox pictureBox;
      int imageWidth = 240;
      int imageHeight = 150;
      private void DoUpdateAnnotation(IAnnotation annotation)
      {
         timeLabel.Text = String.Format("{0} on {1}", 
            annotation.Date.ToLocalTime().ToShortTimeString(),
            annotation.Date.ToLocalTime().ToLongDateString());
         switch (annotation.Type)
         {
            case AnnotationType.Text:
               TextAnnotation textAnnotation = (TextAnnotation)annotation;
               
               this.textData.Location = new System.Drawing.Point(3, 23);
               this.textData.Name = "textData";
               this.textData.Size = new System.Drawing.Size(234, 245);
               this.textData.Text = String.Format("{0}: {1}", textAnnotation.Author, textAnnotation.Text);
               AddIfNotContains(this.textData);
               RemoveIfContains(answerData);
               RemoveIfContains(pictureBox);
               break;

            case AnnotationType.Image:
               ImageAnnotation imageAnnotation = (ImageAnnotation) annotation;
               this.pictureBox.Location = new System.Drawing.Point(0, 31);
               this.pictureBox.Name = "pictureBox";
               this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
               using (MemoryStream ms = new MemoryStream())
               {
                  ms.Write(imageAnnotation.ImageData, 0, imageAnnotation.ImageData.Length);
                  ms.Seek(0, SeekOrigin.Begin);
                  Bitmap image = new Bitmap(ms);
                  Double WidthToHeightAspect = (Double) image.Width / (Double)image.Height;
                  Double xFactor = (Double)image.Width / (Double) imageWidth;
                  Double yFactor = (Double)image.Height / (Double) imageHeight;
                  if (xFactor < yFactor) 
                  {
                     this.pictureBox.Size = new System.Drawing.Size(imageWidth, (int) Math.Round(imageWidth / WidthToHeightAspect));
                  }
                  else
                  {
                     this.pictureBox.Size = new System.Drawing.Size((int)Math.Round(imageHeight * WidthToHeightAspect), imageHeight);
                  }
                  Debug.WriteLine(this.pictureBox.Size);
                  this.pictureBox.Image = image;
               }
               AddIfNotContains(pictureBox);

               this.textData.Location = new System.Drawing.Point(3, this.pictureBox.Location.Y + this.pictureBox.Size.Height+2);
               this.textData.Name = "textData";
               this.textData.Size = new System.Drawing.Size(imageWidth, Math.Max(100, this.Location.Y - 274));
               this.textData.Text = String.Format("{0}: {1}", imageAnnotation.Author, imageAnnotation.Comment);
               AddIfNotContains(this.textData); 
               break;

            case AnnotationType.QuestionAndAnswer:
               QuestionAndAnswerAnnotation qaAnnotation = (QuestionAndAnswerAnnotation)annotation;
               this.textData.Location = new System.Drawing.Point(3, 23);
               this.textData.Name = "textData";
               this.textData.Size = new System.Drawing.Size(234, 110);
               this.textData.Text = String.Format("{0} said: {1}", qaAnnotation.Question.Author, qaAnnotation.Question.Text);
               AddIfNotContains(this.textData);
               this.answerData.Location = new System.Drawing.Point(3, textData.Location.Y + textData.Size.Height + 4);
               this.answerData.Name = "answerData";
               this.answerData.Size = new System.Drawing.Size(234, 110);
               this.answerData.Text = String.Format("{0} replied: {1}", qaAnnotation.Author, qaAnnotation.AnswerText);
               AddIfNotContains(this.answerData);
               RemoveIfContains(pictureBox);
               break;
            default:
               RemoveIfContains(textData);
               RemoveIfContains(pictureBox);
               RemoveIfContains(answerData);
               break;
         }
      }

      private void RemoveIfContains(Control o)
      {
         if (this.Controls.Contains(o))
         {
            this.Controls.Remove(o);
         }
      }

      private void AddIfNotContains(Control o)
      {
         if (!this.Controls.Contains(o))
         {
            this.Controls.Add(o);
         }
      }
   }
}

