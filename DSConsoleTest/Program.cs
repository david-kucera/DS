using DSLib.Generators.Empirical;
using DSLib.MonteCarlo;

namespace DSConsoleTest;

internal static class Program
{
    private static void Main()
    {
        //TestujGeneratory();
        //BuffonNeedle();
        Jan();
    }

    private static void TestujGeneratory()
    {
		GeneratorTester.Test(GeneratorTesterType.Continous);
	}

    private static void BuffonNeedle()
    {
		const int numReps = 1_000;
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
		const int numReps = 1_000;
		const int seed = 0;

		Random rndTlmice = new(seed);
		Random rndBrzdy = new(seed);
		List<(int, int)> intervals =
		[
			(30, 60), (60, 100), (100, 140), (140, 160)
		];
		List<double> probabilities =
		[
			0.2, 0.4, 0.3, 0.1
		];
		DiscreteEmpirical rndSvetlomety = new(intervals, probabilities);

		Random rndDodavatel11 = new(seed);
		Random rndDodavatel12 = new(seed);
		List<(int, int)> intervals2 =
		[
			(5, 10), (10, 50), (50, 70), (70, 80), (80, 95)
		];
		List<double> probabilities2 =
		[
			0.4, 0.3, 0.2, 0.06, 0.04
		];
		ContinousEmpirical rndDodavatel21 = new(intervals2, probabilities2);
		List<double> probabilities3 =
		[
			0.2, 0.4, 0.3, 0.06, 0.04
		];
		ContinousEmpirical rndDodavatel22 = new(intervals2, probabilities3);

		Jan jan = new Jan(JanStrategy.A, rndTlmice, rndBrzdy, rndSvetlomety, rndDodavatel11, rndDodavatel12, rndDodavatel21, rndDodavatel22);
		var output = jan.Run(numReps);
		Console.WriteLine($"Naklady: {output}");
	}
}