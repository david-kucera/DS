namespace DSLib.Generators.Uniform;

public abstract class UniformGenerator : Random
{
    #region Class members
    protected Random _random;
    protected double _min;
    protected double _max;
    #endregion // Class members

    #region Constructor
    protected UniformGenerator(Random seeder, double min, double max)
    {
        _random = new Random(seeder.Next());
        _min = min;
        _max = max;
    }
    #endregion // Constructor
}