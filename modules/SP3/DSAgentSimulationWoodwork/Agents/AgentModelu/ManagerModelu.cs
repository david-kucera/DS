using DSAgentSimulationWoodwork.Entities;
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

		//meta! sender="AgentOkolia", id="52", type="Notice"
		public void ProcessNovaObjednavka(MessageForm message)
		{
			var obj = ((MyMessage)message).Objednavka;
			MyAgent.Objednavky.Add(obj);

			message.Addressee = MySim.FindAgent(SimId.AgentStolarskejDielne);
			Notice(message);
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

		//meta! sender="AgentStolarskejDielne", id="115", type="Notice"
		public void ProcessObjednavkaHotova(MessageForm message)
		{
			var tovar = ((MyMessage)message).Tovar;
			var obj = MyAgent.Objednavky.Find(o => o.Poradie == tovar.ObjednavkaId);

			foreach (var t in obj!.Tovary)
			{
				if (t.Status != TovarStatus.Hotova) return;
			}

			var casVSysteme = MySim.CurrentTime - obj.ArrivalTime;
			((MySimulation)MySim).PriemernyCasObjednavkyVSysteme.AddValue(casVSysteme);
			MyAgent.Objednavky.Remove(obj);
			MyAgent.PocetHotovychObjednavok++;
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.ObjednavkaHotova:
				ProcessObjednavkaHotova(message);
			break;

			case Mc.NovaObjednavka:
				ProcessNovaObjednavka(message);
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