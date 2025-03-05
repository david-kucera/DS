using DSLib.Generators.Empirical;
using DSLib.Generators.Uniform;
using DSLib.MonteCarlo;

namespace DSConsoleTest;

internal static class Program
{
    private static void Main()
    {
		// TestujGeneratory();
		// BuffonNeedle();
		Jan();
    }

    private static void TestujGeneratory()
    {
		GeneratorTester.Test(GeneratorTesterType.Continous);
	}

    private static void BuffonNeedle()
    {
		const int numReps = 1_000_000;
		const double L = 5.0;
		const double D = 10.0;
		Random rndY = new();
		Random rndA = new();

		BuffonNeedle buffonNeedle = new BuffonNeedle(rndY, rndA, D, L);

		var output = buffonNeedle.Run(numReps);
		var piEval = (2 * L) / (D * output);
		Console.WriteLine($"PI: {piEval}");
	}

    private static void Jan()
    {
		const int numReps = 1_000_000;
		int seed = DateTime.Now.Millisecond;
		Random seeder = new(seed);
		
		DiscreteUniform rndTlmice = new(seeder, 50, 100);
		DiscreteUniform rndBrzdy = new(seeder, 60, 250);
		List<(int, int)> intervals =
		[
			(30, 60), (60, 100), (100, 140), (140, 160)
		];
		List<double> probabilities =
		[
			0.2, 0.4, 0.3, 0.1
		];
		DiscreteEmpirical rndSvetlomety = new(seeder, intervals, probabilities);

		ContinousUniform rndDodavatel11 = new(seeder, 10, 70);
		ContinousUniform rndDodavatel12 = new(seeder, 30, 95);
		List<(int, int)> intervals2 =
		[
			(5, 10), (10, 50), (50, 70), (70, 80), (80, 95)
		];
		List<double> probabilities2 =
		[
			0.4, 0.3, 0.2, 0.06, 0.04
		];
		ContinousEmpirical rndDodavatel21 = new(seeder, intervals2, probabilities2);
		List<double> probabilities3 =
		[
			0.2, 0.4, 0.3, 0.06, 0.04
		];
		ContinousEmpirical rndDodavatel22 = new(seeder, intervals2, probabilities3);

		Random rnd = new Random(seeder.Next());

		Jan jan = new Jan(JanStrategy.B, rndTlmice, rndBrzdy, rndSvetlomety, rndDodavatel11, rndDodavatel12, rndDodavatel21, rndDodavatel22, rnd);
		var output = jan.Run(numReps);
		Console.WriteLine($"Naklady: {output}");
	}
}