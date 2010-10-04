using System;
using System.IO;

namespace SensorShare
{
   public class QuestionAndAnswerAnnotation : AnnotationBase
   {
      /// <summary>
      /// The text of the answer
      /// </summary>
      public string AnswerText;
      

      public QuestionMessage Question;

      /// <param name="id">ID of the annotation</param>
      /// <param name="time">Time of annotation</param>
      /// <param name="text">Text of annotation</param>
      public QuestionAndAnswerAnnotation(QuestionMessage question, Guid id, Guid author_id, string author, string answerText)
         : base(id, question.Server, author_id, question.Time, author)
      {
         this.type = this.Type;
         this.AnswerText = answerText;
         this.Question = question;
      }

      public QuestionAndAnswerAnnotation(byte[] data)
      {
         using (MemoryStream ms = new MemoryStream(data, false))
         {
            base.ReadBaseData(ms);
            this.AnswerText = TextHelper.DeStreamString(ms);
            int remainingLength = (int)(ms.Length - ms.Position);
            byte[] questionBytes = new byte[remainingLength];
            ms.Read(questionBytes, 0, remainingLength);
            this.Question = MessageHelper.DeserializeQuestionMessage(questionBytes);
         }
      }

      /// <summary>
      /// QuestionAndAnswerAnnotation
      /// </summary>
      public override AnnotationType Type
      {
         get { return AnnotationType.QuestionAndAnswer; }
      }

      /// <summary>
      /// Return the Annotation as a byte array
      /// </summary>
      public override byte[] GetBytes()
      {
         byte[] writeBytes;
         using (MemoryStream ms = base.WriteBaseData())
         {
            TextHelper.StreamString(ms, AnswerText);
            MessageHelper.SerializeQuestionMessage(ms, Question);
            writeBytes = new byte[ms.Length];
            writeBytes = ms.ToArray();
         }
         return writeBytes;
      }
   }
}
