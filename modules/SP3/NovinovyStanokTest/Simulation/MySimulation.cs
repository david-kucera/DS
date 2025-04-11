using Agents.AgentOkolia;
using Agents.AgentModelu;
using Agents.AgentObsluhy;
using OSPRNG;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		public ExponentialRNG PrichodyGenerator { get; set; }
		public ExponentialRNG ObsluhaGenerator { get; set; }

		public double PriemernyCasCakania { get; set; }
		public double KumulativnyCasCakania { get; set; }
		public int KumulativnyPocetCakajucich { get; set; }

		private Random _seeder;
	
		public MySimulation()
		{
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
			PriemernyCasCakania = 0.0;
			_seeder = new Random(0);

			PrichodyGenerator = new ExponentialRNG(100, _seeder);
			ObsluhaGenerator = new ExponentialRNG(45, _seeder);
			Console.Clear();
			Console.WriteLine("Simulating...");
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...
			KumulativnyCasCakania = 0;
			KumulativnyPocetCakajucich = 0;
		}

		override public void ReplicationFinished()
		{
			PriemernyCasCakania += KumulativnyCasCakania / KumulativnyPocetCakajucich;
		
			Console.WriteLine("R" + CurrentReplication + ": Celkový priemerný čas čakania: " + PriemernyCasCakania / (1 + CurrentReplication));
			base.ReplicationFinished();
		}

		override public void SimulationFinished()
		{
			base.SimulationFinished();
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentStanku = new AgentStanku(SimId.AgentStanku, this, AgentModelu);
		}
		public AgentModelu AgentModelu
		{ get; set; }
		public AgentOkolia AgentOkolia
		{ get; set; }
		public AgentStanku AgentStanku
		{ get; set; }
		//meta! tag="end"
	}
}