using System.Windows;
using MonteCarloLib;
using ScottPlot.WPF;

namespace MonteCarloVisualizer
{
    public partial class MainWindow : Window
    {
        #region Class members
        private MonteCarloLogic _simulation = null!;
        private int _skipFirst = 0;
		private readonly List<double> _valuesA = [];
        private readonly List<double> _valuesB = [];
        private readonly List<double> _valuesC = [];
        private readonly List<double> _valuesD = [];
        #endregion // Class members

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            SeedInput.Text = new Random().Next(0, 1000).ToString();
		}
        #endregion // Constructor

        #region Private functions
        private async void StartSimulation(int numReps, int interval, int seed, MonteCarloLogic.Strategies strategies)
        {
            _valuesA.Clear();
            _valuesB.Clear();
            _valuesC.Clear();
            _valuesD.Clear();

            StabilizationPlot1.Plot.Clear();
            StabilizationPlot2.Plot.Clear();
            StabilizationPlot3.Plot.Clear();
            StabilizationPlot4.Plot.Clear();

            StabilizationPlot1.Refresh();
            StabilizationPlot2.Refresh();
            StabilizationPlot3.Refresh();
            StabilizationPlot4.Refresh();

            _simulation = new MonteCarloLogic(seed, strategies);
            _simulation.SimulationEnded += SimulationEnded;

			_simulation.NewValueA += NewDataA;
            _simulation.NewNakladyA += NewDataA;
			_simulation.NewValueB += NewDataB;
            _simulation.NewNakladyB += NewDataB;
			_simulation.NewValueC += NewDataC;
			_simulation.NewNakladyC += NewDataC;
			_simulation.NewValueD += NewDataD;
			_simulation.NewNakladyD += NewDataD;

			await Task.Run(() => _simulation.Start(numReps, interval));
        }

        private void SimulationEnded(object? sender, EventArgs e)
        {
	        Application.Current.Dispatcher.BeginInvoke(() =>
	        {
				SpustiButton.IsEnabled = true;
				ZastavButton.IsEnabled = false;
                SpustiMojeStrategieButton.IsEnabled = true;
			});
		}

        private void NewDataA(object? sender, double e) => UpdateUi(_valuesA, e, CurrentValueALabel, StabilizationPlot1);
        private void NewDataB(object? sender, double e) => UpdateUi(_valuesB, e, CurrentValueBLabel, StabilizationPlot2);
        private void NewDataC(object? sender, double e) => UpdateUi(_valuesC, e, CurrentValueCLabel, StabilizationPlot3);
        private void NewDataD(object? sender, double e) => UpdateUi(_valuesD, e, CurrentValueDLabel, StabilizationPlot4);

        private void UpdateUi(List<double> values, double newValue, System.Windows.Controls.Label label, WpfPlot plot)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                values.Add(newValue);
                label.Content = $"{newValue:F4}";
                UpdatePlot(values, plot);
            });
        }

        private void UpdatePlot(List<double> values, WpfPlot plot)
        {
            if (values.Count == 0) return; 
            if (values.Count < _skipFirst) return;

			values = values.Skip(_skipFirst).ToList();

			double[] xData = Enumerable.Range(1 + _skipFirst, values.Count).Select(i => (double)i).ToArray();
            double[] yData = values.ToArray();

            plot.Plot.Clear();
            plot.Plot.Add.Scatter(xData, yData);
            plot.Plot.Axes.AutoScale();
            plot.Refresh();
        }

        private void RunSimulation_OnClick(object sender, RoutedEventArgs e)
        {
			Start(MonteCarloLogic.Strategies.Classic);
        }

        private void SpustiMojeStrategieButton_OnClick(object sender, RoutedEventArgs e)
        {
	        Start(MonteCarloLogic.Strategies.Own);
        }

        private void Start(MonteCarloLogic.Strategies strat)
        {
			if (int.TryParse(ReplicationCountInput.Text, out int numReps) &&
	            int.TryParse(IntervalInput.Text, out int interval) &&
	            int.TryParse(SeedInput.Text, out int seed) &&
	            int.TryParse(SkipInput.Text, out int skip))
	        {
		        StartSimulation(numReps, interval, seed, strat);
		        _skipFirst = (int)((numReps / interval) * ((double)skip / 100.0));
				SpustiButton.IsEnabled = false;
		        SpustiMojeStrategieButton.IsEnabled = false;
		        ZastavButton.IsEnabled = true;
	        }
	        else
	        {
		        MessageBox.Show("Zadajte platné čísla pre všetky parametre!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
	        }
		}

		private void StopSimulation_OnClick(object sender, RoutedEventArgs e)
        {
            ZastavButton.IsEnabled = false;
            SpustiMojeStrategieButton.IsEnabled = true;
			SpustiButton.IsEnabled = true;
            _simulation?.Stop();
		}
        #endregion // Private functions

        
    }
}
