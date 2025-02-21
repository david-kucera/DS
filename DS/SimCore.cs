namespace DS;

public abstract class SimCore
{
    #region Public functions
    public double Run(int numReps)
    {
        double cumulativeResult = 0.0;
        for (int i = 0; i < numReps; i++)
        {
            double result = Experiment();
            cumulativeResult += result;
        }
        return cumulativeResult / numReps;
    }
    
    protected abstract double Experiment();
    public abstract List<double> RunWithTracking(int numReps);
    #endregion // Public functions
}