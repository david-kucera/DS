namespace DS;

public abstract class SimCore
{
    public double Run(int numReps)
    {
        double kumVysledok = 0.0;
        for (int i = 0; i < numReps; i++)
        {
            double result = Experiment();
            kumVysledok += result;
        }
        return kumVysledok / numReps;
    }
    
    protected abstract double Experiment();
}