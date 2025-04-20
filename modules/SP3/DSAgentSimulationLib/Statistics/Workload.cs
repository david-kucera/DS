namespace DSAgentSimulationLib.Statistics;

public class Workload
{
    #region Class members
    private double _totalWorkTime { get; set; } = 0.0;
    private double _totalIdleTime { get; set; } = 0.0;
    private double _totalRunTime { get; set; } = 0.0;
    private double _lastTime = 0.0;
    #endregion // Class members

    #region Constructor
    public Workload()
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public void AddValue(bool isWorking, double time)
    {
        var timeDiff = time - _lastTime;
        _totalRunTime += timeDiff;
        
        if (isWorking) _totalWorkTime += timeDiff;
        else _totalIdleTime += timeDiff;
        
        _lastTime = time;
    }

    public double GetValue()
    {
        return _totalRunTime > 0 ? (_totalWorkTime / _totalRunTime) * 100 : 0;
    }

    public void Reset()
    {
        _totalWorkTime = 0.0;
        _totalIdleTime = 0.0;
        _totalRunTime = 0.0;
    }
    #endregion // Public functions
}