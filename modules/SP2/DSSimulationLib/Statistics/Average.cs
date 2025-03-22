namespace DSSimulationLib.Statistics;

public class Average
{
    #region Class members
    private int _count = 0;
    private double _sum = 0.0;
    #endregion // Class members
    
    #region Constructor
    public Average()
    {
        
    }
    #endregion // Constructor
    
    #region Public functions
    public void AddValue(double value)
    {
        _count++;
        _sum += value;
    }

    public double GetValue()
    {
        if (_count <= 0) return 0;
        return _sum / _count;
    }

    public void Reset()
    {
        _count = 0;
        _sum = 0.0;
    }
    #endregion // Public functions
}