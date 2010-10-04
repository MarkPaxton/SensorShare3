using System;

namespace SensorShare
{
   public class NoteMessage
   {
      public Guid user_id;
      public string Note;
      public string User;
      public long TimeBinary;
      public DateTime Time
      {
         get { return DateTime.FromFileTimeUtc(TimeBinary); }
         set { TimeBinary = value.ToFileTimeUtc(); }
      }

      public NoteMessage()
      {
      }

      /// <param name="QuestionID">The ID of the question answered</param>
      /// <param name="Answer">The answer to the question</param>
      public NoteMessage(Guid user_id, string note, string user, DateTime time)
      {
         this.user_id = user_id;
         this.Note = note;
         this.User = user;
         this.Time = time;
      }
   }
}
