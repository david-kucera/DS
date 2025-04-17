using OSPABA;
using Simulation;
using Agents.AgentCStolar.ContinualAssistants;
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
			new ProcessMontovanieKovani(SimId.ProcessMontovanieKovani, MySim, this);
			new ProcessPresun(SimId.ProcessPresun, MySim, this);
			new ProcessLakovanie(SimId.ProcessLakovanie, MySim, this);
			new ProcessMorenie(SimId.ProcessMorenie, MySim, this);
			AddOwnMessage(Mc.Init);
			AddOwnMessage(Mc.VykonajC);
		}
		//meta! tag="end"
	}
}
