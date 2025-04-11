using OSPABA;
using Agents.AgentObsluhy;
using Simulation;
namespace Agents.AgentObsluhy.ContinualAssistants
{
	//meta! id="23"
	public class ProcesObsluhy : OSPABA.Process
	{
		public ProcesObsluhy(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentObsluhy", id="24", type="Start"
		public void ProcessStart(MessageForm message)
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
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
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
