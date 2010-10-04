namespace SensorShare
{
    public enum MessageType : int
    { 
       /// <summary>
       /// Send data to announce server is on line
       /// </summary>
       AliveMessage,
       /// <summary>
       /// Request to a server to send it's details, usually broadcast when a client is looking for servers
       /// </summary>
       DescriptionRequest,
       /// <summary>
       /// Description of the server
       /// </summary>
       DescriptionMessage,
       /// <summary>
       /// Send when a client or server is shutting down
       /// </summary>
       ShutdownMessage,
        /// <summary>
        /// The message is in plain text
        /// </summary>
        TextMessage,
        /// <summary>
        /// Request the archive of readings from the server
        /// </summary>
        BlockReadingsRequest,
        /// <summary>
        /// All the readings stored by the server
        /// </summary>
        ReadingsDataList,
        /// <summary>
        /// The latest reading from the server
        /// </summary>
        SensorReadingsData,
        /// <summary>
        /// Request the description of the server
        /// </summary>
        QuestionMessage,
        /// <summary>
        /// The anwer to a question asked by the server
        /// </summary>
        AnswerMessage,
        /// <summary>
        /// Requests a new question from the server
        /// </summary>
        QuestionRequest,
        /// <summary>
        /// Conataints a picture from a user
        /// </summary>
        PictureMessage,
        /// <summary>
        /// Conataints a request for a particular annotation
        /// </summary>
        AnnotationRequest,
        /// <summary>
        /// Conataints an annotation time list
        /// </summary>
        AnnotationTimes,
        /// <summary>
        /// Conataints an annotation
        /// </summary>
        AnnotationData,
       /// <summary>
       /// Contains a text note
       /// </summary>
       NoteMessage,
    }
}