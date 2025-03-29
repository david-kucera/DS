using DSLib.Generators.Empirical;
using DSLib.Generators.Uniform;
using DSSimulationLib.Generators.Exponential;
using DSSimulationLib.Generators.Triangular;
using DSSimulationLib.Simulation;
using DSSimulationLib.Statistics;
using DSSimulationWoodwork.Events;

namespace DSSimulationWoodwork;

public class Stolaren : SimulationCore
{
    #region Constants
    public double STOP_TIME = 249 * 8 * 60 * 60; // 249 dni po 8 hodin
    #endregion // Constants

    #region Class members
    private Random _seeder = null!;
    private int _stolarACount = 0;
    private int _stolarBCount = 0;
    private int _stolarCCount = 0;
    private bool _virtualTime = false;
    #endregion // Class members
    
    #region Properties
    public int PoradieObjednavky { get; set; } = 0;
    public Queue<Objednavka> CakajuceNaRezanie = [];
    public Queue<Objednavka> CakajuceNaMorenie = [];
    public Queue<Objednavka> CakajuceNaSkladanie = [];
    public Queue<Objednavka> CakajuceNaKovanie = [];
    
    public List<Stolar> Stolari = [];
    public List<MontazneMiesto> MontazneMiesta = [];

    public double PocetHotovychObjednavok { get; set; } = 0.0;
    public ConfidenceInterval GlobalnyPocetHotovychObjednavok { get; set; } = new();
    public ConfidenceInterval PriemernyCasObjednavkyVSysteme { get; set; } = new();
    public ConfidenceInterval GlobalnyPriemernyCasObjednavkyVSysteme { get; set; } = new();
    public ConfidenceInterval PriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat { get; set; } = new();
    public ConfidenceInterval GlobalnyPriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat  { get; set; } = new();
    public ConfidenceInterval GlobalnyPocetObjednavok { get; set; } = new();
    
    public ExponentialGenerator PrichodObjednavokGenerator { get; set; } = null!;
    public Random ObjednavkaTypGenerator { get; set; } = null!;
    
    public TriangularGenerator MontazneMiestoSkladGenerator { get; set; } = null!;
    public TriangularGenerator PripravaDrevaVSkladeGenerator { get; set; } = null!;
    public TriangularGenerator PresunMedziMontaznymiMiestamiGenerator { get; set; } = null!;
    
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
    public Stolaren(Random seeder, int a, int b, int c, bool virtualTime)
    {
        _seeder = seeder;
        _stolarACount = a;
        _stolarBCount = b;
        _stolarCCount = c;
        _virtualTime = virtualTime;
    }
    #endregion // Constructor
    
    #region Protected functions
    protected override void BeforeSimulation()
    {
        PrichodObjednavokGenerator = new ExponentialGenerator(_seeder, 2.0 / 60 / 60);
        ObjednavkaTypGenerator = new Random(_seeder.Next());
        
        MontazneMiestoSkladGenerator = new TriangularGenerator(_seeder, 60.0, 480.0, 120.0);
        PripravaDrevaVSkladeGenerator = new TriangularGenerator(_seeder, 300.0, 900.0, 500.0);
        PresunMedziMontaznymiMiestamiGenerator = new TriangularGenerator(_seeder, 120.0, 500.0, 150.0);

        List<(double,double)> intervals =
        [
            (10.0 * 60, 25.0 * 60),
            (25.0 * 60, 50.0 * 60),
        ];
        List<double> percentages =
        [
            0.6,
            0.4
        ];
        StolRezanieGenerator = new ContinousEmpirical(_seeder, intervals, percentages);
        StolMorenieLakovanieGenerator = new ContinousUniform(_seeder, 200.0 * 60, 610.0 * 60);
        StolSkladanieGenerator = new ContinousUniform(_seeder, 30.0 * 60, 60.0 * 60);
        
        StolickaRezanieGenerator = new ContinousUniform(_seeder, 12.0 * 60, 16.0 * 60);
        StolickaMorenieLakovanieGenerator = new ContinousUniform(_seeder, 210.0 * 60, 540.0 * 60);
        StolickaSkladanieGenerator = new ContinousUniform(_seeder, 14.0 * 60, 24.0 * 60);
        
        SkrinaRezanieGenerator = new ContinousUniform(_seeder, 15.0 * 60, 80.0 * 60);
        SkrinaMorenieLakovanieGenerator = new ContinousUniform(_seeder, 600.0 * 60, 700.0 * 60);
        SkrinaSkladanieGenerator = new ContinousUniform(_seeder, 35.0 * 60, 75.0 * 60);
        SkrinaMontazKovaniGenerator = new ContinousUniform(_seeder, 15.0 * 60, 25.0 * 60);
    }

    protected override void AfterSimulation()
    {
        // vypis konecnych statistik
        Console.WriteLine("montazne miesta pocet:");
        Console.WriteLine(MontazneMiesta.Count);
        Console.WriteLine("[seconds]");
        Console.WriteLine("Priemerný počet objednávok:");
        Console.WriteLine(GlobalnyPocetObjednavok.GetValue());
        Console.WriteLine("Priemerný počet hotových objednávok:");
        Console.WriteLine(GlobalnyPocetHotovychObjednavok.GetValue());
        Console.WriteLine("Priemerny cas objednavky v systeme");
        Console.WriteLine(GlobalnyPriemernyCasObjednavkyVSysteme.GetValue());
        Console.WriteLine("Priemerny pocet nezacatych objednavok");
        Console.WriteLine(GlobalnyPriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.GetValue());
        if (GlobalnyPriemernyCasObjednavkyVSysteme.GetValue() < 16*60*60) Console.WriteLine("VYHOVUJE");
        else Console.WriteLine("NEVYHOVUJE");
    }

    protected override void BeforeSimulationRun(int poradieReplikacie)
    {
        PoradieObjednavky = 0;
        PocetHotovychObjednavok = 0;
        CakajuceNaRezanie.Clear();
        CakajuceNaMorenie.Clear();
        CakajuceNaSkladanie.Clear();
        CakajuceNaKovanie.Clear();
        Time = 0.0;

        StopTime = STOP_TIME;
        
        MontazneMiesta.Clear();
        
        EventQueue.Clear();
        if (!_virtualTime) base.BeforeSimulationRun(poradieReplikacie);
        
        PriemernyCasObjednavkyVSysteme.Reset();
        PriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.Reset();
        
        Stolari = new List<Stolar>(_stolarACount + _stolarBCount + _stolarCCount);
        for (int i = 0; i < _stolarACount; i++)
        {
            var stolar = new Stolar(StolarType.A, i);
            Stolari.Add(stolar);
        }
        for (int i = 0; i < _stolarBCount; i++)
        {
            var stolar = new Stolar(StolarType.B, i);
            Stolari.Add(stolar);
        }
        for (int i = 0; i < _stolarCCount; i++)
        {
            var stolar = new Stolar(StolarType.C, i);
            Stolari.Add(stolar);
        }

        var prichod = PrichodObjednavokGenerator.NextDouble() + Time;
        EventQueue.Enqueue(new PrichodObjednavkyEvent(this, prichod), prichod);
    }
    
    protected override void AfterSimulationRun()
    {
        // update globalnych statistik
        GlobalnyPocetHotovychObjednavok.AddValue(PocetHotovychObjednavok);
        GlobalnyPriemernyCasObjednavkyVSysteme.AddValue(PriemernyCasObjednavkyVSysteme.GetValue());
        GlobalnyPriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.AddValue(PriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.GetValue());
        GlobalnyPocetObjednavok.AddValue(PoradieObjednavky);
    }
    #endregion // Protected functions
}