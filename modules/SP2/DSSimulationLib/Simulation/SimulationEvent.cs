namespace DSSimulationLib.Simulation;

public abstract class SimulationEvent
{
    #region Properties
    protected SimulationCore Core { get; set; }
    public double Time { get; set; }
    #endregion // Properties
    
    #region Constructor
    protected SimulationEvent(SimulationCore core, double time)
    {
        Core = core;
        Time = time;
    }
    #endregion // Constructor

    #region Public functions
    public abstract void Execute();
    #endregion // Public functions
}