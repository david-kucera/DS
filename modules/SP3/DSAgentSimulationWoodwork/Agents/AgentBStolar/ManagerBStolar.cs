using OSPABA;
using Simulation;
namespace Agents.AgentBStolar
{
	//meta! id="5"
	public class ManagerBStolar : OSPABA.Manager
	{
		public ManagerBStolar(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarov", id="84", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="ProcessSkladanie", id="37", type="Finish"
		public void ProcessFinishProcessSkladanie(MessageForm message)
		{
		}

		//meta! sender="ProcessPresun", id="39", type="Finish"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
		}

		//meta! sender="AgentStolarov", id="60", type="Request"
		public void ProcessVykonajB(MessageForm message)
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
			case Mc.VykonajB:
				ProcessVykonajB(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.ProcessSkladanie:
					ProcessFinishProcessSkladanie(message);
				break;

				case SimId.ProcessPresun:
					ProcessFinishProcessPresun(message);
				break;
				}
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentBStolar MyAgent
		{
			get
			{
				return (AgentBStolar)base.MyAgent;
			}
		}
	}
}
