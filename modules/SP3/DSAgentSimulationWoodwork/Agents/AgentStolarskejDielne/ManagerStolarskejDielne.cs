using OSPABA;
using Simulation;
namespace Agents.AgentStolarskejDielne
{
	//meta! id="18"
	public class ManagerStolarskejDielne : OSPABA.Manager
	{
		public ManagerStolarskejDielne(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarov", id="66", type="Notice"
		public void ProcessPracaHotova(MessageForm message)
		{
		}

		//meta! sender="AgentMontaznychMiest", id="64", type="Notice"
		public void ProcessZacniPracu(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="53", type="Notice"
		public void ProcessNovaObjednavka(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
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
			case Mc.PracaHotova:
				ProcessPracaHotova(message);
			break;

			case Mc.NovaObjednavka:
				ProcessNovaObjednavka(message);
			break;

			case Mc.ZacniPracu:
				ProcessZacniPracu(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentStolarskejDielne MyAgent
		{
			get
			{
				return (AgentStolarskejDielne)base.MyAgent;
			}
		}
	}
}