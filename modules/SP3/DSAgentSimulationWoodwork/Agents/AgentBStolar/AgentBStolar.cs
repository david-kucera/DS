using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;

namespace Agents.AgentBStolar
{
	//meta! id="5"
	public class AgentBStolar : OSPABA.Agent
	{
		#region Properties
		public List<Stolar> StolariB { get; set; } = null!;
		#endregion // Properties

		public AgentBStolar(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			StolariB = new List<Stolar>();
			for (int i = 0; i < ((MySimulation)MySim).PocetStolarovB; i++)
			{
                StolariB.Add(new Stolar(StolarType.B, i));
            }
		}

		public void InitAnimator()
		{
			if (StolariB == null) return;

            foreach (var stolar in StolariB)
            {
                if (MySim.AnimatorExists) MySim.Animator.Register(stolar.AnimImageItem);
            }
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerBStolar(SimId.ManagerBStolar, MySim, this);
			AddOwnMessage(Mc.DajStolaraB);
		}
		//meta! tag="end"
	}
}