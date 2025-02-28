namespace DSLib.Generators.Empirical;

public class ContinousEmpirical : EmpiricalGenerator
{
    #region Constructor
    public ContinousEmpirical(List<(int,int)> intervals, List<double> probabilities, int seed = 0) : base(intervals, probabilities, seed)
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public double Next()
    {
        var (intervalGenerator, (min, max)) = GetIntervalGeneratorAndMinMax();
        return min + (max - min) * intervalGenerator.NextDouble();
    }
    #endregion // Public functions
}