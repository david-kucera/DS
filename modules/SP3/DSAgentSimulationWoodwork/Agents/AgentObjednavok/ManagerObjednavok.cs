using OSPABA;
using Simulation;
namespace Agents.AgentObjednavok
{
	//meta! id="19"
	public class ManagerObjednavok : OSPABA.Manager
	{
		public ManagerObjednavok(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarskejDielne", id="69", type="Notice"
		public void ProcessPracaHotova(MessageForm message)
		{
		}

		//meta! sender="AgentStolarskejDielne", id="81", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentStolarskejDielne", id="54", type="Notice"
		public void ProcessNovaObjednavka(MessageForm message)
		{
		}

		//meta! sender="AgentStolarskejDielne", id="92", type="Request"
		public void ProcessDalsiaPolozka(MessageForm message)
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
			case Mc.DalsiaPolozka:
				ProcessDalsiaPolozka(message);
			break;

			case Mc.PracaHotova:
				ProcessPracaHotova(message);
			break;

			case Mc.NovaObjednavka:
				ProcessNovaObjednavka(message);
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
		public new AgentObjednavok MyAgent
		{
			get
			{
				return (AgentObjednavok)base.MyAgent;
			}
		}
	}
}