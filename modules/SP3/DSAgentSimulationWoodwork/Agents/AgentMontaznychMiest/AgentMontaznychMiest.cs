using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;

namespace Agents.AgentMontaznychMiest
{
	//meta! id="8"
	public class AgentMontaznychMiest : OSPABA.Agent
	{
		#region Properties
		public List<MontazneMiesto> MontazneMiesta { get; set; }
		public Queue<Tovar> NepriradeneTovary { get; set; }
		#endregion // Properties

		public AgentMontaznychMiest(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			MontazneMiesta = new List<MontazneMiesto>(((MySimulation)MySim).PocetMontaznychMiest);
			for (int i = 1; i <= ((MySimulation)MySim).PocetMontaznychMiest; i++)
			{
				MontazneMiesta.Add(new MontazneMiesto(i));
			}
			NepriradeneTovary = new Queue<Tovar>();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerMontaznychMiest(SimId.ManagerMontaznychMiest, MySim, this);
			AddOwnMessage(Mc.PriradMiesto);
			AddOwnMessage(Mc.UvolniMiesto);
		}
		//meta! tag="end"
	}
}