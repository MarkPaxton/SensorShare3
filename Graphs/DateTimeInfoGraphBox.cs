using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace mcp.Graphs
{
   public class DateTimeInfoGraphBox : DateTimeGraphBox
   {
      protected struct DateIndex
      {
         public DateTime date;
         public Guid index;
         public int iconIndex;
         public DateIndex(DateTime date, Guid index, int iconIndex)
         {
            this.date = date;
            this.index = index;
            this.iconIndex = iconIndex;
         }
      }

      protected Predicate<DateIndex> itemsWithinGraphRange;
      protected Predicate<DateIndex> itemsWithinMouseRange;

      DateTime startMouseTime;
      DateTime endMouseTime;

      protected int yPositionOfInfoPoints;
      int defaultInfoBoxSize = 20;

      private List<Bitmap> iconImages = new List<Bitmap>();
      public List<Bitmap> IconImages
      {
         get { return iconImages; }
         set
         {
            iconImages = value;
            this.Redraw();
         }
      }

 
      protected List<DateIndex> infoPoints;


      public event InfoPointSelectedEventHandler InfoPointSelected;

      public DateTimeInfoGraphBox()
         : base()
      {
         yPositionOfInfoPoints = (int)Math.Round((double)this.Height / 2);
         itemsWithinGraphRange = new Predicate<DateIndex>(WithinGraphRange);
         itemsWithinMouseRange = new Predicate<DateIndex>(WithinMouseRange);
         infoPoints = new List<DateIndex>();
         this.MouseUp += new MouseEventHandler(DateTimeInfoGraphBox_MouseUp);
      }


      protected void FireOnInfoPointSelected(List<Guid> list)
      {
         InfoPointSelected(this, new InfoPointSelectedEventArgs(list));
      }

      protected bool WithinGraphRange(DateIndex target)
      {
         return ((target.date >= startTime) && (target.date <= endTime));
      }

      protected bool WithinMouseRange(DateIndex target)
      {
         return ((target.date >= startMouseTime) && (target.date <= endMouseTime));
      }

      protected int CompareDates(DateIndex x, DateIndex y)
      {
         return (x.date.CompareTo(y.date));
      }

      void DateTimeInfoGraphBox_MouseUp(object sender, MouseEventArgs e)
      {
         int iconHeight;
         int iconWidth;
 
         iconHeight = defaultInfoBoxSize;
         iconWidth = defaultInfoBoxSize;
      
         if ((e.Y <= (yPositionOfInfoPoints + iconHeight)) &&
             (e.Y >= yPositionOfInfoPoints))
         {
            DateTime mouseTime = startTime.AddTicks((long)Math.Round((e.X - xMargin) / xPxPerTick));
            startMouseTime = mouseTime.AddTicks((long)Math.Round(-1 * (iconWidth / 2) / xPxPerTick));
            endMouseTime = mouseTime.AddTicks((long)Math.Round((iconWidth / 2) / xPxPerTick));

            //Debug.WriteLine(String.Format("Mouseup {0}\r\n{1}\r\n{2}",
            //    mouseTime.ToLocalTime(), startMouseTime.ToLocalTime(), endMouseTime.ToLocalTime()));

            List<DateIndex> infoPointsSelected = infoPoints.FindAll(itemsWithinMouseRange);
            if (infoPointsSelected.Count > 0)
            {
               Debug.WriteLine(String.Format("{0} points selected", infoPointsSelected.Count));
               List<Guid> indexes = new List<Guid>();
               foreach (DateIndex dateIndex in infoPointsSelected)
               {
                  indexes.Add(dateIndex.index);
               }
               FireOnInfoPointSelected(indexes);
            }
         }
      }

      public void AddInfoPoint(DateTime time, Guid index, int iconIndex)
      {
         infoPoints.Add(new DateIndex(time, index, iconIndex));
      }

      protected override void PaintGraph(object sender, System.Windows.Forms.PaintEventArgs e)
      {
         base.PaintGraph(sender, e);
         drawInfoPoints(e.Graphics);
      }

      protected void drawInfoPoints(Graphics g)
      {
         List<DateIndex> pointsToPlot = infoPoints.FindAll(itemsWithinGraphRange);
         int xPosition;
         foreach (DateIndex item in pointsToPlot)
         {
            if ((iconImages.Count == 0) || (iconImages.Count<=item.iconIndex))
            {
               xPosition = xMargin + (int)Math.Round(xPxPerTick * (item.date.Ticks - startTime.Ticks)) - 3;
               if (xPosition > xMargin)
               {
                  Rectangle rectangleForPoint = new Rectangle(xPosition, (int)Math.Round((double)this.Height / 2),
                      defaultInfoBoxSize, defaultInfoBoxSize);
                  Pen bluePen = new Pen(Color.Blue);
                  SolidBrush blueBrush = new SolidBrush(Color.Blue);
                  g.DrawEllipse(bluePen, rectangleForPoint);
                  g.FillEllipse(blueBrush, rectangleForPoint);
                  bluePen.Dispose();
                  blueBrush.Dispose();
               }
            }
            else
            {
               xPosition = xMargin + (int)Math.Round((xPxPerTick * (item.date.Ticks - startTime.Ticks)) - (iconImages[item.iconIndex].Width / 2));
               if (xPosition > xMargin)
               {
                  ImageAttributes infoImageAttributes = new ImageAttributes();
                  infoImageAttributes.SetColorKey(iconImages[item.iconIndex].GetPixel(0, 0), iconImages[item.iconIndex].GetPixel(0, 0));
                  g.DrawImage(iconImages[item.iconIndex], new Rectangle(xPosition, yPositionOfInfoPoints, iconImages[item.iconIndex].Width, iconImages[item.iconIndex].Height),
                     0, 0, iconImages[item.iconIndex].Width, iconImages[item.iconIndex].Height, GraphicsUnit.Pixel, infoImageAttributes);
               }
            }
         }
      }
   }

    public delegate void InfoPointSelectedEventHandler(object sender, InfoPointSelectedEventArgs args);

    public class InfoPointSelectedEventArgs : EventArgs
    {
       public readonly List<Guid> SelectedInfoPoints;

       public InfoPointSelectedEventArgs(List<Guid> selectedPoints)
            : base()
        {
            SelectedInfoPoints = selectedPoints;
        }
    }
}
