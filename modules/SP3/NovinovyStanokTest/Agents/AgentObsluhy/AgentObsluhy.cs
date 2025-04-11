using OSPABA;
using Simulation;
using Agents.AgentObsluhy.ContinualAssistants;
namespace Agents.AgentObsluhy
{
	//meta! id="3"
	public class AgentObsluhy : OSPABA.Agent
	{
		public AgentObsluhy(int id, OSPABA.Simulation mySim, Agent parent) :
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
			new ManagerObsluhy(SimId.ManagerObsluhy, MySim, this);
			new ProcesObsluhy(SimId.ProcesObsluhy, MySim, this);
			AddOwnMessage(Mc.Obsluha);
			AddOwnMessage(Mc.KoniecObsluhy);
		}
		//meta! tag="end"
	}
}
