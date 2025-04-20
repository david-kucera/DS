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

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessMorenie(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessMontovanieKovani(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessLakovanie(MessageForm message)
		{
		}

		//meta! sender="AgentStolarov", id="61", type="Request"
		public void ProcessDajStolaraC(MessageForm message)
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

			case Mc.DajStolaraC:
				ProcessDajStolaraC(message);
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