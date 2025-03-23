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
        
        // naplanovanie dalsej montaze kovani
        if (stolaren.PoskladaneSkrineQueue.Count >= 1)
        {
            var dalsiaObj = stolaren.PoskladaneSkrineQueue.Dequeue();
            stolaren.EventQueue.Enqueue(new ZaciatokMontazeKovani(stolaren, Time, dalsiaObj, _stolar), Time);
        }
    }
    #endregion // Public functions
}