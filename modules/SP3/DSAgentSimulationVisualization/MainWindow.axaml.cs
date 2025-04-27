using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using DSAgentSimulationLib.Statistics;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ScottPlot;
using ScottPlot.Plottables;
using Simulation;

namespace DSAgentSimulationVisualization;

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
    private bool _ignoreData = false;
    private MySimulation _stolaren;
    #endregion // Class members
    
    #region Constructor
    public MainWindow()
    {
        InitializeComponent();
        SeedInput.Text = new Random().Next(0, 1000).ToString();
        ACountInput.Text = "7";
        BCountInput.Text = "7";
        CCountInput.Text = "45";
        MCountInput.Text = "100";
        StabilizationPlot.Plot.XLabel("Replication");
        StabilizationPlot.Plot.YLabel("Hodnota");
    }
    #endregion // Constructor
    
    #region Private functions
    private void StartSimulation(int numReps, int seed, int m, int a, int b, int c)
    {
        Random seeder = new Random(seed);
        _dataCounter = 0;
        _totalValuesProcessed = 0;

        _stolaren = new MySimulation(seeder, m, a, b, c);
        _stolaren.OnRefreshUI(RefreshUI);
        _stolaren.OnReplicationDidFinish(ReplicationDidFinish);
        _stolaren.OnSimulationDidFinish(SimulationDidFinish);
        _stolaren.SetSimSpeed(1,1);
        Task.Run(() => _stolaren.Start(numReps, 249 * 8 * 60 * 60));
    }

    private void SimulationDidFinish(OSPABA.Simulation obj)
    {
	    Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
	    {
		    UkonciButton_OnClick(null, null!);
	    });
	}

    private void ReplicationDidFinish(OSPABA.Simulation sim)
    {
	    _totalValuesProcessed++;
	    var timeOfObjednavka = _stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetValue();
	    
	    Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
	    {
		    CurrentReplicationLabel.Content = sim.CurrentReplication;
		    CurrentValueLabel.Content = Math.Round(timeOfObjednavka, 4);
		    AverageWorkloadAStolar.Content = Math.Round(_stolaren.GlobalneVytazenieA.GetValue(), 4) + " % " +
		                                     " <" + Math.Round(_stolaren.GlobalneVytazenieA.GetConfidenceInterval().Item1, 4) + ";" +
		                                     "" + Math.Round(_stolaren.GlobalneVytazenieA.GetConfidenceInterval().Item2, 4) + ">";
		    AverageWorkloadBStolar.Content = Math.Round(_stolaren.GlobalneVytazenieB.GetValue(), 4) + " % " +
		                                     " <" + Math.Round(_stolaren.GlobalneVytazenieB.GetConfidenceInterval().Item1, 4) + ";" +
		                                     "" + Math.Round(_stolaren.GlobalneVytazenieB.GetConfidenceInterval().Item2, 4) + ">";
		    AverageWorkloadCStolar.Content = Math.Round(_stolaren.GlobalneVytazenieC.GetValue(), 4) + " % " +
		                                     " <" + Math.Round(_stolaren.GlobalneVytazenieC.GetConfidenceInterval().Item1, 4) + ";" +
		                                     "" + Math.Round(_stolaren.GlobalneVytazenieC.GetConfidenceInterval().Item2, 4) + ">";
		    AverageObjednavkasNotStarted.Content =
		        Math.Round(_stolaren.GlobalnyPriemernyPocetNezacatychObjednavok.GetValue(), 4) + "" +
		        " <" + Math.Round(_stolaren.GlobalnyPriemernyPocetNezacatychObjednavok.GetConfidenceInterval().Item1, 4) + ";" +
		        "" + Math.Round(_stolaren.GlobalnyPriemernyPocetNezacatychObjednavok.GetConfidenceInterval().Item2, 4) + ">";
		    AverageObjednavkaTimeInSystem.Content = FormatTime(timeOfObjednavka);
		    
		    var objednavky = ((MySimulation)sim).AgentModelu.Objednavky;
		    var hotove = ((MySimulation)sim).AgentModelu.PocetHotovychObjednavok;
			NumberOfOrders.Content = objednavky.Count;
			NumberOfFinishedOrders.Content = hotove;
	    });
	    
	    if (_totalValuesProcessed <= _skipFirst) return;
	    
	    _dataCounter++;
	    if (_dataCounter % _interval != 0 && _dataCounter != 1) return;
	    
	    var confidenceInterval = _stolaren.GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval();

	    if (timeOfObjednavka == 0) return;
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
	        // if (_dataCounter % 10 == 0) 
	            StabilizationPlot.Plot.Axes.AutoScale();
	        StabilizationPlot.Refresh();
	    });
	}

    private void RefreshUI(OSPABA.Simulation sim)
    {
	    Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
	    {
		    SimulationTime(sim.CurrentTime);
		    var objednavky = ((MySimulation)sim).AgentModelu.Objednavky;
		    var montazneMiesta = ((MySimulation)sim).AgentMontaznychMiest.MontazneMiesta;
		    var hotove = ((MySimulation)sim).AgentModelu.PocetHotovychObjednavok;
			NumberOfOrders.Content = objednavky.Count;
			NumberOfFinishedOrders.Content = hotove;

			if (DurationSlider.Value >= 0.5)
			{
				Average priemernaVytazenostA = new();
				Average priemernaVytazenostB = new();
				Average priemernaVytazenostC = new();
				foreach (var stolar in ((MySimulation)sim).AgentAStolar.StolariA) priemernaVytazenostA.AddValue(stolar.Workload.GetValue());
				foreach (var stolar in ((MySimulation)sim).AgentBStolar.StolariB) priemernaVytazenostB.AddValue(stolar.Workload.GetValue());
				foreach (var stolar in ((MySimulation)sim).AgentCStolar.StolariC) priemernaVytazenostC.AddValue(stolar.Workload.GetValue());

				AverageWorkloadAStolar.Content = (int)priemernaVytazenostA.GetValue() + " %";
				AverageWorkloadBStolar.Content = (int)priemernaVytazenostB.GetValue() + " %";
				AverageWorkloadCStolar.Content = (int)priemernaVytazenostC.GetValue() + " %";


				WaitingQueueRezanie.Content = ((MySimulation)sim).AgentStolarov.CakajuceNaRezanie.Count;
				WaitingQueueMorenie.Content = ((MySimulation)sim).AgentStolarov.CakajuceNaMorenie.Count;
				WaitingQueueLakovanie.Content = ((MySimulation)sim).AgentStolarov.CakajuceNaLakovanie.Count;
				WaitingQueueSkladanie.Content = ((MySimulation)sim).AgentStolarov.CakajuceNaSkladanie.Count;
				WaitingQueueKovanie.Content = ((MySimulation)sim).AgentStolarov.CakajuceNaMontazKovani.Count;

				AverageObjednavkaTimeInSystem.Content = FormatTime(_stolaren.PriemernyCasObjednavkyVSysteme.GetValue());
				AverageObjednavkasNotStarted.Content = Math.Round(_stolaren.PriemernyPocetNezacatychObjednavok.GetValue(), 4);

				ItemsControlMontazneMiesta.Items.Clear();
				foreach (var mm in montazneMiesta) ItemsControlMontazneMiesta.Items.Add(mm.ToString());
			}
		});
    }
    
    private void Start()
    {
        if (int.TryParse((string?)ReplicationCountInput.Text, out int numReps) &&
            int.TryParse((string?)SeedInput.Text, out int seed) &&
            int.TryParse((string?)SkipInput.Text, out int skip) &&
            int.TryParse((string?)MCountInput.Text, out int m) &&
			int.TryParse((string?)ACountInput.Text, out int a) &&
            int.TryParse((string?)BCountInput.Text, out int b) &&
            int.TryParse((string?)CCountInput.Text, out int c) &&
            int.TryParse((string?)IntervalInput.Text, out int interval)
            )
        {
            _interval = interval;
            _skipFirst = (int)((numReps) * ((double)skip / 100.0));
            StartSimulation(numReps, seed, m, a, b, c);
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

        DurationSlider.Value = 1;
		IntervalSlider.Value = 1;
		VirtualSpeedCheckBox.IsChecked = false;
        AnimationCheckBox.IsEnabled = true;

        Start();
    }

    private void UkonciButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SpustiButton.IsEnabled = true;
        UkonciButton.IsEnabled = false;
        PozastavButton.IsEnabled = false;
        PokracujButton.IsEnabled = false;

        VirtualSpeedCheckBox.IsEnabled = true;
        AnimationCheckBox.IsEnabled = false;
        _stolaren.Stop();
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
        _stolaren.Resume();
    }

    private void SimulationTime(double time)
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            CurrentSimulationTime.Content = FormatTimeAndDay(time);
            CurrentSimulationDay.Content = GetDay(time);
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

    private void DurationSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
	    if (DurationSlider is not null) DurationSliderValue.Content = Math.Round(DurationSlider.Value, 4);
	    SetSimulationSpeed();
    }

    private void IntervalSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (IntervalSlider is not null) IntervalSliderValue.Content = (int)IntervalSlider.Value;
		SetSimulationSpeed();
    }

    private void VirtualSpeedCheckBox_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
	    SetSimulationSpeed();
    }

	private void SetSimulationSpeed()
    {
        if (DurationSlider == null || IntervalSlider == null || _stolaren == null) return;

		double durationValue = DurationSlider.Value;
		double intervalValue = IntervalSlider.Value;

		if (VirtualSpeedCheckBox.IsChecked == true)
		{
            _stolaren.SetMaxSimSpeed();
		}
		else
		{
            _stolaren.SetSimSpeed(intervalValue, durationValue);
		}
	}

    private void AnimationCheckBox_IsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        Animate(AnimationCheckBox.IsChecked);
    }

    private void Animate(bool? isChecked)
    {
        if (isChecked == true)
        {
            _stolaren.CreateAnimator();
            var frameworkElementCanvas = _stolaren.Animator.Canvas;
            _stolaren.Animator.SetSynchronizedTime(false); 
            _stolaren.InitAnimator();
            var embedSample = new EmbedFrameworkElement(frameworkElementCanvas);
            MyContentControl.Content = embedSample;
        }
        else
        {
            _stolaren.RemoveAnimator();
            MyContentControl.Content = null;
        }
    }
    #endregion // Private functions
}