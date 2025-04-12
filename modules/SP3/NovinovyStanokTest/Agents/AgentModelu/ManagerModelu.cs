using OSPABA;
using Simulation;
namespace Agents.AgentModelu
{
	//meta! id="1"
	public class ManagerModelu : OSPABA.Manager
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
			//pokial nam pride oznamenie o prichode zakaznika do systemu tak poziadame agenta cerpacej stanice o obsluhu
			message.Code = Mc.Obsluha;
			message.Addressee = MySim.FindAgent(SimId.AgentObsluhy);
			Request(message);
		}

		//meta! sender="AgentObsluhy", id="9", type="Response"
		public void ProcessObsluha(MessageForm message)
		{
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Inicializacia:
					//ak nam pride inicializacna sprava, posleme ju dalej agentovi okolia
					message.Addressee = MySim.FindAgent(SimId.AgentOkolia);
					Notice(message);
					break;

				case Mc.KoniecObsluhyZakaznika:
					//ked skonci agent cerpacej stanice obsluhu zakaznika, oznamime to agentovi okolia
					message.Code = Mc.OdchodZakaznika;
					message.Addressee = MySim.FindAgent(SimId.AgentOkolia);
					Notice(message);
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
			case Mc.PrichodZakaznika:
				ProcessPrichodZakaznika(message);
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
