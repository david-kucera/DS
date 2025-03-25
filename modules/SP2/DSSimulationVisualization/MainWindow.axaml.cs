using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DSSimulationWoodwork;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace DSSimulationVisualization;

public partial class MainWindow : Window
{
    #region Class members
    private int _skipFirst = 0;
    private Multipliers _multiplierType = Multipliers.One;
    private double _multiplier = 1.0;
    private Stolaren Stolaren;
    #endregion // Class members
    
    #region Constructor
    public MainWindow()
    {
        InitializeComponent();
        SeedInput.Text = new Random().Next(0, 1000).ToString();
    }
    #endregion // Constructor
    
    #region Private functions
    private void StartSimulation(int numReps, int seed, int a, int b, int c)
    {
        Random rnd = new Random(seed);
        _multiplier = 1.0;
        _multiplierType = Multipliers.One;
        VykresliRychlost();
        if (VirtualSpeedCheckBox.IsChecked == false)
        {
            Stolaren = new Stolaren(rnd, a, b, c, false);
            Stolaren.NewSimulationTime += SimulationTime;
            Stolaren.NewSimulationData += SimulationData;
        }
        else Stolaren = new Stolaren(rnd, a, b, c, true);
        
        Stolaren.StopSimulation += SimulationEnd;
        Task.Run((() => Stolaren.Run(numReps))) ;
    }

    private void SimulationEnd(EventArgs obj)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            UkonciButton_OnClick(null, null!);
        });
    }
    
    private void Start()
    {
        if (int.TryParse((string?)ReplicationCountInput.Text, out int numReps) &&
            int.TryParse((string?)SeedInput.Text, out int seed) &&
            int.TryParse((string?)SkipInput.Text, out int skip) &&
            int.TryParse((string?)ACountInput.Text, out int a) &&
            int.TryParse((string?)BCountInput.Text, out int b) &&
            int.TryParse((string?)CCountInput.Text, out int c)
            )
        {
            // if (numReps < 30)
            // {
            //     MessageBoxManager
            //         .GetMessageBoxStandard("Chyba", "Počet replikacií musí byť väčší ako 30!", ButtonEnum.Ok)
            //         .ShowAsync();
            //     return;
            // }
            StartSimulation(numReps, seed, a, b, c);
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

        CurrentSimulationDay.Content = 1;
        CurrentSimulationTime.Content = "06:00:00";
        
        ItemsControlMontazneMiesta.Items.Clear();

        WaitingQueueRezanie.Content = 0;
        WaitingQueueMorenie.Content = 0;
        WaitingQueueSkladanie.Content = 0;
        WaitingQueueKovanie.Content = 0;

        NumberOfOrders.Content = 0;
        NumberOfFinishedOrders.Content = 0;
        
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
        Stolaren.Stop();
        Stolaren.NewSimulationTime -= SimulationTime;
        Stolaren.NewSimulationData -= SimulationData;
        Stolaren.StopSimulation -= SimulationEnd;
    }
    
    private void PozastavButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = false;
        UkonciButton.IsEnabled = true;
        PozastavButton.IsEnabled = false;
        PokracujButton.IsEnabled = true;
        Stolaren.Pause();
    }

    private void PokracujButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = false;
        UkonciButton.IsEnabled = true;
        PozastavButton.IsEnabled = true;
        PokracujButton.IsEnabled = false;
        Stolaren.Continue();
    }
    
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
        if (_multiplierType == Multipliers.Thousand)
        {
            VykresliRychlost();
            return;
        }
        
        _multiplierType++;
        VykresliRychlost();
        Stolaren.Multiplier = _multiplier;
    }

    private void SpomalButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_multiplierType == Multipliers.One)
        {
            VykresliRychlost();
            return;
        }
        
        _multiplierType--;
        VykresliRychlost();
        Stolaren.Multiplier = _multiplier;
    }

    private void VykresliRychlost()
    {
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
        }
        
        if (_multiplierType == Multipliers.Thousand) ZrychliButton.IsEnabled = false;
        else ZrychliButton.IsEnabled = true;
        
        if (_multiplierType == Multipliers.One) SpomalButton.IsEnabled = false;
        else SpomalButton.IsEnabled = true;
    }
    
    private void SimulationTime(double obj)
    {
        int day = (int)(obj / (8 * 60 * 60)) + 1; // Calculate the day (1-based index)
        double timeInDay = obj % (8 * 60 * 60); // Time elapsed in the current simulation day

        // Convert to hours, minutes, and seconds
        int hours = (int)(timeInDay / 3600) + 6; // Add 6 to shift to 6:00 start
        int minutes = (int)(timeInDay % 3600) / 60;
        int seconds = (int)timeInDay % 60;

        string timeString = $"{hours:D2}:{minutes:D2}:{seconds:D2}";

        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            CurrentSimulationTime.Content = timeString;
            CurrentSimulationDay.Content = day;
        });
    }
    
    private void SimulationData(EventArgs obj)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            ItemsControlMontazneMiesta.Items.Clear();

            WaitingQueueRezanie.Content = Stolaren.CakajuceNaRezanie.Count;
            WaitingQueueMorenie.Content = Stolaren.CakajuceNaMorenie.Count;
            WaitingQueueSkladanie.Content = Stolaren.CakajuceNaSkladanie.Count;
            WaitingQueueKovanie.Content = Stolaren.CakajuceNaKovanie.Count;

            NumberOfOrders.Content = Stolaren.PoradieObjednavky;
            NumberOfFinishedOrders.Content = Stolaren.PocetHotovychObjednavok;
            
            foreach (var mm in Stolaren.MontazneMiesta)
            {
                ItemsControlMontazneMiesta.Items.Add(mm.ToString());
            }
        });
    }
    #endregion // Private functions
}