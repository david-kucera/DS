using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class PrichodObjednavky : SimulationEvent
{
    public PrichodObjednavky(SimulationCore core, double time) : base(core, time)
    {
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}