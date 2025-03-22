using DSSimulationLib.Simulation;

namespace DSSimulationTest.Events;

public class ZaciatokObsluhyEvent : SimulationEvent
{
    #region Class members
    private Osoba _osoba = null!;
    #endregion // Class members
    
    #region Constructor
    public ZaciatokObsluhyEvent(SimulationCore core, double time, Osoba osoba) : base(core, time)
    {
        _osoba = osoba;
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        Predajna predajna = Core as Predajna ?? throw new InvalidOperationException();
        predajna.ObsluhovanyClovek = true;
        
        
    }
    #endregion // Public functions
}