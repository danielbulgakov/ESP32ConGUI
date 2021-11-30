using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace ESP32ConGUI
{
    class Draw
    {
        public void DrawCircle(Chart Graph, double centerX, double centerY, double radius, int amountOfEdges)
        {


            string name = "circle";

            // Create new data series
            if (Graph.Series.IndexOf(name) == -1)
                Graph.Series.Add(name);

            Graph.Series[0].Points.Clear();
            Graph.Series[1].Points.Clear();



            // preferences of the line
            Graph.Series[name].ChartType = SeriesChartType.Spline;
            Graph.Series[name].Color = Color.FromArgb(0, 0, 0);
            Graph.Series[name].BorderWidth = 1;
            Graph.Series[name].IsVisibleInLegend = false;

            // add line segments (first one also as last one)
            for (int k = 0; k <= amountOfEdges; k++)
            {
                double x = centerX + radius * Math.Cos(k * 2 * Math.PI / amountOfEdges);
                double y = centerY + radius * Math.Sin(k * 2 * Math.PI / amountOfEdges);
                Graph.Series[name].Points.AddXY(x, y);
            }
            
        }

        public void DrawLine(Chart Graph, double z, int xShape_z)
        {
            string name = "line";
            if (Graph.Series.IndexOf(name) == -1)
                Graph.Series.Add(name);

            Graph.Series[0].Points.Clear();
            Graph.Series[1].Points.Clear();

            Graph.Series[name].ChartType = SeriesChartType.Line;
            Graph.Series[name].Color = Color.FromArgb(0, 0, 0);
            Graph.Series[name].BorderWidth = 1;
            Graph.Series[name].IsVisibleInLegend = false;
            
            for (int k = 0; k <= xShape_z; k++)
            {
                double x = k;
                double y = z;
                Graph.Series[name].Points.AddXY(x, y);
            }
            


        }
    }
}
