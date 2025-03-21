namespace DSSimulationLib.Generators.Triangular;

public class TriangularGenerator : Random
{
    #region Class members
    private Random _random = null!;
    private double _min = 0.0;
    private double _max = 0.0;
    private double _mode = 0.0;
    #endregion // Class members

    #region Constructor
    public TriangularGenerator(Random seeder, double min, double max, double mode)
    {
        if (seeder is null) throw new ArgumentNullException(nameof(seeder));
        if (min >= max) throw new ArgumentException("Min must be less than max");
        if (mode < min || mode > max) throw new ArgumentException("Mode must be between min and max");
        
        _random = new Random(seeder.Next());
        _min = min;
        _max = max;
        _mode = mode;
    }
    #endregion // Constructor
    
    #region Public functions
    public override double NextDouble()
    {
        double u = _random.NextDouble();
        double f = (_mode - _min) / (_max - _min);
            
        if (u < f) return _min + Math.Sqrt(u * (_max - _min) * (_mode - _min));
        return _max - Math.Sqrt((1 - u) * (_max - _min) * (_max - _mode));
    }

    public override int Next()
    {
        throw new NotSupportedException("This method is not supported!");
    }
    #endregion // Public functions
}