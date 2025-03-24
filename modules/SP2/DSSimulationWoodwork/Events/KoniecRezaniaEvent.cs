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
        
        if (_stolar.Type != StolarType.A) throw new Exception("Zly stolar!");

        // pokracovanie objednavky pre stolara typu C
        if (stolaren.CakajuceNaMorenie.Count >= 1)
        {
            _objednavka.Status = ObjStatus.CakajucaNaMorenie;
            stolaren.CakajuceNaMorenie.Enqueue(_objednavka);
        }
        else
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny || st.Type != StolarType.C) continue;
                stolar = st;
                break;
            }
            
            if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokMoreniaEvent(stolaren, Time, _objednavka, stolar), Time);
            else
            {
                _objednavka.Status = ObjStatus.CakajucaNaMorenie;
                stolaren.CakajuceNaMorenie.Enqueue(_objednavka);
            }
        }
        
        // naplanovanie dalsieho rezania
        if (stolaren.CakajuceNaRezanie.Count >= 1)
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny || st.Type != StolarType.A) continue;
                stolar = st;
                break;
            }
            if (stolar is null) throw new Exception("Nebol najdeny stolar!");
            
            var dalsiaObj = stolaren.CakajuceNaRezanie.Dequeue();
            stolaren.EventQueue.Enqueue(new ZaciatokRezaniaEvent(stolaren, Time, stolar, dalsiaObj), Time);
        }
    }
    #endregion // Public functions
}