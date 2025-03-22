using DSSimulationLib.MonteCarlo;

namespace DSSimulationLib.Simulation;

public abstract class SimulationCore : SimCore
{
    #region Properties
    public PriorityQueue<SimulationEvent, double> EventQueue = new();
    public double Time = 0.0;
    #endregion // Properties
    
    #region Public functions
    protected override void Experiment()
    {
        while (EventQueue.Count > 0 && _isRunning)
        {
            var evnt = EventQueue.Dequeue();
            if (evnt.Time < Time) throw new Exception("Simulation experiment timing problem!");
            Time += evnt.Time;
            
            evnt.Execute();
        }
    }
    #endregion // Public functions
}