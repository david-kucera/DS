using Agents.AgentStolarov;
using DSLib.Generators.Empirical;
using DSLib.Generators.Uniform;
using DSSimulationLib.Generators.Triangular;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov.ContinualAssistants
{
	//meta! id="101"
	public class ProcessRezanie : OSPABA.Process
	{
		#region Class members
		private TriangularGenerator _pripravaDrevaVSkladeGenerator;
		private ContinousEmpirical _stolRezanieGenerator;
		private ContinousUniform _stolickaRezanieGenerator;
		private ContinousUniform _skrinaRezanieGenerator;
		#endregion // Class members

		public ProcessRezanie(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_pripravaDrevaVSkladeGenerator = new TriangularGenerator(seeder, 300.0, 900.0, 500.0);
			List<(double, double)> intervals =
			[
				(10.0 * 60, 25.0 * 60),
				(25.0 * 60, 50.0 * 60),
			];
			List<double> percentages =
			[
				0.6,
				0.4
			];
			_stolRezanieGenerator = new ContinousEmpirical(seeder, intervals, percentages);
			_stolickaRezanieGenerator = new ContinousUniform(seeder, 12.0 * 60, 16.0 * 60);
			_skrinaRezanieGenerator = new ContinousUniform(seeder, 15.0 * 60, 80.0 * 60);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentStolarov", id="102", type="Start"
		public void ProcessStart(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentStolarov MyAgent
		{
			get
			{
				return (AgentStolarov)base.MyAgent;
			}
		}
	}
}