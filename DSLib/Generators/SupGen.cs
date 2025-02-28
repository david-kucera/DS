namespace DSLib.Generators;

public class SupGen
{
    #region Class members
    private Random _rnd = null;
    private int _generalSeed = 1;
    #endregion // Class members

    #region Constructor
    protected SupGen(int seed = 0)
    {
        if (seed != 0) _generalSeed = seed;
        _rnd = new Random(_generalSeed);
    }
    #endregion // Constructor

    #region Protected functions
    protected int GetNextSeed()
    {
        return _rnd.Next();
    }
    #endregion // Protected functions
}