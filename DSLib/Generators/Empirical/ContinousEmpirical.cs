namespace DSLib.Generators.Empirical;

public class ContinousEmpirical : EmpiricalGenerator
{
    #region Constructor
    public ContinousEmpirical(Random seeder, List<(int,int)> intervals, List<double> probabilities, int seed = 0) : base(seeder, intervals, probabilities)
    {
        
    }
    #endregion // Constructor

    #region Public functions

    public override int Next()
    {
        throw new NotSupportedException("This method is not supported!");
    }

    public override double NextDouble()
    {
        var (intervalGenerator, (min, max)) = GetIntervalGeneratorAndMinMax();
        return min + (max - min) * intervalGenerator.NextDouble();
    }
    #endregion // Public functions
}