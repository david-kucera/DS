using OSPABA;
using Simulation;
using Agents.AgentObsluhy.ContinualAssistants;

namespace Agents.AgentObsluhy
{
	//meta! id="4"
	public class AgentStanku : Agent
	{
		public AgentStanku(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerStanku(SimId.ManagerStanku, MySim, this);
			new ProcesObsluhy(SimId.ProcesObsluhy, MySim, this);
			AddOwnMessage(Mc.NoticeKoniecObsluhy);
			AddOwnMessage(Mc.Obsluha);
		}
		//meta! tag="end"
	}
}