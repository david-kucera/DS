using Agents.AgentStolarov;
using DSSimulationLib.Generators.Triangular;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov.ContinualAssistants
{
	//meta! id="99"
	public class ProcessPresun : OSPABA.Process
	{
		#region Class members
		private TriangularGenerator _montazneMiestoSkladGenerator;
		private TriangularGenerator _presunMedziMontaznymiMiestamiGenerator;
		#endregion // Class members

		public ProcessPresun(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_montazneMiestoSkladGenerator = new TriangularGenerator(seeder, 60.0, 480.0, 120.0);
			_presunMedziMontaznymiMiestamiGenerator = new TriangularGenerator(seeder, 120.0, 500.0, 150.0);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentStolarov", id="100", type="Start"
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