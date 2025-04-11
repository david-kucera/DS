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

		//meta! sender="AgentModelu", id="9", type="Notice"
		public void ProcessOdchodZakaznika(MessageForm message)
		{
		}

		//meta! sender="PlanovacPrichodov", id="16", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="10", type="Notice"
		public void ProcessInicializacia(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="8", type="Notice"
		public void ProcessNovyZakaznik(MessageForm message)
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
			case Mc.OdchodZakaznika:
				ProcessOdchodZakaznika(message);
			break;

			case Mc.NovyZakaznik:
				ProcessNovyZakaznik(message);
			break;

			case Mc.Finish:
				ProcessFinish(message);
			break;

			case Mc.Inicializacia:
				ProcessInicializacia(message);
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
