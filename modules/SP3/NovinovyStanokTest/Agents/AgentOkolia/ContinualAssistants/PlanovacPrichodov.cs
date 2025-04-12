using Agents.AgentOkolia;
using OSPABA;
using OSPRNG;
using Simulation;
namespace Agents.AgentOkolia.ContinualAssistants
{
	//meta! id="11"
	public class PlanovacPrichodov : OSPABA.Scheduler
	{
		private ExponentialRNG _prichodyGenerator = new ExponentialRNG(100);
		
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
			// ked nas spusti manazer okolia tak posielame spravu sebe a medzitym cakame kym ubehne vygenerovany cas
			message.Code = Mc.PrichodZakaznika;
			Hold(_prichodyGenerator.Sample(), message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.PrichodZakaznika:
					//po skonceni cakania oznamime agentovi prichod zakaznika do systemu
					message.Addressee = MyAgent;
					Notice(message);
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
