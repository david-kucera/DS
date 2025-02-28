namespace DSLib.Generators.Empirical;

public class DiscreteEmpirical : EmpiricalGenerator
{
    #region Constructor
    public DiscreteEmpirical(List<(int,int)> intervals, List<double> probabilities, int seed = 0) : base(intervals, probabilities, seed)
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public int Next()
    {
        var (intervalGenerator, (min, max)) = GetIntervalGeneratorAndMinMax();
        return intervalGenerator.Next(min, max);
    }
    #endregion // Public functions
}