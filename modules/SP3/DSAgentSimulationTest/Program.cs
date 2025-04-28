using System.Globalization;
using Simulation;

namespace DSAgentSimulationTest;

class Program
{
    #region Constants
    public static int REP_COUNT = 100;
    public static int END_TIME = 249 * 8 * 60 * 60;
    public static int SEED = 0;
    public static Random SEEDER = new(SEED);
	public static int A;
    public static int B;
    public static int C;
    public static int M;
    public static Configuration CURRENT_CONFIG;
    public static string OUTPUT_DIR = "../../output/";
    public static string OUTPUT_FILE = OUTPUT_DIR + "output.csv";
    public static string PATH_VYHOVUJUCE = OUTPUT_DIR + "vyhovuje.csv";
    public static string PATH_NEVYHOVUJUCE = OUTPUT_DIR + "nevyhovuje.csv";
	public static object LOCK = new();
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
		while (true)
		{
			var randomConfig = new Configuration();
			if (configs.Any(c => c.M == randomConfig.M && c.A == randomConfig.A && c.B == randomConfig.B && c.C == randomConfig.C)) continue;
			configs.Add(randomConfig);
			CURRENT_CONFIG = randomConfig;
			MySimulation sim = new MySimulation(SEEDER, randomConfig.M, randomConfig.A, randomConfig.B, randomConfig.C);
			Console.WriteLine($"Current: M{randomConfig.M}, A{randomConfig.A}, B{randomConfig.B}, C{randomConfig.C}");
			sim.OnSimulationDidFinish(ZapisStat);
			sim.Start(REP_COUNT, END_TIME);
		}
	}

	private static void RunSequentially()
	{
		for (int M = 40; M < 50; M++) // MM
		{
			for (int A = 4; A < 8; A++) // A
			{
				for (int B = 4; B < 8; B++) // B
				{
					for (int C = 36; C < 50; C++) // C
					{
						MySimulation sim = new MySimulation(SEEDER, M, A, B, C);
						Console.WriteLine($"Current: M{M}, A{A}, B{B}, C{C}");
						sim.OnSimulationDidFinish(ZapisStat);
						sim.Start(REP_COUNT, END_TIME);
					}
				}
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
		if (cas < 42 * 60 * 60) Console.WriteLine("VYHOVUJE");
		else Console.WriteLine("NE VYHOVUJE");
	}

	private static void ZapisStat(OSPABA.Simulation sim)
    {
        double cas = ((MySimulation)sim).GlobalnyPriemernyCasObjednavkyVSysteme.GetValue();
		string casf = FormatTime(((MySimulation)sim).GlobalnyPriemernyCasObjednavkyVSysteme.GetValue());
        string pocetNezacatychObj= Math.Round(((MySimulation)sim).GlobalnyPriemernyPocetNezacatychObjednavok.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
        string vytazenieA = Math.Round(((MySimulation)sim).GlobalneVytazenieA.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
		string vytazenieB = Math.Round(((MySimulation)sim).GlobalneVytazenieB.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
	    string vytazenieC = Math.Round(((MySimulation)sim).GlobalneVytazenieC.GetValue(), 2).ToString(CultureInfo.InvariantCulture);
	    string output = $"{CURRENT_CONFIG.M},{CURRENT_CONFIG.A},{CURRENT_CONFIG.B},{CURRENT_CONFIG.C},{casf},{cas.ToString(CultureInfo.InvariantCulture)},{pocetNezacatychObj},{vytazenieA},{vytazenieB},{vytazenieC}\n";

		if (cas < 42 * 60 * 60)
		{
			File.AppendAllText(PATH_VYHOVUJUCE, output);
			output = $"VYHOVUJE: {casf} | {pocetNezacatychObj} | {vytazenieA} | {vytazenieB} | {vytazenieC}\n";
		}
		else
		{
			File.AppendAllText(PATH_NEVYHOVUJUCE, output);
			output = $"NE VYHOVUJE: {casf} | {pocetNezacatychObj} | {vytazenieA} | {vytazenieB} | {vytazenieC}\n";
		}
		
	    Console.WriteLine(output);
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
		public int M { get; set; } = SEEDER.Next(40, 50);
		public int A { get; set; } = SEEDER.Next(4, 8);
		public int B { get; set; } = SEEDER.Next(4, 8);
		public int C { get; set; } = SEEDER.Next(36, 50);
    }
}