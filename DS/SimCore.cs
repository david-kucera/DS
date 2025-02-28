namespace DS;

public abstract class SimCore
{
    #region Public functions
    public double Run(int numReps)
    {
        double cumulativeResult = 0.0;
        for (int i = 0; i < numReps; i++)
        {
            double result = Simulate();
            cumulativeResult += result;
        }
        return cumulativeResult / numReps;
    }
    
    protected abstract double Simulate();
    public abstract void BeforeSimulation();
    public abstract void AfterSimulation();
    public abstract void BeforeSimulationRun();
    public abstract void AfterSimulationRun();
    public abstract List<double> RunWithTracking(int numReps);
    #endregion // Public functions
}