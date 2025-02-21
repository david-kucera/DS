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

    protected override double Experiment()
    {
        double y = RndY.NextDouble() * D;
        double alpha = RndA.NextDouble() * Math.PI;
        double a = L * Math.Sin(alpha);
        
        if (y + a >= D) return 1.0;
        return 0.0;
    }
}