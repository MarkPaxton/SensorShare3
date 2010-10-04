using System;
using System.Collections.Generic;
using System.Diagnostics;
using ScienceScope;

namespace SensorShare
{
   public static class QuestionHelper
   {
      private static void calculateDifferences(double[] overall_stats, double[] previous_stats, double[] current_stats, ref double[] actual_difference, ref double[] percent_difference)
      {
         if ((overall_stats.Length != previous_stats.Length ||
             (overall_stats.Length != current_stats.Length) ||
             (overall_stats.Length != actual_difference.Length) ||
             (overall_stats.Length != percent_difference.Length)))
         {
            throw new ArgumentException("All parameter arrays passed must be the same length");
         }
         if (overall_stats.Length < 12)
         {
            throw new ArgumentException("All parameter arrays must be at least 12 elements");
         }

         // Work out change from maximum
         int[] maxIndex = new int[] { 0, 3, 6, 9 };
         foreach (int i in maxIndex)
         {
            actual_difference[i] = current_stats[i] - previous_stats[i];
            if (actual_difference[i] != 0)
            {
               percent_difference[i] = actual_difference[i] / Math.Abs(previous_stats[i]);
            }
            else
            {
               // If the overall value was 0 then it would be an infinite %
               // That's silly so just ignore it for now
               //  - if it's set to 0 it won't be seen as any change so not interesting
               percent_difference[i] = 0;
            }
         }

         // Work out if minumum has been set
         int[] minsIndex = new int[] { 1, 4, 7, 10 };
         foreach (int i in minsIndex)
         {
            actual_difference[i] = previous_stats[i] - current_stats[i];
            if (actual_difference[i] != 0)
            {
               percent_difference[i] = actual_difference[i] / Math.Abs(previous_stats[i]);
            }
            else
            {
               // If the overall value was 0 then it would be an infinite %
               // That's silly so just ignore it for now
               //  - if it's set to 0 it won't be seen as any change so not interesting
               percent_difference[i] = 0;
            }
         }

         int[] meansIndex = new int[] { 2, 5, 8, 11 };
         foreach (int i in meansIndex)
         {
            actual_difference[i] = current_stats[i] - overall_stats[i];

            // Work out means differently
            if (overall_stats[i] != 0)
            {
               // Work out % increase over mean or % fall from mean
               if (actual_difference[i] > 0)
               {
                  percent_difference[i] = actual_difference[i] / overall_stats[i];
                  //databaseLog.Append(2, String.Format("% Differences[{0}]: {1} / {2} = {3}",
                  //    i, differences[i], previous_stats[i], differencesInPercent[i]));
               }
               else
               {
                  percent_difference[i] = -1 * (actual_difference[i] / overall_stats[i]);
                  //databaseLog.Append(2, String.Format("% Differences[{0}]: 1 - ({1} / {2}) = {3}",
                  //    i, differences[i], overallStats[i], differencesInPercent[i]));
               }
            }
            else
            {
               // If the overall value was 0 then it would be an infinite %
               // That's hard to deal with so just ignore it for now
               //  - if it's set to 0 it won't be seen as any change so not interesting
               percent_difference[i] = 0;
            }
         }
      }

