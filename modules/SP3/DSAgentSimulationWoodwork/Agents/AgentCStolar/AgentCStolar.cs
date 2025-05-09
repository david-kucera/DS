using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;

namespace Agents.AgentCStolar
{
	//meta! id="6"
	public class AgentCStolar : OSPABA.Agent
	{
		#region Properties
		public List<Stolar> StolariC { get; set; } = null!;
		#endregion // Properties

		public AgentCStolar(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			StolariC = new List<Stolar>();
			for (int i = 0; i < ((MySimulation)MySim).PocetStolarovC; i++)
			{
                StolariC.Add(new Stolar(StolarType.C, i));
                
			}
		}

		public void InitAnimator()
		{
			if (StolariC == null) return;

            foreach (var stolar in StolariC)
            {
                if (MySim.AnimatorExists) MySim.Animator.Register(stolar.AnimImageItem);
            }
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerCStolar(SimId.ManagerCStolar, MySim, this);
			AddOwnMessage(Mc.DajStolaraC);
		}
		//meta! tag="end"
	}
}