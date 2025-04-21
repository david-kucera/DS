using OSPABA;
using Simulation;

namespace Agents.AgentCStolar
{
	//meta! id="6"
	public class AgentCStolar : OSPABA.Agent
	{
		public AgentCStolar(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerCStolar(SimId.ManagerCStolar, MySim, this);
			AddOwnMessage(Mc.DajStolaraC);
		}
		//meta! tag="end"
	}
}