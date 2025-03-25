using DSSimulationLib.MonteCarlo;

namespace DSSimulationLib.Simulation;

public abstract class SimulationCore : SimCore
{
    #region Properties
    public PriorityQueue<SimulationEvent, double> EventQueue = new();
    public double Time = 0.0;
    public double StopTime = 0.0;
    #endregion // Properties
    
    #region Events
    public Action<double> NewSimulationTime = null!;
    public Action<EventArgs> NewSimulationData = null!;
    public void OnNewSimulationTime(double time)
    {
        NewSimulationTime?.Invoke(time);
    }
    public void OnNewSimulationData()
    {
        NewSimulationData?.Invoke(EventArgs.Empty);
    }
    #endregion // Events
    
    #region Public functions
    protected override void Experiment()
    {
        while (EventQueue.Count > 0 && _isRunning)
        {
            var evnt = EventQueue.Dequeue();
            if (evnt.Time < Time) throw new Exception("Simulation experiment timing problem!");
            Time = evnt.Time;
            evnt.Execute();
            if (evnt.GetType() != typeof(SystemEvent)) OnNewSimulationData();
            else OnNewSimulationTime(evnt.Time);
        }
    }
    #endregion // Public functions
}