namespace DS.Gen;

public class SupGen
{
    private Random _rnd = null;
    private int _generalSeed = 1;

    protected SupGen(int seed = 0)
    {
        if (seed != 0) _generalSeed = seed;
        _rnd = new Random(_generalSeed);
    }

    protected int GetNextSeed()
    {
        return _rnd.Next();
    }
}