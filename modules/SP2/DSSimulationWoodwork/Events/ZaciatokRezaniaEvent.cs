using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class ZaciatokRezaniaEvent : SimulationEvent
{
    #region Class members
    private Stolar _stolar;
    private Objednavka _objednavka;
    #endregion // Class members
    
    #region Constructor
    public ZaciatokRezaniaEvent(SimulationCore core, double time, Stolar stolar, Objednavka objednavka) : base(core, time)
    {
        _stolar = stolar;
        _objednavka = objednavka;   
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        if (_stolar.Type != StolarType.A) throw new Exception("Nesprávny typ stolára!");
        
        _stolar.Workload.AddValue(_stolar.Obsadeny, Time);
        _stolar.Obsadeny = true;
        _objednavka.Status = ObjednavkaStatus.PriebehRezania;
        
        double casPrechoduZMontaznehoMiestaDoSkladu = 0.0;
        if (_stolar.MontazneMiesto != null)
            casPrechoduZMontaznehoMiestaDoSkladu = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        double casPripravyDrevaVSklade = stolaren.PripravaDrevaVSkladeGenerator.NextDouble();
        double casPrechoduZoSkladuNaMontazneMiesto = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        if (_stolar.MontazneMiesto != null && _stolar.MontazneMiesto.Stolar != null && _stolar.MontazneMiesto.Stolar.ID == _stolar.ID && _stolar.MontazneMiesto.Stolar.Type == _stolar.Type)
            _stolar.MontazneMiesto.Stolar = null;
        _stolar.MontazneMiesto = _objednavka.MontazneMiesto;
        _stolar.MontazneMiesto.Stolar = _stolar;
        
        double casRezania = 0.0;
        switch (_objednavka.Type)
        {
            case ObjednavkaType.Stol:
                casRezania = stolaren.StolRezanieGenerator.NextDouble();
                break;
            case ObjednavkaType.Skrina:
                casRezania = stolaren.SkrinaRezanieGenerator.NextDouble();
                break;
            case ObjednavkaType.Stolicka:
                casRezania = stolaren.StolickaRezanieGenerator.NextDouble();
                break;
            default:
                throw new Exception("Nie je uvedený typ objednávky!");
        }
        
        double trvanieUdalosti = casPrechoduZMontaznehoMiestaDoSkladu + casPripravyDrevaVSklade + casPrechoduZoSkladuNaMontazneMiesto + casRezania;
        double koniecUdalosti = trvanieUdalosti + Time;
        
        stolaren.EventQueue.Enqueue(new KoniecRezaniaEvent(stolaren, koniecUdalosti, _stolar, _objednavka), koniecUdalosti);
    }
    #endregion // Public functions
}