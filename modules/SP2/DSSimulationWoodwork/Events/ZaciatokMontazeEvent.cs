using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class ZaciatokMontazeEvent : SimulationEvent
{
    #region Class members
    private Objednavka _objednavka;
    private Stolar _stolar;
    #endregion // Class members
    
    #region Constructor
    public ZaciatokMontazeEvent(SimulationCore core, double time, Objednavka objednaka, Stolar stolar) : base(core, time)
    {
        _objednavka = objednaka;
        _stolar = stolar;
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        _stolar.Obsadeny = true;
        
        if (_stolar.Type != StolarType.C) throw new Exception("Nesprávny typ stolára!");
        
        double casPrechoduNaMontazneMiesto;
        if (_stolar.Poloha == Poloha.Sklad) 
            casPrechoduNaMontazneMiesto = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        else if (_stolar.IDMiesta != _objednavka.Poradie)
            casPrechoduNaMontazneMiesto = stolaren.PresunMedziMontaznymiMiestamiGenerator.NextDouble();
        else casPrechoduNaMontazneMiesto = 0.0;
        _stolar.Poloha = Poloha.MontazneMiesto;
        _stolar.IDMiesta = _objednavka.Poradie;

        double casMontaze = stolaren.SkrinaMontazKovaniGenerator.NextDouble();

        double trvanieUdalosti = casPrechoduNaMontazneMiesto + casMontaze;
        double koniecUdalosti = trvanieUdalosti + Time;
        
        stolaren.EventQueue.Enqueue(new KoniecMontazeEvent(stolaren, koniecUdalosti, _objednavka, _stolar), koniecUdalosti);
    }
    #endregion // Class members
}