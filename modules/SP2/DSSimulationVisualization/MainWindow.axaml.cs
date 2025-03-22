using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ScottPlot.Avalonia;

namespace DSSimulationVisualization;

public partial class MainWindow : Window
{
    #region Class members
    private int _skipFirst = 0;
    #endregion // Class members
    
    #region Constructor
    public MainWindow()
    {
        InitializeComponent();
        SeedInput.Text = new Random().Next(0, 1000).ToString();
    }
    #endregion // Constructor
    
    #region Private functions
    private void StartSimulation(int numReps, int seed)
    {
        StabilizationPlot1.Plot.Clear();
        StabilizationPlot1.Refresh();
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

    private void SpeedSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        var newVal = (double)e.NewValue;
        SpeedLabel.Content = $"{newVal:F0}";
    }

    private void VirtualSpeedCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (VirtualSpeedCheckBox.IsChecked == true) SpeedSlider.IsEnabled = false;
        else SpeedSlider.IsEnabled = true;
        
    }
}