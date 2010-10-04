
namespace SensorShare
{
   public static class Annotation
   {
      public static IAnnotation FromBytes(byte[] bytes)
      {
         AnnotationBase aBase = new AnnotationBase(bytes);
         switch (aBase.Type)
         {
            case AnnotationType.Base:
               return aBase;
               break;
            case AnnotationType.Image:
               return new ImageAnnotation(bytes);
               break;
            case AnnotationType.QuestionAndAnswer:
               return new QuestionAndAnswerAnnotation(bytes);
               break;
            case AnnotationType.Text:
               return new TextAnnotation(bytes);
               break;
            default:
               return aBase;
               break;
         }
      }
   }
}
