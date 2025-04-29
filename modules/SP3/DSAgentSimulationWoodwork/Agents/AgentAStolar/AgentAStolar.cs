using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;

namespace Agents.AgentAStolar
{
	//meta! id="4"
	public class AgentAStolar : OSPABA.Agent
	{
		#region Properties
		public List<Stolar> StolariA { get; set; }
		#endregion // Properties

		public AgentAStolar(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			StolariA = new List<Stolar>();
			for (int i = 0; i < ((MySimulation)MySim).PocetStolarovA; i++)
			{
                StolariA.Add(new Stolar(StolarType.A, i));
			}
		}

		public void InitAnimator()
		{
            foreach (var stolar in StolariA)
            {
                if (MySim.AnimatorExists) MySim.Animator.Register(stolar.AnimImageItem);
            }
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerAStolar(SimId.ManagerAStolar, MySim, this);
			AddOwnMessage(Mc.DajStolaraA);
		}
		//meta! tag="end"
	}
}