using DSSimulationLib.Simulation;

namespace DSSimulationTest.Events;

public class KoniecObsluhyEvent : SimulationEvent
{
    #region Class members
    private Osoba _osoba = null!;
    #endregion // Class members
    
    #region Constructor
    public KoniecObsluhyEvent(SimulationCore core, double time, Osoba osoba) : base(core, time)
    {
        _osoba = osoba;
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        Predajna predajna = Core as Predajna ?? throw new InvalidOperationException();
        predajna.AverageCasVPredajni.AddValue(predajna.Time - _osoba.ArrivalTime);

        if (predajna.Rad.Count >= 1)
        {
            var osoba = predajna.Rad.Dequeue();
            predajna.EventQueue.Enqueue(new ZaciatokObsluhyEvent(predajna, predajna.Time, osoba), predajna.Time);
        }
        
        predajna.ObsluhovanyClovek = false;
    }
    #endregion // Public functions
}