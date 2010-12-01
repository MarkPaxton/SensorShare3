using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using ScienceScope;
using mcp;

namespace SensorShare
{
   /// <summary>
   /// Helper class to consolidate database access functions into one location
   /// </summary>
   public static class DatabaseHelper
   {
      #region ServerConfigs

      public static bool GetServerSaved(SQLiteConnection connection, Guid serverID)
      {
         bool toReturn = false;
         lock (connection)
         {
            if (connection.State != ConnectionState.Open)
            {
               connection.Open();
            }

            SQLiteCommand command = connection.CreateCommand();
            string commandText = String.Format("SELECT COUNT() FROM [servers] WHERE [server_id]='{0}';", serverID.ToString());
            Debug.WriteLine(commandText);
            command.CommandText = commandText;
            object result = command.ExecuteScalar();
            long intResult = (long)result;
            if (intResult > 0)
            {
               toReturn = true;
            }
            connection.Close();
         }
         return toReturn;
      }

      public static ServerData GetServerByID(SQLiteConnection connection, Guid serverID)
      {
         ServerData toReturn = null;
         lock (connection)
         {
            if (connection.State != ConnectionState.Open)
            {
               connection.Open();
            }
            SQLiteCommand command = connection.CreateCommand();
            string commandText = String.Format("SELECT [server_id], [name], [description], [location], [image], " +
                  "[sensor_1_name], [sensor_2_name], [sensor_3_name], [sensor_4_name], " +
                  "[sensor_1_unit], [sensor_2_unit], [sensor_3_unit], [sensor_4_unit], " +
                  "[sensor_1_id], [sensor_2_id], [sensor_3_id], [sensor_4_id]," +
                  "[sensor_1_range], [sensor_2_range], [sensor_3_range], [sensor_4_range] " +
               "FROM [servers] WHERE [server_id]='{0}';", serverID.ToString());
            Debug.WriteLine(commandText);
            command.CommandText = commandText;
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
               if (reader.Read())
               {
                  toReturn = ResultToServerData(reader);
               }
            }
            connection.Close();
         }
         return toReturn;
      }

