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

		//meta! userInfo="Removed from model"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessSkladanie(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
		}

		//meta! sender="AgentStolarov", id="60", type="Request"
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

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.DajStolaraB:
				ProcessDajStolaraB(message);
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