namespace DS;

public class BuffonNeedle : SimCore
{
    #region Properties
    public Random RndY { get; set; } = null;
    public Random RndA { get; set; } = null;
    public double D { get; set; } = double.NaN;
    public double L { get; set; } = double.NaN;
    #endregion // Properties
    
    #region Constuctor
    public BuffonNeedle(Random rndY, Random rndA, double d, double l)
    {
        RndY = rndY;
        RndA = rndA;
        D = d;
        L = l;
    }
    #endregion // Constructor

    #region Public functions
    protected override double Simulate()
    {
        double y = RndY.NextDouble() * D;
        double alpha = RndA.NextDouble() * Math.PI;
        double a = L * Math.Sin(alpha);
        
        if (y + a >= D) return 1.0;
        return 0.0;
    }

    public override void BeforeSimulation()
    {
        throw new NotImplementedException();
    }

    public override void AfterSimulation()
    {
        throw new NotImplementedException();
    }

    public override void BeforeSimulationRun()
    {
        throw new NotImplementedException();
    }

    public override void AfterSimulationRun()
    {
        throw new NotImplementedException();
    }

    public override List<double> RunWithTracking(int numReps)
    {
        double cumulativeResult = 0.0;
        List<double> estimates = [];

        for (int i = 1; i <= numReps; i++)
        {
            double result = Simulate();
            cumulativeResult += result;
            double piEval = (2 * L) / (D * (cumulativeResult / i));
            estimates.Add(piEval);
        }

        return estimates;
    }

    #endregion // Public functions
}