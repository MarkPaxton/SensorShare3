using System;
using System.IO;

namespace SensorShare
{
    public class ImageAnnotation:AnnotationBase
    {
        public byte[] ImageData;
        public string Comment;

       public ImageAnnotation(Guid id, Guid server_id, Guid author_id, DateTime time, string author, string comment, byte[] imageData)
            :base(id, server_id, author_id, time, author)
        {
            this.type = this.Type;
            this.Comment = comment;
            this.ImageData = imageData;
        }

        public ImageAnnotation(byte[] data)
        {
            using(MemoryStream ms = new MemoryStream(data))
            {
                base.ReadBaseData(ms);
                this.Comment = TextHelper.DeStreamString(ms);
                byte[] imageLengthBytes = new byte[sizeof(Int32)];
                ms.Read(imageLengthBytes, 0, sizeof(Int32));
                int imageLength = BitConverter.ToInt32(imageLengthBytes, 0);
                ImageData = new byte[imageLength];
                ms.Read(ImageData, 0, imageLength);
            }
        }

        public override AnnotationType Type
        {
            get { return AnnotationType.Image; }
        }

        public override byte[] GetBytes()
        {
            byte[] writeBytes;
            using (MemoryStream ms = base.WriteBaseData())
            {
                TextHelper.StreamString(ms, this.Comment);
                writeBytes = BitConverter.GetBytes(ImageData.Length);
                ms.Write(writeBytes, 0, sizeof(Int32));
                ms.Write(ImageData, 0, ImageData.Length);
                writeBytes = new byte[ms.Length];
                writeBytes = ms.ToArray();
            }
            return writeBytes;
        }
    }
}
