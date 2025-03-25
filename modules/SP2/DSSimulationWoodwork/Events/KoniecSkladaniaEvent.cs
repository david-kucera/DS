using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class KoniecSkladaniaEvent : SimulationEvent
{
    #region Class members
    private Objednavka _objednavka;
    private Stolar _stolar;
    #endregion // Class members
    
    #region Constructor
    public KoniecSkladaniaEvent(SimulationCore core, double time, Objednavka objednavka, Stolar stolar) : base(core, time)
    {
        _objednavka = objednavka;
        _stolar = stolar;
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        _stolar.Obsadeny = false;
        _objednavka.Status = ObjednavkaStatus.CakajucaNaMontazKovani;
        
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        
        if (_stolar.Type != StolarType.B) throw new Exception("Zly stolar!");
        
        // ak je objednavka skrina, tak pokracuje na montaz kovani
        if (_objednavka.Type == ObjednavkaType.Skrina)
        {
            if (stolaren.CakajuceNaKovanie.Count >= 1) stolaren.CakajuceNaKovanie.Enqueue(_objednavka);
            else
            {
                Stolar stolar = null;
                foreach (var st in stolaren.Stolari)
                {
                    if (st.Obsadeny || st.Type != StolarType.C) continue;
                    stolar = st;
                    break;
                }

                if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokMontazeEvent(stolaren, Time, _objednavka, stolar), Time);
                else stolaren.CakajuceNaKovanie.Enqueue(_objednavka);
            }
        }
        else
        {
            // zber statistik objednavok, ktore koncia v systeme
            _objednavka.Status = ObjednavkaStatus.Hotova;
            _objednavka.MontazneMiesto.Objednavka = null;
            stolaren.PocetHotovychObjednavok++;
            stolaren.PriemernyCasObjednavkyVSysteme.AddValue(Time - _objednavka.ArrivalTime);
        }
        
        // naplanovanie dalsieho skladania
        if (stolaren.CakajuceNaSkladanie.Count >= 1)
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny || st.Type != StolarType.B) continue;
                stolar = st;
                break;
            }
            if (stolar is not null)
            {
                var dalsiaObj = stolaren.CakajuceNaSkladanie.Dequeue();
                stolaren.EventQueue.Enqueue(new ZaciatokSkladaniaEvent(stolaren, Time, dalsiaObj, stolar), Time);
            }
        }
    }
    #endregion // Public functions
}