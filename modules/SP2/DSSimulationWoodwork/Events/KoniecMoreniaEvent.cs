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

    #region Public functions
    public override void Execute()
    {
        _stolar.Obsadeny = false;
        _objednavka.Status = ObjednavkaStatus.CakajucaNaSkladanie;
        
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        
        if (_stolar.Type != StolarType.C) throw new Exception("Zly stolar!");
        
        // pokracovanie objednavky na skladanie
        if (stolaren.CakajuceNaSkladanie.Count >= 1)
        {
            stolaren.CakajuceNaSkladanie.Enqueue(_objednavka);
        }
        else
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny || st.Type != StolarType.B) continue;
                stolar = st;
                break;
            }

            if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokSkladaniaEvent(stolaren, Time, _objednavka, stolar), Time);
            else
            {
                stolaren.CakajuceNaSkladanie.Enqueue(_objednavka);
            }
        }
        
        // naplanovanie dalsej aktivity pre stolarov typu C
        if (stolaren.CakajuceNaKovanie.Count >= 1)
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny || st.Type != StolarType.C) continue;
                stolar = st;
                break;
            }
            if (stolar is not null)
            {
                var dalsiaObj = stolaren.CakajuceNaKovanie.Dequeue();
                stolaren.EventQueue.Enqueue(new ZaciatokMontazeEvent(stolaren, Time, dalsiaObj, stolar), Time);
            }
        }
        else if (stolaren.CakajuceNaMorenie.Count >= 1)
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny || st.Type != StolarType.C) continue;
                stolar = st;
                break;
            }
            if (stolar is not null)
            {
                var dalsiaObj = stolaren.CakajuceNaMorenie.Dequeue();
                stolaren.EventQueue.Enqueue(new ZaciatokMoreniaEvent(stolaren, Time, dalsiaObj, stolar), Time);
            }
        }
    }
    #endregion // Public functions
}