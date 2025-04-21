using OSPABA;
using Simulation;
using Agents.AgentStolarov.ContinualAssistants;
using DSAgentSimulationWoodwork.Entities;
using OSPDataStruct;

namespace Agents.AgentStolarov
{
	//meta! id="7"
	public class AgentStolarov : OSPABA.Agent
	{
		#region Properties
		public SimQueue<Tovar> CakajuceNaRezanie { get; set; }
		public SimQueue<Tovar> CakajuceNaMorenie { get; set; }
		public SimQueue<Tovar> CakajuceNaLakovanie { get; set; }
		public SimQueue<Tovar> CakajuceNaSkladanie { get; set; }
		public SimQueue<Tovar> CakajuceNaMontazKovani { get; set; }
		#endregion // Properties

		public AgentStolarov(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			CakajuceNaRezanie = new SimQueue<Tovar>();
			CakajuceNaMorenie = new SimQueue<Tovar>();
			CakajuceNaLakovanie = new SimQueue<Tovar>();
			CakajuceNaSkladanie = new SimQueue<Tovar>();
			CakajuceNaMontazKovani = new SimQueue<Tovar>();
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