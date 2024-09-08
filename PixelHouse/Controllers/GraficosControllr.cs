using Microsoft.AspNetCore.Mvc;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

public class GraphController : Controller
{
    public IActionResult Graficos()
    {
        var plotModel = new PlotModel { Title = "Sample Graph" };

        var lineSeries = new LineSeries
        {
            Title = "Sample Data",
            Color = OxyColors.Blue
        };

        lineSeries.Points.Add(new DataPoint(0, 0));
        lineSeries.Points.Add(new DataPoint(1, 2));
        lineSeries.Points.Add(new DataPoint(2, 1));
        lineSeries.Points.Add(new DataPoint(3, 3));

        plotModel.Series.Add(lineSeries);

        // Serialize PlotModel to PNG image
        using (var stream = new MemoryStream())
        {
            var pngExporter = new PngExporter { Width = 600, Height = 400 };
            pngExporter.Export(plotModel, stream);
            var chartImage = Convert.ToBase64String(stream.ToArray());
            ViewData["ChartImage"] = chartImage;
        }

        return View();
    }
}
