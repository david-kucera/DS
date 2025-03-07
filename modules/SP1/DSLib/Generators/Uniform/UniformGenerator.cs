namespace DSLib.Generators.Uniform;

public abstract class UniformGenerator : Random
{
    #region Class members
    protected Random _random;
    protected int _min;
    protected int _max;
    #endregion // Class members

    #region Constructor
    protected UniformGenerator(Random seeder, int min, int max)
    {
        _random = new Random(seeder.Next());
        _min = min;
        _max = max;
    }
    #endregion // Constructor
}