using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;
namespace Agents.AgentCStolar
{
	//meta! id="6"
	public class ManagerCStolar : OSPABA.Manager
	{
		public ManagerCStolar(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarov", id="61", type="Request"
		public void ProcessDajStolaraC(MessageForm message)
		{
			var msg = ((MyMessage)message);

			Stolar volny = null;
			var stolari = MyAgent.StolariC;
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
			case Mc.DajStolaraC:
				ProcessDajStolaraC(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentCStolar MyAgent
		{
			get
			{
				return (AgentCStolar)base.MyAgent;
			}
		}
	}
}