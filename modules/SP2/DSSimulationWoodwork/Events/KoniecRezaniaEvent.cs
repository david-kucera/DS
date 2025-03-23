using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class KoniecRezaniaEvent : SimulationEvent
{
    #region Class members
    private Objednavka _objednavka;
    private Stolar _stolar;
    #endregion // Class members
    
    #region Constructor
    public KoniecRezaniaEvent(SimulationCore core, double time, Stolar stolar, Objednavka objednavka) : base(core, time)
    {
        _stolar = stolar;
        _objednavka = objednavka;
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        _stolar.Obsadeny = false;
        _objednavka.Status = ObjStatus.Narezana;
        
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();

        // pokracovanie objednavky na morenie a lakovanie
        if (stolaren.NarezaneObjednavkyQueue.Count >= 1)
        {
            _objednavka.Status = ObjStatus.CakajucaNaMorenie;
            stolaren.NarezaneObjednavkyQueue.Enqueue(_objednavka);
        }
        else
        {
            Stolar stolar = null;
            foreach (var st in stolaren.StolariC)
            {
                if (st.Obsadeny) continue;
                stolar = st;
                break;
            }

            if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokMoreniaEvent(stolaren, Time, _objednavka, _stolar), Time);
            else
            {
                _objednavka.Status = ObjStatus.CakajucaNaMorenie;
                stolaren.NarezaneObjednavkyQueue.Enqueue(_objednavka);
            }
        }
        
        // naplanovanie dalsieho rezania
        if (stolaren.NezacateObjednavkyQueue.Count >= 1)
        {
            var dalsiaObj = stolaren.NezacateObjednavkyQueue.Dequeue();
            stolaren.EventQueue.Enqueue(new ZaciatokRezaniaEvent(stolaren, Time, _stolar, dalsiaObj), Time);
        }
    }
    #endregion // Public functions
}