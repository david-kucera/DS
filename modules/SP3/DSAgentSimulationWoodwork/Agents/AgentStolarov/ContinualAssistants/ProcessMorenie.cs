using Agents.AgentStolarov;
using DSAgentSimulationWoodwork.Entities;
using DSLib.Generators.Uniform;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov.ContinualAssistants
{
	//meta! id="103"
	public class ProcessMorenie : OSPABA.Process
	{
		#region Class members
		private ContinousUniform _stolMorenieGenerator;
		private ContinousUniform _stolickaMorenieGenerator;
		private ContinousUniform _skrinaMorenieGenerator;
		#endregion // Class members

		public ProcessMorenie(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_stolMorenieGenerator = new ContinousUniform(seeder, 100.0 * 60, 480.0 * 60);
			_stolickaMorenieGenerator = new ContinousUniform(seeder, 90.0 * 60, 400.0 * 60);
			_skrinaMorenieGenerator = new ContinousUniform(seeder, 300.0 * 60, 600.0 * 60);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentStolarov", id="104", type="Start"
		public void ProcessStart(MessageForm message)
		{
			message.Code = Mc.Finish;
			var sprava = ((MyMessage)message);
			var tovar = sprava.Tovar;

			if (tovar.Status != TovarStatus.PriebehMorenia) throw new Exception("Neoèakávaná chyba: Tovar nie je v správnom procese!");

			double casMorenia = 0.0;
			switch (tovar.Type)
			{
				case TovarType.Stol:
					casMorenia = _stolMorenieGenerator.NextDouble();
					break;
				case TovarType.Skrina:
					casMorenia = _skrinaMorenieGenerator.NextDouble();
					break;
				case TovarType.Stolicka:
					casMorenia = _stolickaMorenieGenerator.NextDouble();
					break;
				default:
					throw new Exception("Nie je uvedený typ objednávky!");
			}

			Hold(casMorenia, message);
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