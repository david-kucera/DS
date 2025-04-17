using OSPABA;
using Simulation;
namespace Agents.AgentAStolar
{
	//meta! id="4"
	public class ManagerAStolar : OSPABA.Manager
	{
		public ManagerAStolar(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarov", id="83", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="ProcessPresun", id="30", type="Finish"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
		}

		//meta! sender="ProcessMontovanieKovani", id="34", type="Finish"
		public void ProcessFinishProcessMontovanieKovani(MessageForm message)
		{
		}

		//meta! sender="ProcessRezanie", id="32", type="Finish"
		public void ProcessFinishProcessRezanie(MessageForm message)
		{
		}

		//meta! sender="AgentStolarov", id="57", type="Request"
		public void ProcessVykonajA(MessageForm message)
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

				case SimId.ProcessMontovanieKovani:
					ProcessFinishProcessMontovanieKovani(message);
				break;

				case SimId.ProcessRezanie:
					ProcessFinishProcessRezanie(message);
				break;
				}
			break;

			case Mc.VykonajA:
				ProcessVykonajA(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentAStolar MyAgent
		{
			get
			{
				return (AgentAStolar)base.MyAgent;
			}
		}
	}
}
