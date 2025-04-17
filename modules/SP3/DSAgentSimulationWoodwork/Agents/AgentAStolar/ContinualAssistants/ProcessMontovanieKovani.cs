using OSPABA;
using Simulation;
using Agents.AgentAStolar;
namespace Agents.AgentAStolar.ContinualAssistants
{
	//meta! id="33"
	public class ProcessMontovanieKovani : OSPABA.Process
	{
		public ProcessMontovanieKovani(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentAStolar", id="34", type="Start"
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
		public new AgentAStolar MyAgent
		{
			get
			{
				return (AgentAStolar)base.MyAgent;
			}
		}
	}
}
