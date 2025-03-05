namespace DSLib.Generators.Uniform;

public class DiscreteUniform : UniformGenerator
{
    #region Constructor
    public DiscreteUniform(Random seeder, int min, int max) : base(seeder, min, max)
    {
    }
    #endregion // Constructor

    #region Public functions
    public override int Next()
    {
        return _random.Next(_min, _max);
    }

    public override double NextDouble()
    {
        throw new NotSupportedException("This method is not supported!");
    }
    #endregion // Public functions
}