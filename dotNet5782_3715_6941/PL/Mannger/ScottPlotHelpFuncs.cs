using System;
using System.Drawing;


namespace PL
{
    /// <summary>
    /// Scott Plot Functions to create some coolm graphs 
    /// </summary>
    public static class ScottPlotHELP
    {
        #region ScottPlotsFunctions
        internal static void createModelsBar(ScottPlot.WpfPlot Bar, double[] pos, string[] names, double[] vals)
        {
            Bar.Plot.Clear();
            Bar.Plot.AddBar(vals, pos, ColorTranslator.FromHtml("#6600cc"));
            Bar.Plot.XTicks(pos, names);
            Bar.Plot.SetAxisLimits(yMin: 0);



        }
        internal static void CreateSingleGauge<T>(ScottPlot.WpfPlot Gauge, double[] values)
        {

            Gauge.Plot.Clear();
            Gauge.Plot.Palette = ScottPlot.Drawing.Palette.Nord;

            var gauges = Gauge.Plot.AddRadialGauge(values);
            gauges.Clockwise = false;

            Gauge.Plot.AxisAuto(0);



        }
        internal static void CreateDountPie<T>(ScottPlot.WpfPlot Pie, double[] values)
        {
            Pie.Plot.Clear();
            string[] labels = Enum.GetNames(typeof(T));
            if (labels.Length >= 4)
            {
                string tmp = labels[0];
                double tmp2 = values[0];
                values[0] = values[1];
                labels[0] = labels[1];
                values[1] = tmp2;
                labels[1] = tmp;
                    }

            // Language colors from https://github.com/ozh/github-colors
            System.Drawing.Color[] sliceColors =
            {
                ColorTranslator.FromHtml("#DBCDC6"),
                ColorTranslator.FromHtml("#DD99BB"),
                ColorTranslator.FromHtml("#7B506F"),
                ColorTranslator.FromHtml("#1F1A38"),
                ColorTranslator.FromHtml("#C7EFCF"),
};

            // Show labels using different transparencies
            System.Drawing.Color[] labelColors =
                new System.Drawing.Color[] {
     System.Drawing.Color.FromArgb(255,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(100,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(250,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(150,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(200,  System.Drawing.Color.White),
            };

            var pie = Pie.Plot.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.ShowPercentages = true;

            pie.SliceFillColors = sliceColors;
            pie.SliceLabelColors = labelColors;



        }
        internal static void ClearGraph(ScottPlot.WpfPlot graph)
        {
            graph.Plot.Clear();

        }

        #endregion


    }
}