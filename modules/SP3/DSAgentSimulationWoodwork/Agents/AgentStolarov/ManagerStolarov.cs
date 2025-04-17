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

		//meta! sender="AgentStolarskejDielne", id="82", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentAStolar", id="57", type="Response"
		public void ProcessVykonajA(MessageForm message)
		{
		}

		//meta! sender="AgentCStolar", id="61", type="Response"
		public void ProcessVykonajC(MessageForm message)
		{
		}

		//meta! sender="AgentBStolar", id="60", type="Response"
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

			case Mc.ZacniPracu:
				ProcessZacniPracu(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.VykonajA:
				ProcessVykonajA(message);
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
		public new AgentStolarov MyAgent
		{
			get
			{
				return (AgentStolarov)base.MyAgent;
			}
		}
	}
}
