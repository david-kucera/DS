using ScottPlot;

namespace DS;

internal static class Program
{
    private static void Main()
    {
        const int numReps = 1_000;
        const double L = 5.0;
        const double D = 10.0;
        Random rndY = new();
        Random rndA = new();

        BuffonNeedle buffonNeedle = new BuffonNeedle(rndY, rndA, D, L);
        
        Plot(buffonNeedle, numReps);
        
        var output = buffonNeedle.Run(numReps);
        var piEval = (2 * L)/ (D * output);
        Console.WriteLine($"PI: {piEval}");
    }

    public static void Plot(BuffonNeedle buffonNeedle, int numReps)
    {
        var piEstimates = buffonNeedle.RunWithTracking(numReps);
        var plt = new Plot();
        double[] xValues = new double[numReps];
        for (int i = 1; i <= numReps; i++)
        {
            xValues[i - 1] = i;
        }
        double[] yValues = piEstimates.ToArray();
        
        var scatter = plt.Add.Scatter(xValues, yValues);
        scatter.LegendText = "Evaluation of π";
        scatter.LineWidth = 1;
        scatter.LineStyle.Pattern = LinePattern.Dashed;
        
        plt.Title("Convergence of π");
        plt.XLabel("Number of iterations");
        plt.YLabel("Value of π");

        plt.SavePng("buffon_plot.png", 800, 600);
        Console.WriteLine("Plot saved as 'buffon_plot.png'.");
    }
}