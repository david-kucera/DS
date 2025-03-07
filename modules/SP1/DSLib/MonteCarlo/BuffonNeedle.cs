namespace DSLib.MonteCarlo;

public class BuffonNeedle : SimCore
{
    #region Class members
    private Random _rndY { get; set; } = null;
    private Random _rndA { get; set; } = null;
    private double _d { get; set; } = double.NaN;
    private double _l { get; set; } = double.NaN;
	#endregion // Class members

	#region Events
	public event EventHandler<double> NewPiEstimate;

	private void OnNewPiEstimate(int i, double cumulativeResult)
	{
		double piEstimate = (2 * _l) / (_d * (cumulativeResult / i));
		NewPiEstimate?.Invoke(this, piEstimate);
	}
	#endregion // Events

	#region Constuctor
	public BuffonNeedle(Random rndY, Random rndA, double d, double l)
    {
        _rndY = rndY;
        _rndA = rndA;
        _d = d;
        _l = l;
    }
    #endregion // Constructor

    #region Protected functions
    protected override double Experiment()
    {
        double y = _rndY.NextDouble() * _d;
        double alpha = _rndA.NextDouble() * Math.PI;
        double a = _l * Math.Sin(alpha);
        
        if (y + a >= _d) return 1.0;
        return 0.0;
    }

    protected override void BeforeSimulation()
    {

    }

    protected override void AfterSimulation(double cumulativeResult)
    {
	    
	}

    protected override void BeforeSimulationRun(int replication, double cumulativeResult)
    {

    }

    protected override void AfterSimulationRun(int replication, double cumulativeResult)
    {
	    OnNewPiEstimate(replication, cumulativeResult);
	}
	#endregion // Protected functions
}