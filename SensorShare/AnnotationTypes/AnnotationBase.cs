using System;
using System.IO;

namespace SensorShare
{
   /// <summary>
   /// The base class for annotations, providing the ID and Time
   /// </summary>
   public class AnnotationBase : IAnnotation
   {
      private Guid id;
      public Guid ID
      {
         get { return id; }
         set { id = value; }
      }

      private Guid author_id;
      public Guid AuthorID
      {
         get { return author_id; }
         set { author_id = value; }
      }

      private Guid server_id;
      public Guid ServerID
      {
         get { return server_id; }
         set { server_id = value; }
      }
      public DateTime Date
      {
         get { return DateTime.FromFileTimeUtc(BinaryDate); }
         set { BinaryDate = value.ToFileTimeUtc(); }
      }
      
      private long binaryDate;
      public long BinaryDate
      {
         get { return binaryDate; }
         set { binaryDate = value; }
      }
      protected AnnotationType type;
      public virtual AnnotationType Type
      {
         get { return type; }
      }

      protected string author;
      public string Author
      {
         get { return author; }
         set { author = value; }
      }

      public AnnotationBase(Guid id, Guid server_id, Guid author_id, DateTime time, string author)
         :this(id, server_id, author_id, time, author, AnnotationType.Base)
      { }

      public AnnotationBase(Guid id, Guid server_id, Guid author_id, DateTime time, string author, AnnotationType type)
      {
         this.ID = id;
         this.server_id = server_id;
         this.author_id = author_id;
         this.Date = time;
         this.Author = author;
         this.type = type;
      }

      public AnnotationBase(byte[] data)
      {
         using (MemoryStream ms = new MemoryStream(data, false))
         {
            this.ReadBaseData(ms);
         }
      }

      public AnnotationBase()
      {
         
      }


      /// <summary>
      /// Generate a MemoryStream containing the base into which to add the data for the rest of the annotation.
      /// </summary>
      /// <remarks>The MemoryStream is a new stream and therefore must be closed after use.</remarks>
      /// <returns>MemoryStream with int32:ID, Int64:Time and int32:Type</returns>
      protected MemoryStream WriteBaseData()
      {
         MemoryStream ms = new MemoryStream();
         // ID
         byte[] tempBytes = ID.ToByteArray();
         ms.Write(tempBytes, 0, 16);
         // ServerID
         tempBytes = server_id.ToByteArray();
         ms.Write(tempBytes, 0, 16);
         //AuthorID
         tempBytes = AuthorID.ToByteArray();
         ms.Write(tempBytes, 0, 16);
         // Date
         tempBytes = BitConverter.GetBytes(this.BinaryDate);
         ms.Write(tempBytes, 0, sizeof(Int64));
         // Type
         tempBytes = BitConverter.GetBytes((Int32)this.Type);
         ms.Write(tempBytes, 0, sizeof(AnnotationType));
         // Author
         TextHelper.StreamString(ms, this.Author);
         return ms;
      }

      /// <summary>
      /// Returns the annotation data as an array of bytes
      /// </summary>
      public virtual byte[] GetBytes()
      {
         MemoryStream ms = WriteBaseData();
         byte[] data = new byte[ms.Length];
         data = ms.ToArray();
         ms.Close();
         return data;
      }

      /// <summary>
      /// Read the data from a given MemoryStream and return that steam with the pointer at the end of the data
      /// </summary>
      /// <remarks>Will Seek the start of the MemoryStream given and leave it at the end of the bytes loaded.  Exceptions must be caught upstream.</remarks>
      /// <returns>A MemoryStream with the pointer set at the end of the data read.</returns>
      protected MemoryStream ReadBaseData(MemoryStream ms)
      {
         ms.Seek(0, SeekOrigin.Begin);
         // ID
         byte[] readData = new byte[16];
         ms.Read(readData, 0, 16);
         this.ID = new Guid(readData);
         // ServerID
         readData = new byte[16];
         ms.Read(readData, 0, 16);
         this.server_id = new Guid(readData);
         // AuthorID
         readData = new byte[16];
         ms.Read(readData, 0, 16);
         this.AuthorID = new Guid(readData);
         // Date
         readData = new byte[sizeof(Int64)];
         ms.Read(readData, 0, sizeof(Int64));
         this.BinaryDate = BitConverter.ToInt64(readData, 0);
         // Type
         readData = new byte[sizeof(Int32)];
         ms.Read(readData, 0, sizeof(Int32));
         this.type = (AnnotationType)BitConverter.ToInt32(readData, 0);

         //Author
         this.Author = TextHelper.DeStreamString(ms);

         return ms;
      }


   }
}
