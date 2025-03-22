using DSSimulationLib.MonteCarlo;

namespace DSSimulationLib.Simulation;

public abstract class SimulationCore : SimCore
{
    #region Class members
    private PriorityQueue<SimulationEvent, double> _eventQueue = new PriorityQueue<SimulationEvent, double>();
    private double _time = 0.0;
    #endregion // Class members
    
    #region Public functions
    protected override double Experiment()
    {
        while (_eventQueue.Count > 0 && _isRunning)
        {
            var evnt = _eventQueue.Dequeue();
            if (evnt.Time < _time) throw new Exception("Simulation experiment timing problem!");
            _time += evnt.Time;
            
            evnt.Execute();
        }
        throw new NotImplementedException();
    }
    
    
    public void ScheduleEvent(SimulationEvent simEvent)
    {
        if (simEvent.Time < _time) throw new ArgumentException("The time is smaller than the simulation time.");
        _eventQueue.Enqueue(simEvent, simEvent.Time);
    }
    #endregion // Public functions
}