using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DSSimulationLib.Statistics;
using DSSimulationWoodwork;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ScottPlot;
using ScottPlot.Plottables;

namespace DSSimulationVisualization;

public partial class MainWindow : Window
{
    #region Class members
    private int _skipFirst = 0;
    private int _totalValuesProcessed = 0;
    private int _interval = 1000;
    private int _dataCounter = 0;
    private DataLogger _replicationValuesMean = new();
    private DataLogger _replicationValuesTop = new();
    private DataLogger _replicationValuesBottom = new();
    private Multipliers _multiplierType = Multipliers.One;
    private double _multiplier = 1.0;
    private bool _ignoreData = false;
    private Stolaren _stolaren;
    #endregion // Class members
    
    #region Constructor
    public MainWindow()
    {
        InitializeComponent();
        SeedInput.Text = new Random().Next(0, 1000).ToString();
        IntervalInput.Text = "1";
        ACountInput.Text = "2";
        BCountInput.Text = "2";
        CCountInput.Text = "18";
        ReplicationCountInput.Text = "1000";
        SkipInput.Text = "5";
        StabilizationPlot.Plot.XLabel("Replication");
        StabilizationPlot.Plot.YLabel("Hodnota");
    }
    #endregion // Constructor
    
    #region Private functions
    private void StartSimulation(int numReps, int seed, int a, int b, int c)
    {
        Random rnd = new Random(seed);
        _dataCounter = 0;
        _totalValuesProcessed = 0;
        _multiplier = 1.0;
        _multiplierType = Multipliers.One;
        VykresliRychlost();
        
        if (VirtualSpeedCheckBox.IsChecked == false)
        {
            _stolaren = new Stolaren(rnd, a, b, c, false);
            _stolaren.NewSimulationData += SimulationData;
            _stolaren.NewSimulationTime += SimulationTime;
        }
        else
        {
            _stolaren = new Stolaren(rnd, a, b, c, true);
            _stolaren.NewReplicationData += ReplicationData;
        }
        _stolaren.StopSimulation += SimulationEnd;
        Task.Run(() => _stolaren.Run(numReps)) ;
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
            int.TryParse((string?)CCountInput.Text, out int c) &&
            int.TryParse((string?)IntervalInput.Text, out int interval)
            )
        {
            _interval = interval;
            _skipFirst = (int)((numReps) * ((double)skip / 100.0));
            StartSimulation(numReps, seed, a, b, c);
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
        
        StabilizationPlot.Plot.Clear();
        _replicationValuesMean.Clear();
        _replicationValuesTop.Clear();
        _replicationValuesBottom.Clear();

        _replicationValuesMean = StabilizationPlot.Plot.Add.DataLogger();
        _replicationValuesMean.Color = Colors.Blue;
        _replicationValuesTop = StabilizationPlot.Plot.Add.DataLogger();
        _replicationValuesTop.Color = Colors.Red;
        _replicationValuesBottom = StabilizationPlot.Plot.Add.DataLogger();
        _replicationValuesBottom.Color = Colors.Green;
        
        StabilizationPlot.Refresh();
        

        WaitingQueueRezanie.Content = 0;
        WaitingQueueMorenie.Content = 0;
        WaitingQueueSkladanie.Content = 0;
        WaitingQueueKovanie.Content = 0;

        NumberOfOrders.Content = 0;
        NumberOfFinishedOrders.Content = 0;

        AverageObjednavkaTimeInSystem.Content = 0;
        AverageObjednavkasNotStarted.Content = 0;
        AverageWorkloadAStolar.Content = "0 %";
        AverageWorkloadBStolar.Content = "0 %";
        AverageWorkloadCStolar.Content = "0 %";
        
        VirtualSpeedCheckBox.IsEnabled = false;
        Start();
    }

    private void UkonciButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = true;
        UkonciButton.IsEnabled = false;
        PozastavButton.IsEnabled = false;
        PokracujButton.IsEnabled = false;
        
        if (VirtualSpeedCheckBox.IsChecked == false) ReplicationEnd();

