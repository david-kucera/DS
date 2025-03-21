namespace DSSimulationLib.Simulation;

public abstract class SimulationEvent
{
    #region Properties
    public string Name { get; set; } = string.Empty;
    public double Time { get; set; } = 0.0;
    #endregion // Properties
    
    #region Constructor
    protected SimulationEvent(string name, double time)
    {
        Name = name;
        Time = time;
    }
    #endregion // Constructor

    #region Public functions
    public abstract void Execute();

    public override string ToString()
    {
        return $"{Name} @ {Time:F2}";
    }
    #endregion // Public functions
}