using OSPABA;
using Simulation;
namespace Agents.AgentObsluhy
{
	//meta! id="4"
	public class ManagerObsluhy : OSPABA.Manager
	{
		public ManagerObsluhy(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="ProcesObsluhy", id="15", type="Finish"
		public void ProcessFinish(MessageForm message)
		{
			//ak proces obsluhy skoncil, notifikujeme o tom agenta modelu
			message.Code = Mc.KoniecObsluhyZakaznika;
			Response(message);

			//ak front nie je prazdny, zacneme s obsluhou dalsieho zakaznika
			if (MyAgent.Rad.Count > 0) //(MyAgent.ABAFront.Count > 0)
			{
				//ak pouzivam front simulacneho jadra, nemusim do statistik pridavat vzorky, urobi to za mna
				message = MyAgent.Rad.Dequeue();
				MyAgent.CasCakaniaStat.AddSample(MySim.CurrentTime - ((MyMessage)message).StartWaitingTime);

				message.Addressee = MyAgent.FindAssistant(SimId.ProcesObsluhy);
				StartContinualAssistant(message);
			}
			else MyAgent.Obsadene = false;
		}

		//meta! sender="AgentModelu", id="9", type="Request"
		public void ProcessObsluha(MessageForm message)
		{
			// ak je pokladna volna, tak okamzite spustime proces obsluhy zakaznika
			if (!MyAgent.Obsadene)
			{
				MyAgent.Obsadene = true;
				MyAgent.CasCakaniaStat.AddSample(0);
				message.Addressee = MyAgent.FindAssistant(SimId.ProcesObsluhy);
				StartContinualAssistant(message);
			} //ak je pokladna obsadena, pridame zakaznika do rady
			else
			{
				((MyMessage)message).StartWaitingTime = MySim.CurrentTime;
				//pri pouzivani frontu simulacneho jadra nemusim pridavat do statistiky vzorku, spravi to za mna
				MyAgent.Rad.Enqueue((MyMessage)message);
			}
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
			case Mc.Obsluha:
				ProcessObsluha(message);
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
		public new AgentObsluhy MyAgent
		{
			get
			{
				return (AgentObsluhy)base.MyAgent;
			}
		}
	}
}
