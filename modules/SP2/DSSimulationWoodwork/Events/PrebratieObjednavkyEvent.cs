using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class PrebratieObjednavkyEvent : SimulationEvent
{
    private Stolar _stolar;
    private Objednavka _objednavka;
    public PrebratieObjednavkyEvent(SimulationCore core, double time, Stolar stolar, Objednavka objednavka) : base(core, time)
    {
        _stolar = stolar;
        _objednavka = objednavka;
    }

    public override void Execute()
    {
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        if (_stolar.Type != StolarType.A) throw new Exception("Zly stolar!");
        
        //_stolar.Workload.AddValue(_stolar.Obsadeny, Time);
        //_stolar.Obsadeny = true;
        _objednavka.Status = ObjednavkaStatus.Preberana;
        
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
        
        double trvaniePreberania = stolaren.PrebratieObjednavokGenerator.NextDouble();
        stolaren.EventQueue.Enqueue(new ZaciatokRezaniaEvent(stolaren, Time + trvaniePreberania, _stolar, _objednavka), Time + trvaniePreberania);
    }
}