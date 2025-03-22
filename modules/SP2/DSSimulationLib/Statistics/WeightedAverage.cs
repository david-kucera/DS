namespace DSSimulationLib.Statistics;

public class WeightedAverage
{
    #region Class members
    private double _weightedSum = 0.0;
    private int _totalWeight = 0;
    #endregion // Class members

    #region Constructor
    public WeightedAverage()
    {
    }
    #endregion // Constructor

    #region Public functions
    public void AddValue(double value, int weight)
    {
        if (weight <= 0) return;
        _weightedSum += value * weight;
        _totalWeight += weight;
    }

    public double GetValue()
    {
        if (_totalWeight > 0)
        {
            return _weightedSum / _totalWeight;
        }

        return double.NaN;
    }

    public void Reset()
    {
        _weightedSum = 0.0;
        _totalWeight = 0;
    }
    #endregion // Public functions
}