using OSPABA;
using Simulation;

namespace Agents.AgentObjednavok
{
	//meta! id="19"
	public class AgentObjednavok : OSPABA.Agent
	{
		public AgentObjednavok(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerObjednavok(SimId.ManagerObjednavok, MySim, this);
			AddOwnMessage(Mc.PracaHotova);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.NovaObjednavka);
			AddOwnMessage(Mc.DalsiaPolozka);
		}
		//meta! tag="end"
	}
}