namespace DS.Gen;

public interface IGen
{
    public int NextInt();
    public int NextInt(int min, int max);
    public double NextDouble();
    public double NextDouble(double min, double max);
    public bool NextBool();
}