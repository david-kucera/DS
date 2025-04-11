using OSPABA;
using Simulation;
namespace Agents.AgentObsluhy
{
	//meta! id="3"
	public class ManagerObsluhy : OSPABA.Manager
	{
		public ManagerObsluhy(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="ProcesObsluhy", id="24", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="11", type="Request"
		public void ProcessObsluha(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="12", type="Notice"
		public void ProcessKoniecObsluhy(MessageForm message)
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
			case Mc.KoniecObsluhy:
				ProcessKoniecObsluhy(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			case Mc.Obsluha:
				ProcessObsluha(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentObsluhy MyAgent
		{
			get
			{
				return (AgentObsluhy)base.MyAgent;
			}
		}
	}
}
