using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SensorShare
{
   public class MessageHelper
   {
      private static object deserialiseItem(XmlSerializer serializer, byte[] data)
      {
         object toReturn = null;
         using (MemoryStream ms = new MemoryStream(data, false))
         {
            toReturn = serializer.Deserialize(ms);
         }
         return toReturn;
      }

      #region QuestionMessage
      private static XmlSerializer QuestionMessageSerializer = new XmlSerializer(typeof(QuestionMessage));
      public static void SerializeQuestionMessage(MemoryStream ms, QuestionMessage message)
      {
         QuestionMessageSerializer.Serialize(ms, message);
      }

      public static QuestionMessage DeserializeQuestionMessage(byte[] data)
      {
         return (QuestionMessage)deserialiseItem(QuestionMessageSerializer, data);
      }
      
      #endregion

      #region AnswerMessage
      private static XmlSerializer AnswerMessageSerilaizer = new XmlSerializer(typeof(AnswerMessage));
      
      public static void SerializeAnswerMessage(MemoryStream ms, AnswerMessage message)
      {
         AnswerMessageSerilaizer.Serialize(ms, message);
      }

      public static AnswerMessage DeserializeAnswerMessage(byte[] data)
      {
         return (AnswerMessage)deserialiseItem(AnswerMessageSerilaizer, data);
      }
      #endregion


      #region NoteMessage
      private static XmlSerializer NoteMessageSerilaizer = new XmlSerializer(typeof(NoteMessage));

      public static void SerializeNoteMessage(MemoryStream ms, NoteMessage message)
      {
         NoteMessageSerilaizer.Serialize(ms, message);
      }

      public static NoteMessage DeserializeNoteMessage(byte[] data)
      {
         return (NoteMessage)deserialiseItem(NoteMessageSerilaizer, data);
      }
      #endregion

      #region AnnotationTimes

      private static XmlSerializer AnnotationTimesSerializer = new XmlSerializer(typeof(List<byte[]>));
      
      public static void SerializeAnnotationTimes(MemoryStream ms, List<byte[]> data)
      {
         AnnotationTimesSerializer.Serialize(ms, data);
      }

      public static List<byte[]> DeserializeAnnotationTimes(byte[] data)
      {
         return (List<byte[]>)deserialiseItem(AnnotationTimesSerializer, data);
      }
      
      #endregion

      #region SensorDescriptionsData
      private static XmlSerializer SensorDescriptionsDataSerializer = new XmlSerializer(typeof(SensorDescriptionsData));

      public static void SerializeSensorDescriptionsDataTimes(MemoryStream ms, SensorDescriptionsData data)
      {
         SensorDescriptionsDataSerializer.Serialize(ms, data);
      }

      public static SensorDescriptionsData DeserializeSensorDescriptionsData(byte[] data)
      {
         return (SensorDescriptionsData)deserialiseItem(SensorDescriptionsDataSerializer, data);
      }

      #endregion

      #region ReadingsRequest
      private static XmlSerializer ReadingsRequestSerializer = new XmlSerializer(typeof(ReadingsRequest));

      public static void SerializeReadingsRequest(MemoryStream ms, ReadingsRequest data)
      {
         ReadingsRequestSerializer.Serialize(ms, data);
      }

      public static ReadingsRequest DeserializeReadingsRequest(byte[] data)
      {
         return (ReadingsRequest)deserialiseItem(ReadingsRequestSerializer, data);
      }

      #endregion

      #region SensorReadingsData

      private static XmlSerializer SensorReadingsDataSerializer = new XmlSerializer(typeof(SensorReadingsData));

      public static void SerializeSensorReadingsData(MemoryStream ms, SensorReadingsData data)
      {
         SensorReadingsDataSerializer.Serialize(ms, data);
      }

      public static SensorReadingsData DeserializeSensorReadingsData(byte[] data)
      {
         return (SensorReadingsData)deserialiseItem(SensorReadingsDataSerializer, data);
      }

      #endregion

      #region SensorReadingsDataList

      private static XmlSerializer SensorReadingsDataListSerializer = new XmlSerializer(typeof(List<SensorReadingsData>));

      public static void SerializeSensorReadingsDataList(MemoryStream ms, List<SensorReadingsData> data)
      {
         SensorReadingsDataListSerializer.Serialize(ms, data);
      }

      public static List<SensorReadingsData> DeserializeSensorReadingsDataList(byte[] data)
      {
         return (List<SensorReadingsData>)deserialiseItem(SensorReadingsDataListSerializer, data);
      }

      #endregion


   }
}
