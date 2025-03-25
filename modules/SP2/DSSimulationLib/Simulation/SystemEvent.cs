namespace DSSimulationLib.Simulation;

public class SystemEvent : SimulationEvent
{
    #region Class members
    private int _shift = 1;
    private int _duration = 1000;
    private double _multiplier = 1.0;
    #endregion // Class members
    
    #region Constructor
    public SystemEvent(SimulationCore core, double time, double multiplier) : base(core, time)
    {
        _multiplier = multiplier;
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        int sleep = (int)(_duration / _multiplier);
        if (Time + _shift < Core.StopTime) Core.EventQueue.Enqueue(new SystemEvent(Core, Time + _shift, _multiplier), Time + _shift);
        Thread.Sleep(sleep);
    }
    #endregion // Public functions
}