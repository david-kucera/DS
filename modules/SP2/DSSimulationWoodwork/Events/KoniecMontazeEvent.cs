using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class KoniecMontazeEvent : SimulationEvent
{
    #region Class members
    private Objednavka _objednavka;
    private Stolar _stolar;
    #endregion // Class members

    #region Constructor
    public KoniecMontazeEvent(SimulationCore core, double time, Objednavka objednavka, Stolar stolar) : base(core, time)
    {
        _objednavka = objednavka;
        _stolar = stolar;
    }
    #endregion // Constructor
    
    #region Public functions
    public override void Execute()
    {
        _stolar.Obsadeny = false;
        _objednavka.Status = ObjStatus.Hotova;
        
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        
        // koniec cyklu objednavky ... zber statistik
        stolaren.PocetHotovychObjednavok++;
        stolaren.PriemernyCasObjednavkyVSysteme.AddValue(Time - _objednavka.ArrivalTime);
        
        // naplanovanie dalsej aktivity pre stolarov typu C
        if (stolaren.StolariCQueue.Count >= 1)
        {
            Stolar stolar = null;
            foreach (var st in stolaren.StolariC)
            {
                if (st.Obsadeny) continue;
                stolar = st;
                break;
            }
            if (stolar is not null)
            {
                var dalsiaObj = stolaren.StolariCQueue.Dequeue();
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
        }
    }
    #endregion // Public functions
}