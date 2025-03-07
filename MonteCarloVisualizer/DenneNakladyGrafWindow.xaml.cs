using System.Windows;
using DSLib.MonteCarlo;
using ScottPlot.WPF;

namespace MonteCarloVisualizer
{
    public partial class DenneNakladyGrafWindow : Window
    {
        #region Class members
        private List<double> _values = new();
        private double _curr = 0.0;
        private MonteCarloLib.MonteCarloLogic _monteCarloLogic;
        #endregion // Class members

        #region Constructor
        public DenneNakladyGrafWindow(int seed, string strategy)
        {
            InitializeComponent();
            _monteCarloLogic = new MonteCarloLib.MonteCarloLogic(seed);
            _values.Clear();
            NakladyPlot.Plot.Clear();
            _monteCarloLogic.NewNaklady += NewData;

            JanStrategy strat = JanStrategy.A;
            switch (strategy)
            {
                case "A":
                    strat = JanStrategy.A;
                    break;
                case "B":
                    strat = JanStrategy.B;
                    break;
                case "C":
                    strat = JanStrategy.C;
                    break;
                case "D":
                    strat = JanStrategy.D;
                    break;
                default:
                    break;
            }

            Task.Run(() =>
            {
                _monteCarloLogic.StartOne(strat);
            });
        }
        #endregion // Constructor

        #region Private functions
        private void NewData(object? sender, double e) => UpdateUI(_values, e, NakladyPlot);

        private void UpdateUI(List<double> values, double newValue, WpfPlot plot)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _curr += newValue;
                values.Add(_curr);
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
        #endregion // Private functions
    }
}
