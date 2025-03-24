using DSSimulationLib.Generators.Exponential;
using DSSimulationLib.Simulation;
using DSSimulationLib.Statistics;
using DSSimulationTest.Events;

namespace DSSimulationTest;

public class Predajna : SimulationCore
{
    #region Constants
    public double MULTIPLIER = 3600.0;
    public static double START_TIME = 6 * 60 * 60 * 1000;
    public double STOP_TIME = START_TIME + 8 * 60 * 60 * 1000; // 8 hodin
    public int Den = 1;
    #endregion // Constants
    
    #region Class members
    private Random _seeder { get; set; } = null!;
    private Average _globalAveragePocetLudi { get; set; } = new();
    private Average _globalAverageCasStravenyVRade { get; set; } = new();
    private Average _globalAverageCasStravenyObsluhou { get; set; } = new();
    private Average _globalAverageCasVObchode { get; set; } = new();
    private Average _globalAverageDlzkaRadu { get; set; } = new();
    #endregion // Class members
    
    #region Properties
    public Queue<Osoba> Rad = new();
    public int PoradieOsoby { get; set; } = 0;
    public bool ObsluhovanyClovek { get; set; } = false;
    
    public WeightedAverage AverageDlzkaRadu { get; set; } = new();
    
    public Average AverageCasStravenyVRade { get; set; } = new();
    public Average AverageCasStravenyObsluhou { get; set; } = new();
    public Average AverageCasVPredajni { get; set; } = new();
    
    public ExponentialGenerator PrichodLudiGenerator { get; set; } = null!;
    public ExponentialGenerator TrvanieObsluhy = null!;
    #endregion // Properties
    
    #region Constructor
    public Predajna(Random seeder)
    {
        _seeder = seeder;
    }
    #endregion // Constructor
    
    #region Public functions
    protected override void BeforeSimulation()
    {
        PrichodLudiGenerator = new ExponentialGenerator(_seeder, 0.2/60/1000);
        TrvanieObsluhy = new ExponentialGenerator(_seeder, 0.25/60/1000);
    }

    protected override void AfterSimulation()
    {
        Console.WriteLine($"Primerná dĺžka radu: {_globalAverageDlzkaRadu.GetValue()}");
        Console.WriteLine($"Priemerný čas strávený v predajni: {_globalAverageCasVObchode.GetValue()}");
        Console.WriteLine($"Priemerný čas strávený v rade: {_globalAverageCasStravenyVRade.GetValue()}");
        Console.WriteLine($"Priemerný čas strávený obsluhou: {_globalAverageCasStravenyObsluhou.GetValue()}");
        Console.WriteLine($"Priemerný počet ľudí v predajni: {_globalAveragePocetLudi.GetValue()}");
    }

    protected override void BeforeSimulationRun(int poradieReplikacie)
    {
        Rad.Clear();
        Time = START_TIME;
        StopTime = STOP_TIME;
        Den = poradieReplikacie;
        EventQueue.Clear();
        EventQueue.Enqueue(new SystemEvent(this, Time, MULTIPLIER), Time);

        AverageDlzkaRadu.Reset();
        AverageCasVPredajni.Reset();
        PoradieOsoby = 0;

        ObsluhovanyClovek = false;
        
        var prichod = PrichodLudiGenerator.NextDouble() + Time;
        EventQueue.Enqueue(new PrichodEvent(this, prichod), prichod);
    }

    protected override void AfterSimulationRun()
    {
        _globalAveragePocetLudi.AddValue(PoradieOsoby);
        _globalAverageCasStravenyVRade.AddValue(AverageCasStravenyVRade.GetValue());
        _globalAverageCasStravenyObsluhou.AddValue(AverageCasStravenyObsluhou.GetValue());
        _globalAverageDlzkaRadu.AddValue(AverageDlzkaRadu.GetValue());
        _globalAverageCasVObchode.AddValue(AverageCasVPredajni.GetValue());
    }
    #endregion // Public functions
}