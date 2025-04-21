using Agents.AgentStolarov;
using DSLib.Generators.Empirical;
using DSLib.Generators.Uniform;
using OSPABA;
using OSPRNG;
using Simulation;
namespace Agents.AgentStolarov.ContinualAssistants
{
	//meta! id="105"
	public class ProcessLakovanie : OSPABA.Process
	{
		#region Class members
		private ContinousEmpirical _stolLakovanieGenerator;
		private ContinousUniform _stolickaLakovanieGenerator;
		private ContinousUniform _skrinaLakovanieGenerator;
		#endregion // Class members

		public ProcessLakovanie(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			List<(double, double)> intervals =
			[
				(50.0 * 60, 70.0 * 60),
				(70.0 * 60, 150.0 * 60),
				(150.0 * 60, 200.0 * 60)
			];
			List<double> percentages =
			[
				0.1,
				0.6,
				0.3
			];
			_stolLakovanieGenerator = new ContinousEmpirical(seeder, intervals, percentages);
			_stolickaLakovanieGenerator = new ContinousUniform(seeder, 40.0 * 60, 200.0 * 60);
			_skrinaLakovanieGenerator = new ContinousUniform(seeder, 250.0 * 60, 560.0 * 60);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentStolarov", id="106", type="Start"
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