        VirtualSpeedCheckBox.IsEnabled = true;
        _stolaren.Stop();
        _stolaren.NewSimulationData -= SimulationData;
        _stolaren.NewSimulationTime -= SimulationTime;
        _stolaren.NewReplicationData -= ReplicationData;
        _stolaren.StopSimulation -= SimulationEnd;
    }
    
    private void PozastavButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = false;
        UkonciButton.IsEnabled = true;
        PozastavButton.IsEnabled = false;
        PokracujButton.IsEnabled = true;
        _stolaren.Pause();
    }

    private void PokracujButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = false;
        UkonciButton.IsEnabled = true;
        PozastavButton.IsEnabled = true;
        PokracujButton.IsEnabled = false;
        _stolaren.Continue();
    }
    
    private void VirtualSpeedCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (VirtualSpeedCheckBox.IsChecked == true)
        {
            SpomalButton.IsEnabled = false;
            ZrychliButton.IsEnabled = false;
        }
        else VykresliRychlost();
    }

    private void ZrychliButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_multiplierType == Multipliers.TenMillion)
        {
            VykresliRychlost();
            return;
        }
        
        _multiplierType++;
        VykresliRychlost();
        _stolaren.Multiplier = _multiplier;
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
        _stolaren.Multiplier = _multiplier;
    }

    private void VykresliRychlost()
    {
        switch (_multiplierType)
        {
            case Multipliers.One:
                _multiplier = 1.0;
                _ignoreData = false;
                SpeedLabel.Content = "1x";
                break;
            case Multipliers.Two:
                _multiplier = 2.0;
                _ignoreData = false;
                SpeedLabel.Content = "2x";
                break;
            case Multipliers.Five:
                _multiplier = 5.0;
                _ignoreData = false;
                SpeedLabel.Content = "5x";
                break;
            case Multipliers.Ten:
                _multiplier = 10.0;
                _ignoreData = false;
                SpeedLabel.Content = "10x";
                break;
            case Multipliers.TwentyFive:
                _multiplier = 25.0;
                _ignoreData = false;
                SpeedLabel.Content = "25x";
                break;
            case Multipliers.Fifty:
                _multiplier = 50.0;
                _ignoreData = false;
                SpeedLabel.Content = "50x";
                break;
            case Multipliers.Hundred:
                _multiplier = 100.0;
                _ignoreData = false;
                SpeedLabel.Content = "100x";
                break;
            case Multipliers.TwoHundredFifty:
                _multiplier = 250.0;
                _ignoreData = false;
                SpeedLabel.Content = "250x";
                break;
            case Multipliers.FiveHundred:
                _multiplier = 500.0;
                _ignoreData = false;
                SpeedLabel.Content = "500x";
                break;
            case Multipliers.Thousand:
                _multiplier = 1_000.0;
                _ignoreData = false;
                SpeedLabel.Content = "1 000x";
                break;
            case Multipliers.TenThousand:
                _multiplier = 10_000.0;
                _ignoreData = false;
                SpeedLabel.Content = "10 000x";
                break;
            case Multipliers.HundredThousand:
                _multiplier = 100_000.0;
                _ignoreData = true;
                SpeedLabel.Content = "100 000x";
                break;
            case Multipliers.Million:
                _multiplier = 1_000_000.0;
                _ignoreData = true;
                SpeedLabel.Content = "1 000 000x";
                break;
            case Multipliers.TenMillion:
                _multiplier = 10_000_000.0;
                _ignoreData = true;
                SpeedLabel.Content = "10 000 000x";
                break;
        }
        
        if (_multiplierType == Multipliers.TenMillion) ZrychliButton.IsEnabled = false;
        else ZrychliButton.IsEnabled = true;
        
        if (_multiplierType == Multipliers.One) SpomalButton.IsEnabled = false;
        else SpomalButton.IsEnabled = true;
    }

    private void SimulationTime(double time)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            CurrentSimulationTime.Content = FormatTimeAndDay(time);
            CurrentSimulationDay.Content = GetDay(time);
        });
    }
    
    private void SimulationData(EventArgs obj)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (!_ignoreData)
            {
                ItemsControlMontazneMiesta.Items.Clear();
                WaitingQueueRezanie.Content = _stolaren.CakajuceNaRezanie.Count;
                WaitingQueueMorenie.Content = _stolaren.CakajuceNaMorenie.Count;
                WaitingQueueSkladanie.Content = _stolaren.CakajuceNaSkladanie.Count;
                WaitingQueueKovanie.Content = _stolaren.CakajuceNaKovanie.Count;
            }

            NumberOfOrders.Content = _stolaren.PoradieObjednavky;
            NumberOfFinishedOrders.Content = Math.Round(_stolaren.PocetHotovychObjednavok, 4);

            AverageObjednavkaTimeInSystem.Content = FormatTime(_stolaren.PriemernyCasObjednavkyVSysteme.GetValue());
            AverageObjednavkasNotStarted.Content = Math.Round(_stolaren.PriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.GetValue(), 4);

            if (!_ignoreData)
            {
                Average priemernaVytazenostA = new();
                Average priemernaVytazenostB = new();
                Average priemernaVytazenostC = new();
                foreach (var stolar in _stolaren.Stolari)
                {
                    switch (stolar.Type)
                    {
                        case StolarType.A:
                            priemernaVytazenostA.AddValue(stolar.Workload.GetValue());
                            break;
                        case StolarType.B:
                            priemernaVytazenostB.AddValue(stolar.Workload.GetValue());
                            break;
                        default:
                            priemernaVytazenostC.AddValue(stolar.Workload.GetValue());
                            break;
                    }
                }
            
                // AverageWorkloadAStolar.Content = Math.Round(priemernaVytazenostA.GetValue(), 4) + " %";
                // AverageWorkloadBStolar.Content = Math.Round(priemernaVytazenostB.GetValue(), 4) + " %";
                // AverageWorkloadCStolar.Content = Math.Round(priemernaVytazenostC.GetValue(), 4) + " %";
                
                AverageWorkloadAStolar.Content = (int)priemernaVytazenostA.GetValue() + " %";
                AverageWorkloadBStolar.Content = (int)priemernaVytazenostB.GetValue() + " %";
                AverageWorkloadCStolar.Content = (int)priemernaVytazenostC.GetValue() + " %";
            
                foreach (var mm in _stolaren.MontazneMiesta)
                {
                    ItemsControlMontazneMiesta.Items.Add(mm.ToString());
                }
            }
        });
    }
    
    private void ReplicationData(double i)
    {
        _totalValuesProcessed++;
        
        var timeOfObjednavka = _stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetValue();
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            CurrentReplicationLabel.Content = i;
            CurrentValueLabel.Content = Math.Round(timeOfObjednavka, 4);
            // AverageWorkloadAStolar.Content = Math.Round(_stolaren.GlobalneVytazenieA.GetValue(), 4) + " %";
            // AverageWorkloadBStolar.Content = Math.Round(_stolaren.GlobalneVytazenieB.GetValue(), 4) + " %";
            // AverageWorkloadCStolar.Content = Math.Round(_stolaren.GlobalneVytazenieC.GetValue(), 4) + " %";
            AverageWorkloadAStolar.Content = (int)_stolaren.GlobalneVytazenieA.GetValue() + " %";
            AverageWorkloadBStolar.Content = (int)_stolaren.GlobalneVytazenieB.GetValue() + " %";
            AverageWorkloadCStolar.Content = (int)_stolaren.GlobalneVytazenieC.GetValue() + " %";
            AverageObjednavkasNotStarted.Content =
                Math.Round(_stolaren.GlobalnyPriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.GetValue(), 4);
            AverageObjednavkaTimeInSystem.Content = FormatTime(timeOfObjednavka);
        });
        
        if (_totalValuesProcessed <= _skipFirst) return;
        
        _dataCounter++;
        if (_dataCounter % _interval != 0 && _dataCounter != 1) return;
        
        var confidenceInterval = _stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval();
        
        _replicationValuesMean.Add(timeOfObjednavka);
        _replicationValuesTop.Add(confidenceInterval.Item2);
        _replicationValuesBottom.Add(confidenceInterval.Item1);
        
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            CurrentValueLabel.Content = Math.Round(timeOfObjednavka, 4) 
                                        + " <" 
                                        + Math.Round(_stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval().Item1, 4) 
                                        + ", " 
                                        + Math.Round(_stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval().Item2, 4)
                                        + ">";
            if (_dataCounter % 100 == 0) 
                StabilizationPlot.Plot.Axes.AutoScale();
            StabilizationPlot.Refresh();
        });
    }
    
    private void ReplicationEnd()
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            ItemsControlMontazneMiesta.Items.Clear();
            WaitingQueueRezanie.Content = _stolaren.CakajuceNaRezanie.Count;
            WaitingQueueMorenie.Content = _stolaren.CakajuceNaMorenie.Count;
            WaitingQueueSkladanie.Content = _stolaren.CakajuceNaSkladanie.Count;
            WaitingQueueKovanie.Content = _stolaren.CakajuceNaKovanie.Count;

            CurrentReplicationLabel.Content = "1";
            
            NumberOfOrders.Content = _stolaren.PoradieObjednavky;
            NumberOfFinishedOrders.Content = Math.Round(_stolaren.PocetHotovychObjednavok, 4);

            AverageObjednavkaTimeInSystem.Content = Math.Round(_stolaren.PriemernyCasObjednavkyVSysteme.GetValue(), 4) 
                                                    + "<" + Math.Round(_stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval().Item1, 4) 
                                                    + ", " + Math.Round(_stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval().Item2, 4) 
                                                    + ">";
            AverageObjednavkasNotStarted.Content = Math.Round(_stolaren.GlobalnyPriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.GetValue(), 4);
        
            AverageWorkloadAStolar.Content = Math.Round(_stolaren.GlobalneVytazenieA.GetValue(), 4) + " %";
            AverageWorkloadBStolar.Content = Math.Round(_stolaren.GlobalneVytazenieB.GetValue(), 4) + " %";
            AverageWorkloadCStolar.Content = Math.Round(_stolaren.GlobalneVytazenieC.GetValue(), 4) + " %";
        
            foreach (var mm in _stolaren.MontazneMiesta) ItemsControlMontazneMiesta.Items.Add(mm.ToString());
        });
    }

    private string FormatTimeAndDay(double time)
    {
        double timeInDay = time % (8 * 60 * 60); 
        
        int hours = (int)(timeInDay / 3600) + 6; 
        int minutes = (int)(timeInDay % 3600) / 60;
        int seconds = (int)timeInDay % 60;

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    private string FormatTime(double time)
    {
        int hours = (int)(time / 3600); 
        int minutes = (int)(time % 3600) / 60;
        int seconds = (int)time % 60;

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    private int GetDay(double time)
    {
        return (int)(time / (8 * 60 * 60)) + 1;
    }
    #endregion // Private functions
}