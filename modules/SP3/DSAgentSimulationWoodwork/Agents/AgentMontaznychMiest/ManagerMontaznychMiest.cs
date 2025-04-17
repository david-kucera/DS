using OSPABA;
using Simulation;
namespace Agents.AgentMontaznychMiest
{
	//meta! id="8"
	public class ManagerMontaznychMiest : OSPABA.Manager
	{
		public ManagerMontaznychMiest(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarskejDielne", id="80", type="Notice"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! sender="AgentStolarskejDielne", id="58", type="Notice"
		public void ProcessPriradMiesto(MessageForm message)
		{
		}

		//meta! sender="AgentStolarskejDielne", id="68", type="Notice"
		public void ProcessUvolniMiesto(MessageForm message)
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
			case Mc.UvolniMiesto:
				ProcessUvolniMiesto(message);
			break;

			case Mc.Init:
				ProcessInit(message);
			break;

			case Mc.PriradMiesto:
				ProcessPriradMiesto(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentMontaznychMiest MyAgent
		{
			get
			{
				return (AgentMontaznychMiest)base.MyAgent;
			}
		}
	}
}
