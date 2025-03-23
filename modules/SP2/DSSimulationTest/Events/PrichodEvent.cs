using DSSimulationLib.Simulation;

namespace DSSimulationTest.Events;

public class PrichodEvent : SimulationEvent
{
    #region Constructor
    public PrichodEvent(SimulationCore core, double time) : base(core, time)
    {
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        Predajna predajna = Core as Predajna ?? throw new InvalidOperationException();

        Osoba osoba = new()
        {
            Id = predajna.PoradieOsoby,
            ArrivalTime = Time,
        };
        predajna.PoradieOsoby++;
        osoba.StartWaitingTimeQueue = Time;
        
        if (predajna.Rad.Count >= 1 || predajna.ObsluhovanyClovek) predajna.Rad.Enqueue(osoba);
        else predajna.EventQueue.Enqueue(new ZaciatokObsluhyEvent(predajna, predajna.Time, osoba), predajna.Time);

        var dalsiPrichod = predajna.PrichodLudiGenerator.NextDouble() + predajna.Time;
        if (dalsiPrichod < predajna.STOP_TIME) predajna.EventQueue.Enqueue(new PrichodEvent(predajna, dalsiPrichod), dalsiPrichod);
        predajna.AverageDlzkaRadu.AddValue(predajna.Rad.Count, predajna.Time);
    }
    #endregion // Public functions
}