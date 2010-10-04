using System;
using System.Drawing;
using System.Windows.Forms;

namespace mcp.Graphs
{
    public class BarGraphBox : GraphBox
    {
        public BarGraphBox()
            : base()
        {
        }

        public BarGraphBox(double minY, double maxY, int pointsToPlot)
            : base()
        {
            MinY = minY;
            MaxY = maxY;
            this.PointsToPlot = pointsToPlot;
        }

        protected int pointsToPlot = 10;
        public int PointsToPlot
        {
            get { return pointsToPlot; }
            set
            {
                pointsToPlot = value;
                this.Refresh();
            }
        }


        public void Plot(int xVal, double yVal)
        {
            if (xyData.Count == pointsToPlot)
            {
                int[] keys = new int[pointsToPlot];
                xyData.Keys.CopyTo(keys, 0);
                xyData.Remove(keys[0]);
            }
            base.Plot(xyData.Count, yVal);
        }

        protected override void PaintGraph(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            graphArea = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            Pen dPen = new Pen(Color.SteelBlue);
            g.DrawRectangle(dPen, graphArea);

            int xStep = (int)Math.Round((double)graphArea.Width / pointsToPlot);

            double yScale;
            int interceptXPx;

            double yRange = maxY - minY;
            if (yRange <= 0)
            {
                //If there's only one Y value then make the graph fill half the box
                yScale = graphArea.Height / 2;
                interceptXPx = (int)Math.Round(graphArea.Height - (yScale * (maxY - interceptX)));
            }
            else
            {
                // If it's a dynamic Y axis, then leave a 2.5% gap at the top and bottom
                yScale = graphArea.Height / (yRange * 1.05);
                interceptXPx = (int)Math.Round((graphArea.Height / yRange) * (maxY - interceptX));
            }

            int xPosition;
            double point, yToPlot;
            SolidBrush dBrush = new SolidBrush(Color.Tomato);
            Rectangle rect;

            double distanceFromX;
            int yPosition;
            int yHeight;

            lock (xyData)
            {
                foreach (int xValue in xyData.Keys)
                {
                    xPosition = xValue * xStep;
                    point = (double)xyData[xValue];
                    if (point > maxY)
                    {
                        // point goes off top - set height to be max 
                        yToPlot = maxY;
                    }
                    else
                    {
                        if (point < minY)
                        {
                            // point goes off bottom - set height to minY
                            yToPlot = minY;
                        }
                        else
                        {
                            // point is on graph
                            yToPlot = point;
                        }
                    }

                    //is it above or below the X axis?
                    if (yToPlot > interceptX)
                    {
                        distanceFromX = (yToPlot - interceptX) * yScale;
                        yPosition = (int)Math.Round(interceptXPx - distanceFromX);
                    }
                    else
                    {
                        distanceFromX = (interceptX - yToPlot) * yScale;
                        yPosition = interceptXPx;
                    }
                    yHeight = (int)Math.Round(distanceFromX);

                    rect = new Rectangle(xPosition, yPosition, xStep, yHeight);
                    g.DrawRectangle(dPen, rect);
                    g.FillRectangle(dBrush, rect);
                }
            }


            // If y crosses interceptX then draw the axis
            if ((minY < interceptX) && (maxY > interceptX))
            {
                g.DrawLine(dPen, 0, interceptXPx, graphArea.Width, interceptXPx);
            }

            //If the graph is wide enough, print the latest figure.
            if ((xyData.Count > 0) && graphArea.Width > 40)
            {
                string drawString = String.Format("{0}", xyData[xyData.Count - 1]);
                Font drawFont = new Font("Arial", 8, FontStyle.Regular);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                g.DrawString(drawString, drawFont, drawBrush, graphArea.Width - 30, 10);
                drawBrush.Dispose();
                drawFont.Dispose();
            }
            dPen.Dispose();
            dBrush.Dispose();
        }
    }

}
