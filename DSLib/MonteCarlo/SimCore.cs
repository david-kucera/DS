namespace DSLib.MonteCarlo;

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
    protected abstract void BeforeSimulation();
    protected abstract void AfterSimulation();
    protected abstract void BeforeSimulationRun();
    protected abstract void AfterSimulationRun();
    #endregion // Public functions
}