      public static QuestionMessage CreateStatsCompareQuestion(
          double[] overall_stats, double[] current_stats, double[] previous_stats,
          double[] std_devs, SensorDefinition[] sensorDefinitions)
      {
         int readings_count = Convert.ToInt32(overall_stats[12]);

         // If no readings were taken, create a question about this as a priority
         if (readings_count == 0)
         {
            return null;
         }
         else
         {
            double[] overall_means = new double[] { overall_stats[2], overall_stats[5], overall_stats[8], overall_stats[11] };
            double[] current_means = new double[] { current_stats[2], current_stats[5], current_stats[8], current_stats[11] };

            double[] significances = new double[4];
            for (int stat_count = 0; stat_count < sensorDefinitions.Length; stat_count++)
            {
               significances[stat_count] = Stats.Significance(overall_means[stat_count], std_devs[stat_count], current_means[stat_count]);
               Debug.WriteLine(String.Format("Mean[{0}]: {1} :SD[{0}]: {2}", stat_count, overall_means[stat_count], std_devs[stat_count]));
               Debug.WriteLine(String.Format("Reading[{0}]: {1}, Significance[{0}]: {2}", stat_count, current_means[stat_count], significances[stat_count]));
            }
            // Work out differences between all time previous and last 5 minutes
            // Work out real and % difference
            double[] actual_differences = new double[13];
            double[] percent_differences = new double[13];

            QuestionHelper.calculateDifferences(overall_stats, previous_stats, current_stats, ref actual_differences, ref percent_differences);
#if DEBUG
                for (int count = 0; count < 12; count++)
                {
                    Debug.WriteLine(String.Format("[{0}]: Prev:{1:f2}, Curr:{2:f2}, Diff:{3:f2}, {4:p1}", count,
                         previous_stats[count], current_stats[count], actual_differences[count], percent_differences[count]));
                }
#endif
            QuestionMessage qMessage = new QuestionMessage();
            bool questionChosen = false;

            if (!questionChosen)
            {
               // Find which parameter differs most
               // use largest positive or negitive difference in mean
               // but only reductions or increases in min and max respectively
               // the differences will be normalised by looking at the % increase
               // also only look at 1st 3 sensors from here
               int selectedItemIndex = 0;
               double percentChangeAmount = percent_differences[0];
               double differenceAmount = actual_differences[0];
               for (int count = 1; count < 9; count++)
               {
                  if (percent_differences[count] > percentChangeAmount)
                  {
                     //databaseLog.Append(3, String.Format("[{0}] {1} > {2}", count, differencesInPercent[count], percentChangeAmount));
                     percentChangeAmount = percent_differences[count];
                     selectedItemIndex = count;
                  }
               }

               differenceAmount = actual_differences[selectedItemIndex];

               Debug.WriteLine(String.Format("Parameter {0} is chosen, change is {1:p2}, {2:f2} from {3:f2} to {4:f2}",
                   selectedItemIndex, percentChangeAmount, differenceAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex]));

               // Message templates
               // {0} = Reading type
               // {1} = Percent change
               // {2} = Previous
               // {3} = Current
               // {4} = Unit
               string newMaxMessage = "In the last few minutes the {0} reading reached a new high! It was {2:f2}{4}, {1:p1} higher than before.  "
                   + "Can you explain this?";
               string newMinMessage = "The lowest {0} reading has recently been set.  It has fallen {1:p1} from {2:f2}{4} to {3:f2}{4}.  "
                   + "Can you describe what you're currently doing or what is happening around you at the moment?";
               string meanHigherMessage = "The current {0} reading is {1:p1} above average, at {3:f1}{4}.  "
                   + "Do you know what could be causing this?";
               string meanLowerMessage = "The {0} reading is {1:p1} lower than usual, "
                   + "could you describe anything that might have caused this?";
               string meanMessage;
               if (differenceAmount > 0)
               {
                  meanMessage = meanHigherMessage;
               }
               else
               {
                  meanMessage = meanLowerMessage;
               }

               // Now work out what to do with the greatest difference
               switch (selectedItemIndex)
               {
                  case 0:
                     // First sensor new max
                     qMessage.Text = String.Format(newMaxMessage, sensorDefinitions[0].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[0].Unit);
                     break;
                  case 1:
                     // First sensor new min
                     qMessage.Text = String.Format(newMinMessage, sensorDefinitions[0].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[0].Unit);
                     break;
                  case 2:
                     // Fist sensor mean change
                     qMessage.Text = String.Format(meanMessage, sensorDefinitions[0].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[0].Unit);
                     break;
                  case 3:
                     // second sensor new max
                     qMessage.Text = String.Format(newMaxMessage, sensorDefinitions[1].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[1].Unit);
                     break;
                  case 4:
                     // second sensor new min
                     qMessage.Text = String.Format(newMinMessage, sensorDefinitions[1].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[1].Unit);
                     break;
                  case 5:
                     // second sensor mean change
                     qMessage.Text = String.Format(meanMessage, sensorDefinitions[1].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[1].Unit);
                     break;
                  case 6:
                     // third sensor new max
                     qMessage.Text = String.Format(newMaxMessage, sensorDefinitions[2].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[2].Unit);
                     break;
                  case 7:
                     // third sensor new min
                     qMessage.Text = String.Format(newMinMessage, sensorDefinitions[2].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[2].Unit);
                     break;
                  case 8:
                     //third sensor mean change
                     qMessage.Text = String.Format(meanMessage, sensorDefinitions[2].Name,
                         percentChangeAmount, previous_stats[selectedItemIndex], current_stats[selectedItemIndex],
                         sensorDefinitions[2].Unit);
                     break;
                  default:
                     break;
               }
               questionChosen = true;
            }
            return qMessage;
         }
      }

