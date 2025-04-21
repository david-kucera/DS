using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov
{
	//meta! id="7"
	public class ManagerStolarov : OSPABA.Manager
	{
		public ManagerStolarov(int id, OSPABA.Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentStolarskejDielne", id="55", type="Notice"
		public void ProcessZacniPracu(MessageForm message)
		{
			message.Addressee = MySim.FindAgent(SimId.AgentAStolar);
			message.Code = Mc.DajStolaraA;
			Request(message);
		}

		//meta! userInfo="Removed from model"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentAStolar", id="57", type="Response"
		public void ProcessDajStolaraA(MessageForm message)
		{
			var msg = ((MyMessage)message);
			if (msg.Stolar != null)
			{
				if (msg.Stolar.Type != StolarType.A) throw new Exception("Nesprávny typ stolára!");

				msg.Tovar.Status = TovarStatus.CakajucaNaRezanie;
				message.Addressee = MyAgent.FindAssistant(SimId.ProcessPresun);
				StartContinualAssistant(message);
			}
			else
			{
				MyAgent.CakajuceNaRezanie.Enqueue(msg.Tovar);
			}
		}

		//meta! sender="AgentCStolar", id="61", type="Response"
		public void ProcessDajStolaraC(MessageForm message)
		{
		}

		//meta! sender="AgentBStolar", id="60", type="Response"
		public void ProcessDajStolaraB(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="ProcessRezanie", id="102", type="Finish"
		public void ProcessFinishProcessRezanie(MessageForm message)
		{
			var sprava = ((MyMessage)message);
			sprava.Stolar.Obsadeny = false;
			sprava.Tovar.Status = TovarStatus.CakajucaNaMorenie;

			message.Addressee = MySim.FindAgent(SimId.AgentCStolar);
			message.Code = Mc.DajStolaraC;
			Request(message);

			// Naplanovanie dalsieho rezania
			if (MyAgent.CakajuceNaRezanie.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaRezanie.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaRezanie) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentAStolar);
				msg.Code = Mc.DajStolaraA;
				Request(msg);
			}
		}

		//meta! sender="ProcessMontazKovani", id="110", type="Finish"
		public void ProcessFinishProcessMontazKovani(MessageForm message)
		{
		}

		//meta! sender="ProcessMorenie", id="104", type="Finish"
		public void ProcessFinishProcessMorenie(MessageForm message)
		{
			// todo 15 percent tovarov musi byt aj nalakovanych!!!
		}

		//meta! sender="ProcessPresun", id="100", type="Finish"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
			var msg = ((MyMessage)message);
			msg.Stolar.MontazneMiesto = msg.Tovar.MontazneMiesto;
			msg.Stolar.MontazneMiesto.Stolar = msg.Stolar;

			switch (msg.Tovar.Status)
			{
				case TovarStatus.CakajucaNaRezanie:
					msg.Tovar.Status = TovarStatus.PriebehRezania;
					message.Addressee = MyAgent.FindAssistant(SimId.ProcessRezanie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaMorenie:
					msg.Tovar.Status = TovarStatus.PriebehMorenia;
					message.Addressee = MyAgent.FindAssistant(SimId.ProcessMorenie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaLakovanie:
					msg.Tovar.Status = TovarStatus.PriebehLakovania;
					message.Addressee = MyAgent.FindAssistant(SimId.ProcessLakovanie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaSkladanie:
					msg.Tovar.Status = TovarStatus.PriebehSkladania;
					message.Addressee = MyAgent.FindAssistant(SimId.ProcessSkladanie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaMontazKovani:
					msg.Tovar.Status = TovarStatus.PriebehMontazeKovani;
					message.Addressee = MyAgent.FindAssistant(SimId.ProcessMontazKovani);
					StartContinualAssistant(message);
					break;
			}
		}

		//meta! sender="ProcessLakovanie", id="106", type="Finish"
		public void ProcessFinishProcessLakovanie(MessageForm message)
		{
		}

		//meta! sender="ProcessSkladanie", id="108", type="Finish"
		public void ProcessFinishProcessSkladanie(MessageForm message)
		{
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.ZacniPracu:
				ProcessZacniPracu(message);
			break;

			case Mc.DajStolaraA:
				ProcessDajStolaraA(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.ProcessMorenie:
					ProcessFinishProcessMorenie(message);
				break;

				case SimId.ProcessPresun:
					ProcessFinishProcessPresun(message);
				break;

				case SimId.ProcessSkladanie:
					ProcessFinishProcessSkladanie(message);
				break;

				case SimId.ProcessMontazKovani:
					ProcessFinishProcessMontazKovani(message);
				break;

				case SimId.ProcessRezanie:
					ProcessFinishProcessRezanie(message);
				break;

				case SimId.ProcessLakovanie:
					ProcessFinishProcessLakovanie(message);
				break;
				}
			break;

			case Mc.DajStolaraB:
				ProcessDajStolaraB(message);
			break;

			case Mc.DajStolaraC:
				ProcessDajStolaraC(message);
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