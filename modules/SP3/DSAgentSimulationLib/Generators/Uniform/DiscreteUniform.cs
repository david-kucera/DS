namespace DSLib.Generators.Uniform;

public class DiscreteUniform : UniformGenerator
{
    #region Constructor
    public DiscreteUniform(Random seeder, double min, double max) : base(seeder, min, max)
    {
    }
    #endregion // Constructor

    #region Public functions
    public override int Next()
    {
        return _random.Next((int)_min, (int)_max);
    }

    public override double NextDouble()
    {
        throw new NotSupportedException("This method is not supported!");
    }
    #endregion // Public functions
}