using Agents.AgentStolarov;
using DSAgentSimulationWoodwork.Entities;
using DSLib.Generators.Uniform;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov.ContinualAssistants
{
	//meta! id="107"
	public class ProcessSkladanie : OSPABA.Process
	{
		#region Class members
		private ContinousUniform _stolSkladanieGenerator;
		private ContinousUniform _stolickaSkladanieGenerator;
		private ContinousUniform _skrinaSkladanieGenerator;
		#endregion // Class members

		public ProcessSkladanie(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_stolSkladanieGenerator = new ContinousUniform(seeder, 30.0 * 60, 60.0 * 60);
			_stolickaSkladanieGenerator = new ContinousUniform(seeder, 14.0 * 60, 24.0 * 60);
			_skrinaSkladanieGenerator = new ContinousUniform(seeder, 35.0 * 60, 75.0 * 60);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentStolarov", id="108", type="Start"
		public void ProcessStart(MessageForm message)
		{
			message.Code = Mc.Finish;
			var sprava = ((MyMessage)message);
			var tovar = sprava.Tovar;

			if (tovar.Status != TovarStatus.PriebehSkladania) throw new Exception("Neoèakávaná chyba: Tovar nie je v správnom procese!");

			double casSkladania = 0.0;
			switch (tovar.Type)
			{
				case TovarType.Stol:
					casSkladania = _stolSkladanieGenerator.NextDouble();
					break;
				case TovarType.Skrina:
					casSkladania = _skrinaSkladanieGenerator.NextDouble();
					break;
				case TovarType.Stolicka:
					casSkladania = _stolickaSkladanieGenerator.NextDouble();
					break;
				default:
					throw new Exception("Nie je uvedený typ objednávky!");
			}

			Hold(casSkladania, message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					AssistantFinished(message);
					break;
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