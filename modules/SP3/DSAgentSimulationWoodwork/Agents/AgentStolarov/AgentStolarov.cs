using OSPABA;
using Simulation;
using Agents.AgentStolarov.ContinualAssistants;

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
			new ProcessMorenie(SimId.ProcessMorenie, MySim, this);
			new ProcessLakovanie(SimId.ProcessLakovanie, MySim, this);
			new ProcessPresun(SimId.ProcessPresun, MySim, this);
			new ProcessMontazKovani(SimId.ProcessMontazKovani, MySim, this);
			new ProcessRezanie(SimId.ProcessRezanie, MySim, this);
			new ProcessSkladanie(SimId.ProcessSkladanie, MySim, this);
			AddOwnMessage(Mc.ZacniPracu);
			AddOwnMessage(Mc.DajStolaraC);
			AddOwnMessage(Mc.DajStolaraB);
			AddOwnMessage(Mc.DajStolaraA);
		}
		//meta! tag="end"
	}
}