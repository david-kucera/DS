using OSPABA;
using Simulation;
namespace Agents.AgentOkolia
{
	//meta! id="3"
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

		//meta! sender="PlanovacPrichodov", id="12", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="20", type="Notice"
		public void ProcessOdchodZakaznika(MessageForm message)
		{
		}

		//meta! sender="AgentModelu", id="23", type="Notice"
		public void ProcessInicializacia(MessageForm message)
		{
			// Prisla pociatocna sprava, spustim planovac prichodov
			message.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodov);
			StartContinualAssistant(message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.PrichodZakaznika:
					//ked ubehne cas, ktory bol naplanovany medzi prichodmi zakaznika,
					//tak to oznamime agentovi modelu a spustime s novou spravou proces cakania na dalsieho zakaznika
					message.Addressee = MyAgent.Parent;
					Notice(message);
					MyMessage newMessage = (MyMessage)message.CreateCopy();
					newMessage.Addressee = MyAgent.FindAssistant(SimId.PlanovacPrichodov);
					StartContinualAssistant(newMessage);
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
			case Mc.Finish:
				ProcessFinish(message);
			break;

			case Mc.Inicializacia:
				ProcessInicializacia(message);
			break;

			case Mc.OdchodZakaznika:
				ProcessOdchodZakaznika(message);
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
