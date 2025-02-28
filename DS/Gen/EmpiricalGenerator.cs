namespace DS.Gen;

public class EmpiricalGenerator : SupGen
{
    #region Class members
    private List<Random> _generators = new();
    private List<(int, int)> _intervals;
    private List<double> _cumulativeProbabilities = new();
    #endregion // Class members
    
    #region Constructor
    protected EmpiricalGenerator(List<(int,int)> intervals, List<double> probabilities, int seed = 0) : base(seed)
    {
        if (intervals.Count != probabilities.Count) throw new ArgumentException("Incorrect number of parameters!");

        double cum = 0;
        foreach (var t in probabilities)
        {
            cum += t;
            _cumulativeProbabilities.Add(cum);
        }
        if (Math.Abs(cum - 1) > 0.00001) throw new ArgumentException("Incorrect sum of probabilities!");
        
        _intervals = intervals;
        
        for (int i = 0; i <= probabilities.Count; i++)
        {
            _generators.Add(new Random(GetNextSeed()));
        }
    }
    #endregion // Constructor

    #region Protected functions
    protected double NextDouble()
    {
        var (intervalGenerator, (min, max)) = GetIntervalGeneratorAndMinMax();
        return min + (max - min) * intervalGenerator.NextDouble();
    }
    
    protected int NextInt()
    {
        var (intervalGenerator, (min, max)) = GetIntervalGeneratorAndMinMax();
        return intervalGenerator.Next(min, max);
    }
    #endregion // Protected functions   
    
    #region Private functions
    private (Random generator, (int min, int max) interval) GetIntervalGeneratorAndMinMax()
    {
        var probabilityGenerator = _generators[0];
        double genProbability = probabilityGenerator.NextDouble();

        int intervalIndex = _cumulativeProbabilities.FindIndex(p => genProbability < p);
        if (intervalIndex == -1) intervalIndex = _cumulativeProbabilities.Count - 1;

        return (_generators[intervalIndex], _intervals[intervalIndex]);
    }
    #endregion // Private functions
}