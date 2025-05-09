using System.Globalization;
using System.Text;
using DSSimulationLib.Generators.Exponential;
using DSSimulationLib.Generators.Triangular;

namespace DSSimulationTest;

public class GeneratorTester
{
    #region Constants
    private const int NUMBER_COUNT = 10_000;
    private static readonly string OUTPUT_DIR = Path.Combine("..", "..", "output");
    #endregion // Constants
    
    #region Constructor
    public GeneratorTester()
    {
        
    }
    #endregion // Constructor

    #region Public functions
    public void Run()
    {
        if (!Directory.Exists(OUTPUT_DIR)) Directory.CreateDirectory(OUTPUT_DIR);
        
        Random seeder = new();
        TestExponentialGenerator(seeder, 1.0);
        TestTriangularGenerator(seeder, 10,200,50);
    }
    #endregion // Public functions
    
    #region Private functions
    private void TestExponentialGenerator(Random seeder, double lambda)
    {
        string outputFilePath = Path.Combine(OUTPUT_DIR, "exponential.txt");
        if (!File.Exists(outputFilePath)) using (File.Create(outputFilePath));
        ExponentialGenerator gen = new(seeder, lambda);
        StringBuilder output = new();
        for (int i = 0; i < NUMBER_COUNT; i++)
        {
            output.AppendLine(gen.NextDouble().ToString(CultureInfo.InvariantCulture).Replace(",", "."));
        }
        File.WriteAllText(outputFilePath, output.ToString());
    }

    private void TestTriangularGenerator(Random seeder, double min, double max, double mode)
    {
        string outputFilePath = Path.Combine(OUTPUT_DIR, "triangular.txt");
        if (!File.Exists(outputFilePath)) using (File.Create(outputFilePath));
        TriangularGenerator gen = new(seeder, min, max, mode);
        StringBuilder output = new();
        for (int i = 0; i < NUMBER_COUNT; i++)
        {
            output.AppendLine(gen.NextDouble().ToString(CultureInfo.InvariantCulture).Replace(",", "."));
        }
        File.WriteAllText(outputFilePath, output.ToString());

    }
    #endregion // Private functions
}