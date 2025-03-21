namespace DSSimulationLib.Simulation;

public abstract class SimulationCore
{
    #region Class members
    private PriorityQueue<SimulationEvent, double> _eventQueue;
    private double _time;
    private Statistics _statistics;
    public bool _isRunning;
    #endregion // Class members
    
    #region Public functions
    public double Run(int numReps)
    {
        _isRunning = true;
        _eventQueue = new PriorityQueue<SimulationEvent, double>();
        _time = 0.0;
        _statistics = new Statistics();
        BeforeSimulation();
        double cumulativeResult = 0.0;

        for (int i = 1; i <= numReps; i++)
        {
            BeforeSimulationRun();
            double res = Experiment();
            cumulativeResult += res;
            AfterSimulationRun();
        }
        
        AfterSimulation();
        return cumulativeResult / numReps;
    }

    public void Stop()
    {
        _isRunning = false;
    }
    
    public void ScheduleEvent(SimulationEvent simEvent)
    {
        if (simEvent.Time < _time) throw new ArgumentException("The time is smaller than the simulation time.");
        _eventQueue.Enqueue(simEvent, simEvent.Time);
    }
    #endregion // Public functions
    
    #region Protected functions
    protected abstract double Experiment();
    protected abstract void BeforeSimulation();
    protected abstract void AfterSimulation();
    protected abstract void BeforeSimulationRun();
    protected abstract void AfterSimulationRun();
    #endregion // Protected functions
}