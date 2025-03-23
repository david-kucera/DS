using DSLib.Generators.Empirical;
using DSLib.Generators.Uniform;
using DSSimulationLib.Generators.Exponential;
using DSSimulationLib.Generators.Triangular;
using DSSimulationLib.Simulation;
using DSSimulationWoodwork.Events;

namespace DSSimulationWoodwork;

public class Stolaren : SimulationCore
{
    #region Constants
    public double START_TIME = 0.0;
    public double STOP_TIME = 8*60; // 8 hodin
    #endregion // Constants

    #region Class members
    private Random _seeder = null!;
    private int _stolarACount = 0;
    private int _stolarBCount = 0;
    private int _stolarCCount = 0;
    #endregion // Class members
    
    #region Properties
    public int PoradieObjednavky { get; set; } = 0;
    public Queue<Objednavka> NezacateObjednavky = new();
    
    public List<Stolar> StolarA = new();
    public List<Stolar> StolarB = new();
    public List<Stolar> StolarC = new();
    
    
    public ExponentialGenerator PrichodObjednavokGenerator { get; set; } = null!;
    
    public TriangularGenerator MontazneMiestoSkladGenerator { get; set; } = null!;
    public TriangularGenerator PripravaDrevaVSkladeGenerator { get; set; } = null!;
    public TriangularGenerator PresunMedziMontaznymiMiestami { get; set; } = null!;
    
    public ContinousEmpirical StolRezanieGenerator { get; set; } = null!;
    public ContinousUniform StolMorenieLakovanieGenerator { get; set; } = null!;
    public ContinousUniform StolSkladanieGenerator { get; set; } = null!;
    
    public ContinousUniform StolickaRezanieGenerator { get; set; } = null!;
    public ContinousUniform StolickaMorenieLakovanieGenerator { get; set; } = null!;
    public ContinousUniform StolickaSkladanieGenerator { get; set; } = null!;
    
    public ContinousUniform SkrinaRezanieGenerator { get; set; } = null!;
    public ContinousUniform SkrinaMorenieLakovanieGenerator { get; set; } = null!;
    public ContinousUniform SkrinaSkladanieGenerator { get; set; } = null!;
    public ContinousUniform SkrinaMontazKovaniGenerator { get; set; } = null!;
    #endregion // Properties
    
    #region Constructor
    public Stolaren(Random seeder, int a, int b, int c)
    {
        _seeder = seeder;
        _stolarACount = a;
        _stolarBCount = b;
        _stolarCCount = c;
    }
    #endregion // Constructor
    
    protected override void BeforeSimulation()
    {
        PrichodObjednavokGenerator = new ExponentialGenerator(_seeder, 2.0);
        
        MontazneMiestoSkladGenerator = new TriangularGenerator(_seeder, (60.0 / 60), (480.0 / 60), (120.0 / 60));
        PripravaDrevaVSkladeGenerator = new TriangularGenerator(_seeder, (300.0 / 60), (900.0 / 60), (500.0 / 60));
        PresunMedziMontaznymiMiestami = new TriangularGenerator(_seeder, (120.0 / 60), (500.0 / 60), (150.0 / 60));

        List<(int,int)> intervals =
        [
            (10, 25),
            (25, 50)
        ];
        List<double> percentages =
        [
            0.6,
            0.4
        ];
        StolRezanieGenerator = new ContinousEmpirical(_seeder, intervals, percentages);
        StolMorenieLakovanieGenerator = new ContinousUniform(_seeder, 200, 610);
        StolSkladanieGenerator = new ContinousUniform(_seeder, 30, 60);
        
        StolickaRezanieGenerator = new ContinousUniform(_seeder, 12, 16);
        StolickaMorenieLakovanieGenerator = new ContinousUniform(_seeder, 210, 540);
        StolickaSkladanieGenerator = new ContinousUniform(_seeder, 14, 24);
        
        SkrinaRezanieGenerator = new ContinousUniform(_seeder, 15, 80);
        SkrinaMorenieLakovanieGenerator = new ContinousUniform(_seeder, 600, 700);
        SkrinaSkladanieGenerator = new ContinousUniform(_seeder, 35, 75);
        SkrinaMontazKovaniGenerator = new ContinousUniform(_seeder, 15, 25);
        
        StolarA = new List<Stolar>(_stolarACount);
        for (int i = 0; i < _stolarACount; i++)
        {
            var stolar = new Stolar(StolarType.A)
            {
                Poloha = Poloha.Sklad
            };
            StolarA.Add(stolar);
        }
        StolarB = new List<Stolar>(_stolarBCount);
        for (int i = 0; i < _stolarBCount; i++)
        {
            var stolar = new Stolar(StolarType.B)
            {
                Poloha = Poloha.Sklad
            };
            StolarB.Add(stolar);
        }
        StolarC = new List<Stolar>(_stolarCCount);
        for (int i = 0; i < _stolarCCount; i++)
        {
            var stolar = new Stolar(StolarType.C)
            {
                Poloha = Poloha.Sklad
            };
            StolarC.Add(stolar);
        }
    }

    protected override void AfterSimulation()
    {
        // vypis konecnych statistik
        throw new NotImplementedException();
    }

    protected override void BeforeSimulationRun()
    {
        PoradieObjednavky = 0;
        NezacateObjednavky.Clear();
        Time = 0.0;

        var prichod = PrichodObjednavokGenerator.NextDouble() + Time;
        EventQueue.Enqueue(new PrichodObjednavky(this, prichod), prichod);
    }

    protected override void AfterSimulationRun()
    {
        // update globalnych statistik
        throw new NotImplementedException();
    }
}