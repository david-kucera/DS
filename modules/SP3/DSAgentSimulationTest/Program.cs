using System.Diagnostics;
using System.Globalization;
using Simulation;

namespace DSAgentSimulationTest;

class Program
{
    #region Constants
    public static int REP_COUNT = 500;
    public static int END_TIME = 249 * 8 * 60 * 60;
    public static int SEED = 0;
    public static Random SEEDER = new(SEED);
	public static int A;
    public static int B;
    public static int C;
    public static int M;
    public static Configuration CURRENT_CONFIG;
    public static string OUTPUT_DIR = "../../output/";
    public static string PATH_VYHOVUJUCE = OUTPUT_DIR + "vyhovuje.csv";
    public static string PATH_NEVYHOVUJUCE = OUTPUT_DIR + "nevyhovuje.csv";
	public static object LOCK = new();
    public static Stopwatch Stopwatch = new();
    public static double Percentage = 0.01;
    public static int STOP_MAX = 0;
    #endregion // Constants

    static void Main(string[] args)
	{
		if (!Directory.Exists(OUTPUT_DIR)) Directory.CreateDirectory(OUTPUT_DIR);
		if (!File.Exists(PATH_VYHOVUJUCE)) File.Create(PATH_VYHOVUJUCE).Close();
		if (!File.Exists(PATH_NEVYHOVUJUCE)) File.Create(PATH_NEVYHOVUJUCE).Close();

		RunRandomly();
		//RunSequentially();
	}

	private static void RunRandomly()
	{
		List<Configuration> configs = [];
		ReadConfigs(configs);
		Random random = new(SEED);
        while (STOP_MAX < 20)
		{
            Stopwatch.Reset();
            var randomConfig = new Configuration(random);
			if (configs.Any(c => c.M == randomConfig.M && c.A == randomConfig.A && c.B == randomConfig.B && c.C == randomConfig.C)) 
			{
                STOP_MAX++;
                Console.WriteLine($"Configuration M{randomConfig.M},A{randomConfig.A},B{randomConfig.B},C{randomConfig.C} already done.\n");
                continue; 
			}
            STOP_MAX = 0;
            configs.Add(randomConfig);
			CURRENT_CONFIG = randomConfig;
			MySimulation sim = new MySimulation(SEEDER, randomConfig.M, randomConfig.A, randomConfig.B, randomConfig.C);
			Console.WriteLine($"{configs.Count}. Current: M{randomConfig.M}, A{randomConfig.A}, B{randomConfig.B}, C{randomConfig.C}");
			sim.OnSimulationDidFinish(ZapisStat);
			sim.OnReplicationDidFinish(CheckStat);
            Stopwatch.Start();
            sim.Start(REP_COUNT, END_TIME);
		}
	}

    private static void CheckStat(OSPABA.Simulation simulation)
    {
		if (simulation.CurrentReplication < 100) return;
        var mean = ((MySimulation)simulation).GlobalnyPriemernyCasObjednavkyVSysteme.GetValue();
		var interval = ((MySimulation)simulation).GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval();

        var lowerBound = interval.Item1;
        var upperBound = interval.Item2;
        var margin = mean * Percentage;

        var minutesElapsed = Stopwatch.Elapsed.TotalMinutes;

		if (lowerBound > mean - margin && upperBound < mean + margin || minutesElapsed >= 15)
		{
            Console.WriteLine("Stopping simulation at " + simulation.CurrentReplication);
			simulation.StopSimulation();
            Stopwatch.Stop();
            Console.WriteLine($"Simulation finished in {Stopwatch.Elapsed:hh\\:mm\\:ss}");
        }
    }

    private static void RunSequentially()
	{
		List<Configuration> configs = [];
        ReadConfigs(configs);
        for (int M = 40; M < 50; M++) // MM
		{
			for (int A = 4; A < 8; A++) // A
			{
				for (int B = 4; B < 8; B++) // B
				{
					for (int C = 36; C < 50; C++) // C
					{
						var config = new Configuration
                        {
                            M = M,
                            A = A,
                            B = B,
                            C = C
                        };
						if (configs.Any(c => c.M == config.M && c.A == config.A && c.B == config.B && c.C == config.C)) 
						{
							Console.WriteLine($"Configuration M{config.M},A{config.A},B{config.B},C{config.C} already done.");
							continue; 
						}
                        MySimulation sim = new MySimulation(SEEDER, config.M, config.A, config.B, config.C);
						Console.WriteLine($"{configs.Count}. Current: M{M}, A{A}, B{B}, C{C}");
						sim.OnSimulationDidFinish(ZapisStat);
						sim.OnReplicationDidFinish(CheckStat);
						Stopwatch.Start();
						sim.Start(REP_COUNT, END_TIME);
					}
				}
			}
		}
	}

    private static void ReadConfigs(List<Configuration> configs)
    {
        var allLines = new List<string>();

        if (File.Exists(PATH_VYHOVUJUCE)) allLines.AddRange(File.ReadAllLines(PATH_VYHOVUJUCE));
        if (File.Exists(PATH_NEVYHOVUJUCE)) allLines.AddRange(File.ReadAllLines(PATH_NEVYHOVUJUCE));

        foreach (var line in allLines)
        {
            var parts = line.Split(',');
            if (int.TryParse(parts[0], out int m) &&
                int.TryParse(parts[1], out int a) &&
                int.TryParse(parts[2], out int b) &&
                int.TryParse(parts[3], out int c))
            {
                configs.Add(new Configuration { M = m, A = a, B = b, C = c });
            }
        }
    }

