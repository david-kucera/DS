namespace DSSimulationLib.Statistics;

public class ConfidenceInterval
{
    #region Class members
    private int _count = 0;
    private double _sum = 0;
    private double _sumSquared = 0;
    #endregion // Class members

    #region Constructor
    public ConfidenceInterval()
    {
        
    }
    #endregion // Constructor
    
    #region Public functions
    public void AddValue(double value)
    {
        _count++;
        _sum += value;
        _sumSquared += value * value;
    }

    public (double, double) GetConfidenceInterval(double confidenceLevel = 1.96)
    {
        if (_count < 2) throw new InvalidOperationException("Confidence interval can't be made from less than 2 numbers.");
        
        var error = confidenceLevel * StdDev() / Math.Sqrt(_count);
        var avg = _sum / _count;
        
        return (avg - error, avg + error);
    }

    public void Reset()
    {
        _count = 0;
        _sum = 0;
        _sumSquared = 0;
    }
    #endregion // Public functions
    
    #region Private functions
    private double StdDev()
    {
        if (_count < 2) return 0;
        
        return Math.Sqrt((_sumSquared - (_sum * _sum) / _count) / (_count - 1));
    }
    #endregion // Private functions
}