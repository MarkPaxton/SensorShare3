using System;
using System.Windows.Forms;
using System.Data.SQLite;
using SensorShare;

namespace SensorShare.Compact
{
   partial class LogBrowser
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;
      private System.Windows.Forms.MainMenu mainMenu;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.Data.SQLite.SQLiteParameter sqLiteParameter1 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter2 = new System.Data.SQLite.SQLiteParameter();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogBrowser));
         System.Data.SQLite.SQLiteParameter sqLiteParameter3 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter4 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter5 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter6 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter7 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter8 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter9 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter10 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter11 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter12 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter13 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter14 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter15 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter16 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter17 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter18 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter19 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter20 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter21 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter22 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter23 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter24 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter25 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter26 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter27 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter28 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter29 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter30 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter31 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter32 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter33 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter34 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter35 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter36 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter37 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter38 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter39 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter40 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter41 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter42 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter43 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter44 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter45 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter46 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter47 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter48 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter49 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter50 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter51 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter52 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter53 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter54 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter55 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter56 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter57 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter58 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter59 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter60 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter61 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter62 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter63 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter64 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter65 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter66 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter67 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter68 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter69 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter70 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter71 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter72 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter73 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter74 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter75 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter76 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter77 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter78 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter79 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter80 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter81 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter82 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter83 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter84 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter85 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter86 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter87 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter88 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter89 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter90 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter91 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter92 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter93 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter94 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter95 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter96 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter97 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter98 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter99 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter100 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter101 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter102 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter103 = new System.Data.SQLite.SQLiteParameter();
         System.Data.SQLite.SQLiteParameter sqLiteParameter104 = new System.Data.SQLite.SQLiteParameter();
         this.sqLiteConnection = new System.Data.SQLite.SQLiteConnection();
         this.sqliteSelectCommand1 = new System.Data.SQLite.SQLiteCommand();
         this.logDataAdapter = new System.Data.SQLite.SQLiteDataAdapter();
         this.InsertCommand = new System.Data.SQLite.SQLiteCommand();
         this.sqliteSelectCommand2 = new System.Data.SQLite.SQLiteCommand();
         this.sqliteInsertCommand1 = new System.Data.SQLite.SQLiteCommand();
         this.sqliteUpdateCommand1 = new System.Data.SQLite.SQLiteCommand();
         this.sqliteDeleteCommand1 = new System.Data.SQLite.SQLiteCommand();
         this.serversDataAdapter = new System.Data.SQLite.SQLiteDataAdapter();
         this.serverDataSet = new SensorShare.serverDataSet();
         this.logDataSet = new SensorShare.logDataSet();
         this.logDataGrid = new System.Windows.Forms.DataGrid();
         this.logDataSet1BindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.logBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.timeDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.messageDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.tabPage2 = new System.Windows.Forms.TabPage();
         this.DataGrid2 = new System.Windows.Forms.DataGrid();
         this.serversBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.serveridDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.nameDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor1unitDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor3unitDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor4unitDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor1rangeDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor2rangeDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor3rangeDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor4rangeDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor1idDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor2idDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor3idDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor4idDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor2unitDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.descriptionDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.locationDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.imageDataGridImageColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor1nameDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor2nameDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor3nameDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         this.sensor4nameDataGridTextBoxColumn = new System.Windows.Forms.DataGridTextBoxColumn();
         ((System.ComponentModel.ISupportInitialize)(this.serverDataSet)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.logDataSet)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.logDataGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.logDataSet1BindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.logBindingSource)).BeginInit();
         this.tabControl1.SuspendLayout();
         this.tabPage1.SuspendLayout();
         this.tabPage2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.DataGrid2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.serversBindingSource)).BeginInit();
         this.mainMenu = new System.Windows.Forms.MainMenu();
         this.closeMenuItem = new System.Windows.Forms.MenuItem();
         this.SuspendLayout();
         // 
         // dbConnection
         // 
         this.sqLiteConnection.ConnectionString = "Data Source=\"" +
            SensorShareConfig.DatabaseFolder + "\\" + SensorShareConfig.ServerDatabase + "\"";
         this.sqLiteConnection.DefaultTimeout = 30;
         // 
         // sqliteSelectCommand1
         // 
         this.sqliteSelectCommand1.CommandText = "SELECT     log.[time], log.message\r\nFROM         log";
         this.sqliteSelectCommand1.Connection = this.sqLiteConnection;
         // 
         // logDataAdapter
         // 
         this.logDataAdapter.InsertCommand = this.InsertCommand;
         this.logDataAdapter.SelectCommand = this.sqliteSelectCommand1;
         this.logDataAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "log", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("time", "time"),
                        new System.Data.Common.DataColumnMapping("message", "message")})});
         // 
         // InsertCommand
         // 
         this.InsertCommand.CommandText = "INSERT INTO [log] ([time], [message]) VALUES (@time, @message)";
         this.InsertCommand.Connection = this.sqLiteConnection;
         sqLiteParameter1.ParameterName = "@time";
         sqLiteParameter1.SourceColumn = "time";
         sqLiteParameter2.ParameterName = "@message";
         sqLiteParameter2.SourceColumn = "message";
         this.InsertCommand.Parameters.AddRange(new System.Data.SQLite.SQLiteParameter[] {
            sqLiteParameter1,
            sqLiteParameter2});
         // 
         // sqliteSelectCommand2
         // 
         this.sqliteSelectCommand2.CommandText = "SELECT     servers.*\r\nFROM         servers";
         this.sqliteSelectCommand2.Connection = this.sqLiteConnection;
         // 
         // sqliteInsertCommand1
         // 
         this.sqliteInsertCommand1.CommandText = resources.GetString("sqliteInsertCommand1.CommandText");
         this.sqliteInsertCommand1.Connection = this.sqLiteConnection;
         sqLiteParameter3.ParameterName = "@server_id";
         sqLiteParameter3.SourceColumn = "server_id";
         sqLiteParameter4.ParameterName = "@name";
         sqLiteParameter4.SourceColumn = "name";
         sqLiteParameter5.ParameterName = "@description";
         sqLiteParameter5.SourceColumn = "description";
         sqLiteParameter6.ParameterName = "@location";
         sqLiteParameter6.SourceColumn = "location";
         sqLiteParameter7.ParameterName = "@image";
         sqLiteParameter7.SourceColumn = "image";
         sqLiteParameter8.ParameterName = "@sensor_1_name";
         sqLiteParameter8.SourceColumn = "sensor_1_name";
         sqLiteParameter9.ParameterName = "@sensor_2_name";
         sqLiteParameter9.SourceColumn = "sensor_2_name";
         sqLiteParameter10.ParameterName = "@sensor_3_name";
         sqLiteParameter10.SourceColumn = "sensor_3_name";
         sqLiteParameter11.ParameterName = "@sensor_4_name";
         sqLiteParameter11.SourceColumn = "sensor_4_name";
         sqLiteParameter12.ParameterName = "@sensor_1_unit";
         sqLiteParameter12.SourceColumn = "sensor_1_unit";
         sqLiteParameter13.ParameterName = "@sensor_2_unit";
         sqLiteParameter13.SourceColumn = "sensor_2_unit";
         sqLiteParameter14.ParameterName = "@sensor_3_unit";
         sqLiteParameter14.SourceColumn = "sensor_3_unit";
         sqLiteParameter15.ParameterName = "@sensor_4_unit";
         sqLiteParameter15.SourceColumn = "sensor_4_unit";
         sqLiteParameter16.ParameterName = "@sensor_1_range";
         sqLiteParameter16.SourceColumn = "sensor_1_range";
         sqLiteParameter17.ParameterName = "@sensor_2_range";
         sqLiteParameter17.SourceColumn = "sensor_2_range";
         sqLiteParameter18.ParameterName = "@sensor_3_range";
         sqLiteParameter18.SourceColumn = "sensor_3_range";
         sqLiteParameter19.ParameterName = "@sensor_4_range";
         sqLiteParameter19.SourceColumn = "sensor_4_range";
         sqLiteParameter20.ParameterName = "@sensor_1_id";
         sqLiteParameter20.SourceColumn = "sensor_1_id";
         sqLiteParameter21.ParameterName = "@sensor_2_id";
         sqLiteParameter21.SourceColumn = "sensor_2_id";
         sqLiteParameter22.ParameterName = "@sensor_3_id";
         sqLiteParameter22.SourceColumn = "sensor_3_id";
         sqLiteParameter23.ParameterName = "@sensor_4_id";
         sqLiteParameter23.SourceColumn = "sensor_4_id";
         this.sqliteInsertCommand1.Parameters.AddRange(new System.Data.SQLite.SQLiteParameter[] {
            sqLiteParameter3,
            sqLiteParameter4,
            sqLiteParameter5,
            sqLiteParameter6,
            sqLiteParameter7,
            sqLiteParameter8,
            sqLiteParameter9,
            sqLiteParameter10,
            sqLiteParameter11,
            sqLiteParameter12,
            sqLiteParameter13,
            sqLiteParameter14,
            sqLiteParameter15,
            sqLiteParameter16,
            sqLiteParameter17,
            sqLiteParameter18,
            sqLiteParameter19,
            sqLiteParameter20,
            sqLiteParameter21,
            sqLiteParameter22,
            sqLiteParameter23});
         // 
         // sqliteUpdateCommand1
         // 
         this.sqliteUpdateCommand1.CommandText = resources.GetString("sqliteUpdateCommand1.CommandText");
         this.sqliteUpdateCommand1.Connection = this.sqLiteConnection;
         sqLiteParameter24.ParameterName = "@server_id";
         sqLiteParameter24.SourceColumn = "server_id";
         sqLiteParameter25.ParameterName = "@name";
         sqLiteParameter25.SourceColumn = "name";
         sqLiteParameter26.ParameterName = "@description";
         sqLiteParameter26.SourceColumn = "description";
         sqLiteParameter27.ParameterName = "@location";
         sqLiteParameter27.SourceColumn = "location";
         sqLiteParameter28.ParameterName = "@image";
         sqLiteParameter28.SourceColumn = "image";
         sqLiteParameter29.ParameterName = "@sensor_1_name";
         sqLiteParameter29.SourceColumn = "sensor_1_name";
         sqLiteParameter30.ParameterName = "@sensor_2_name";
         sqLiteParameter30.SourceColumn = "sensor_2_name";
         sqLiteParameter31.ParameterName = "@sensor_3_name";
         sqLiteParameter31.SourceColumn = "sensor_3_name";
         sqLiteParameter32.ParameterName = "@sensor_4_name";
         sqLiteParameter32.SourceColumn = "sensor_4_name";
         sqLiteParameter33.ParameterName = "@sensor_1_unit";
         sqLiteParameter33.SourceColumn = "sensor_1_unit";
         sqLiteParameter34.ParameterName = "@sensor_2_unit";
         sqLiteParameter34.SourceColumn = "sensor_2_unit";
         sqLiteParameter35.ParameterName = "@sensor_3_unit";
         sqLiteParameter35.SourceColumn = "sensor_3_unit";
         sqLiteParameter36.ParameterName = "@sensor_4_unit";
         sqLiteParameter36.SourceColumn = "sensor_4_unit";
         sqLiteParameter37.ParameterName = "@sensor_1_range";
         sqLiteParameter37.SourceColumn = "sensor_1_range";
         sqLiteParameter38.ParameterName = "@sensor_2_range";
         sqLiteParameter38.SourceColumn = "sensor_2_range";
         sqLiteParameter39.ParameterName = "@sensor_3_range";
         sqLiteParameter39.SourceColumn = "sensor_3_range";
         sqLiteParameter40.ParameterName = "@sensor_4_range";
         sqLiteParameter40.SourceColumn = "sensor_4_range";
         sqLiteParameter41.ParameterName = "@sensor_1_id";
         sqLiteParameter41.SourceColumn = "sensor_1_id";
         sqLiteParameter42.ParameterName = "@sensor_2_id";
         sqLiteParameter42.SourceColumn = "sensor_2_id";
         sqLiteParameter43.ParameterName = "@sensor_3_id";
         sqLiteParameter43.SourceColumn = "sensor_3_id";
         sqLiteParameter44.ParameterName = "@sensor_4_id";
         sqLiteParameter44.SourceColumn = "sensor_4_id";
         sqLiteParameter45.ParameterName = "@Original_server_id";
         sqLiteParameter45.SourceColumn = "server_id";
         sqLiteParameter45.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter46.ParameterName = "@Original_name";
         sqLiteParameter46.SourceColumn = "name";
         sqLiteParameter46.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter47.ParameterName = "@Original_description";
         sqLiteParameter47.SourceColumn = "description";
         sqLiteParameter47.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter48.ParameterName = "@Original_location";
         sqLiteParameter48.SourceColumn = "location";
         sqLiteParameter48.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter49.ParameterName = "@IsNull_image";
         sqLiteParameter49.SourceColumn = "image";
         sqLiteParameter49.SourceColumnNullMapping = true;
         sqLiteParameter49.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter50.ParameterName = "@Original_image";
         sqLiteParameter50.SourceColumn = "image";
         sqLiteParameter50.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter51.ParameterName = "@IsNull_sensor_1_name";
         sqLiteParameter51.SourceColumn = "sensor_1_name";
         sqLiteParameter51.SourceColumnNullMapping = true;
         sqLiteParameter51.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter52.ParameterName = "@Original_sensor_1_name";
         sqLiteParameter52.SourceColumn = "sensor_1_name";
         sqLiteParameter52.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter53.ParameterName = "@IsNull_sensor_2_name";
         sqLiteParameter53.SourceColumn = "sensor_2_name";
         sqLiteParameter53.SourceColumnNullMapping = true;
         sqLiteParameter53.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter54.ParameterName = "@Original_sensor_2_name";
         sqLiteParameter54.SourceColumn = "sensor_2_name";
         sqLiteParameter54.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter55.ParameterName = "@IsNull_sensor_3_name";
         sqLiteParameter55.SourceColumn = "sensor_3_name";
         sqLiteParameter55.SourceColumnNullMapping = true;
         sqLiteParameter55.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter56.ParameterName = "@Original_sensor_3_name";
         sqLiteParameter56.SourceColumn = "sensor_3_name";
         sqLiteParameter56.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter57.ParameterName = "@IsNull_sensor_4_name";
         sqLiteParameter57.SourceColumn = "sensor_4_name";
         sqLiteParameter57.SourceColumnNullMapping = true;
         sqLiteParameter57.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter58.ParameterName = "@Original_sensor_4_name";
         sqLiteParameter58.SourceColumn = "sensor_4_name";
         sqLiteParameter58.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter59.ParameterName = "@IsNull_sensor_1_unit";
         sqLiteParameter59.SourceColumn = "sensor_1_unit";
         sqLiteParameter59.SourceColumnNullMapping = true;
         sqLiteParameter59.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter60.ParameterName = "@Original_sensor_1_unit";
         sqLiteParameter60.SourceColumn = "sensor_1_unit";
         sqLiteParameter60.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter61.ParameterName = "@IsNull_sensor_2_unit";
         sqLiteParameter61.SourceColumn = "sensor_2_unit";
         sqLiteParameter61.SourceColumnNullMapping = true;
         sqLiteParameter61.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter62.ParameterName = "@Original_sensor_2_unit";
         sqLiteParameter62.SourceColumn = "sensor_2_unit";
         sqLiteParameter62.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter63.ParameterName = "@IsNull_sensor_3_unit";
         sqLiteParameter63.SourceColumn = "sensor_3_unit";
         sqLiteParameter63.SourceColumnNullMapping = true;
         sqLiteParameter63.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter64.ParameterName = "@Original_sensor_3_unit";
         sqLiteParameter64.SourceColumn = "sensor_3_unit";
         sqLiteParameter64.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter65.ParameterName = "@IsNull_sensor_4_unit";
         sqLiteParameter65.SourceColumn = "sensor_4_unit";
         sqLiteParameter65.SourceColumnNullMapping = true;
         sqLiteParameter65.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter66.ParameterName = "@Original_sensor_4_unit";
         sqLiteParameter66.SourceColumn = "sensor_4_unit";
         sqLiteParameter66.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter67.ParameterName = "@Original_sensor_1_range";
         sqLiteParameter67.SourceColumn = "sensor_1_range";
         sqLiteParameter67.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter68.ParameterName = "@Original_sensor_2_range";
         sqLiteParameter68.SourceColumn = "sensor_2_range";
         sqLiteParameter68.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter69.ParameterName = "@Original_sensor_3_range";
         sqLiteParameter69.SourceColumn = "sensor_3_range";
         sqLiteParameter69.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter70.ParameterName = "@Original_sensor_4_range";
         sqLiteParameter70.SourceColumn = "sensor_4_range";
         sqLiteParameter70.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter71.ParameterName = "@Original_sensor_1_id";
         sqLiteParameter71.SourceColumn = "sensor_1_id";
         sqLiteParameter71.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter72.ParameterName = "@Original_sensor_2_id";
         sqLiteParameter72.SourceColumn = "sensor_2_id";
         sqLiteParameter72.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter73.ParameterName = "@Original_sensor_3_id";
         sqLiteParameter73.SourceColumn = "sensor_3_id";
         sqLiteParameter73.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter74.ParameterName = "@Original_sensor_4_id";
         sqLiteParameter74.SourceColumn = "sensor_4_id";
         sqLiteParameter74.SourceVersion = System.Data.DataRowVersion.Original;
         this.sqliteUpdateCommand1.Parameters.AddRange(new System.Data.SQLite.SQLiteParameter[] {
            sqLiteParameter24,
            sqLiteParameter25,
            sqLiteParameter26,
            sqLiteParameter27,
            sqLiteParameter28,
            sqLiteParameter29,
            sqLiteParameter30,
            sqLiteParameter31,
            sqLiteParameter32,
            sqLiteParameter33,
            sqLiteParameter34,
            sqLiteParameter35,
            sqLiteParameter36,
            sqLiteParameter37,
            sqLiteParameter38,
            sqLiteParameter39,
            sqLiteParameter40,
            sqLiteParameter41,
            sqLiteParameter42,
            sqLiteParameter43,
            sqLiteParameter44,
            sqLiteParameter45,
            sqLiteParameter46,
            sqLiteParameter47,
            sqLiteParameter48,
            sqLiteParameter49,
            sqLiteParameter50,
            sqLiteParameter51,
            sqLiteParameter52,
            sqLiteParameter53,
            sqLiteParameter54,
            sqLiteParameter55,
            sqLiteParameter56,
            sqLiteParameter57,
            sqLiteParameter58,
            sqLiteParameter59,
            sqLiteParameter60,
            sqLiteParameter61,
            sqLiteParameter62,
            sqLiteParameter63,
            sqLiteParameter64,
            sqLiteParameter65,
            sqLiteParameter66,
            sqLiteParameter67,
            sqLiteParameter68,
            sqLiteParameter69,
            sqLiteParameter70,
            sqLiteParameter71,
            sqLiteParameter72,
            sqLiteParameter73,
            sqLiteParameter74});
         // 
         // sqliteDeleteCommand1
         // 
         this.sqliteDeleteCommand1.CommandText = resources.GetString("sqliteDeleteCommand1.CommandText");
         this.sqliteDeleteCommand1.Connection = this.sqLiteConnection;
         sqLiteParameter75.ParameterName = "@Original_server_id";
         sqLiteParameter75.SourceColumn = "server_id";
         sqLiteParameter75.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter76.ParameterName = "@Original_name";
         sqLiteParameter76.SourceColumn = "name";
         sqLiteParameter76.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter77.ParameterName = "@Original_description";
         sqLiteParameter77.SourceColumn = "description";
         sqLiteParameter77.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter78.ParameterName = "@Original_location";
         sqLiteParameter78.SourceColumn = "location";
         sqLiteParameter78.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter79.ParameterName = "@IsNull_image";
         sqLiteParameter79.SourceColumn = "image";
         sqLiteParameter79.SourceColumnNullMapping = true;
         sqLiteParameter79.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter80.ParameterName = "@Original_image";
         sqLiteParameter80.SourceColumn = "image";
         sqLiteParameter80.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter81.ParameterName = "@IsNull_sensor_1_name";
         sqLiteParameter81.SourceColumn = "sensor_1_name";
         sqLiteParameter81.SourceColumnNullMapping = true;
         sqLiteParameter81.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter82.ParameterName = "@Original_sensor_1_name";
         sqLiteParameter82.SourceColumn = "sensor_1_name";
         sqLiteParameter82.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter83.ParameterName = "@IsNull_sensor_2_name";
         sqLiteParameter83.SourceColumn = "sensor_2_name";
         sqLiteParameter83.SourceColumnNullMapping = true;
         sqLiteParameter83.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter84.ParameterName = "@Original_sensor_2_name";
         sqLiteParameter84.SourceColumn = "sensor_2_name";
         sqLiteParameter84.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter85.ParameterName = "@IsNull_sensor_3_name";
         sqLiteParameter85.SourceColumn = "sensor_3_name";
         sqLiteParameter85.SourceColumnNullMapping = true;
         sqLiteParameter85.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter86.ParameterName = "@Original_sensor_3_name";
         sqLiteParameter86.SourceColumn = "sensor_3_name";
         sqLiteParameter86.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter87.ParameterName = "@IsNull_sensor_4_name";
         sqLiteParameter87.SourceColumn = "sensor_4_name";
         sqLiteParameter87.SourceColumnNullMapping = true;
         sqLiteParameter87.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter88.ParameterName = "@Original_sensor_4_name";
         sqLiteParameter88.SourceColumn = "sensor_4_name";
         sqLiteParameter88.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter89.ParameterName = "@IsNull_sensor_1_unit";
         sqLiteParameter89.SourceColumn = "sensor_1_unit";
         sqLiteParameter89.SourceColumnNullMapping = true;
         sqLiteParameter89.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter90.ParameterName = "@Original_sensor_1_unit";
         sqLiteParameter90.SourceColumn = "sensor_1_unit";
         sqLiteParameter90.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter91.ParameterName = "@IsNull_sensor_2_unit";
         sqLiteParameter91.SourceColumn = "sensor_2_unit";
         sqLiteParameter91.SourceColumnNullMapping = true;
         sqLiteParameter91.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter92.ParameterName = "@Original_sensor_2_unit";
         sqLiteParameter92.SourceColumn = "sensor_2_unit";
         sqLiteParameter92.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter93.ParameterName = "@IsNull_sensor_3_unit";
         sqLiteParameter93.SourceColumn = "sensor_3_unit";
         sqLiteParameter93.SourceColumnNullMapping = true;
         sqLiteParameter93.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter94.ParameterName = "@Original_sensor_3_unit";
         sqLiteParameter94.SourceColumn = "sensor_3_unit";
         sqLiteParameter94.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter95.ParameterName = "@IsNull_sensor_4_unit";
         sqLiteParameter95.SourceColumn = "sensor_4_unit";
         sqLiteParameter95.SourceColumnNullMapping = true;
         sqLiteParameter95.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter96.ParameterName = "@Original_sensor_4_unit";
         sqLiteParameter96.SourceColumn = "sensor_4_unit";
         sqLiteParameter96.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter97.ParameterName = "@Original_sensor_1_range";
         sqLiteParameter97.SourceColumn = "sensor_1_range";
         sqLiteParameter97.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter98.ParameterName = "@Original_sensor_2_range";
         sqLiteParameter98.SourceColumn = "sensor_2_range";
         sqLiteParameter98.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter99.ParameterName = "@Original_sensor_3_range";
         sqLiteParameter99.SourceColumn = "sensor_3_range";
         sqLiteParameter99.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter100.ParameterName = "@Original_sensor_4_range";
         sqLiteParameter100.SourceColumn = "sensor_4_range";
         sqLiteParameter100.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter101.ParameterName = "@Original_sensor_1_id";
         sqLiteParameter101.SourceColumn = "sensor_1_id";
         sqLiteParameter101.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter102.ParameterName = "@Original_sensor_2_id";
         sqLiteParameter102.SourceColumn = "sensor_2_id";
         sqLiteParameter102.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter103.ParameterName = "@Original_sensor_3_id";
         sqLiteParameter103.SourceColumn = "sensor_3_id";
         sqLiteParameter103.SourceVersion = System.Data.DataRowVersion.Original;
         sqLiteParameter104.ParameterName = "@Original_sensor_4_id";
         sqLiteParameter104.SourceColumn = "sensor_4_id";
         sqLiteParameter104.SourceVersion = System.Data.DataRowVersion.Original;
         this.sqliteDeleteCommand1.Parameters.AddRange(new System.Data.SQLite.SQLiteParameter[] {
            sqLiteParameter75,
            sqLiteParameter76,
            sqLiteParameter77,
            sqLiteParameter78,
            sqLiteParameter79,
            sqLiteParameter80,
            sqLiteParameter81,
            sqLiteParameter82,
            sqLiteParameter83,
            sqLiteParameter84,
            sqLiteParameter85,
            sqLiteParameter86,
            sqLiteParameter87,
            sqLiteParameter88,
            sqLiteParameter89,
            sqLiteParameter90,
            sqLiteParameter91,
            sqLiteParameter92,
            sqLiteParameter93,
            sqLiteParameter94,
            sqLiteParameter95,
            sqLiteParameter96,
            sqLiteParameter97,
            sqLiteParameter98,
            sqLiteParameter99,
            sqLiteParameter100,
            sqLiteParameter101,
            sqLiteParameter102,
            sqLiteParameter103,
            sqLiteParameter104});
         // 
         // serversDataAdapter
         // 
         this.serversDataAdapter.DeleteCommand = this.sqliteDeleteCommand1;
         this.serversDataAdapter.InsertCommand = this.sqliteInsertCommand1;
         this.serversDataAdapter.SelectCommand = this.sqliteSelectCommand2;
         this.serversDataAdapter.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "servers", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("server_id", "server_id"),
                        new System.Data.Common.DataColumnMapping("name", "name"),
                        new System.Data.Common.DataColumnMapping("description", "description"),
                        new System.Data.Common.DataColumnMapping("location", "location"),
                        new System.Data.Common.DataColumnMapping("image", "image"),
                        new System.Data.Common.DataColumnMapping("sensor_1_name", "sensor_1_name"),
                        new System.Data.Common.DataColumnMapping("sensor_2_name", "sensor_2_name"),
                        new System.Data.Common.DataColumnMapping("sensor_3_name", "sensor_3_name"),
                        new System.Data.Common.DataColumnMapping("sensor_4_name", "sensor_4_name"),
                        new System.Data.Common.DataColumnMapping("sensor_1_unit", "sensor_1_unit"),
                        new System.Data.Common.DataColumnMapping("sensor_2_unit", "sensor_2_unit"),
                        new System.Data.Common.DataColumnMapping("sensor_3_unit", "sensor_3_unit"),
                        new System.Data.Common.DataColumnMapping("sensor_4_unit", "sensor_4_unit"),
                        new System.Data.Common.DataColumnMapping("sensor_1_range", "sensor_1_range"),
                        new System.Data.Common.DataColumnMapping("sensor_2_range", "sensor_2_range"),
                        new System.Data.Common.DataColumnMapping("sensor_3_range", "sensor_3_range"),
                        new System.Data.Common.DataColumnMapping("sensor_4_range", "sensor_4_range"),
                        new System.Data.Common.DataColumnMapping("sensor_1_id", "sensor_1_id"),
                        new System.Data.Common.DataColumnMapping("sensor_2_id", "sensor_2_id"),
                        new System.Data.Common.DataColumnMapping("sensor_3_id", "sensor_3_id"),
                        new System.Data.Common.DataColumnMapping("sensor_4_id", "sensor_4_id")})});
         this.serversDataAdapter.UpdateCommand = this.sqliteUpdateCommand1;
         // 
         // serverDataSet
         // 
         this.serverDataSet.DataSetName = "serverDataSet";
         this.serverDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
         // 
         // logDataSet
         // 
         this.logDataSet.DataSetName = "Log";
         this.logDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
         // 
         // logDataGrid
         // 
         this.logDataGrid.TableStyles.Add(new DataGridTableStyle());
         //this.logDataGrid.AllowUserToAddRows = false;
         //this.logDataGrid.AllowUserToDeleteRows = false;
         //this.logDataGrid.AutoGenerateColumns = false;
         //this.logDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridColumnHeadersHeightSizeMode.AutoSize;
         this.logDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridColumn[] {
            this.timeDataGridTextBoxColumn,
            this.messageDataGridTextBoxColumn});
         this.logDataGrid.DataSource = this.logBindingSource;
         this.logDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.logDataGrid.Location = new System.Drawing.Point(3, 3);
         this.logDataGrid.Name = "logDataGrid";
         this.logDataGrid.ReadOnly = true;
         this.logDataGrid.Size = new System.Drawing.Size(596, 393);
         this.logDataGrid.TabIndex = 0;
         // 
         // logDataSet1BindingSource
         // 
         this.logDataSet1BindingSource.DataSource = this.logDataSet;
         this.logDataSet1BindingSource.Position = 0;
         // 
         // logBindingSource
         // 
         this.logBindingSource.DataMember = "log";
         this.logBindingSource.DataSource = this.logDataSet;
         // 
         // timeDataGridTextBoxColumn
         // 
         this.timeDataGridTextBoxColumn.DataPropertyName = "time";
         this.timeDataGridTextBoxColumn.HeaderText = "time";
         this.timeDataGridTextBoxColumn.Name = "timeDataGridTextBoxColumn";
         this.timeDataGridTextBoxColumn.ReadOnly = true;
         // 
         // messageDataGridTextBoxColumn
         // 
         this.messageDataGridTextBoxColumn.DataPropertyName = "message";
         this.messageDataGridTextBoxColumn.HeaderText = "message";
         this.messageDataGridTextBoxColumn.Name = "messageDataGridTextBoxColumn";
         this.messageDataGridTextBoxColumn.ReadOnly = true;
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tabPage1);
         this.tabControl1.Controls.Add(this.tabPage2);
         this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabControl1.Location = new System.Drawing.Point(0, 0);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(610, 425);
         this.tabControl1.TabIndex = 1;
         // 
         // tabPage1
         // 
         this.tabPage1.Controls.Add(this.logDataGrid);
         this.tabPage1.Location = new System.Drawing.Point(4, 22);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(602, 399);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "tabPage1";
         this.tabPage1.UseVisualStyleBackColor = true;
         // 
         // tabPage2
         // 
         this.tabPage2.Controls.Add(this.DataGrid2);
         this.tabPage2.Location = new System.Drawing.Point(4, 22);
         this.tabPage2.Name = "tabPage2";
         this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage2.Size = new System.Drawing.Size(602, 399);
         this.tabPage2.TabIndex = 1;
         this.tabPage2.Text = "tabPage2";
         this.tabPage2.UseVisualStyleBackColor = true;
         // 
         // DataGrid2
         // 
         this.DataGrid2.AllowUserToAddRows = false;
         this.DataGrid2.AllowUserToDeleteRows = false;
         this.DataGrid2.AllowUserToOrderColumns = true;
         this.DataGrid2.AutoGenerateColumns = false;
         this.DataGrid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridColumnHeadersHeightSizeMode.AutoSize;
         this.DataGrid2.Columns.AddRange(new System.Windows.Forms.DataGridColumn[] {
            this.serveridDataGridTextBoxColumn,
            this.nameDataGridTextBoxColumn,
            this.sensor1unitDataGridTextBoxColumn,
            this.sensor3unitDataGridTextBoxColumn,
            this.sensor4unitDataGridTextBoxColumn,
            this.sensor1rangeDataGridTextBoxColumn,
            this.sensor2rangeDataGridTextBoxColumn,
            this.sensor3rangeDataGridTextBoxColumn,
            this.sensor4rangeDataGridTextBoxColumn,
            this.sensor1idDataGridTextBoxColumn,
            this.sensor2idDataGridTextBoxColumn,
            this.sensor3idDataGridTextBoxColumn,
            this.sensor4idDataGridTextBoxColumn,
            this.sensor2unitDataGridTextBoxColumn,
            this.descriptionDataGridTextBoxColumn,
            this.locationDataGridTextBoxColumn,
            this.imageDataGridImageColumn,
            this.sensor1nameDataGridTextBoxColumn,
            this.sensor2nameDataGridTextBoxColumn,
            this.sensor3nameDataGridTextBoxColumn,
            this.sensor4nameDataGridTextBoxColumn});
         this.DataGrid2.DataSource = this.serversBindingSource;
         this.DataGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.DataGrid2.Location = new System.Drawing.Point(3, 3);
         this.DataGrid2.Name = "DataGrid2";
         this.DataGrid2.ReadOnly = true;
         this.DataGrid2.Size = new System.Drawing.Size(596, 393);
         this.DataGrid2.TabIndex = 0;
         // 
         // serversBindingSource
         // 
         this.serversBindingSource.DataMember = "servers";
         this.serversBindingSource.DataSource = this.serverDataSet;
         // 
         // serveridDataGridTextBoxColumn
         // 
         this.serveridDataGridTextBoxColumn.DataPropertyName = "server_id";
         this.serveridDataGridTextBoxColumn.HeaderText = "server_id";
         this.serveridDataGridTextBoxColumn.Name = "serveridDataGridTextBoxColumn";
         this.serveridDataGridTextBoxColumn.ReadOnly = true;
         // 
         // nameDataGridTextBoxColumn
         // 
         this.nameDataGridTextBoxColumn.DataPropertyName = "name";
         this.nameDataGridTextBoxColumn.HeaderText = "name";
         this.nameDataGridTextBoxColumn.Name = "nameDataGridTextBoxColumn";
         this.nameDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor1unitDataGridTextBoxColumn
         // 
         this.sensor1unitDataGridTextBoxColumn.DataPropertyName = "sensor_1_unit";
         this.sensor1unitDataGridTextBoxColumn.HeaderText = "sensor_1_unit";
         this.sensor1unitDataGridTextBoxColumn.Name = "sensor1unitDataGridTextBoxColumn";
         this.sensor1unitDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor3unitDataGridTextBoxColumn
         // 
         this.sensor3unitDataGridTextBoxColumn.DataPropertyName = "sensor_3_unit";
         this.sensor3unitDataGridTextBoxColumn.HeaderText = "sensor_3_unit";
         this.sensor3unitDataGridTextBoxColumn.Name = "sensor3unitDataGridTextBoxColumn";
         this.sensor3unitDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor4unitDataGridTextBoxColumn
         // 
         this.sensor4unitDataGridTextBoxColumn.DataPropertyName = "sensor_4_unit";
         this.sensor4unitDataGridTextBoxColumn.HeaderText = "sensor_4_unit";
         this.sensor4unitDataGridTextBoxColumn.Name = "sensor4unitDataGridTextBoxColumn";
         this.sensor4unitDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor1rangeDataGridTextBoxColumn
         // 
         this.sensor1rangeDataGridTextBoxColumn.DataPropertyName = "sensor_1_range";
         this.sensor1rangeDataGridTextBoxColumn.HeaderText = "sensor_1_range";
         this.sensor1rangeDataGridTextBoxColumn.Name = "sensor1rangeDataGridTextBoxColumn";
         this.sensor1rangeDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor2rangeDataGridTextBoxColumn
         // 
         this.sensor2rangeDataGridTextBoxColumn.DataPropertyName = "sensor_2_range";
         this.sensor2rangeDataGridTextBoxColumn.HeaderText = "sensor_2_range";
         this.sensor2rangeDataGridTextBoxColumn.Name = "sensor2rangeDataGridTextBoxColumn";
         this.sensor2rangeDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor3rangeDataGridTextBoxColumn
         // 
         this.sensor3rangeDataGridTextBoxColumn.DataPropertyName = "sensor_3_range";
         this.sensor3rangeDataGridTextBoxColumn.HeaderText = "sensor_3_range";
         this.sensor3rangeDataGridTextBoxColumn.Name = "sensor3rangeDataGridTextBoxColumn";
         this.sensor3rangeDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor4rangeDataGridTextBoxColumn
         // 
         this.sensor4rangeDataGridTextBoxColumn.DataPropertyName = "sensor_4_range";
         this.sensor4rangeDataGridTextBoxColumn.HeaderText = "sensor_4_range";
         this.sensor4rangeDataGridTextBoxColumn.Name = "sensor4rangeDataGridTextBoxColumn";
         this.sensor4rangeDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor1idDataGridTextBoxColumn
         // 
         this.sensor1idDataGridTextBoxColumn.DataPropertyName = "sensor_1_id";
         this.sensor1idDataGridTextBoxColumn.HeaderText = "sensor_1_id";
         this.sensor1idDataGridTextBoxColumn.Name = "sensor1idDataGridTextBoxColumn";
         this.sensor1idDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor2idDataGridTextBoxColumn
         // 
         this.sensor2idDataGridTextBoxColumn.DataPropertyName = "sensor_2_id";
         this.sensor2idDataGridTextBoxColumn.HeaderText = "sensor_2_id";
         this.sensor2idDataGridTextBoxColumn.Name = "sensor2idDataGridTextBoxColumn";
         this.sensor2idDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor3idDataGridTextBoxColumn
         // 
         this.sensor3idDataGridTextBoxColumn.DataPropertyName = "sensor_3_id";
         this.sensor3idDataGridTextBoxColumn.HeaderText = "sensor_3_id";
         this.sensor3idDataGridTextBoxColumn.Name = "sensor3idDataGridTextBoxColumn";
         this.sensor3idDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor4idDataGridTextBoxColumn
         // 
         this.sensor4idDataGridTextBoxColumn.DataPropertyName = "sensor_4_id";
         this.sensor4idDataGridTextBoxColumn.HeaderText = "sensor_4_id";
         this.sensor4idDataGridTextBoxColumn.Name = "sensor4idDataGridTextBoxColumn";
         this.sensor4idDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor2unitDataGridTextBoxColumn
         // 
         this.sensor2unitDataGridTextBoxColumn.DataPropertyName = "sensor_2_unit";
         this.sensor2unitDataGridTextBoxColumn.HeaderText = "sensor_2_unit";
         this.sensor2unitDataGridTextBoxColumn.Name = "sensor2unitDataGridTextBoxColumn";
         this.sensor2unitDataGridTextBoxColumn.ReadOnly = true;
         // 
         // descriptionDataGridTextBoxColumn
         // 
         this.descriptionDataGridTextBoxColumn.DataPropertyName = "description";
         this.descriptionDataGridTextBoxColumn.HeaderText = "description";
         this.descriptionDataGridTextBoxColumn.Name = "descriptionDataGridTextBoxColumn";
         this.descriptionDataGridTextBoxColumn.ReadOnly = true;
         // 
         // locationDataGridTextBoxColumn
         // 
         this.locationDataGridTextBoxColumn.DataPropertyName = "location";
         this.locationDataGridTextBoxColumn.HeaderText = "location";
         this.locationDataGridTextBoxColumn.Name = "locationDataGridTextBoxColumn";
         this.locationDataGridTextBoxColumn.ReadOnly = true;
         // 
         // imageDataGridImageColumn
         // 
         this.imageDataGridImageColumn.DataPropertyName = "image";
         this.imageDataGridImageColumn.HeaderText = "image";
         this.imageDataGridImageColumn.Name = "imageDataGridImageColumn";
         this.imageDataGridImageColumn.ReadOnly = true;
         // 
         // sensor1nameDataGridTextBoxColumn
         // 
         this.sensor1nameDataGridTextBoxColumn.DataPropertyName = "sensor_1_name";
         this.sensor1nameDataGridTextBoxColumn.HeaderText = "sensor_1_name";
         this.sensor1nameDataGridTextBoxColumn.Name = "sensor1nameDataGridTextBoxColumn";
         this.sensor1nameDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor2nameDataGridTextBoxColumn
         // 
         this.sensor2nameDataGridTextBoxColumn.DataPropertyName = "sensor_2_name";
         this.sensor2nameDataGridTextBoxColumn.HeaderText = "sensor_2_name";
         this.sensor2nameDataGridTextBoxColumn.Name = "sensor2nameDataGridTextBoxColumn";
         this.sensor2nameDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor3nameDataGridTextBoxColumn
         // 
         this.sensor3nameDataGridTextBoxColumn.DataPropertyName = "sensor_3_name";
         this.sensor3nameDataGridTextBoxColumn.HeaderText = "sensor_3_name";
         this.sensor3nameDataGridTextBoxColumn.Name = "sensor3nameDataGridTextBoxColumn";
         this.sensor3nameDataGridTextBoxColumn.ReadOnly = true;
         // 
         // sensor4nameDataGridTextBoxColumn
         // 
         this.sensor4nameDataGridTextBoxColumn.DataPropertyName = "sensor_4_name";
         this.sensor4nameDataGridTextBoxColumn.HeaderText = "sensor_4_name";
         this.sensor4nameDataGridTextBoxColumn.Name = "sensor4nameDataGridTextBoxColumn";
         this.sensor4nameDataGridTextBoxColumn.ReadOnly = true;
         // 
         // mainMenu
         // 
         this.mainMenu.MenuItems.Add(this.closeMenuItem);
         // 
         // closeMenuItem
         // 
         this.closeMenuItem.Text = "Exit";
         this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
         // 
         // LogBrowser
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(240, 268);
         this.Controls.Add(this.tabControl1);
         this.Menu = this.mainMenu;
         this.Name = "LogBrowser";
         this.Text = "Log Browser";
         ((System.ComponentModel.ISupportInitialize)(this.serverDataSet)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.logDataSet)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.logDataGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.logDataSet1BindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.logBindingSource)).EndInit();
         this.tabControl1.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         this.tabPage2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.DataGrid2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.serversBindingSource)).EndInit();
         this.Closed += new System.EventHandler(this.LogBrowser_Closed);
         this.ResumeLayout(false);

      }

      #endregion
      private System.Data.SQLite.SQLiteConnection sqLiteConnection;
      private System.Data.SQLite.SQLiteCommand sqliteSelectCommand1;
      private System.Data.SQLite.SQLiteDataAdapter logDataAdapter;
      private System.Data.SQLite.SQLiteCommand InsertCommand;
      private System.Data.SQLite.SQLiteCommand sqliteSelectCommand2;
      private System.Data.SQLite.SQLiteCommand sqliteInsertCommand1;
      private System.Data.SQLite.SQLiteCommand sqliteUpdateCommand1;
      private System.Data.SQLite.SQLiteCommand sqliteDeleteCommand1;
      private System.Data.SQLite.SQLiteDataAdapter serversDataAdapter;
      private serverDataSet serverDataSet;
      private logDataSet logDataSet;
      private System.Windows.Forms.DataGrid logDataGrid;
      private System.Windows.Forms.DataGridTextBoxColumn timeDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn messageDataGridTextBoxColumn;
      private System.Windows.Forms.BindingSource logBindingSource;
      private System.Windows.Forms.BindingSource logDataSet1BindingSource;
      private System.Windows.Forms.TabControl tabControl1;
      private System.Windows.Forms.TabPage tabPage1;
      private System.Windows.Forms.TabPage tabPage2;
      private System.Windows.Forms.DataGrid DataGrid2;
      private System.Windows.Forms.DataGridTextBoxColumn serveridDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn nameDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor1unitDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor3unitDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor4unitDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor1rangeDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor2rangeDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor3rangeDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor4rangeDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor1idDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor2idDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor3idDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor4idDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor2unitDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn descriptionDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn locationDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn imageDataGridImageColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor1nameDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor2nameDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor3nameDataGridTextBoxColumn;
      private System.Windows.Forms.DataGridTextBoxColumn sensor4nameDataGridTextBoxColumn;
      private System.Windows.Forms.BindingSource serversBindingSource; 
      private System.Windows.Forms.MenuItem closeMenuItem;
   }
}

