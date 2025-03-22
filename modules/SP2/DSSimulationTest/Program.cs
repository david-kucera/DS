namespace DSSimulationTest;

static class Program
{
    static void Main()
    {
        // TestGenerators();
        TestPredajna();
    }

    private static void TestPredajna()
    {
        Random seeder = new Random(0);
        Predajna predajna = new Predajna(seeder);
        predajna.Run(1);
    }

    private static void TestGenerators()
    {
        GeneratorTester tester = new GeneratorTester();
        tester.Run();
    }
}
