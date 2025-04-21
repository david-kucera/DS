namespace DSAgentSimulationLib.Statistics;

public class WeightedAverage
{
    #region Class members
    private double _lastTime = 0.0;
    private double _weightedSum = 0.0;
    private double _totalWeight = 0.0;
    #endregion // Class members

    #region Constructor
    public WeightedAverage()
    {
    }
    #endregion // Constructor

    #region Public functions
    public void AddValue(double value, double time)
    {
        if (time < _lastTime) throw new ArgumentException("Timing error!.");
        
        double weight = time - _lastTime;
        _lastTime = time;
        _weightedSum += value * weight;
        _totalWeight += weight;
    }

    public double GetValue()
    {
        if (_totalWeight > 0) return _weightedSum / _totalWeight;
        return double.NaN;
    }

    public void Reset()
    {
        _lastTime = 0.0;
        _weightedSum = 0.0;
        _totalWeight = 0;
    }
    #endregion // Public functions
}