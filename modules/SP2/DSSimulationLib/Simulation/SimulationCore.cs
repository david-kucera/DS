using DSSimulationLib.MonteCarlo;

namespace DSSimulationLib.Simulation;

public class SimulationCore : SimCore
{
    #region Class members
    private bool _pause = false;
    #endregion // Class members
    
    #region Properties
    public PriorityQueue<SimulationEvent, double> EventQueue = new();
    public double Multiplier { get; set; } = 1.0;
    protected double Time { get; set; } = 0.0;
    public double StopTime { get; set; } = 0.0;
    #endregion // Properties
    
    #region Events
    public Action<EventArgs> NewSimulationData = null!;
    public Action<double> NewSimulationTime = null!;
    public Action<double> NewReplicationData = null;
    
    private void OnNewSimulationData()
    {
        NewSimulationData?.Invoke(EventArgs.Empty);
    }
    private void OnNewSimulationTime(double time)
    {
        NewSimulationTime?.Invoke(time);
    }
    public void OnNewReplicationData(double e)
    {
        NewReplicationData?.Invoke(e);
    }
    #endregion // Events
    
    #region Public functions
    protected override void Experiment()
    {
        while (EventQueue.Count > 0 && _isRunning && EventQueue.Peek().Time < StopTime)
        {
            while (_pause) Thread.Sleep(100);
            
            var evnt = EventQueue.Dequeue();
            if (evnt.Time < Time) throw new Exception("Simulation experiment timing problem!");
            
            Time = evnt.Time;
            evnt.Execute();
            
            if (evnt.GetType() != typeof(SystemEvent))
            {
                OnNewSimulationTime(Time);
                OnNewSimulationData();
            }
            else OnNewSimulationTime(Time);
        }
    }

    protected override void BeforeSimulation()
    {
        
    }

    protected override void AfterSimulation()
    {
        
    }

    protected override void BeforeSimulationRun()
    {
        EventQueue.Enqueue(new SystemEvent(this, Time), Time);
    }

    protected override void AfterSimulationRun(int i)
    {
        OnNewReplicationData(i);
    }

    public void Pause()
    {
        _pause = true;
    }
    
    public void Continue()
    {
        _pause = false;
    }
    #endregion // Public functions
}