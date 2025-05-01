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

			double casPrechoduZMontaznehoMiestaDoSkladu = 0.0;
			double casPripravyDrevaVSklade = 0.0;
			double casPrechoduZoSkladuNaMontazneMiesto = 0.0;
			double casPrechoduNaMontazneMiesto = 0.0;
			
			if (stolar.Type == StolarType.A && tovar.Status == TovarStatus.CakajucaNaRezanie)
			{
				if (stolar.MontazneMiesto != null)	
				{
					casPrechoduZMontaznehoMiestaDoSkladu = _montazneMiestoSkladGenerator.NextDouble();
					stolar.AnimImageItem.MoveTo(MySim.CurrentTime, casPrechoduZMontaznehoMiestaDoSkladu, Sklad.GetRandomSkladPosX(), Sklad.GetRandomSkladPosY());
				}
				casPripravyDrevaVSklade = _pripravaDrevaGenerator.NextDouble();
				casPrechoduZoSkladuNaMontazneMiesto = _montazneMiestoSkladGenerator.NextDouble();
				stolar.AnimImageItem.MoveTo(MySim.CurrentTime + casPrechoduZMontaznehoMiestaDoSkladu + casPripravyDrevaVSklade + casPrechoduZoSkladuNaMontazneMiesto, casPrechoduZoSkladuNaMontazneMiesto, tovar.MontazneMiesto.PosX + 50, tovar.MontazneMiesto.PosY);
            }
			else
			{
				if (stolar.MontazneMiesto == null)
				{ 
					casPrechoduNaMontazneMiesto = _montazneMiestoSkladGenerator.NextDouble();
                    stolar.AnimImageItem.MoveTo(MySim.CurrentTime, casPrechoduNaMontazneMiesto, tovar.MontazneMiesto.PosX + 50, tovar.MontazneMiesto.PosY);
                }
				else if (stolar.MontazneMiesto != tovar.MontazneMiesto)
				{ 
					casPrechoduNaMontazneMiesto = _presunMedziMontaznymiMiestamiGenerator.NextDouble();
                    stolar.AnimImageItem.MoveTo(MySim.CurrentTime, casPrechoduNaMontazneMiesto, tovar.MontazneMiesto.PosX + 50, tovar.MontazneMiesto.PosY);
                }
				else
				{
                    stolar.AnimImageItem.MoveTo(MySim.CurrentTime, 0, tovar.MontazneMiesto.PosX + 50, tovar.MontazneMiesto.PosY);
                }
			}
			
			if (stolar.MontazneMiesto != null && stolar.MontazneMiesto.Stolar != null && stolar.MontazneMiesto.Stolar.ID == stolar.ID && stolar.MontazneMiesto.Stolar.Type == stolar.Type)
				stolar.MontazneMiesto.Stolar = null;
			
			double trvanieUdalosti = casPrechoduZMontaznehoMiestaDoSkladu + casPripravyDrevaVSklade + casPrechoduZoSkladuNaMontazneMiesto +casPrechoduNaMontazneMiesto;
			Hold(trvanieUdalosti, message);
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