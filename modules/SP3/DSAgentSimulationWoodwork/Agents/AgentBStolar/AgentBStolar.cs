using OSPABA;
using Simulation;

namespace Agents.AgentBStolar
{
	//meta! id="5"
	public class AgentBStolar : OSPABA.Agent
	{
		public AgentBStolar(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerBStolar(SimId.ManagerBStolar, MySim, this);
			AddOwnMessage(Mc.DajStolaraB);
		}
		//meta! tag="end"
	}
}