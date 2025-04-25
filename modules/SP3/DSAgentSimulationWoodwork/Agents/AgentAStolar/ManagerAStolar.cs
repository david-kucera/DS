using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;
namespace Agents.AgentAStolar
{
	//meta! id="4"
	public class ManagerAStolar : OSPABA.Manager
	{
		public ManagerAStolar(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarov", id="57", type="Request"
		public void ProcessDajStolaraA(MessageForm message)
		{
			var msg = ((MyMessage)message);

			Stolar volny = null;
			var stolari = MyAgent.StolariA;
			foreach (var stolar in stolari)
			{
				if (!stolar.Obsadeny)
				{
					stolar.Workload.AddValue(stolar.Obsadeny, MySim.CurrentTime);
					stolar.Obsadeny = true;
					volny = stolar;
					break;
				}
			}

			msg.Stolar = volny;
			Response(message);
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
			case Mc.DajStolaraA:
				ProcessDajStolaraA(message);
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