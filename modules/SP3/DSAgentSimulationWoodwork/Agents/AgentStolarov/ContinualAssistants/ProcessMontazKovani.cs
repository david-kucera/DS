using Agents.AgentStolarov;
using DSAgentSimulationWoodwork.Entities;
using DSLib.Generators.Uniform;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov.ContinualAssistants
{
	//meta! id="109"
	public class ProcessMontazKovani : OSPABA.Process
	{
		#region Class members
		private ContinousUniform _skrinaMontazKovaniGenerator;
		#endregion // Class members

		public ProcessMontazKovani(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_skrinaMontazKovaniGenerator = new ContinousUniform(seeder, 15.0 * 60, 25.0 * 60);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentStolarov", id="110", type="Start"
		public void ProcessStart(MessageForm message)
		{
			message.Code = Mc.Finish;
			var sprava = ((MyMessage)message);
			var tovar = sprava.Tovar;

			if (tovar.Status != TovarStatus.PriebehMontazeKovani) throw new Exception("Neo�ak�van� chyba: Tovar nie je v spr�vnom procese!");

			double casSkladania = _skrinaMontazKovaniGenerator.NextDouble();

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