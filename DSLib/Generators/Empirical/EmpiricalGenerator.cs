namespace DSLib.Generators.Empirical;

public class EmpiricalGenerator : Random
{
    #region Class members
    private List<Random> _generators = new();
    private List<(int, int)> _intervals;
    private List<double> _cumulativeProbabilities = new();
    #endregion // Class members
    
    #region Constructor
    protected EmpiricalGenerator(Random seeder, List<(int,int)> intervals, List<double> probabilities)
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
        
        for (int i = 0; i <= probabilities.Count; i++) _generators.Add(new Random(seeder.Next()));
    }
    #endregion // Constructor

    #region Protected functions
    protected (Random generator, (int min, int max) interval) GetIntervalGeneratorAndMinMax()
    {
        var probabilityGenerator = _generators[0];
        double genProbability = probabilityGenerator.NextDouble();
        int intervalIndex = _cumulativeProbabilities.FindIndex(p => genProbability < p);
        return (_generators[intervalIndex + 1], _intervals[intervalIndex]);
    }
    #endregion // Protected functions   
}