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

        _stolar.Obsadeny = true;
        double casPrechoduZMontaznehoMiestaDoSkladu = 0.0;
        if (_stolar.Poloha == Poloha.MontazneMiesto)
        {
            casPrechoduZMontaznehoMiestaDoSkladu = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        }
        double casPripravyDrevaVSklade = stolaren.PripravaDrevaVSkladeGenerator.NextDouble();
        double casPrechoduZoSkladuNaMontazneMiesto = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        double casRezania = 0.0;
        switch (_objednavka.Type)
        {
            case ObjType.Stol:
                casRezania = stolaren.StolRezanieGenerator.NextDouble();
                break;
            case ObjType.Skrina:
                casRezania = stolaren.SkrinaRezanieGenerator.NextDouble();
                break;
            case ObjType.Stolicka:
                casRezania = stolaren.StolickaRezanieGenerator.NextDouble();
                break;
            default:
                throw new Exception("Nie je uvedený typ objednávky!");
        }
        _stolar.Poloha = Poloha.MontazneMiesto;
        _stolar.IDMiesta = _objednavka.Poradie;
        
        double trvanieUdalosti = casPrechoduZMontaznehoMiestaDoSkladu + casPripravyDrevaVSklade + casPrechoduZoSkladuNaMontazneMiesto + casRezania;
        double koniecUdalosti = trvanieUdalosti + Time;
        stolaren.EventQueue.Enqueue(new KoniecRezaniaEvent(stolaren, koniecUdalosti, _stolar, _objednavka), koniecUdalosti);
    }
    #endregion // Public functions
}