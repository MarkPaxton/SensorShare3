using System;
using System.Windows.Forms;

namespace SensorShare.Desktop
{
   public partial class AnnotationView : Panel
   {
      DateTime annotationTime;
      IAnnotation annotation;
      Boolean showData = false;

      public AnnotationView()
      {
         InitializeComponent();
         updateDel = new UpdateDelegate(DoUpdate);
         this.Height = timeLabel.Height;
         this.pictureBox.Width = this.Width;
         this.textLabel.Width = this.Width;
      }

      public DateTime AnnotationTime
      {
         set
         {
            annotationTime = value;
            DoUpdateInvoke();
         }
         get { return annotationTime; }
      }

      public Boolean ShowData
      {
         get { return showData; }
         set
         {
            showData = value;
            DoUpdateInvoke();
         }
      }


      public IAnnotation Annotation
      {
         set
         {
            annotation = value;
            showData = (annotation != null);
            DoUpdateInvoke();
         }
         get { return annotation; }
      }
      protected delegate void UpdateDelegate();
      UpdateDelegate updateDel;

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
         if (annotation == null)
         {
            button1.Visible = false;
         }
         else
         {
            button1.Visible = true;
         }
         if (showData == true)
         {
            button1.Text = "Hide";
            timeLabel.Text = annotation.Date.ToLocalTime().ToLongTimeString();
            this.timeLabel.Visible = true;
            switch (annotation.Type)
            {
               case AnnotationType.Image:

                  ImageAnnotation imagaAnno = (ImageAnnotation)annotation;
                  this.pictureBox.Image = JpegImage.GetThumbnail(JpegImage.LoadFromBytes(imagaAnno.ImageData), pictureBox.Size);
                  this.pictureBox.Visible = true;
                  this.textLabel.Text = imagaAnno.Comment;
                  this.textLabel.Visible = true;
                  break;
               case AnnotationType.Text:
                  TextAnnotation textAnno = (TextAnnotation)annotation;
                  this.textLabel.Text = textAnno.Text;
                  this.textLabel.Visible = true;
                  break;
               case AnnotationType.QuestionAndAnswer:
                  QuestionAndAnswerAnnotation qaAnno = (QuestionAndAnswerAnnotation)annotation;
                  this.textLabel.Text = qaAnno.Question.Text + "\r\n" + qaAnno.AnswerText;
                  this.textLabel.Visible = true;
                  break;
               default:
                  break;
            }
            this.Height = timeLabel.Height + pictureBox.Height + textLabel.Height;
         }
         else
         {
            button1.Text = "Show";
            timeLabel.Text = annotationTime.ToLocalTime().ToLongTimeString();
            this.timeLabel.Visible = true;
            this.pictureBox.Visible = false;
            this.textLabel.Visible = false;
            this.Height = timeLabel.Height;
         }
      }

      private void button1_Click(object sender, EventArgs e)
      {
         showData = !showData;
         DoUpdateInvoke();
      }

      private void AnnotationView_Resize(object sender, EventArgs e)
      {
         this.pictureBox.Width = this.Width;
         this.textLabel.Width = this.Width;
      }

      private void pictureBox_Click(object sender, EventArgs e)
      {
         if (annotation.Type == AnnotationType.Image)
         {
            ImageAnnotation imAnno = (ImageAnnotation)annotation;
            ImageViewForm imageForm = new ImageViewForm(JpegImage.LoadFromBytes(imAnno.ImageData));
            imageForm.ShowDialog();
            imageForm.Dispose();
         }
      }
   }
}