      public static QuestionMessage CreateGeneralQuestion()
      {
         QuestionMessage qMessage = new QuestionMessage();

         qMessage.Text = "Help us record the area, tell us something you can happening see nearby...";
         return qMessage;
      }

      private const double significance_threshold = 0.97;

      public static QuestionMessage CreateCurrentReadingQuestion(double[] overall_stats, SensorReadingsData data, double[] significances, SensorDefinition[] sensorDefinitions)
      {
         double[] value = new double[4] { data.Reading1, data.Reading2, data.Reading3, data.Reading4 };

         if (value.Length != significances.Length)
         {
            throw new ArgumentException("Must provide an equal amount of reading values and significance values");
         }
         if (value.Length != sensorDefinitions.Length)
         {
            throw new ArgumentException("Must provide an equal amount of reading values and sensor descriptions");
         }
         if (overall_stats.Length < 11)
         {
            throw new ArgumentException("Overall stats needs to be for at least 12 elements", "overall_stats");
         }
         double[] overall_means = new double[] { overall_stats[2], overall_stats[5], overall_stats[8], overall_stats[11] };

         // Find out how many of the readings are significant
         List<int> significant_reading_indexes = new List<int>();
         for (int i = 0; i < value.Length; i++)
         {
            if (significances[i] > significance_threshold)
            {
               significant_reading_indexes.Add(i);
            }
         }
         // If some readings are significant, randomly choose one to ask about
         if (significant_reading_indexes.Count > 0)
         {
            Random rand = new Random();
            int chosen_index = Convert.ToInt32(Math.Floor(rand.Next(significant_reading_indexes.Count)));
            int chosen_reading_index = significant_reading_indexes[chosen_index];
            double chosen_value = value[chosen_reading_index];
            // A reading and sensor have been selected, socreate the question
            QuestionMessage qMessage = new QuestionMessage();
            string message_string;
            // Now one is chosen, work out whether it's high or low

            // Message substitution index
            // 0: Reading time
            // 1: Sensor Name
            // 2: Reading value
            // 3: Unit

            if (overall_means[chosen_reading_index] < chosen_value)
            {
               message_string = "We think the {1} is high at the moment... what do you think?  What might have caused this?";
            }
            else
            {
               message_string = "Low {1} readings have been detected, has anything happend that might explain this?";
            }
            qMessage.Text = String.Format(message_string,
                data.Time.ToLocalTime().ToShortTimeString(),
                sensorDefinitions[chosen_reading_index].Name.ToLower(),
                value[chosen_reading_index],
                sensorDefinitions[chosen_reading_index].Unit);
            return qMessage;
         }
         else
         {
            return null;
         }
      }
   }
}
