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
        _objednavka.Status = ObjednavkaStatus.PriebehMontazeKovani;
        
        if (_stolar.Type != StolarType.C) throw new Exception("Nesprávny typ stolára!");
        
        double casPrechoduNaMontazneMiesto;
        if (_stolar.MontazneMiesto == null) 
            casPrechoduNaMontazneMiesto = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        else if (_stolar.MontazneMiesto != _objednavka.MontazneMiesto)
            casPrechoduNaMontazneMiesto = stolaren.PresunMedziMontaznymiMiestamiGenerator.NextDouble();
        else casPrechoduNaMontazneMiesto = 0.0;
        _stolar.MontazneMiesto = _objednavka.MontazneMiesto;

        double casMontaze = stolaren.SkrinaMontazKovaniGenerator.NextDouble();

        double trvanieUdalosti = casPrechoduNaMontazneMiesto + casMontaze;
        double koniecUdalosti = trvanieUdalosti + Time;
        
        stolaren.EventQueue.Enqueue(new KoniecMontazeEvent(stolaren, koniecUdalosti, _objednavka, _stolar), koniecUdalosti);
    }
    #endregion // Class members
}