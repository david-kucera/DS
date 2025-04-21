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
		}

		//meta! userInfo="Removed from model"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentAStolar", id="57", type="Response"
		public void ProcessDajStolaraA(MessageForm message)
		{
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
		}

		//meta! sender="ProcessMontazKovani", id="110", type="Finish"
		public void ProcessFinishProcessMontazKovani(MessageForm message)
		{
		}

		//meta! sender="ProcessMorenie", id="104", type="Finish"
		public void ProcessFinishProcessMorenie(MessageForm message)
		{
		}

		//meta! sender="ProcessPresun", id="100", type="Finish"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
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