using System;

namespace SensorShare
{
   public class AnswerMessage
   {
      public Guid question_id;
      public Guid user_id;
      public string Answer;
      public string User;

      public AnswerMessage()
      {
      }

      /// <param name="QuestionID">The ID of the question answered</param>
      /// <param name="Answer">The answer to the question</param>
      public AnswerMessage(Guid questionID, Guid user_id, string answer, string user)
      {
         this.question_id = questionID;
         this.user_id = user_id;
         this.Answer = answer;
         this.User = user;
      }
   }
}
