using System;

namespace SensorShare
{
   /// <summary>
   /// A message containing a question from a sensor server
   /// </summary>
   public class QuestionMessage
   {
      /// <summary>
      /// The id of the question
      /// </summary>
      public Guid id=Guid.Empty;        /// Unique ID allocated by the server to identify the question
      public long TimeBinary;
      /// <summary>
      /// The Autor name of the question
      /// </summary>
      public string Author;
      /// <summary>
      /// The message of the question
      /// </summary>
      public string Text;
      public Guid Server;        /// The text of the question

      public DateTime Time
      {
         get { return DateTime.FromFileTimeUtc(TimeBinary); }
         set { TimeBinary = value.ToFileTimeUtc(); }
      }

      public QuestionMessage()
      {
      }

      public QuestionMessage(Guid id, string author, string text, Guid server)
      {
         this.id = id;
         this.Author = author;
         this.Text = text;
         this.Server = server;
      }
   }
}
