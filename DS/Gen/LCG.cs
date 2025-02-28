namespace DS.Gen;

public class LCG : IGen
{
    #region Class members
    private long _seed = 0;
    private readonly long _a = 0;
    private readonly long _c = 0;
    private readonly long _m = 0;
    #endregion // Class members
    
    #region Constructors
    public LCG(/*long seed,*/ long a, long c, long m)
    {
        // _seed = seed;
        _a = a;
        _c = c;
        _m = m;
    }
    #endregion // Constructors
    
    
    public int NextInt()
    {
        _seed = (_a * _seed + _c) % _m;
        return (int)(_seed & 0x7FFFFFFF);
    }

    public int NextInt(int min, int max)
    {
        return min + NextInt() % (max - min);
    }

    public double NextDouble()
    {
        throw new NotImplementedException(); 
    }

    public double NextDouble(double min, double max)
    {
        throw new NotImplementedException();
    }

    public bool NextBool()
    {
        throw new NotImplementedException();
    }
}