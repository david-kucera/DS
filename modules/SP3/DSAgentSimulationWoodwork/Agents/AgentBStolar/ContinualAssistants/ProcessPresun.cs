using Agents.AgentBStolar;
using OSPABA;
using Simulation;
namespace Agents.AgentBStolar.ContinualAssistants
{
	//meta! id="38"
	public class ProcessPresun : OSPABA.Process
	{
		public ProcessPresun(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentBStolar", id="39", type="Start"
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
		public new AgentBStolar MyAgent
		{
			get
			{
				return (AgentBStolar)base.MyAgent;
			}
		}
	}
}
