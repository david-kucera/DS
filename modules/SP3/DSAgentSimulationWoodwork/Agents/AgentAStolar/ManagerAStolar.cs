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

		//meta! userInfo="Removed from model"
		public void ProcessInit(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessMontovanieKovani(MessageForm message)
		{
		}

		//meta! userInfo="Removed from model"
		public void ProcessFinishProcessRezanie(MessageForm message)
		{
		}

		//meta! sender="AgentStolarov", id="57", type="Request"
		public void ProcessDajStolaraA(MessageForm message)
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
			case Mc.DajStolaraA:
				ProcessDajStolaraA(message);
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