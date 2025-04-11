using OSPABA;
using Simulation;

namespace Agents.AgentModelu
{
	//meta! id="1"
	public class ManagerModelu : Manager
	{
		public ManagerModelu(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentOkolia", id="8", type="Notice"
		public void ProcessPrichodZakaznika(MessageForm message)
		{
			var mess = (MyMessage)message.CreateCopy();
			mess.Addressee = MySim.FindAgent(SimId.AgentStanku);
			mess.Code = Mc.Obsluha;
			Request(mess);
		}

		//meta! sender="AgentStanku", id="9", type="Response"
		public void ProcessObsluha(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="AgentStanku", id="35", type="Notice"
		public void ProcessNoticeKoniecObsluhy(MessageForm message)
		{
			var mess = (MyMessage)message.CreateCopy();
			mess.Addressee = MySim.FindAgent(SimId.AgentOkolia);
			mess.Code = Mc.OdchodZakaznika;
			Notice(mess);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.PrichodZakaznika:
					ProcessPrichodZakaznika(message);
					break;

				case Mc.NoticeKoniecObsluhy:
					ProcessNoticeKoniecObsluhy(message);
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
		public new AgentModelu MyAgent
		{
			get
			{
				return (AgentModelu)base.MyAgent;
			}
		}
	}
}