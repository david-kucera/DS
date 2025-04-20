using OSPABA;
using Simulation;

namespace Agents.AgentAStolar
{
	//meta! id="4"
	public class AgentAStolar : OSPABA.Agent
	{
		public AgentAStolar(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerAStolar(SimId.ManagerAStolar, MySim, this);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.DajStolaraA);
		}
		//meta! tag="end"
	}
}