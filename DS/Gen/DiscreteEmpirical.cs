namespace DS.Gen;

public class DiscreteEmpirical : SupGen
{
    #region Class members
    private List<Random> _generators = new();
    private List<(int, int)> _intervals;
    private List<double> _cumulativeProbabilities = new();
    #endregion // Class members
    
    #region Constructor
    public DiscreteEmpirical(List<(int,int)> intervals, List<double> probabilities, int seed = 0) : base(seed)
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
        
        // Init of generators for probab. and interval values
        for (int i = 0; i <= probabilities.Count; i++)
        {
            _generators.Add(new Random(GetNextSeed()));
        }
    }
    #endregion // Constructor

    #region Public functions
    public int Next()
    {
        var probabilityGenerator = _generators[0];
        double genProbability = probabilityGenerator.NextDouble();
        
        int intervalIndex = _cumulativeProbabilities.FindIndex(p => genProbability < p);
        if (intervalIndex == -1) intervalIndex = _cumulativeProbabilities.Count - 1;
        
        var intervalGenerator = _generators[intervalIndex];
        var (min,max) = _intervals[intervalIndex];
        int generatedValue = intervalGenerator.Next(min, max);
        return generatedValue;
    }
    #endregion // Public functions
}