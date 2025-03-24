namespace DSLib.Generators.Empirical;

public class DiscreteEmpirical : EmpiricalGenerator
{
    #region Constructor
    public DiscreteEmpirical(Random seeder, List<(double,double)> intervals, List<double> probabilities) : base(seeder, intervals, probabilities)
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public override int Next()
    {
        var (intervalGenerator, (min, max)) = GetIntervalGeneratorAndMinMax();
        return intervalGenerator.Next((int)min, (int)max);
    }

    public override double NextDouble()
    {
        throw new NotSupportedException("This method is not supported!");
    }
    #endregion // Public functions
}