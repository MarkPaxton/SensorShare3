using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ScienceScope
{
   public partial class Logbook
   {
      /// <summary>
      /// Translate special characters from the logbook into windows/ASCII printable versions.
      /// </summary>
      public static string ConvertChar(byte letterAsByte)
      {
         string printChar;
         switch (letterAsByte)
         {
            case 0:
               printChar = "l";
               break;
            case 3:
               printChar = "²";
               break;
            case 4:
               printChar = "³";
               break;
            case 5:
               printChar = "-1";
               break;
            case 6:
               printChar = "-²";
               break;
            case 7:
               printChar = "-³";
               break;
            case 0xDF:
               printChar = "°";
               break;
            case 0xE4:
               printChar = "µ";
               break;
            case 31:
               printChar = " ";
               break;
            default:
               byte[] letterAsByteArray = new byte[1];
               letterAsByteArray[0] = letterAsByte;
               printChar = Encoding.ASCII.GetString(letterAsByteArray, 0, 1);
               break;
         }
         return printChar;
      }

      public static string ConvertChars(string letters)
      {
         string result = "";
         foreach (char letter in letters)
         {
            result += ConvertChar((byte)letter);
         }
         return result;
      }

      /// <summary>
      /// Splits the ASCII string from ReadASCII Command into an string array with an element or each of the 
      ///  four channels.  Will throw exceptions if strings aren't right.
      /// </summary>
      /// <param name="ASCIIData">ASCII string from ReadASCII Command result</param>
      /// <returns>Four element array of strings representing channles 1, 2 3 and 4</returns>
      public static string[] SplitChannels(byte[] ASCIIData)
      {
         string[] result = new string[4] { "", "", "", "" };

         for (int i = 1; i < 9; i++)
         {
            result[0] += ConvertChar(ASCIIData[i]);
         }

         for (int i = 17; i < 25; i++)
         {
            result[1] += ConvertChar(ASCIIData[i]);
         }

         for (int i = 10; i < 17; i++)
         {
            result[2] += ConvertChar(ASCIIData[i]);
         }

         for (int i = 26; i <= 32; i++)
         {
            result[3] += ConvertChar(ASCIIData[i]);
         }
         return result;
      }

      public static Double ReadingToValue(string ASCIIData, SensorDefinition sensorInfo)
      {
         Double returnValue = 0;
         if (ASCIIData != null)
         {
            Regex reg = new Regex("(" + ConvertChars(sensorInfo.ASCIIUnit) + ")");
            string readingValue1 = reg.Replace(ASCIIData, "");

            reg = new Regex("[^0-9\\.-]");
            string readingValue = reg.Replace(readingValue1, "");
            if (readingValue.Length > 0)
            {
               returnValue = Convert.ToDouble(readingValue);
            }
         }
         return returnValue;
      }
   }
}