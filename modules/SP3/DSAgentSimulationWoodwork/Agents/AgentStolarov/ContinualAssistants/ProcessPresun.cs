using Agents.AgentStolarov;
using DSAgentSimulationWoodwork.Entities;
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
		private TriangularGenerator _pripravaDrevaGenerator;
		private TriangularGenerator _presunMedziMontaznymiMiestamiGenerator;
		#endregion // Class members

		public ProcessPresun(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_montazneMiestoSkladGenerator = new TriangularGenerator(seeder, 60.0, 480.0, 120.0);
			_pripravaDrevaGenerator = new TriangularGenerator(seeder, 300.0, 900.0, 500.0);
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
			message.Code = Mc.Finish;
			var sprava = ((MyMessage)message);
			var stolar = sprava.Stolar;
			var tovar = sprava.Tovar;

			if (stolar.Type == StolarType.A && tovar.Status == TovarStatus.CakajucaNaRezanie)
			{
				double casPrechoduZMontaznehoMiestaDoSkladu = 0.0;
				if (stolar.MontazneMiesto != null)
					casPrechoduZMontaznehoMiestaDoSkladu = _montazneMiestoSkladGenerator.NextDouble();
				double casPripravyDrevaVSklade = _pripravaDrevaGenerator.NextDouble();
				double casPrechoduZoSkladuNaMontazneMiesto = _montazneMiestoSkladGenerator.NextDouble();
				if (stolar.MontazneMiesto != null && stolar.MontazneMiesto.Stolar != null && stolar.MontazneMiesto.Stolar.ID == stolar.ID && stolar.MontazneMiesto.Stolar.Type == stolar.Type)
					stolar.MontazneMiesto.Stolar = null;

				double trvanieUdalosti = casPrechoduZMontaznehoMiestaDoSkladu + casPripravyDrevaVSklade + casPrechoduZoSkladuNaMontazneMiesto;
				Hold(trvanieUdalosti, message);
			}
			else
			{
				double casPrechoduNaMontazneMiesto;
				if (stolar.MontazneMiesto == null)
					casPrechoduNaMontazneMiesto = _montazneMiestoSkladGenerator.NextDouble();
				else if (stolar.MontazneMiesto != tovar.MontazneMiesto)
					casPrechoduNaMontazneMiesto = _presunMedziMontaznymiMiestamiGenerator.NextDouble();
				else casPrechoduNaMontazneMiesto = 0.0;
				if (stolar.MontazneMiesto != null && stolar.MontazneMiesto.Stolar != null && stolar.MontazneMiesto.Stolar.ID == stolar.ID && stolar.MontazneMiesto.Stolar.Type == stolar.Type)
					stolar.MontazneMiesto.Stolar = null;

				double trvanieUdalosti = casPrechoduNaMontazneMiesto;
				Hold(trvanieUdalosti, message);
			}
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