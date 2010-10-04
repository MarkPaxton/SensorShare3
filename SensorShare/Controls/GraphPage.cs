using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using mcp;
using mcp.Graphs;
using ScienceScope;

namespace SensorShare
{
   public partial class GraphPage : UserControl
   {
      DateTime yougestReading = DateTime.MinValue;
      TextDelegate updateYoungestReadingDelegate;

      public event InfoPointSelectedEventHandler InfoPointSelected;
      public event GraphChagedEventHandler GraphChanged;

      protected string unit = "";
      public string Unit
      {
         get { return unit; }
      }

      private List<Bitmap> infoIcons;
      public List<Bitmap> InfoIcons
      {
         get { return infoIcons; }
         set
         {
            infoIcons = value;
            graphBox.IconImages = infoIcons;
            this.Redraw();
         }
      }

      public GraphPage()
      {
         InitializeComponent();
         updateYoungestReadingDelegate = new TextDelegate(UpdateYoungestReading);
      }

      public GraphPage(SensorDefinition sensor)
         : this()
      {
         this.titleLabel.Text = sensor.Description;
         this.unit = sensor.Unit;
         bool sensorIdentified = false;

         if ((sensor.ID == 6) && (sensor.Range == 16))
         {
            this.graphBox.MaxY = 7000;
            this.graphBox.MinY = 0;
            this.graphBox.ShowNegitiveYLables = false;
            this.graphBox.yMarkIncrement = 2000;
            sensorIdentified = true;

         }
         if ((sensor.ID == 14) && (sensor.Range == 24))
         {
            this.graphBox.MaxY = 40;
            this.graphBox.MinY = 0;
            this.graphBox.ShowNegitiveYLables = false;
            this.graphBox.yMarkIncrement = 5;
            sensorIdentified = true;
         }
         if ((sensor.ID == 9) && (sensor.Range == 8))
         {
            this.graphBox.MaxY = 130;
            this.graphBox.MinY = 0;
            this.graphBox.ShowNegitiveYLables = false;
            this.graphBox.yMarkIncrement = 10;
            sensorIdentified = true;
         }
         //default fall through
         if (!sensorIdentified)
         {
            this.graphBox.MaxY = sensor.MaxValue;
            this.graphBox.MinY = 0;
            this.graphBox.yMarkIncrement = (int)Math.Round((sensor.MaxValue - sensor.MinValue) / 6);
         }
         this.graphBox.XWidth = 55;
         this.graphBox.ToNow();

         this.graphBox.IconImages = InfoIcons;

      }

      protected void plot(DateTime time, double value)
      {
         graphBox.Plot(time, value);
         if (DateTime.Compare(yougestReading, time) <= 0)
         {
            string readingString = String.Format("{0:f2}{1}", value, Unit);
            yougestReading = time;
            if (lastReadingLabel.InvokeRequired)
            {
               lastReadingLabel.Invoke(updateYoungestReadingDelegate, readingString);
            }
            else
            {
               UpdateYoungestReading(readingString);
            }
         }
      }

      public void Plot(DateTime time, double value)
      {
         graphBox.SuspendPaint();
         plot(time, value);
         graphBox.ResumePaint();
      }

      public void Plot(DateTime time, double value, double mean, double stdDev)
      {
         graphBox.SuspendPaint();
         plot(time, value);
         if (DateTime.Compare(yougestReading, time) <= 0)
         {
            PlotStats(mean, stdDev);
         }
         graphBox.ResumePaint();
      }

      public void PlotStats(double mean, double stdDev)
      {
         graphBox.Mean = mean;
         if (stdDev != 0)
         {
            graphBox.MaxY = mean + (2.5 * stdDev);
            graphBox.MinY = Math.Min(0, mean - (2.5 * stdDev));
         }
         else
         {
            graphBox.MaxY = mean * 1.5;
            graphBox.MinY = 0;
         }
         graphBox.yMarkIncrement = Math.Round((graphBox.MaxY - graphBox.MinY) / 8, 0);
      }

      public void PlotInfoPoints(List<IAnnotation> annotations)
      {
         graphBox.SuspendPaint();
         foreach (IAnnotation annotation in annotations)
         {
            graphBox.AddInfoPoint(annotation.Date, annotation.ID, (int)annotation.Type);
         }
         graphBox.ResumePaint();
      }

