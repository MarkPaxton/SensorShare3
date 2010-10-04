namespace SensorShare
{
    public enum AnnotationType : int
    {
        /// <summary>
        /// Just the base data
        /// </summary>
        Base,
        /// <summary>
        /// A simple text annotation
        /// </summary>
        Text,
        /// <summary>
        /// An annotation containing a question and answer
        /// </summary>
        QuestionAndAnswer,
        /// <summary>
        /// An annotation that contains an image and comment
        /// </summary>
        Image,
    }
}