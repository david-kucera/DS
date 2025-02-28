using DS.Gen;

namespace DS;

internal static class Program
{
    private static void Main()
    {
        GeneratorTester.Test(GeneratorTesterType.Continous);
        return;
        
        const int numReps = 1_000;
        const double L = 5.0;
        const double D = 10.0;
        Random rndY = new();
        Random rndA = new();
        
        BuffonNeedle buffonNeedle = new BuffonNeedle(rndY, rndA, D, L);
        
        var output = buffonNeedle.Run(numReps);
        var piEval = (2 * L)/ (D * output);
        Console.WriteLine($"PI: {piEval}");
    }
}