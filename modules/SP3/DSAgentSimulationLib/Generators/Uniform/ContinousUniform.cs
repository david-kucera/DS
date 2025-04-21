namespace DSLib.Generators.Uniform;

public class ContinousUniform : UniformGenerator
{
    #region Constructor
    public ContinousUniform(Random seeder, double min, double  max) : base(seeder, min, max)
    {
    }
    #endregion // Constructor

    #region Public functions
    public override double NextDouble()
    {
        return _min + _random.NextDouble() * (_max - _min);
    }

    public override int Next()
    {
        throw new NotSupportedException("This method is not supported!");
    }
    #endregion // Public functions
}