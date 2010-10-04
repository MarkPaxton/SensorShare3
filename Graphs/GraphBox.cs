using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace mcp.Graphs
{

   /// <summary>
   /// GraphBox: a box containing a graph
   /// </summary>
   public abstract class GraphBox : UserControl
   {
      protected Hashtable xyData;
      protected ArrayList data;

      protected double percentOfXToFill = 0.95;
      protected double maxY = 10;
      protected double minY = 0;
      protected double interceptX = 0;
      protected double interceptY = 0;

      protected bool fixedY = true;
      protected bool showNegitiveYLables = true;

      protected bool suspendPaint = false;

      protected Rectangle graphArea;

      public GraphBox()
         : base()
      {
         /// <summary>
         /// Required for Windows.Forms Class Composition Designer support
         /// </summary>
         InitializeComponent();
         _updateGraphInvoker = new EventHandler(UpdateGraphInvoke);
         graphArea = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

         calcAxisDimensions();

         xyData = new Hashtable();
      }

      public void PercentOfXToFill(double percent)
      {
         if (percent > 1)
         {
            throw new ArgumentException("Percent cannot be greater than 1", "percent");
         }
         percentOfXToFill = percent;
         calcAxisDimensions();
         DoUpdateGraphInvoke();
      }

      #region Component Designer generated code
      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.SuspendLayout();
         // 
         // GraphBox
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Name = "GraphBox";
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphBox_Paint);
         this.ResumeLayout(false);

      }
      #endregion

      public double MaxY
      {
         get { return maxY; }
         set
         {
            maxY = value;
            calcAxisDimensions();
            DoUpdateGraphInvoke();
         }
      }

      public double MinY
      {
         get { return minY; }
         set
         {
            minY = value;
            calcAxisDimensions();
            DoUpdateGraphInvoke();
         }
      }

      public bool FixedY
      {
         get { return fixedY; }
         set { fixedY = value; }
      }

      public bool ShowNegitiveYLables
      {
         get { return showNegitiveYLables; }
         set
         {
            showNegitiveYLables = value;
            DoUpdateGraphInvoke();
         }
      }

      protected virtual void GraphBox_Paint(object sender, PaintEventArgs e)
      {
         SuspendLayout();
         PaintGraph(sender, e);
         ResumeLayout();
      }

      protected abstract void PaintGraph(object sender, PaintEventArgs e);



      protected void Plot(object x, double y)
      {
         lock (xyData)
         {
            xyData[x] = y;

            if (!fixedY)
            {
               maxY = Math.Max(y, maxY);
               minY = Math.Min(y, maxY);
            }
            calcAxisDimensions();
         }
         DoUpdateGraphInvoke();
      }

      // How many pixels per unit on the Y axis - need to add to set on max and min y 
      protected double yPxPerUnit;

      // Pixel row where Y axis intercepts the X axis
      protected int interceptXPx;


      protected virtual void calcAxisDimensions()
      {
         double yRange = maxY - minY;
         if (yRange <= 0)
         {
            //If there's only one Y value then make the graph fill half the box
            yPxPerUnit = graphArea.Height / (2 * MaxY);
            interceptXPx = (int)Math.Round(0.66 * graphArea.Height);
         }
         else
         {
            // If it's a dynamic Y axis, then leave a 2.5% gap at the top and bottom
            yPxPerUnit = graphArea.Height / (yRange * 1.05);
            interceptXPx = (int)Math.Round(graphArea.Height + graphArea.Y - yPxPerUnit * (interceptX - MinY));
         }
      }

      public void RemoveItem(object key)
      {
         lock (xyData)
         {
            if (xyData.ContainsKey(key))
            {
               xyData.Remove(key);
            }
         }
      }

      protected delegate void updateGraph();
      protected void UpdateGraph()
      {
         this.Refresh();
      }

      //  Helpers to transfer control to UI thread for display updates
      protected EventHandler _updateGraphInvoker = null;

      protected virtual void DoUpdateGraphInvoke()
      {
         if (!suspendPaint)
         {
            if (this.InvokeRequired)
            {
               this.Invoke(_updateGraphInvoker);
            }
            else
            {
               UpdateGraph();
            }
         }
      }


      protected void UpdateGraphInvoke(object sender, EventArgs e)
      {
         UpdateGraph();
      }

      public void SuspendPaint()
      {
         suspendPaint = true;
      }

      public virtual void ResumePaint()
      {
         suspendPaint = false;
         DoUpdateGraphInvoke();
      }

      public void Redraw()
      {
         DoUpdateGraphInvoke();
      }

      public void Clear()
      {
         lock (xyData)
         {
            xyData.Clear();
         }
         DoUpdateGraphInvoke();
      }
   }
}