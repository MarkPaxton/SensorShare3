using System;

namespace SensorShare
{
   public interface IAnnotation
   {
      /// <summary>
      /// The GUID of the annotation
      /// </summary>
      Guid ID
      {
         get;
         set;
      }

      /// <summary>
      /// The GUID of the author
      /// </summary>
      Guid AuthorID
      {
         get;
         set;
      }

      /// <summary>
      /// The UTC DateTime of the annotation
      /// </summary>
      DateTime Date
      {
         get;
         set;
      }

      /// <summary>
      /// UTC Filetime of the annotation
      /// </summary>
      long BinaryDate
      {
         get;
         set;
      }

      /// <summary>
      /// The author of the annotation
      /// </summary>
      string Author
      {
         get;
         set;
      }

      /// <summary>
      /// The annotation type
      /// </summary>
      AnnotationType Type
      {
         get;
      }

      /// <summary>
      /// The id of the server the annotation is attatched to
      /// </summary>
      Guid ServerID
      {
         get;
         set;
      }

      /// <summary>
      /// Convert the annotation into an array of bytes.
      /// </summary>
      /// <returns>A byte[] containing the annotation converted to bytes.</returns>
      byte[] GetBytes();
   }
}
