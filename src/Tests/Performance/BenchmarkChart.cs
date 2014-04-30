using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using Flexo.Extensions;

namespace Tests.Performance
{
    public static class BenchmarkChart
    {
        public static void SaveSummary(IEnumerable<Benchmarks.Benchmark> results, string path, bool cold, bool warm)
        {
            var chart = new Chart
            {
                Size = new Size(800, 400)
            };

            chart.Legends.Add(new Legend { 
                Alignment = StringAlignment.Center,
                Docking = Docking.Bottom,
                Enabled = true,
                IsDockedInsideChartArea = false,
                TableStyle = LegendTableStyle.Wide 
            });

            chart.ChartAreas.Add(new ChartArea
            {
                Position =
                {
                    X = 1,
                    Y = 1,
                    Width = 100,
                    Height = 93
                },
                AxisY =
                {
                    Enabled = AxisEnabled.False,
                    MajorGrid = { Enabled = false }
                },
                AxisX =
                {
                    LabelStyle = { Interval = 1 },
                    LineWidth = 0,
                    LineColor = Color.White,
                    MajorTickMark = { Enabled = false },
                    MajorGrid = { Enabled = false }
                }
            });

            Action<Func<Benchmarks.Benchmark, long>, string, Color> createSeries = (timing, label, color) =>
            {
                var series = new Series
                {
                    XValueMember = "Name",
                    ChartType = SeriesChartType.StackedBar,
                    Color = color,
                    LegendText = label,
                    IsVisibleInLegend = true
                };

                series["PixelPointWidth"] = "10";

                results.OrderBy(x => x.Name).ForEach(x => series.Points.AddXY(
                    "{0} {1}".ToFormat(x.Name, x.Format), timing(x)));

                chart.Series.Add(series);
            };

            if (warm)
            {
                createSeries(y => y.WarmDeserialize + y.WarmParse, "Warm Deserialization", Color.Red);
                createSeries(y => y.WarmSerialize + y.WarmEncode, "Warm Serialization", Color.DarkSalmon);
            }

            if (cold)
            {
                createSeries(y => y.ColdDeserialize + y.ColdParse, "Cold Deserialization", Color.Blue);
                createSeries(y => y.ColdSerialize + y.ColdEncode, "Cold Serialization", Color.LightBlue);
            }

            chart.SaveImage(path, ChartImageFormat.Png);
        }
    }
}