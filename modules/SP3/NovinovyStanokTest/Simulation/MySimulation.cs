using Agents.AgentOkolia;
using OSPABA;
using Agents.AgentModelu;
using Agents.AgentObsluhy;
using OSPStat;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		public Stat AverageCasCakania { get; set; }
		public Stat AverageDlzkaRadu { get; set; }
		
		public MySimulation()
		{
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
			
			AverageCasCakania = new Stat();
			AverageDlzkaRadu = new Stat();
			Console.WriteLine("-------------------------------------");
			Console.WriteLine("Simulating...");
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
			
			Console.WriteLine("-------------------------------------");
			AverageCasCakania.AddSample(AgentObsluhy.CasCakaniaStat.Mean());
			AverageDlzkaRadu.AddSample(AgentObsluhy.DlzkaRaduStat.Mean());
			
			Console.WriteLine("Replikácia číslo {0}. Celkový priemerný čas čakania: {1:0.00000} ({2:0.00000})",
				CurrentReplication,
				AverageCasCakania.Mean(),
				AgentObsluhy.CasCakaniaStat.Mean());
			Console.WriteLine("{0}Celková priemerná dĺžka rady(ABAFront): {1:0.00000} ({2:0.00000})",
				new string(' ', 20),
				AverageDlzkaRadu.Mean(),
				AgentObsluhy.Rad.LengthStatistic.Mean());
		}

		override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();

			Console.WriteLine("-------------------------------------");
			Console.WriteLine("Simulation finished");
			Console.WriteLine("Výsledky simulácie:");
			Console.WriteLine("----------------------------------------------------------------------");
			Console.WriteLine("Celkový priemerný čas čakania: {0:0.00000}", AverageCasCakania.Mean());
			Console.WriteLine("Celková priemerná dĺžka radu: {0:0.00000}", AverageDlzkaRadu.Mean());
			Console.WriteLine("----------------------------------------------------------------------");
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentObsluhy = new AgentObsluhy(SimId.AgentObsluhy, this, AgentModelu);
		}
		public AgentModelu AgentModelu
		{ get; set; }
		public AgentOkolia AgentOkolia
		{ get; set; }
		public AgentObsluhy AgentObsluhy
		{ get; set; }
		//meta! tag="end"
	}
}
