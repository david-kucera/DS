namespace DS.Gen.Empirical;

public class ContinousEmpirical : EmpiricalGenerator
{
    #region Constructor
    public ContinousEmpirical(List<(int,int)> intervals, List<double> probabilities, int seed = 0) : base(intervals, probabilities, seed)
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public double Next()
    {
        return NextDouble();
    }
    #endregion // Public functions
}