namespace DSSimulationLib.Simulation;

public class SystemEvent : SimulationEvent
{
    #region Class members
    private int _shift = 1;
    private int _duration = 1000;
    #endregion // Class members
    
    #region Constructor
    public SystemEvent(SimulationCore core, double time) : base(core, time)
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        if (Core.Multiplier > 1000)
        {
            var times = Core.Multiplier / 1000;
            _shift = (int)times;
        }
        else _shift = 1;
        
        int sleep = (int)(_duration / Core.Multiplier);
        if (Time + _shift < Core.StopTime) Core.EventQueue.Enqueue(new SystemEvent(Core, Time + _shift), Time + _shift);

        Thread.Sleep(sleep > 0 ? sleep : 1);
    }
    #endregion // Public functions
}