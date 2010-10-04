using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace mcp.Graphs
{
    public class DateTimeGraphBox : GraphBox
    {
        protected int xMargin = 35;
        protected int yMargin = 25;

        protected DateTime startTime;
        protected DateTime endTime;
        public DateTime XAxisMax
        {
            get { return endTime; }
            set { ToDateTime(value); }
        }

        protected long ticksOffset = 0;

        protected bool followTime = false;
        public bool FollowingTime
        {
            get { return followTime; }
        }

        Thread followTimeThread = null;

        protected int secondsWidth = 30;

        int updateTimeInMs = 200;
        
        /// <summary>
        /// The number of seconds that the X axis will span
        /// </summary>
        public int XWidth
        {
            get { return secondsWidth; }
            set
            {
                secondsWidth = value;
                calcAxisDimensions();
                updateTimeInMs = (int)Math.Max(Math.Round(1 / (xPxPerTick * 1E7)), 100);
                ToDateTime(endTime);
            }
        }

        public void TicksOffset(long ticks)
        {
            if (ticks != ticksOffset)
            {
                ticksOffset = ticks;
                DoUpdateGraphInvoke();
            }
        }

        public long TicksOffset()
        {
            return ticksOffset;
        }

        bool _showMeanLine = false;
        public bool ShowMeanLine
        {
            get { return _showMeanLine; }
            set
            {
                _showMeanLine = value;
                DoUpdateGraphInvoke();
            }
        }

        double _mean;
        public double Mean
        {
            get { return _mean; }
            set
            {
                if (value != _mean)
                {
                    _mean = value;
                    ShowMeanLine = true;
                }
            }
        }

        public DateTimeGraphBox()
            : base()
        {
            graphArea = new Rectangle(xMargin, 0, this.Width - xMargin, this.Height - yMargin);
            ToNow();
        }

        ~DateTimeGraphBox()
        {
            suspendPaint = true;
            if (followTime)
            {
                StopFollowingTime();
            }
        }

        protected void CompleteUpdateGraphInvoke()
        {
            try
            {
                if (!suspendPaint)
                {
                   if (this != null)
                   {
                      if (this.InvokeRequired)
                      {
                         this.BeginInvoke(_updateGraphInvoker);
                      }
                      else
                      {
                         UpdateGraph();
                      }
                   }
                }
            }
            catch (ObjectDisposedException ex)
            {
                //this happens when the form closes, but it should be ok to ignore it until I work out how to stop it
            }
        }


        protected override void DoUpdateGraphInvoke()
        {
            if (!followTime)
            {
                CompleteUpdateGraphInvoke();
            }
        }

        public void StartFollowingTime()
        {
            if (!followTime)
            {
                followTime = true;
                updateTimeInMs = (int)Math.Max(Math.Round(1 / (xPxPerTick * 1E7)), 250);
                followTimeThread = new Thread(new ThreadStart(followingTimeThread));
                followTimeThread.Start();
            }
        }

        public void StopFollowingTime()
        {
            if (followTime)
            {
                followTime = false;
            }
        }
      
        protected void followingTimeThread()
        {
            while(followTime)
            {
                ToNow();
                CompleteUpdateGraphInvoke();
                Thread.Sleep(updateTimeInMs);
            }
        }


        public DateTime XAxisMin()
        {
            return startTime;
        }

        public void XAxisMin(DateTime value)
        {
            this.ToDateTime(DateAndTime.DateAdd(DateInterval.Second, secondsWidth, value));
        }


        public void ToNow()
        {
            ToDateTime(DateTime.UtcNow.AddTicks(ticksOffset));
        }

        public void ToDateTime(DateTime time)
        {
            startTime = DateAndTime.DateAdd(DateInterval.Second, -1 * secondsWidth, time);
            endTime = time;

            DoUpdateGraphInvoke();
        }

        public void Plot(DateTime xValue, double yValue)
        {
            base.Plot(xValue, yValue);
            DoUpdateGraphInvoke();
        }


        // How many pixels per Tick on the X axis
        protected double xPxPerTick;

        protected override void calcAxisDimensions()
        {
            base.calcAxisDimensions();
            if (startTime != endTime)
            {
                xPxPerTick = (graphArea.Width * percentOfXToFill) / (endTime.Ticks - startTime.Ticks);
            }
            else
            {
                xPxPerTick = 0;
            }

            if (MaxY != 0)
            {
                xMargin = (int)(Math.Round((Math.Log10(MaxY) + 1) * 6 + markSize + 1));
            }
            else
            {
                xMargin = 10 + markSize + 1;
            }
            
        }

        protected override void PaintGraph(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            graphArea.Width = this.Width - xMargin;
            graphArea.Height = this.Height - yMargin;
            graphArea.X = xMargin;
            graphArea.Y = 0;

            Pen dPen = new Pen(Color.SteelBlue);
            SolidBrush dBrush = new SolidBrush(Color.Tomato);

            lock (xyData)
            {
                if (xyData.Count > 0)
                {
                    DateTime minX = startTime;
                    DateTime maxX = endTime;

                    // How wide is each bar on the graph
                    int xWidth = (int)Math.Round(xPxPerTick);
                    if (xWidth < 1)
                    {
                        xWidth = 1;
                    }
                    else
                    {
                        if (xWidth > 5)
                        {
                            xWidth = 5;
                        }
                    }

                    double xToPlot;  // THe X poistioin to plot
                    double yValue; // the y value to plot

                    double yPositionPx; // The y position to plot
                    double yHeightPx; // THe height of the y value to plot
                    Rectangle rect; // The graph bar

                    foreach (DateTime xValue in xyData.Keys)
                    {
                        if ((xValue.CompareTo(minX) >= 0) && (xValue.CompareTo(maxX) <= 0))
                        {
                            xToPlot = xMargin + ((xValue.Ticks - minX.Ticks) * xPxPerTick);

                            yValue = (double)xyData[xValue];

                            // if point goes off top - set height to be max and if it goes off the bottom set point to be miny
                            yValue = Math.Min(yValue, maxY);
                            yValue = Math.Max(yValue, minY);

                            //is it above or below the X axis?
                            if (yValue > interceptX)
                            {
                                yHeightPx = (yValue - interceptX) * yPxPerUnit;
                                yPositionPx = interceptXPx - yHeightPx;
                            }
                            else
                            {
                                yHeightPx = (interceptX - yValue) * yPxPerUnit;
                                yPositionPx = interceptXPx;
                            }

                            rect = new Rectangle((int)Math.Round(xToPlot), (int)Math.Round(yPositionPx),
                                                 (int)Math.Min(xWidth, Math.Round(xMargin + graphArea.Width - xToPlot)), (int)Math.Round(yHeightPx));
                            g.DrawRectangle(dPen, rect);
                            g.FillRectangle(dBrush, rect);
                        }
                    }

                    //If the graph is wide enough, print the latest figure.
                    if ((xyData.Count > 0) && (graphArea.Width > 40))
                    {
                        string drawString = String.Format("{0}", xyData[maxX]);
                        Font drawFont = new Font("Arial", 8, FontStyle.Regular);
                        SolidBrush drawBrush = new SolidBrush(Color.Black);
                        g.DrawString(drawString, drawFont, drawBrush, graphArea.Width - 30, 10);
                        drawFont.Dispose();
                        drawBrush.Dispose();
                    }
                }
                
            }

            drawAxes(g, dPen, graphArea);

            dPen.Dispose();
            dBrush.Dispose();
        }

        int markSize = 3;
        
        private int marksEverySec = 5;
        public int xMarkIncrement
        {
            get { return marksEverySec; }
            set
            {
                marksEverySec = value;
                UpdateGraph();
            }
        }
        
        private double yMarksEvery = 10;
        public double yMarkIncrement
        {
            get { return yMarksEvery; }
            set
            {
                yMarksEvery = value;
                DoUpdateGraphInvoke();
            }
        }

        protected void drawAxes(Graphics g, Pen dPen, Rectangle graphArea)
        {
            // x y crosses interceptX then draw X the axis in location
            g.DrawLine(dPen, xMargin, interceptXPx, xMargin + graphArea.Width, interceptXPx);
            bool drawExtraAxis = false;
            if ((interceptXPx >= 0) && (interceptXPx < graphArea.Height))
            {
                g.DrawLine(dPen, xMargin, interceptXPx, xMargin + graphArea.Width, interceptXPx);
            }
            else
            {
                drawExtraAxis = true;
            }

            //The size in pixels between the marks on the x axis
            double xIncrementPx = marksEverySec * xPxPerTick * 1E7;

            //Work out the next increment
            IFormatProvider culture = new CultureInfo("en-GB", true);
            DateTime xMarksStart = DateTime.Parse(XAxisMin().ToString("dd-MM-yyyy HH:mm:00"), culture);

            while (xMarksStart.CompareTo(XAxisMin()) < 0)
            {
                xMarksStart = xMarksStart.AddSeconds(marksEverySec);
            }

            double firstTickPx = xMargin + ((xMarksStart.Ticks - XAxisMin().Ticks) * xPxPerTick);
            DateTime xMarkTime = xMarksStart;
            TimeZone localTimeZone = TimeZone.CurrentTimeZone;
            DateTime printTime;            
            bool first = true;

            for (double x = firstTickPx; x < (xMargin + graphArea.Width); x += xIncrementPx)
            {
                g.DrawLine(dPen, (int)Math.Round(x), interceptXPx, (int)Math.Round(x), interceptXPx + markSize);
                if (!drawExtraAxis || (interceptXPx < (graphArea.Height - (2 * yMargin))))
                {
                    g.DrawLine(dPen, (int)Math.Round(x), graphArea.Height, (int)Math.Round(x), graphArea.Height + markSize);
                }
                if ((x - 5) > xMargin)
                {
                    printTime = localTimeZone.ToLocalTime(xMarkTime);
                    Font arial7Font = new Font("Arial", 7, FontStyle.Regular);
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    g.DrawString(printTime.ToString("%s") + "s", arial7Font, blackBrush, 
                        (float)Math.Round(x - 5), (float)graphArea.Height + markSize + 1);
                    arial7Font.Dispose();
                    if (first || (DateAndTime.DateAdd(DateInterval.Second, (-1 * marksEverySec), xMarkTime).Minute != xMarkTime.Minute))
                    {
                        Font arial7BoldFont = new Font("Arial", 7, FontStyle.Bold);                    
                        g.DrawString(printTime.ToString("h:mmtt"), arial7BoldFont, blackBrush, 
                            (float)Math.Round(x - 10), (float)graphArea.Height + markSize + 9);
                        first = false;
                        arial7BoldFont.Dispose();
                    }
                }
                xMarkTime = DateAndTime.DateAdd(DateInterval.Second, marksEverySec, xMarkTime);
            }

            // Draw the mean line is in view
            if (ShowMeanLine)
            {
                if ((minY < Mean) && (maxY > Mean))
                {
                    //Draw the line
                    int meanLinePx = (int)Math.Round(yPxPerUnit * (maxY - Mean));
                    Pen grayPen = new Pen(Color.Gray);
                    g.DrawLine(grayPen, xMargin, meanLinePx, xMargin + graphArea.Width, meanLinePx);
                    grayPen.Dispose();

                    // Add a label on the line, make sure it's on the graph though!
                    int labelPosition = meanLinePx;
                    if (meanLinePx < 15)
                    {
                        labelPosition += 1;
                    }
                    else
                    {
                        labelPosition -= 13;
                    }
                    Font arial8Font = new Font("Arial", 8, FontStyle.Regular);
                    SolidBrush grayBrush = new SolidBrush(Color.Gray); 
                    g.DrawString("Average", arial8Font, grayBrush, xMargin + 10, labelPosition);
                    arial8Font.Dispose();
                    grayBrush.Dispose();
                }
            }

            // Y Axis
            g.DrawLine(dPen, xMargin, 0, xMargin, graphArea.Height);

            //The size in pixels between the marks on the x axis
            double yIncrementPx = yMarksEvery * yPxPerUnit;

            //stop labels overlapping
            if (yIncrementPx < 8)
            {
                if ((MaxY - MinY) > 5)
                {
                    yMarksEvery = (double)Math.Ceiling(8 / yPxPerUnit);
                }
                else
                {
                    yMarksEvery = (double)Math.Round(8 / yPxPerUnit, 2);
                }
            }

            double yStart = yMarksEvery * Math.Ceiling(minY / yMarksEvery);
            int yPx;
            for (double y = yStart; y < MaxY; y += yMarksEvery)
            {
                if ((y >= 0) || (y < 0 && showNegitiveYLables))
                {
                    yPx = (int) Math.Round(graphArea.Height + graphArea.Y - ((y - MinY) * yPxPerUnit));

                    g.DrawLine(dPen, xMargin, yPx, xMargin - markSize, yPx);
                    Font arial7Font = new Font("Arial", 7, FontStyle.Regular);
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    g.DrawString(String.Format("{0}", y), arial7Font, blackBrush, 1, (float)yPx - 4);
                    arial7Font.Dispose();
                    blackBrush.Dispose();
                }
            }


            // The 'now' line
            DateTime nowInstance = DateTime.UtcNow.AddTicks(ticksOffset);
            if ((nowInstance.CompareTo(startTime) >= 0) && (nowInstance.CompareTo(endTime) <= 0))
            {
                int nowLinePx = (int)Math.Round(xMargin + ((nowInstance.Ticks - startTime.Ticks) * xPxPerTick));
                if (nowLinePx < this.Width)
                {
                    Pen nowPen = new Pen(Color.Red);
                    g.DrawLine(nowPen, nowLinePx, graphArea.Y + 1, nowLinePx, graphArea.Y + graphArea.Height - 1);
                    nowPen.Dispose();
                }
            }          
        }
    }
}
