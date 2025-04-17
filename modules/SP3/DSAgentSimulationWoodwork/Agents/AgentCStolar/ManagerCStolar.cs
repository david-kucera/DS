using OSPABA;
using Simulation;
namespace Agents.AgentCStolar
{
	//meta! id="6"
	public class ManagerCStolar : OSPABA.Manager
	{
		public ManagerCStolar(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarov", id="85", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="ProcessPresun", id="42", type="Finish"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
		}

		//meta! sender="ProcessMorenie", id="44", type="Finish"
		public void ProcessFinishProcessMorenie(MessageForm message)
		{
		}

		//meta! sender="ProcessMontovanieKovani", id="46", type="Finish"
		public void ProcessFinishProcessMontovanieKovani(MessageForm message)
		{
		}

		//meta! sender="ProcessLakovanie", id="48", type="Finish"
		public void ProcessFinishProcessLakovanie(MessageForm message)
		{
		}

		//meta! sender="AgentStolarov", id="61", type="Request"
		public void ProcessVykonajC(MessageForm message)
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
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.ProcessPresun:
					ProcessFinishProcessPresun(message);
				break;

				case SimId.ProcessMorenie:
					ProcessFinishProcessMorenie(message);
				break;

				case SimId.ProcessMontovanieKovani:
					ProcessFinishProcessMontovanieKovani(message);
				break;

				case SimId.ProcessLakovanie:
					ProcessFinishProcessLakovanie(message);
				break;
				}
			break;

			case Mc.VykonajC:
				ProcessVykonajC(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentCStolar MyAgent
		{
			get
			{
				return (AgentCStolar)base.MyAgent;
			}
		}
	}
}
