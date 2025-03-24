using DSSimulationWoodwork;

namespace DSSimulationTest;

static class Program
{
    static void Main()
    {
        // TestGenerators();
        TestPredajna();
        // TestStolaren();
    }

    private static void TestStolaren()
    {
        Random seeder = new Random(0);
        Stolaren stolaren = new(seeder, 2, 2, 18);
        stolaren.Run(30);
    }

    private static void TestPredajna()
    {
        Random seeder = new Random();
        Predajna predajna = new Predajna(seeder);
        predajna.Run(1);
    }

    private static void TestGenerators()
    {
        GeneratorTester tester = new GeneratorTester();
        tester.Run();
    }
}
