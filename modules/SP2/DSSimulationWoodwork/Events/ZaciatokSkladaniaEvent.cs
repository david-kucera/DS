using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class ZaciatokSkladaniaEvent : SimulationEvent
{
    #region Class members
    private Objednavka _objednavka;
    private Stolar _stolar;
    #endregion // Class members
    
    #region Constructor
    public ZaciatokSkladaniaEvent(SimulationCore core, double time, Objednavka objednavka, Stolar stolar) : base(core, time)
    {
        _objednavka = objednavka;
        _stolar = stolar;
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        _stolar.Obsadeny = true;
        
        if (_stolar.Type != StolarType.B) throw new Exception("Nesprávny typ stolára!");
        
        double casPrechoduNaMontazneMiesto;
        if (_stolar.MontazneMiesto == null) 
            casPrechoduNaMontazneMiesto = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        else if (_stolar.MontazneMiesto != _objednavka.MontazneMiesto)
            casPrechoduNaMontazneMiesto = stolaren.PresunMedziMontaznymiMiestamiGenerator.NextDouble();
        else casPrechoduNaMontazneMiesto = 0.0;
        _stolar.MontazneMiesto = _objednavka.MontazneMiesto;

        double casSkladania = 0.0;
        switch (_objednavka.Type)
        {
            case ObjType.Stol:
                casSkladania = stolaren.StolSkladanieGenerator.NextDouble();
                break;
            case ObjType.Skrina:
                casSkladania = stolaren.SkrinaSkladanieGenerator.NextDouble();
                break;
            case ObjType.Stolicka:
                casSkladania = stolaren.StolickaSkladanieGenerator.NextDouble();
                break;
            default:
                throw new Exception("Nie je uvedený typ objednávky!");
        }
        _objednavka.Status = ObjStatus.Poskladana;

        double trvanieUdalosti = casPrechoduNaMontazneMiesto + casSkladania;
        double koniecUdalosti = trvanieUdalosti + Time;

        stolaren.EventQueue.Enqueue(new KoniecSkladaniaEvent(stolaren, koniecUdalosti, _objednavka, _stolar), koniecUdalosti);
    }
    #endregion // Public functions
}