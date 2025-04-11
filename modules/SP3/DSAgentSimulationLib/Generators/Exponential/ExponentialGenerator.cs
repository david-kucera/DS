namespace DSAgentSimulationLib.Generators.Exponential;

public class ExponentialGenerator : Random
{
    #region Class members
    private Random _random = null!;
    private double _lambda = 0.0;
    #endregion // Class members
    
    #region Constructor
    public ExponentialGenerator(Random seeder, double lambda)
    {
        if (lambda <= 0) throw new ArgumentException("Lambda value must be greater than zero.");
        if (seeder is null) throw new ArgumentNullException(nameof(seeder));
        
        _lambda = lambda;
        _random = new Random(seeder.Next());
    }
    #endregion // Constructor
    
    #region Public functions
    public override double NextDouble()
    {
        return -Math.Log(1.0 - _random.NextDouble()) / _lambda;
    }

    public override int Next()
    {
        throw new NotSupportedException("This method is not supported!");
    }
    #endregion // Public functions
}