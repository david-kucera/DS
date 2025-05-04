using OSPABA;
using Simulation;
namespace Agents.AgentOkolia
{
	//meta! id="2"
	public class ManagerOkolia : OSPABA.Manager
	{
		public ManagerOkolia(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentModelu", id="78", type="Notice"
		public void ProcessInit(MessageForm message)
		{
			message.Addressee = MyAgent.FindAssistant(SimId.SchedulerPrichodovObjednavok);
			StartContinualAssistant(message.CreateCopy());
		}

		//meta! sender="SchedulerPrichodovObjednavok", id="16", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
			message.Code = Mc.NovaObjednavka;
			message.Addressee = MySim.FindAgent(SimId.AgentModelu);
			Notice(message.CreateCopy());

			message.Code = Mc.Start;
			message.Addressee = MyAgent.FindAssistant(SimId.SchedulerPrichodovObjednavok);
			StartContinualAssistant(message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
                default:
                    break;
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

			case Mc.Finish:
				ProcessFinish(message);
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