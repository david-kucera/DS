using System.Windows;
using DSLib.MonteCarlo;

namespace MonteCarloVisualizer;

public partial class MainWindow : Window
{
	private List<double> values = new();
	private BuffonNeedle _simulation;
	private int _updateCount = 0;

	public MainWindow()
    {
        InitializeComponent();

        StabilizationPlot1.Plot.XLabel("Iterácia");
        StabilizationPlot1.Plot.YLabel("Hodnota");
        StabilizationPlot1.Plot.Title("Graf ustáľovania");
	}

	private void NewData(object? sender, double e)
	{
		var newValue = e;
		Application.Current.Dispatcher.Invoke(() =>
		{
			values.Add(newValue);
			CurrentValue1Label.Content = $"{newValue}";
			_updateCount++;
			if (_updateCount % 100 == 0) UpdatePlot();
		});
	}

	private void StartSimulation(int numReps)
	{
		values.Clear();
		StabilizationPlot1.Plot.Clear();
		StabilizationPlot1.Refresh();

		Task.Run(() =>
		{
			double L = 5.0;
			double D = 10.0;
			_simulation = new(new Random(), new Random(), D, L);
			_simulation.NewPiEstimate += NewData;
			_simulation.Run(numReps);
		});
	}

	private void UpdatePlot()
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			if (values.Count > 1)
			{
				double[] xData = Enumerable.Range(1, values.Count).Select(i => (double)i).ToArray();
				double[] yData = values.ToArray();

				StabilizationPlot1.Plot.Clear();
				StabilizationPlot1.Plot.Add.Scatter(xData, yData);
				StabilizationPlot1.Refresh();
			}
		});
	}

	private void RunSimulation_OnClick(object sender, RoutedEventArgs e)
	{
		if (int.TryParse(ReplicationCountInput.Text, out int numReps))
		{
			StartSimulation(numReps);
		}
	}

	private void StopSimulation_OnClick(object sender, RoutedEventArgs e)
	{
		_simulation.NewPiEstimate -= NewData;
	}
}