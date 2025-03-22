using DSSimulationLib.Generators.Exponential;
using DSSimulationLib.Simulation;
using DSSimulationLib.Statistics;

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
    public Average GlobalAverageDlzkaRadu { get; set; } = new();
    
    public Random Seeder { get; set; } = null!;
    public ExponentialGenerator PrichodLudiGenerator { get; set; } = null!;
    public int TrvanieObsluhy = 0;
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
        PrichodLudiGenerator = new ExponentialGenerator(Seeder,(60/12));
        TrvanieObsluhy = 4;
    }

    protected override void AfterSimulation()
    {
        throw new NotImplementedException();
    }

    protected override void BeforeSimulationRun()
    {
        throw new NotImplementedException();
    }

    protected override void AfterSimulationRun()
    {
        throw new NotImplementedException();
    }
    #endregion // Public functions
}