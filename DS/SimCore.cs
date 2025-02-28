namespace DS;

public abstract class SimCore
{
    #region Public functions
    public double Run(int numReps)
    {
        BeforeSimulation();
        double cumulativeResult = 0.0;
        for (int i = 0; i < numReps; i++)
        {
            BeforeSimulationRun();
            double result = Experiment();
            cumulativeResult += result;
            AfterSimulationRun();
        }
        AfterSimulation();
        return cumulativeResult / numReps;
    }
    
    protected abstract double Experiment();
    public abstract void BeforeSimulation();
    public abstract void AfterSimulation();
    public abstract void BeforeSimulationRun();
    public abstract void AfterSimulationRun();
    #endregion // Public functions
}