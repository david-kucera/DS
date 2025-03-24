namespace DSSimulationLib.MonteCarlo;

public abstract class SimCore
{
    #region Class members
    protected bool _isRunning = false;
    #endregion // Class members

    #region Public functions
    public void Run(int numReps)
    {
        _isRunning = true;
        BeforeSimulation();

        for (int i = 1; i <= numReps; i++)
        {
            if (!_isRunning) break;
            BeforeSimulationRun(i);
            Experiment();
            AfterSimulationRun();
        }

        AfterSimulation();
    }

    public void Stop()
    {
        _isRunning = false;
    }
    #endregion // Public functions

    #region Protected functions
    protected abstract void Experiment();
    protected abstract void BeforeSimulation();
    protected abstract void AfterSimulation();
    protected abstract void BeforeSimulationRun(int cisloReplikacie);
    protected abstract void AfterSimulationRun();
	#endregion // Protected functions
}