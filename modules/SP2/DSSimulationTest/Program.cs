using DSSimulationLib.Generators.Exponential;
using DSSimulationLib.Generators.Triangular;

namespace DSSimulationTest;

class Program
{
    #region Constants
    public static int NUMBER_COUNT = 1000;
    #endregion // Constants
    
    static void Main(string[] args)
    {
        Random seeder = new Random();
        TestExponentialGenerator(seeder, 1.0);
        TestTriangularGenerator(seeder, 10,200,50);
    }

    static void TestExponentialGenerator(Random seeder, double lambda)
    {
        ExponentialGenerator gen = new(seeder, lambda);
        for (int i = 0; i < NUMBER_COUNT; i++)
        {
            Console.WriteLine(gen.NextDouble());
        }
    }

    static void TestTriangularGenerator(Random seeder, double min, double max, double mode)
    {
        TriangularGenerator gen = new(seeder, min, max, mode);
        for (int i = 0; i < NUMBER_COUNT; i++)
        {
            Console.WriteLine(gen.NextDouble());
        }
    }
}
