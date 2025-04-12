using OSPABA;
using Agents.AgentObsluhy;
using OSPRNG;
using Simulation;
namespace Agents.AgentObsluhy.ContinualAssistants
{
	//meta! id="14"
	public class ProcesObsluhy : OSPABA.Process
	{
		private ExponentialRNG _obsluhaGenerator = new ExponentialRNG(45);
		
		public ProcesObsluhy(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentObsluhy", id="15", type="Start"
		public void ProcessStart(MessageForm message)
		{
			//ked nas spusti manazer cerpacej stanice, tak posielame spravu sebe a medzitym cakame kym ubehne vygenerovany cas
			message.Code = Mc.KoniecObsluhyZakaznika;
			Hold(_obsluhaGenerator.Sample(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.KoniecObsluhyZakaznika:
					//po skonceni obsluhy oznamime agentovi cerpacej stanice koniec obsluhy
					message.Addressee = MyAgent;
					AssistantFinished(message);
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
		public new AgentObsluhy MyAgent
		{
			get
			{
				return (AgentObsluhy)base.MyAgent;
			}
		}
	}
}
