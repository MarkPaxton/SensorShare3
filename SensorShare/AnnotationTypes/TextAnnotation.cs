using System;
using System.IO;

namespace SensorShare
{
    public class TextAnnotation : AnnotationBase
    {
        /// <summary>
        /// The text message in the annotation
        /// </summary>
        public String Text;

        /// <param name="id">ID of the annotation</param>
        /// <param name="time">Time of annotation</param>
        /// <param name="text">Text of annotation</param>
        public TextAnnotation(Guid id, Guid server_id, Guid author_id, DateTime time, string author,  string text)
            :base(id, server_id, author_id, time, author)
        {
            this.type = this.Type;
            this.Text = text;
        }

        /// <param name="data">A buffer containing the annotation data</param>
        public TextAnnotation(byte[] data)
        {
           using (MemoryStream ms = new MemoryStream(data))
            {
                base.ReadBaseData(ms);
                this.Text = TextHelper.DeStreamString(ms);
            }
        }

        public override AnnotationType Type
        {
            get { return AnnotationType.Text; }

        }

        /// <summary>
        /// Return the Annotation as a byte array
        /// </summary>
        public override byte[] GetBytes()
        {
            byte[] writeBytes;
            using (MemoryStream ms = base.WriteBaseData())
            {
                TextHelper.StreamString(ms, this.Text);
                writeBytes = new byte[ms.Length];
                writeBytes = ms.ToArray();
            }
            return writeBytes;
        }

    }

 }