    private static void VypisStat(OSPABA.Simulation sim)
	{
		Console.WriteLine("-----------------");
		var cas = ((MySimulation)sim).GlobalnyPriemernyCasObjednavkyVSysteme.GetValue();
		Console.WriteLine(FormatTime(cas));
		Console.WriteLine(Math.Round(((MySimulation)sim).GlobalnyPriemernyPocetNezacatychObjednavok.GetValue(), 4));
		Console.WriteLine();
		if (cas < 32 * 60 * 60) Console.WriteLine("VYHOVUJE");
		else Console.WriteLine("NE VYHOVUJE");
	}

	private static void ZapisStat(OSPABA.Simulation sim)
    {
        Stopwatch.Stop();
        double cas = ((MySimulation)sim).GlobalnyPriemernyCasObjednavkyVSysteme.GetValue();
		string casf = FormatTime(((MySimulation)sim).GlobalnyPriemernyCasObjednavkyVSysteme.GetValue());
		string casInt =
                "<" + Math.Round(((MySimulation)sim).GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval().Item1, 4).ToString(CultureInfo.InvariantCulture) + ";" +
                "" + Math.Round(((MySimulation)sim).GlobalnyPriemernyCasObjednavkyVSysteme.GetConfidenceInterval().Item2, 4).ToString(CultureInfo.InvariantCulture) + ">";

        string pocetNezacatychObj= Math.Round(((MySimulation)sim).GlobalnyPriemernyPocetNezacatychObjednavok.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
        string pocetNezacatychObjInt = 
                "<" + Math.Round(((MySimulation)sim).GlobalnyPriemernyPocetNezacatychObjednavok.GetConfidenceInterval().Item1, 4).ToString(CultureInfo.InvariantCulture) + ";" +
                "" + Math.Round(((MySimulation)sim).GlobalnyPriemernyPocetNezacatychObjednavok.GetConfidenceInterval().Item2, 4).ToString(CultureInfo.InvariantCulture) + ">";

        string vytazenieA = Math.Round(((MySimulation)sim).GlobalneVytazenieA.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
        string vytazenieAInt =
                "<" + Math.Round(((MySimulation)sim).GlobalneVytazenieA.GetConfidenceInterval().Item1, 4).ToString(CultureInfo.InvariantCulture) + ";" +
                "" + Math.Round(((MySimulation)sim).GlobalneVytazenieA.GetConfidenceInterval().Item2, 4).ToString(CultureInfo.InvariantCulture) + ">";

        string vytazenieB = Math.Round(((MySimulation)sim).GlobalneVytazenieB.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
        string vytazenieBInt =
                "<" + Math.Round(((MySimulation)sim).GlobalneVytazenieB.GetConfidenceInterval().Item1, 4).ToString(CultureInfo.InvariantCulture) + ";" +
                "" + Math.Round(((MySimulation)sim).GlobalneVytazenieB.GetConfidenceInterval().Item2, 4).ToString(CultureInfo.InvariantCulture) + ">";

        string vytazenieC = Math.Round(((MySimulation)sim).GlobalneVytazenieC.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
        string vytazenieCInt =
                "<" + Math.Round(((MySimulation)sim).GlobalneVytazenieC.GetConfidenceInterval().Item1, 4).ToString(CultureInfo.InvariantCulture) + ";" +
                "" + Math.Round(((MySimulation)sim).GlobalneVytazenieC.GetConfidenceInterval().Item2, 4).ToString(CultureInfo.InvariantCulture)  + ">";

        string output = $"{CURRENT_CONFIG.M},{CURRENT_CONFIG.A},{CURRENT_CONFIG.B},{CURRENT_CONFIG.C},{casf},{cas.ToString(CultureInfo.InvariantCulture)},{casInt},{pocetNezacatychObj},{pocetNezacatychObjInt},{vytazenieA},{vytazenieAInt},{vytazenieB},{vytazenieBInt},{vytazenieC},{vytazenieCInt}\n";

		if (cas < 32 * 60 * 60)
		{
			File.AppendAllText(PATH_VYHOVUJUCE, output);
			output = $"VYHOVUJE: {casf} | {pocetNezacatychObj} | {vytazenieA} | {vytazenieB} | {vytazenieC}";
		}
		else
		{
			File.AppendAllText(PATH_NEVYHOVUJUCE, output);
			output = $"NE VYHOVUJE: {casf} | {pocetNezacatychObj} | {vytazenieA} | {vytazenieB} | {vytazenieC}";
		}
		
	    Console.WriteLine(output);
        Console.WriteLine($"Simulation finished in {Stopwatch.Elapsed:hh\\:mm\\:ss}");
        Console.WriteLine();
    }

    private static string FormatTime(double time)
    {
        int hours = (int)(time / 3600); 
        int minutes = (int)(time % 3600) / 60;
        int seconds = (int)time % 60;

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    public class Configuration
    {
		public int M { get; set; }
		public int A { get; set; }
		public int B { get; set; }
		public int C { get; set; }
        public Configuration(Random rnd) 
		{
			M = rnd.Next(49, 60);
            A = rnd.Next(5, 8);
            B = rnd.Next(5, 8);
            C = rnd.Next(35, 50);
        }

        public Configuration()
        {
        }
    }
}