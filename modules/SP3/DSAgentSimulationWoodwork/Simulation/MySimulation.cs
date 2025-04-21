using Agents.AgentOkolia;
using Agents.AgentBStolar;
using Agents.AgentStolarov;
using OSPABA;
using Agents.AgentModelu;
using Agents.AgentMontaznychMiest;
using Agents.AgentAStolar;
using Agents.AgentCStolar;
using Agents.AgentStolarskejDielne;
using DSAgentSimulationWoodwork.Entities;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		#region Properties
		public Random? Seeder { get; set; } = null;
		public int PocetMontaznychMiest { get; set; } = 30;
		public int PocetStolarovA { get; set; } = 2;
		public int PocetStolarovB { get; set; } = 2;
		public int PocetStolarovC { get; set; } = 18;
		#endregion // Properties

		public MySimulation(Random seeder, int pocetMiest, int pocetA, int pocetB, int pocetC)
		{
			Seeder = seeder;
			PocetMontaznychMiest = pocetMiest;
			PocetStolarovA = pocetA;
			PocetStolarovB = pocetB;
			PocetStolarovC = pocetC;
			Objednavka.ResetPoradie();
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
		}

		override public void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();
		}

		override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();
		}

		public void Start(int repCount, double endTime)
		{
			Simulate(repCount, endTime);
		}

		public void Pause()
		{
			PauseSimulation();
		}

		public void Resume()
		{
			ResumeSimulation();
		}

		public void Stop()
		{
			StopSimulation();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentStolarskejDielne = new AgentStolarskejDielne(SimId.AgentStolarskejDielne, this, AgentModelu);
			AgentStolarov = new AgentStolarov(SimId.AgentStolarov, this, AgentStolarskejDielne);
			AgentMontaznychMiest = new AgentMontaznychMiest(SimId.AgentMontaznychMiest, this, AgentStolarskejDielne);
			AgentAStolar = new AgentAStolar(SimId.AgentAStolar, this, AgentStolarov);
			AgentBStolar = new AgentBStolar(SimId.AgentBStolar, this, AgentStolarov);
			AgentCStolar = new AgentCStolar(SimId.AgentCStolar, this, AgentStolarov);
		}
		public AgentModelu AgentModelu
		{ get; set; }
		public AgentOkolia AgentOkolia
		{ get; set; }
		public AgentStolarskejDielne AgentStolarskejDielne
		{ get; set; }
		public AgentStolarov AgentStolarov
		{ get; set; }
		public AgentMontaznychMiest AgentMontaznychMiest
		{ get; set; }
		public AgentAStolar AgentAStolar
		{ get; set; }
		public AgentBStolar AgentBStolar
		{ get; set; }
		public AgentCStolar AgentCStolar
		{ get; set; }
		//meta! tag="end"
	}
}