      public static ServerData GetServerNameByID(SQLiteConnection connection, Guid serverID)
      {
         ServerData toReturn = null;
         lock (connection)
         {
            if (connection.State != ConnectionState.Open)
            {
               connection.Open();
            }
            SQLiteCommand command = connection.CreateCommand();
            string commandText = String.Format("SELECT [server_id], [name], [description], [location] from [servers] WHERE [server_id]='{0}';", serverID.ToString());
            command.CommandText = commandText;
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.Depth > 0)
            {
               Guid id = reader.GetGuid(0);
               string name = reader.GetString(1);
               string description = reader.GetString(2);
               string location = reader.GetString(3);
               byte[] imageBytes = new byte[] { };
               toReturn = new ServerData(id, name, location, description, new SensorDescriptionsData(), imageBytes);
            }
            connection.Close();
         }
         return toReturn;
      }

      public static List<ServerData> GetAllServerData(SQLiteConnection database)
      {
         string query = String.Format("SELECT [server_id], [name], [description], [location], [image] " +
                  "[sensor_1_name], [sensor_2_name], [sensor_3_name], [sensor_4_name], " +
                  "[sensor_1_unit], [sensor_2_unit], [sensor_3_unit], [sensor_4_unit], " +
                  "[sensor_1_id], [sensor_2_id], [sensor_3_id], [sensor_4_id]," +
                  "[sensor_1_range], [sensor_2_range], [sensor_3_range], [sensor_4_range] " +
               "FROM [servers]");
         List<ServerData> serverList = new List<ServerData>();

         lock (database)
         {
            using (SQLiteCommand command = new SQLiteCommand(query, database))
            {
               if (command.Connection.State != ConnectionState.Open)
               {
                  command.Connection.Open();
               }
               SQLiteDataReader serverReader = command.ExecuteReader();

               while (serverReader.Read())
               {
                  serverList.Add(ResultToServerData(serverReader));
               }
               serverReader.Close();
            }
            database.Close();
         }
         return serverList;
      }

      public static void SaveServerConfigData(SQLiteConnection connection, ServerData data)
      {
         Hashtable serverRow = new Hashtable();
         serverRow["server_id"] = SQLHelper.NullOrFormatAndAddQuotes(data.id.ToString());
         serverRow["name"] = SQLHelper.NullOrFormatAndAddQuotes(data.name);
         serverRow["location"] = SQLHelper.NullOrFormatAndAddQuotes(data.location);
         serverRow["description"] = SQLHelper.NullOrFormatAndAddQuotes(data.description);
         serverRow["image"] = "X'" + BitConverter.ToString(data.pictureBytes).Replace("-", "") + "'";
         serverRow["sensor_1_name"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[0].Name);
         serverRow["sensor_2_name"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[1].Name);
         serverRow["sensor_3_name"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[2].Name);
         serverRow["sensor_4_name"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[3].Name);
         serverRow["sensor_1_unit"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[0].Unit);
         serverRow["sensor_2_unit"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[1].Unit);
         serverRow["sensor_3_unit"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[2].Unit);
         serverRow["sensor_4_unit"] = SQLHelper.NullOrFormatAndAddQuotes(data.sensors[3].Unit);
         serverRow["sensor_1_id"] = data.sensors[0].ID;
         serverRow["sensor_2_id"] = data.sensors[1].ID;
         serverRow["sensor_3_id"] = data.sensors[2].ID;
         serverRow["sensor_4_id"] = data.sensors[3].ID;
         serverRow["sensor_1_range"] = data.sensors[0].Range;
         serverRow["sensor_2_range"] = data.sensors[1].Range;
         serverRow["sensor_3_range"] = data.sensors[2].Range;
         serverRow["sensor_4_range"] = data.sensors[3].Range;

         string query = String.Format("INSERT INTO [servers] {0}", SQLHelper.GenerateColumnsAndValues(serverRow));
         SQLHelper.ExecuteQuery(connection, query);
      }
      
      private static ServerData ResultToServerData(SQLiteDataReader result)
      {
         ServerData serverToReturn = null;

         Guid id = result.GetGuid(0);
         string name = result.GetString(1);
         string description = result.GetString(2);
         string location = result.GetString(3);
         byte[] imageBytes = (byte[])result.GetValue(4);

         SensorDescriptionsData sensorData = new SensorDescriptionsData();

         SensorDefinition[] sensor = new SensorDefinition[4] { new SensorDefinition(), new SensorDefinition(), new SensorDefinition(), new SensorDefinition()};

         for (int s = 0; s < 4; s++)
         {
            // Do 4 sensor descriptions and 4 fields for each
            sensor[s].Name = (string)SQLHelper.ValueIfNull(result, s + 5, "");
            sensor[s].Unit = (string)SQLHelper.ValueIfNull(result, s + 9, "");
            sensor[s].ID = Convert.ToInt16(SQLHelper.ValueIfNull(result, s + 13, 255));
            sensor[s].Range = Convert.ToInt16(SQLHelper.ValueIfNull(result, s + 17, 255));

            sensorData.Add(sensor[s]);
         }
         serverToReturn = new ServerData(id, name, location, description, sensorData, imageBytes);
         return serverToReturn;
      }

      public static void DeleteServerConfigData(Guid id, SQLiteConnection database)
      {
         SQLHelper.DeleteIDFromTable(id, SensorShareConfig.ClientServerDataTableName, database);
      }

      #endregion

      public static void SaveLogMessage(SQLiteConnection connection, String message)
      {
         string command = String.Format("INSERT INTO [log] ([message], [time]) VALUES ({0}, datetime('now'))",
            SQLHelper.FormatAndAddQuotes(message));
         SQLHelper.ExecuteQuery(connection, command);
      }

      #region Annoataions

      /// <summary>
      /// Gets an annotation from the client annotation database
      /// </summary>
      /// <returns>Annotation with GUID id</returns>
      public static IAnnotation getAnnotation(Guid id, SQLiteConnection database)
      {
         return getAnnotation(id, database, String.Empty);
      }

      public static IAnnotation getSavedAnnotation(Guid id, SQLiteConnection database)
      {
         return DatabaseHelper.getAnnotation(id, database, "saved=1");
      }

      /// <summary>
      /// Gets an annotation from the client annotation database and the where condition specified
      /// </summary>
      /// <returns>Annotation with GUID id and additional where condition</returns>
      public static IAnnotation getAnnotation(Guid id, SQLiteConnection database, string whereCondition)
      {
         IAnnotation annotationToReturn = null;

         string query = String.Format("SELECT id, type, time, text, photo_path, server_id, author, question_id, author_id, saved FROM {0} WHERE (id='{1}')",
             SensorShareConfig.AnnotationTableName, id);
         if (whereCondition != String.Empty)
         {
            query = String.Format("{0} AND ({1})", query, whereCondition);
         }
         Debug.WriteLine(query);

         using (SQLiteCommand command = new SQLiteCommand(query, database))
         {
            if (command.Connection.State != ConnectionState.Open)
            {
               command.Connection.Open();
            }
            SQLiteDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
               Object[] dataRow = new Object[dataReader.FieldCount];
               dataReader.GetValues(dataRow);
               Guid author_id = (Guid)dataRow[8];
               AnnotationType type = (AnnotationType)dataReader.GetInt32(1);
               if ((Boolean)dataRow[9])
               {
                  switch (type)
                  {
                     case AnnotationType.Image:
                        Bitmap image = new Bitmap(1, 1);
                        try
                        {
                           image = new Bitmap((String)dataRow[4]);
                        }
                        catch (FileNotFoundException ex)
                        {
                           Debug.WriteLine(String.Format("File not found: {0}", (String)dataRow[4]));
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                           image.Save(ms, ImageFormat.Jpeg);
                           ImageAnnotation iAnnotation = new ImageAnnotation(
                              (Guid)dataRow[0], (Guid)dataRow[5], author_id, (DateTime)dataRow[2],
                              (String)dataRow[6], (String)dataRow[3],
                              ms.ToArray());
                           annotationToReturn = iAnnotation;
                        }
                        break;
                     case AnnotationType.Text:
                        TextAnnotation tAnnotation = new TextAnnotation(
                           (Guid)dataRow[0], (Guid)dataRow[5], author_id, (DateTime)dataRow[2],
                           (String)dataRow[6], (String)dataRow[3]);
                        annotationToReturn = tAnnotation;
                        break;
                     case AnnotationType.QuestionAndAnswer:
                        QuestionMessage question = null;
                        if ((bool)dataRow[9])
                        {
                           question = getQuestionMessage(dataReader.GetGuid(7), database);
                        }
                        if (question != null)
                        {
                           QuestionAndAnswerAnnotation qaAnnotation = new QuestionAndAnswerAnnotation(
                              question, (Guid)dataRow[0], author_id,
                              (String)dataRow[6], (String)dataRow[3]);
                           annotationToReturn = qaAnnotation;
                        }
                        else
                        {
                           // Can't find the question so return an annotationbase
                           annotationToReturn = new AnnotationBase(
                              (Guid)dataRow[0], (Guid)dataRow[5], author_id, (DateTime)dataRow[2],
                              (String)dataRow[6]);
                        }
                        break;
                     //case AnnotationType.Base:
                     default:
                        annotationToReturn = new AnnotationBase(
                              (Guid)dataRow[0], (Guid)dataRow[5], author_id, (DateTime)dataRow[2],
                              (String)dataRow[6]);
                        break;
                  }
               }
               else
               {
                  annotationToReturn = new AnnotationBase(
                     (Guid)dataRow[0], (Guid)dataRow[5], author_id, (DateTime)dataRow[2],
                     (String)dataRow[6], type);
               }
            }
            dataReader.Close();
         }

         return annotationToReturn;
      }

      public static bool isAnnotationSaved(Guid id, SQLiteConnection database)
      {
         bool return_value = false;
         string query = String.Format("SELECT saved FROM {0} WHERE id='{1}'",
            SensorShareConfig.AnnotationTableName, id);
         Debug.WriteLine(query);

         using (SQLiteCommand command = new SQLiteCommand(query, database))
         {
            if (command.Connection.State != ConnectionState.Open)
            {
               command.Connection.Open();
            }
            Object result = command.ExecuteScalar();
            if (result != null)
            {
               return_value = (bool)result;
            }
         }
         database.Close();
         return return_value;
      }

      public static void deleteAnnotation(Guid id, SQLiteConnection database)
      {
         SQLHelper.DeleteIDFromTable(id, SensorShareConfig.AnnotationTableName, database);
      }

      public static void saveAnnotation(Guid id, DateTime time, Guid author_id, string author, AnnotationType type, string text, string picture_location, Guid server_id, SQLiteConnection database)
      {
         DatabaseHelper.saveAnnotation(id, time, author_id, author, type, text, picture_location, Guid.Empty, server_id, true, database);
      }

      public static void saveAnnotation(Guid id, DateTime time, Guid author_id, string author, AnnotationType type, string text, string picture_location, Guid question_id, Guid server_id, SQLiteConnection database)
      {
         DatabaseHelper.saveAnnotation(id, time, author_id, author, type, text, picture_location, question_id, server_id, true, database);
      }

      public static void saveAnnotation(Guid id, DateTime time, Guid author_id, string author, AnnotationType type, string text, string picture_location, Guid question_id, Guid server_id, bool saved, SQLiteConnection database)
      {
         Debug.WriteLine(String.Format("Saving annotation ID {0}", id));
         if (author == null)
         {
            throw new ArgumentNullException("author");
         }
         Hashtable data = new Hashtable();
         data["id"] = SQLHelper.FormatAndAddQuotes(id.ToString());

         data["text"] = SQLHelper.NullOrFormatAndAddQuotes(text);
         data["photo_path"] = SQLHelper.NullOrFormatAndAddQuotes(picture_location);
         
         data["saved"] = Convert.ToInt16(saved);
         data["time"] = SQLHelper.FormatAndAddQuotes(time.ToUniversalTime().ToString(SQLHelper.DateFormatString));
         data["type"] = (int)type;
         data["author"] = SQLHelper.FormatAndAddQuotes(author);
         data["server_id"] = SQLHelper.FormatAndAddQuotes(server_id.ToString());
         data["question_id"] = SQLHelper.NullOrFormatAndAddQuotes(question_id.ToString());

         data["author_id"] = SQLHelper.FormatAndAddQuotes(author_id.ToString());

         string insert = String.Format("INSERT INTO {0} {1}",
            SensorShareConfig.AnnotationTableName, SQLHelper.GenerateColumnsAndValues(data));
         Debug.WriteLine(insert);

         using (SQLiteCommand command = new SQLiteCommand(insert, database))
         {
            if (command.Connection.State != ConnectionState.Open)
            {
               command.Connection.Open();
            }
            command.ExecuteNonQuery();
         }
         database.Close();
      }

      #endregion

      #region Questions
      public static QuestionMessage getQuestionMessage(Guid id, SQLiteConnection database)
      {
         QuestionMessage messageToReturn = null;

         string query = String.Format("SELECT id, question_text, question_time, question_author, question_server FROM {0} WHERE id='{1}'",
                SensorShareConfig.QuestionDataTableName, id);
         Debug.WriteLine(query);
           
         using (SQLiteCommand command = new SQLiteCommand(query, database))
         {
            if (command.Connection.State != ConnectionState.Open)
            {
               command.Connection.Open();
            }
            SQLiteDataReader questionReader = command.ExecuteReader();
            if (questionReader.Read())
            {
               Object[] dataRow = new Object[questionReader.FieldCount];
               questionReader.GetValues(dataRow);

               messageToReturn = new QuestionMessage();
               messageToReturn.id = (Guid)dataRow[0];
               messageToReturn.Text = (String)dataRow[1];
               messageToReturn.Time = (DateTime)dataRow[2];
               messageToReturn.Author = (String)dataRow[3];
               messageToReturn.Server = (Guid)dataRow[4];
            }
            questionReader.Close();
         }
         database.Close();
         return messageToReturn;
      }

      public static void SaveQuestionMessage(QuestionMessage question, SQLiteConnection database)
      {
         Hashtable questionRow = new Hashtable();
         questionRow["id"] = SQLHelper.FormatAndAddQuotes(question.id.ToString());
         questionRow["question_text"] = SQLHelper.FormatAndAddQuotes(question.Text);
         questionRow["question_server"] = SQLHelper.FormatAndAddQuotes(question.Server.ToString());
         questionRow["question_time"] = SQLHelper.FormatAndAddQuotes(question.Time.ToString(SQLHelper.DateFormatString));
         questionRow["question_author"] = SQLHelper.FormatAndAddQuotes(question.Author);

         string query = String.Format("INSERT INTO {0} {1}", SensorShareConfig.QuestionDataTableName, SQLHelper.GenerateColumnsAndValues(questionRow));
         Debug.Write(query);
         using (SQLiteCommand command = new SQLiteCommand(query, database))
         {
            if (command.Connection.State != ConnectionState.Open)
            {
               command.Connection.Open();
            }
            command.ExecuteNonQuery();
         }
         database.Close();
      }

      #endregion

      #region Answers

      public static QuestionAndAnswerAnnotation getAnswer(Guid question_id, Guid author_id, SQLiteConnection database)
      {
         QuestionAndAnswerAnnotation messageToReturn = null;

         string query = String.Format("SELECT id FROM {0} WHERE (question_id='{1}') AND (author_id='{2}')",
             SensorShareConfig.AnnotationTableName, question_id, author_id);
         Debug.WriteLine(query);

         using (SQLiteCommand command = new SQLiteCommand(query, database))
         {
            if (command.Connection.State != ConnectionState.Open)
            {
               command.Connection.Open();
            }
            SQLiteDataReader result = command.ExecuteReader();
            if (result.Read())
            {
               if (!result.IsDBNull(0))
               {
                  Guid id = result.GetGuid(0);
                  messageToReturn = (QuestionAndAnswerAnnotation)DatabaseHelper.getAnnotation(id, database);
               }
            }
            result.Close();
         }
         database.Close();
         return messageToReturn;
      }

      #endregion

      #region Readings
      public static void saveSensorReadings(SensorReadings data, SQLiteConnection database)
      {
         List<SensorReadings> list = new List<SensorReadings>(1);
         list.Add(data);
         saveSensorReadingsList(list, database);
      }

      public static void saveSensorReadingsList(List<SensorReadingsData> dataList, SQLiteConnection database)
      {
         List<SensorReadings> list = new List<SensorReadings>(dataList.Count);
         foreach (SensorReadingsData readingsData in dataList)
         {
            list.Add((SensorReadings)readingsData);
         }
         saveSensorReadingsList(list, database);
      }

      public static void saveSensorReadingsList(List<SensorReadings> dataList, SQLiteConnection database)
      {
          lock (database)
          {
              using (SQLiteCommand command = database.CreateCommand())
              {
                  if (command.Connection.State != ConnectionState.Open)
                  {
                      command.Connection.Open();
                  }
                  string query;
                  Hashtable dataRow = new Hashtable();
                  foreach (SensorReadings reading in dataList)
                  {
                      dataRow.Clear();

                      dataRow["server_id"] = SQLHelper.FormatAndAddQuotes(reading.ServerID.ToString());
                      dataRow["reading_1"] = reading.Reading1;
                      dataRow["reading_2"] = reading.Reading2;
                      dataRow["reading_3"] = reading.Reading3;
                      dataRow["reading_4"] = reading.Reading4;
                      dataRow["reading_time"] = SQLHelper.FormatAndAddQuotes(reading.Time.ToString(SQLHelper.DateFormatString));

                      query = String.Format("INSERT INTO {0} {1}", SensorShareConfig.ClientReadingsTableName, SQLHelper.GenerateColumnsAndValues(dataRow));
                      Debug.WriteLine(query);
                      command.CommandText = query;
                      command.ExecuteNonQuery();
                  }
              }
              database.Close();
          }
      }

      #endregion

      #region Sensor Database

      public static SensorDefinition GetSensorData(SensorID idToFind, SQLiteConnection database)
      {
         SensorDefinition sensorDataToReturn = new SensorDefinition();
         string query = String.Format("SELECT * FROM {0} WHERE ([sn]={1}) AND ([sr]={2})",
                SensorShareConfig.SensorInfoTableName, idToFind.ID, idToFind.Range);
         Debug.WriteLine(query);
         lock(database)
         {
            using (SQLiteCommand command = new SQLiteCommand(query, database))
            {
               if (command.Connection.State != ConnectionState.Open)
               {
                  command.Connection.Open();
               }
               SQLiteDataReader dataReader = null;
               dataReader = command.ExecuteReader();
               if (dataReader.Read())
               {
                  sensorDataToReturn.ID = idToFind.ID;
                  sensorDataToReturn.Range = idToFind.Range;
                  sensorDataToReturn.Name = (string)dataReader["Parameter name"]; ;
                  sensorDataToReturn.ASCIIUnit = (string)dataReader["WL/UL"];
                  sensorDataToReturn.Unit = (string)dataReader["Datadisc"];
                  sensorDataToReturn.MinValue = (double)dataReader["Lowerlimit"];
                  sensorDataToReturn.MaxValue = (double)dataReader["Upperlimit"];

               }
               else
               {
                  sensorDataToReturn.ID = 255;
                  sensorDataToReturn.Range = 255;
                  sensorDataToReturn.Name = null;
                  sensorDataToReturn.Unit = null;
               }
               dataReader.Close();
               database.Close();
            }
         }
         return sensorDataToReturn;
      }

      #endregion

      #region Stats
      public static double[] getStdDev(Guid server_id, double[] means, int count, SQLiteConnection database)
      {
         double[] stdDevs = new double[means.Length];

         int n = count;

         if (n < 2)
         {
            for (int i = 0; i < means.Length; i++)
            {
               stdDevs[i] = 0;

            }
            return stdDevs;
         }

         string query = "SELECT ";
         for (int i = 1; i <= means.Length; i++)
         {
            query += String.Format("SQRT(SUM(POWER(reading_{0}-{1}, 2))/{2}) AS stddev_{0}", i, means[i - 1], (n - 1));
            if (i < means.Length)
            {
               query += ", ";
            }
         }
         query += String.Format(" FROM {0} WHERE server_id='{1}'", SensorShareConfig.SensorReadingsTableName, server_id);
         Debug.WriteLine(query);
         if (database.State != ConnectionState.Open)
         {
            database.Open();
         }
         using (SQLiteCommand getStdDevCommand = new SQLiteCommand(query, database))
         {
            SQLiteDataReader stdDevData = getStdDevCommand.ExecuteReader();
            if (stdDevData.Read())
            {
               for (int i = 0; i < stdDevData.FieldCount; i++)
               {
                  if (!stdDevData.IsDBNull(i))
                  {
                     stdDevs[i] = stdDevData.GetDouble(i);
                  }
                  else
                  {
                     //If the value is null it means there's no stats so set it as 0
                     stdDevs[i] = 0;
                  }
               }
            }
            stdDevData.Close();
         }
         database.Close();
         return stdDevs;
      }

      public static double[] getStats(Guid server_id, string whereCondition, SQLiteConnection database)
      {
         string query = String.Format("SELECT MAX(reading_1), MIN(reading_1), AVG(reading_1), " +
               "MAX(reading_2), MIN(reading_2), AVG(reading_2), " +
               "MAX(reading_3), MIN(reading_3), AVG(reading_3), " +
               "MAX(reading_4), MIN(reading_4), AVG(reading_4), COUNT(*) FROM {0} WHERE (server_id='{1}')",
               SensorShareConfig.SensorReadingsTableName, server_id);
         if (whereCondition.Length > 0)
         {
            query += String.Format(" AND ({0})", whereCondition);
         }
         Debug.WriteLine(query);

         if (database.State == ConnectionState.Closed)
         {
            database.Open();
         }
         double[] stats = new double[13];
         using (SQLiteCommand getMaxMinMeanCommand = new SQLiteCommand(query, database))
         {
            SQLiteDataReader maxMinsMeansData = getMaxMinMeanCommand.ExecuteReader();
            if (maxMinsMeansData.Read())
            {
               for (int i = 0; i < maxMinsMeansData.FieldCount; i++)
               {
                  if (!maxMinsMeansData.IsDBNull(i))
                  {
                     if (i == 12)
                     {
                        stats[12] = maxMinsMeansData.GetInt32(12);
                     }
                     else
                     {
                        stats[i] = maxMinsMeansData.GetDouble(i);
                     }
                  }
                  else
                  {
                     //If the value is null it means there's no stats so set it as 0
                     stats[i] = 0;
                  }

               }
            }
            maxMinsMeansData.Close();
         }
         database.Close();
         return stats;
      }
      #endregion

   }
}