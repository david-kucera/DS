using OSPABA;
using Simulation;
namespace Agents.AgentStolarov
{
	//meta! id="7"
	public class AgentStolarov : OSPABA.Agent
	{
		public AgentStolarov(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerStolarov(SimId.ManagerStolarov, MySim, this);
			AddOwnMessage(Mc.ZacniPracu);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.VykonajA);
			AddOwnMessage(Mc.VykonajC);
			AddOwnMessage(Mc.VykonajB);
		}
		//meta! tag="end"
	}
}
