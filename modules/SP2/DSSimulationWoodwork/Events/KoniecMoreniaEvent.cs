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
        if (stolaren.CakajuceNaSkladanie.Count >= 1)
        {
            _objednavka.Status = ObjStatus.CakajucaNaSkladanie;
            stolaren.CakajuceNaSkladanie.Enqueue(_objednavka);
        }
        else
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny && st.Type != StolarType.B) continue;
                stolar = st;
                break;
            }

            if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokSkladaniaEvent(stolaren, Time, _objednavka, stolar), Time);
            else
            {
                _objednavka.Status = ObjStatus.CakajucaNaSkladanie;
                stolaren.CakajuceNaSkladanie.Enqueue(_objednavka);
            }
        }
        
        // naplanovanie dalsej aktivity pre stolarov typu C
        if (stolaren.CakajuceNaMorenie.Count >= 1)
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny && st.Type != StolarType.C) continue;
                stolar = st;
                break;
            }
            if (stolar is not null)
            {
                var dalsiaObj = stolaren.CakajuceNaMorenie.Dequeue();
                if (dalsiaObj.Status == ObjStatus.CakajucaNaMontazKovani)
                {
                    stolaren.EventQueue.Enqueue(new ZaciatokMontazeEvent(stolaren, Time, dalsiaObj, _stolar), Time);
                }
                else if (dalsiaObj.Status == ObjStatus.CakajucaNaMorenie)
                {
                    stolaren.EventQueue.Enqueue(new ZaciatokMoreniaEvent(stolaren, Time, dalsiaObj, _stolar), Time);
                }
                else throw new Exception("Chyba statusu objednavky!");
            }
            else throw new Exception("Chyba!");
        }
    }
}