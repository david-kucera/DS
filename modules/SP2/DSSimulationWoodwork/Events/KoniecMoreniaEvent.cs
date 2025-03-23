using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class KoniecMoreniaEvent : SimulationEvent
{
    #region Class members
    private Objednavka _objednavka;
    private Stolar _stolar;
    #endregion // Class members
    
    #region Constructor
    public KoniecMoreniaEvent(SimulationCore core, double time, Objednavka objednavka, Stolar stolar) : base(core, time)
    {
        _objednavka = objednavka;
        _stolar = stolar;
    }
    #endregion // Constructor

    public override void Execute()
    {
        _stolar.Obsadeny = false;
        _objednavka.Status = ObjStatus.Namorena;
        
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        
        // pokracovanie objednavky na skladanie
        if (stolaren.NamoreneObjednavkyQueue.Count >= 1)
        {
            _objednavka.Status = ObjStatus.CakajucaNaSkladanie;
            stolaren.NamoreneObjednavkyQueue.Enqueue(_objednavka);
        }
        else
        {
            Stolar stolar = null;
            foreach (var st in stolaren.StolariB)
            {
                if (st.Obsadeny) continue;
                stolar = st;
                break;
            }

            if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokSkladaniaEvent(stolaren, Time, _objednavka, stolar), Time);
            else
            {
                _objednavka.Status = ObjStatus.CakajucaNaSkladanie;
                stolaren.NamoreneObjednavkyQueue.Enqueue(_objednavka);
            }
        }
        
        // naplanovanie dalsieho morenia
        if (stolaren.NarezaneObjednavkyQueue.Count >= 1)
        {
            var dalsiaObj = stolaren.NarezaneObjednavkyQueue.Dequeue();
            stolaren.EventQueue.Enqueue(new ZaciatokMoreniaEvent(stolaren, Time, _objednavka, _stolar), Time);
        }
    }
}