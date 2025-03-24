using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DSSimulationTest;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ScottPlot.Avalonia;

namespace DSSimulationVisualization;

public partial class MainWindow : Window
{
    #region Class members
    private int _skipFirst = 0;
    private Multipliers _multiplierType = Multipliers.One;
    private double _multiplier = 1.0;
    private Predajna Predajna;
    #endregion // Class members
    
    #region Constructor
    public MainWindow()
    {
        InitializeComponent();
        SeedInput.Text = new Random().Next(0, 1000).ToString();
        Random rnd = new Random(int.Parse(SeedInput.Text));
        Predajna = new Predajna(rnd);
        Predajna.NewSimulationTime += SimulationTime;
    }

    private void SimulationTime(double obj)
    {
        int totalSeconds = (int)(obj / 1000); // Prevod milisekúnd na sekundy
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        string timeString = $"{hours:D2}:{minutes:D2}:{seconds:D2}";

        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            CurrentSimulationTime.Content = timeString;
        });
    }
    #endregion // Constructor
    
    #region Private functions
    private void StartSimulation(int numReps, int seed)
    {
        Task.Run((() => Predajna.Run(1))) ;
        // StabilizationPlot1.Plot.Clear();
        // StabilizationPlot1.Refresh();
    }
    
    private void SimulationEnded(object? sender, EventArgs e)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            UkonciButton_OnClick(null, null!);
        });
    }
    
    private void UpdateUi(List<double> values, double newValue, TextBlock label, AvaPlot plot)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            values.Add(newValue);
            label.Text = $"{newValue:F4}";
            UpdatePlot(values, plot);
        });
    }

    private void UpdatePlot(List<double> values, AvaPlot plot)
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
    
    private void Start()
    {
        if (int.TryParse((string?)ReplicationCountInput.Text, out int numReps) &&
            int.TryParse((string?)SeedInput.Text, out int seed) &&
            int.TryParse((string?)SkipInput.Text, out int skip))
        {
            if (numReps < 30)
            {
                MessageBoxManager
                    .GetMessageBoxStandard("Chyba", "Počet replikacií musí byť väčší ako 30!", ButtonEnum.Ok)
                    .ShowAsync();
                return;
            }
            StartSimulation(numReps, seed);
            _skipFirst = (int)((numReps) * ((double)skip / 100.0));
        }
        else
        {
            MessageBoxManager
                .GetMessageBoxStandard("Chyba", "Zadajte platné čísla pre všetky parametre!", ButtonEnum.Ok)
                .ShowAsync();
        }
    }

    private void SpustiButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = false;
        UkonciButton.IsEnabled = true;
        PozastavButton.IsEnabled = true;
        PokracujButton.IsEnabled = false;
        
        VirtualSpeedCheckBox.IsEnabled = false;
        Start();
    }

    private void UkonciButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = true;
        UkonciButton.IsEnabled = false;
        PozastavButton.IsEnabled = false;
        PokracujButton.IsEnabled = false;

        VirtualSpeedCheckBox.IsEnabled = true;
    }
    
    private void PozastavButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = false;
        UkonciButton.IsEnabled = true;
        PozastavButton.IsEnabled = false;
        PokracujButton.IsEnabled = true;
    }

    private void PokracujButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = false;
        UkonciButton.IsEnabled = true;
        PozastavButton.IsEnabled = true;
        PokracujButton.IsEnabled = false;
    }
    #endregion // Private functions
    

    private void VirtualSpeedCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (VirtualSpeedCheckBox.IsChecked == true)
        {
            SpomalButton.IsEnabled = false;
            ZrychliButton.IsEnabled = false;
        }
        else
        {
            VykresliRychlost();
        }
    }

    private void ZrychliButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _multiplierType++;
        VykresliRychlost();
    }

    private void SpomalButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _multiplierType--;
        VykresliRychlost();
    }

    private void VykresliRychlost()
    {
        if (_multiplierType == Multipliers.Thousand) ZrychliButton.IsEnabled = false;
        else ZrychliButton.IsEnabled = true;
        if (_multiplierType == Multipliers.MinusThousand) SpomalButton.IsEnabled = false;
        else SpomalButton.IsEnabled = true;
        
        switch (_multiplierType)
        {
            case Multipliers.One:
                _multiplier = 1.0;
                SpeedLabel.Content = "1x";
                break;
            case Multipliers.Two:
                _multiplier = 2.0;
                SpeedLabel.Content = "2x";
                break;
            case Multipliers.Five:
                _multiplier = 5.0;
                SpeedLabel.Content = "5x";
                break;
            case Multipliers.Ten:
                _multiplier = 10.0;
                SpeedLabel.Content = "10x";
                break;
            case Multipliers.TwentyFive:
                _multiplier = 25.0;
                SpeedLabel.Content = "25x";
                break;
            case Multipliers.Fifty:
                _multiplier = 50.0;
                SpeedLabel.Content = "50x";
                break;
            case Multipliers.Hundred:
                _multiplier = 100.0;
                SpeedLabel.Content = "100x";
                break;
            case Multipliers.TwoHundredFifty:
                _multiplier = 250.0;
                SpeedLabel.Content = "250x";
                break;
            case Multipliers.FiveHundred:
                _multiplier = 500.0;
                SpeedLabel.Content = "500x";
                break;
            case Multipliers.Thousand:
                _multiplier = 1000.0;
                SpeedLabel.Content = "1000x";
                break;
            
            case Multipliers.MinusTwo:
                _multiplier = 1.0/2.0;
                SpeedLabel.Content = "1/2x";
                break;
            case Multipliers.MinusFive:
                _multiplier = 1.0/5.0;
                SpeedLabel.Content = "1/5x";
                break;
            case Multipliers.MinusTen:
                _multiplier = 1.0/10.0;
                SpeedLabel.Content = "1/10x";
                break;
            case Multipliers.MinusTwentyFive:
                _multiplier = 1.0/25.0;
                SpeedLabel.Content = "1/25x";
                break;
            case Multipliers.MinusFifty:
                _multiplier = 1.0/50.0;
                SpeedLabel.Content = "1/50x";
                break;
            case Multipliers.MinusHundred:
                _multiplier = 1.0/100.0;
                SpeedLabel.Content = "1/100x";
                break;
            case Multipliers.MinusTwoHundredFifty:
                _multiplier = 1.0/250.0;
                SpeedLabel.Content = "1/250x";
                break;
            case Multipliers.MinusFiveHundred:
                _multiplier = 1.0/500.0;
                SpeedLabel.Content = "1/500x";
                break;
            case Multipliers.MinusThousand:
                _multiplier = 1.0/1000.0;
                SpeedLabel.Content = "1/1000x";
                break;
        }
    }
}