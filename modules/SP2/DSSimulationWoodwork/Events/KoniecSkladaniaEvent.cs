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
        _objednavka.Status = ObjStatus.Poskladana;
        
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        
        // ak je objednavka skrina, tak pokracuje na montaz kovani
        if (_objednavka.Type == ObjType.Skrina)
        {
            if (stolaren.PoskladaneSkrineQueue.Count >= 1)
            {
                _objednavka.Status = ObjStatus.CakajucaNaMontazKovani;
                stolaren.PoskladaneSkrineQueue.Enqueue(_objednavka);
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

                if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokMontazeKovani(stolaren, Time, _objednavka, stolar), Time);
                else
                {
                    _objednavka.Status = ObjStatus.CakajucaNaMontazKovani;
                    stolaren.PoskladaneSkrineQueue.Enqueue(_objednavka);
                }
            }
        }
        else
        {
            // zber statistik objednavok, ktore koncia v systeme
            stolaren.PriemernyCasObjednavkyVSysteme.AddValue(Time - _objednavka.ArrivalTime);
        }
        
        // naplanovanie dalsieho skladania
        if (stolaren.NamoreneObjednavkyQueue.Count >= 1)
        {
            var dalsiaObj = stolaren.NamoreneObjednavkyQueue.Dequeue();
            stolaren.EventQueue.Enqueue(new ZaciatokSkladaniaEvent(stolaren, Time, _objednavka, _stolar), Time);
        }
    }
    #endregion // Public functions
}