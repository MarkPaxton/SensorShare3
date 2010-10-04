using System.Text;

namespace SensorShare
{
   public static class SensorShareConfig
   {
      public readonly static int CommunicationPort = 49990;
      public readonly static int AliveTimeout = 5000;

      public readonly static string WiFiName = "SensorShare";
      public readonly static string WiFiKey = "1234567890";

      // Encode/Decode text for sending/receiving
      public readonly static Encoding TextEncoding = Encoding.UTF8;

      //Databases
      public readonly static string DatabaseFolder = "\\My Documents\\Sensor Data";
      public readonly static string ServerDatabase = "SensorShareServer.db";
      public readonly static string ClientDatabase = "SensorShareClient.db";
      public readonly static string SensorsDatabase = "sensors.db";

      // Option settings
      public readonly static string ConfigKeyName = "Software\\SensorShare\\Server";

      public readonly static string SensorInfoTableName = "Sensors";

      public readonly static string SessionDataTableName = "sessions";
      public readonly static string SensorReadingsTableName = "readings";
      public readonly static string QuestionDataTableName = "questions";
      public readonly static string AnswerDataTableName = "answers";
      public readonly static string AnnotationTableName = "annotations";

      public readonly static string ClientReadingsTableName = "readings";
      public readonly static string ClientServerDataTableName = "servers";
      public readonly static string ClientPictureFolder = "\\My Documents\\My Pictures\\SensorShare Images";
      public readonly static string ServerPictureFolder = "\\My Documents\\My Pictures\\SensorShare Images";

      //public readonly static string TextAnnotationImage = "textAnnotation.jpg";
      //public readonly static string BaseAnnotationImage = "baseAnnotation.jpg";
      //public readonly static string QuestionAndAnswerAnnotationImage = "qaAnnotation.jpg";

      public readonly static string TextAnnotationImage = "\\comment.png";
      public readonly static string BaseAnnotationImage = "\\comment.png";
      public readonly static string QuestionAndAnswerAnnotationImage = "\\comment.png";

      public readonly static string ImageAnnotationIcon = "\\CameraIcon.png";
      public readonly static string TextAnnotationIcon = "\\infoIcon.png";
      public readonly static string BaseAnnotationIcon = "\\infoIcon.png";
      public readonly static string QuestionAndAnswerAnnotationIcon = "\\questionIcon.png";

   
   }
}
