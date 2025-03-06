using System.Windows;
using MonteCarloLib;
using ScottPlot.WPF;

namespace MonteCarloVisualizer
{
    public partial class MainWindow : Window
    {
        private MonteCarloLogic _simulation;
        private List<double> _valuesA = new();
        private List<double> _valuesB = new();
        private List<double> _valuesC = new();
        private List<double> _valuesD = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartSimulation(int numReps, int interval, int seed)
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

            _simulation = new MonteCarloLogic(seed);

            _simulation.NewValueA += NewDataA;
            _simulation.NewValueB += NewDataB;
            _simulation.NewValueC += NewDataC;
            _simulation.NewValueD += NewDataD;

            Task.Run(() =>
            {
                _simulation.Start(numReps, interval);
            });
        }

        private void NewDataA(object? sender, double e) => UpdateUI(_valuesA, e, CurrentValueALabel, StabilizationPlot1);
        private void NewDataB(object? sender, double e) => UpdateUI(_valuesB, e, CurrentValueBLabel, StabilizationPlot2);
        private void NewDataC(object? sender, double e) => UpdateUI(_valuesC, e, CurrentValueCLabel, StabilizationPlot3);
        private void NewDataD(object? sender, double e) => UpdateUI(_valuesD, e, CurrentValueDLabel, StabilizationPlot4);

        private void UpdateUI(List<double> values, double newValue, System.Windows.Controls.Label label, WpfPlot plot)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                values.Add(newValue);
                label.Content = $"{newValue:F4}";
                UpdatePlot(values, plot);
            });
        }

        private void UpdatePlot(List<double> values, WpfPlot plot)
        {
            if (values.Count == 0) return; 
            
            double[] xData = Enumerable.Range(1, values.Count).Select(i => (double)i).ToArray();
            double[] yData = values.ToArray();

            plot.Plot.Clear();
            plot.Plot.Add.Scatter(xData, yData);
            plot.Plot.Axes.AutoScale();
            plot.Refresh();
        }

        private void RunSimulation_OnClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ReplicationCountInput.Text, out int numReps) &&
                int.TryParse(IntervalInput.Text, out int interval) &&
                int.TryParse(SeedInput.Text, out int seed))
            {
                StartSimulation(numReps, interval, seed);
            }
            else
            {
                MessageBox.Show("Zadajte platné čísla pre všetky parametre!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopSimulation_OnClick(object sender, RoutedEventArgs e)
        {
            _simulation?.Stop();
        }
    }
}
