using DSLib.Generators.Empirical;

namespace DSConsoleTest;

public static class GeneratorTester
{
    #region Constants
    private const int NUMBER_OF_ITERATIONS = 1_000_000;
    #endregion // Constants
    
    #region Public functions
    public static void Test(GeneratorTesterType testerType)
    {
        for (int i = 0; i < 100; i++)
        {
            switch (testerType)
            {
                case GeneratorTesterType.Continous: 
                    TestContinous(i);
                    break;
                case GeneratorTesterType.Discrete:
                    TestDiscrete(i);
                    break;
            }
        }
    }
    #endregion // Public functions

    #region Private functions
    private static void TestDiscrete(int seed)
    {
        List<(int,int)> intervals =
        [
            (30, 60),
            (60, 100),
            (100, 140),
            (140, 160)
        ];
        List<double> probabilities =
        [
            0.2,
            0.4,
            0.3,
            0.1
        ];
        DiscreteEmpirical gen = new(intervals, probabilities, seed);

        List<int> int1 = [];
        List<int> int2 = [];
        List<int> int3 = [];
        List<int> int4 = [];

        // Console.WriteLine("Generating data...");
        for (int i = 0; i < NUMBER_OF_ITERATIONS; i++)
        {
            int val = gen.Next();
            
            if (val >= intervals[0].Item1 && val < intervals[0].Item2) int1.Add(val);
            else if (val >= intervals[1].Item1 && val < intervals[1].Item2) int2.Add(val);
            else if (val >= intervals[2].Item1 && val < intervals[2].Item2) int3.Add(val);
            else if (val >= intervals[3].Item1 && val < intervals[3].Item2) int4.Add(val);
            else throw new Exception();
        }

        // Console.WriteLine("Generated data");
        // foreach (double val in probabilities)
        // {
        //     Console.Write(val + "\t");
        // }

        // Console.WriteLine();
        Console.WriteLine((double)int1.Count/NUMBER_OF_ITERATIONS + "\t" +  (double)int2.Count/NUMBER_OF_ITERATIONS + "\t" + (double)int3.Count/NUMBER_OF_ITERATIONS + "\t" + (double)int4.Count/NUMBER_OF_ITERATIONS);
        // Console.WriteLine(int1.Count + int2.Count + int3.Count + int4.Count);
    }
    
    private static void TestContinous(int seed)
    {
        List<(int,int)> intervals =
        [
            (5, 10),
            (10, 50),
            (50, 70),
            (70, 80),
            (80, 95)
        ];
        List<double> probabilities =
        [
            0.4,
            0.3,
            0.2,
            0.06,
            0.04
        ];
        ContinousEmpirical gen = new(intervals, probabilities, seed);

        List<double> int1 = [];
        List<double> int2 = [];
        List<double> int3 = [];
        List<double> int4 = [];
        List<double> int5 = [];

        // Console.WriteLine("Generating data...");
        for (int i = 0; i < NUMBER_OF_ITERATIONS; i++)
        {
            double val = gen.Next();
            
            if (val >= intervals[0].Item1 && val < intervals[0].Item2) int1.Add(val);
            else if (val >= intervals[1].Item1 && val < intervals[1].Item2) int2.Add(val);
            else if (val >= intervals[2].Item1 && val < intervals[2].Item2) int3.Add(val);
            else if (val >= intervals[3].Item1 && val < intervals[3].Item2) int4.Add(val);
            else if (val >= intervals[4].Item1 && val < intervals[4].Item2) int5.Add(val);
            else throw new Exception();
        }

        // Console.WriteLine("Generated data");
        // foreach (double val in probabilities)
        // {
        //     Console.Write(val + "\t");
        // }

        // Console.WriteLine();
        Console.WriteLine((double)int1.Count/NUMBER_OF_ITERATIONS + "\t" +  (double)int2.Count/NUMBER_OF_ITERATIONS + "\t" + (double)int3.Count/NUMBER_OF_ITERATIONS + "\t" + (double)int4.Count/NUMBER_OF_ITERATIONS + "\t" + (double)int5.Count/NUMBER_OF_ITERATIONS);
        // Console.WriteLine(int1.Count + int2.Count + int3.Count + int4.Count + int5.Count);
    }
    #endregion // Private functions
}

public enum GeneratorTesterType
{
    Discrete,
    Continous
}