      public void SusupendPaint()
      {
         graphBox.SuspendPaint();
      }

      public void ResumePaint()
      {
         graphBox.ResumePaint();
      }

      public void Redraw()
      {
         graphBox.Redraw();
      }

      public void StartFollowingTime()
      {
         graphBox.StartFollowingTime();
      }

      public void StopFollowingTime()
      {
         graphBox.StopFollowingTime();
      }

      public void ToDateTime(DateTime time)
      {
         graphBox.ToDateTime(time);
      }

      public void ToNow()
      {
         graphBox.ToNow();
      }

      public void SetEndTime(DateTime time)
      {
         graphBox.XAxisMax = time;
      }

      public void SetStartTime(DateTime time)
      {
         graphBox.XAxisMin(time);
      }

      public void SetXWidth(int width)
      {
         graphBox.XWidth = width;
      }

      private double zoomFactor = 1.25;
      public void ZoomIn()
      {
         graphBox.XWidth = Convert.ToInt32(Math.Round(graphBox.XWidth / zoomFactor));
      }

      public void ZoomOut()
      {
         graphBox.XWidth = Convert.ToInt32(Math.Round(graphBox.XWidth * zoomFactor));
      }

      public void MoveLater()
      {
         graphBox.XAxisMax = DateAndTime.DateAdd(DateInterval.Second, graphBox.XWidth * 0.20, graphBox.XAxisMax);
      }

      public void MoveEarlier()
      {
         graphBox.XAxisMax = DateAndTime.DateAdd(DateInterval.Second, graphBox.XWidth * -0.20, graphBox.XAxisMax);
      }

      private void backButton_Click(object sender, EventArgs e)
      {
         graphBox.StopFollowingTime();
         MoveEarlier();
         FireGraphChanged();
      }

      private void forwardButton_Click(object sender, EventArgs e)
      {
         graphBox.StopFollowingTime();
         MoveLater();
         FireGraphChanged();
      }

      private void toNowButton_Click(object sender, EventArgs e)
      {
         graphBox.StartFollowingTime();
         FireGraphChanged();
      }

      protected void UpdateYoungestReading(string text)
      {
         this.lastReadingLabel.Text = text;
      }

      public void ClearData()
      {
         graphBox.Clear();
      }

      private void graphBox_InfoPointSelected(object sender, InfoPointSelectedEventArgs args)
      {
         //just refiring the event from the graph
         if (InfoPointSelected != null)
         {
            InfoPointSelected(this, args);
         }
      }

      private void ZoomInButton_Click(object sender, EventArgs e)
      {
         StopFollowingTime();
         ZoomIn();
         FireGraphChanged();
      }

      private void ZoomOutButton_Click(object sender, EventArgs e)
      {
         StopFollowingTime();
         ZoomOut();
         FireGraphChanged();
      }

      public void PercentOfXToFill(double percent)
      {
         graphBox.PercentOfXToFill(percent);
      }


      public void SetTicksOffset(long offset)
      {
         graphBox.TicksOffset(offset);
      }

      protected void FireGraphChanged()
      {
         if (GraphChanged != null)
         {
            GraphChanged(this, new GraphChagedEventArgs(graphBox.XAxisMax, graphBox.XWidth, graphBox.FollowingTime));
         }
      }

      public void SetGraphParameters(GraphChagedEventArgs args)
      {
         SusupendPaint();
         graphBox.XAxisMax = args.EndTime;
         graphBox.XWidth = args.Width;
         if (args.FollowingTime)
         {
            graphBox.StartFollowingTime();
         }
         else
         {
            graphBox.StopFollowingTime();
         }
         ResumePaint();
      }
   }

   public class GraphChagedEventArgs : EventArgs
   {
      public readonly DateTime EndTime;
      public readonly int Width;
      public readonly bool FollowingTime;

      public GraphChagedEventArgs(DateTime endTime, int width, bool follow)
         : base()
      {
         EndTime = endTime;
         Width = width;
         FollowingTime = follow;
      }
   }

   public delegate void GraphChagedEventHandler(object sender, GraphChagedEventArgs args);
}