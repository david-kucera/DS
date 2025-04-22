using Simulation;

namespace DSAgentSimulationTest;

class Program
{
    #region Constants
    public static int REP_COUNT = 100;
    public static int END_TIME = 249 * 8 * 60 * 60;
    public static int SEED = 0;
    public static int A = 7;
    public static int B = 7;
    public static int C = 43;
    public static int M = 100;
    #endregion // Constants
    
    static void Main(string[] args)
    {
        Random seeder = new Random(SEED);
        MySimulation sim = new MySimulation(seeder, M, A, B, C);
        sim.OnReplicationDidFinish(VypisRep);
        sim.OnSimulationDidFinish(VypisStat);
        sim.Start(REP_COUNT, END_TIME);
    }

    private static void VypisRep(OSPABA.Simulation obj)
    {
        Console.WriteLine($"R{obj.CurrentReplication}");
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
    
    private static string FormatTime(double time)
    {
        int hours = (int)(time / 3600); 
        int minutes = (int)(time % 3600) / 60;
        int seconds = (int)time % 60;

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}