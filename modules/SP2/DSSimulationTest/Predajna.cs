using DSSimulationLib.Generators.Exponential;
using DSSimulationLib.Simulation;
using DSSimulationLib.Statistics;
using DSSimulationTest.Events;

namespace DSSimulationTest;

public class Predajna : SimulationCore
{
    #region Constants
    public double START_TIME = 0.0;
    public double STOP_TIME = 8*60; // 8 hodin
    #endregion // Constants
    
    #region Properties
    public Queue<Osoba> Rad = new();
    public int PoradieOsoby { get; set; } = 0;
    public bool ObsluhovanyClovek { get; set; } = false;

    public WeightedAverage AverageDlzkaRadu { get; set; } = new();
    public Average AverageCasVPredajni { get; set; } = new();
    
    public Average GlobalAveragePocetLudi { get; set; } = new();
    public Average GlobalAverageCasVObchode { get; set; } = new();
    //public Average GlobalAverageDlzkaRadu { get; set; } = new();
    
    public Random Seeder { get; set; } = null!;
    public ExponentialGenerator PrichodLudiGenerator { get; set; } = null!;
    public double TrvanieObsluhy = 0;
    #endregion // Properties
    
    #region Constructor
    public Predajna(Random seeder)
    {
        Seeder = seeder;
    }
    #endregion // Constructor
    
    #region Public functions
    protected override void BeforeSimulation()
    {
        PrichodLudiGenerator = new ExponentialGenerator(Seeder,0.2);
        TrvanieObsluhy = 4;
    }

    protected override void AfterSimulation()
    {
        // Console.WriteLine($"Primerná dĺžka radu: {GlobalAverageDlzkaRadu.GetValue()}");
        Console.WriteLine($"Priemerný čas strávený v predajni: {GlobalAverageCasVObchode.GetValue()}");
        Console.WriteLine($"Priemerný počet ľudí v predajni: {GlobalAveragePocetLudi.GetValue()}");
    }

    protected override void BeforeSimulationRun()
    {
        Rad.Clear();
        Time = 0.0;
        EventQueue.Clear();

        AverageDlzkaRadu.Reset();
        AverageCasVPredajni.Reset();
        PoradieOsoby = 0;

        ObsluhovanyClovek = false;
        
        var prichod = PrichodLudiGenerator.NextDouble() + Time;
        EventQueue.Enqueue(new PrichodEvent(this, prichod), prichod);
    }

    protected override void AfterSimulationRun()
    {
        GlobalAveragePocetLudi.AddValue(PoradieOsoby);
        // GlobalAverageDlzkaRadu.AddValue(AverageDlzkaRadu.GetValue());
        GlobalAverageCasVObchode.AddValue(AverageCasVPredajni.GetValue());
    }
    #endregion // Public functions
}