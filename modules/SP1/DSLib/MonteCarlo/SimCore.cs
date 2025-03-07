namespace DSLib.MonteCarlo;

public abstract class SimCore
{
    #region Class members
    private bool _isRunning = false;
    #endregion // Class members

    #region Public functions
    public double Run(int numReps)
    {
        _isRunning = true;
        BeforeSimulation();
        double cumulativeResult = 0.0;

        for (int i = 1; i <= numReps; i++)
        {
            if (!_isRunning) break;
            BeforeSimulationRun(i, cumulativeResult);
            double result = Experiment();
            cumulativeResult += result;
            AfterSimulationRun(i, cumulativeResult);
        }

        AfterSimulation(cumulativeResult);
        return cumulativeResult / numReps;
    }

    public void Stop()
    {
        _isRunning = false;
    }
    #endregion // Public functions

    #region Protected functions
    protected abstract double Experiment();
    protected abstract void BeforeSimulation();
    protected abstract void AfterSimulation(double cumulativeResult);
    protected abstract void BeforeSimulationRun(int replication, double cumulativeResult);
    protected abstract void AfterSimulationRun(int replication, double cumulativeResult);
	#endregion // Protected functions
}