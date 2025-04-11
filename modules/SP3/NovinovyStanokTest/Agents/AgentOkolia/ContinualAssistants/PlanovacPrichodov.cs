using Agents.AgentOkolia;
using OSPABA;
using Simulation;
namespace Agents.AgentOkolia.ContinualAssistants
{
	//meta! id="11"
	public class PlanovacPrichodov : Scheduler
	{
		public PlanovacPrichodov(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentOkolia", id="12", type="Start"
		public void ProcessStart(MessageForm message)
		{
			var mess = (MyMessage)message.CreateCopy();
			mess.Code = Mc.NoticeNovyZakaznik;
			double cas = ((MySimulation)MySim).PrichodyGenerator.Sample();
			Hold(cas, mess);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.NoticeNovyZakaznik:
					var mess = (MyMessage)message.CreateCopy();
					mess.Addressee = MyAgent;
					Notice(mess);
					break;
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
		public new AgentOkolia MyAgent
		{
			get
			{
				return (AgentOkolia)base.MyAgent;
			}
		}
	}
}