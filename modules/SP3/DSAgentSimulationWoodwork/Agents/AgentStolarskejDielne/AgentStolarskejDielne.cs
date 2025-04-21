using OSPABA;
using Simulation;

namespace Agents.AgentStolarskejDielne
{
	//meta! id="18"
	public class AgentStolarskejDielne : OSPABA.Agent
	{
		public AgentStolarskejDielne(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerStolarskejDielne(SimId.ManagerStolarskejDielne, MySim, this);
			AddOwnMessage(Mc.PracaHotova);
			AddOwnMessage(Mc.ZacniPracu);
			AddOwnMessage(Mc.NovaObjednavka);
		}
		//meta! tag="end"
	}
}