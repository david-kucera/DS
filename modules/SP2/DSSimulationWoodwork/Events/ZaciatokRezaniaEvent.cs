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
        
        _stolar.Obsadeny = true;
        _objednavka.Status = ObjednavkaStatus.PriebehRezania;

        // hladam volne montazne miesto pre objednavku
        foreach (var mm in stolaren.MontazneMiesta)
        {
            if (mm.Objednavka == null || mm.Objednavka.Status == ObjednavkaStatus.Hotova)
            {
                _objednavka.MontazneMiesto = mm;
                mm.Objednavka = _objednavka;
                break;
            }
        }
        // ak ho nenajdem, vytvorim nove
        if (_objednavka.MontazneMiesto == null!)
        {
            var idNovehoMiesta = stolaren.MontazneMiesta.Count;
            var montazneMiesto = new MontazneMiesto(idNovehoMiesta)
            {
                Objednavka = _objednavka,
            };
            stolaren.MontazneMiesta.Add(montazneMiesto);
            _objednavka.MontazneMiesto = montazneMiesto;
        }
        
        double casPrechoduZMontaznehoMiestaDoSkladu = 0.0;
        if (_stolar.MontazneMiesto != null)
            casPrechoduZMontaznehoMiestaDoSkladu = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        double casPripravyDrevaVSklade = stolaren.PripravaDrevaVSkladeGenerator.NextDouble();
        double casPrechoduZoSkladuNaMontazneMiesto = stolaren.MontazneMiestoSkladGenerator.NextDouble();
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