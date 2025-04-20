using OSPABA;
using Simulation;

namespace Agents.AgentMontaznychMiest
{
	//meta! id="8"
	public class AgentMontaznychMiest : OSPABA.Agent
	{
		public AgentMontaznychMiest(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerMontaznychMiest(SimId.ManagerMontaznychMiest, MySim, this);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.PriradMiesto);
			AddOwnMessage(Mc.UvolniMiesto);
		}
		//meta! tag="end"
	}
}