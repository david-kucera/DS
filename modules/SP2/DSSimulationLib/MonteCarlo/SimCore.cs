namespace DSSimulationLib.MonteCarlo;

public abstract class SimCore
{
    #region Class members
    protected bool _isRunning = false;
    #endregion // Class members
    
    #region Events
    public Action<EventArgs> StopSimulation = null!;
    public void OnStopSimulation()
    {
        StopSimulation?.Invoke(EventArgs.Empty);
    }
    #endregion // Events

    #region Public functions
    public void Run(int numReps)
    {
        _isRunning = true;
        BeforeSimulation();

        for (int i = 1; i <= numReps; i++)
        {
            if (!_isRunning) break;
            BeforeSimulationRun();
            Experiment();
            AfterSimulationRun(i);
        }

        AfterSimulation();
        OnStopSimulation();
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
    protected abstract void BeforeSimulationRun();
    protected abstract void AfterSimulationRun(int i);
	#endregion // Protected functions
}