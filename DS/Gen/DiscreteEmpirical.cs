namespace DS.Gen;

public class DiscreteEmpirical : EmpiricalGenerator
{
    #region Constructor
    public DiscreteEmpirical(List<(int,int)> intervals, List<double> probabilities, int seed = 0) : base(intervals, probabilities, seed)
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public int Next()
    {
        return NextInt();
    }
    #endregion // Public functions
}