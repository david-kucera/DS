using OSPABA;
using Simulation;
using Agents.AgentStolarov.ContinualAssistants;
using DSAgentSimulationWoodwork.Entities;

namespace Agents.AgentStolarov
{
	//meta! id="7"
	public class AgentStolarov : OSPABA.Agent
	{
		#region Properties
		public PriorityQueue<Tovar, (double, double)> CakajuceNaRezanie { get; set; } = null!;
		public PriorityQueue<Tovar, (double, double)> CakajuceNaMorenie { get; set; } = null!;
		public PriorityQueue<Tovar, (double, double)> CakajuceNaLakovanie { get; set; } = null!;
		public PriorityQueue<Tovar, (double, double)> CakajuceNaSkladanie { get; set; } = null!;
		public PriorityQueue<Tovar, (double, double)> CakajuceNaMontazKovani { get; set; } = null!;
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

			CakajuceNaRezanie = new();
			CakajuceNaMorenie = new();
			CakajuceNaLakovanie = new();
			CakajuceNaSkladanie = new();
			CakajuceNaMontazKovani = new();
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