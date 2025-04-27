using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using Simulation;
namespace Agents.AgentMontaznychMiest
{
	//meta! id="8"
	public class ManagerMontaznychMiest : OSPABA.Manager
	{
        public ManagerMontaznychMiest(int id, OSPABA.Simulation mySim, Agent myAgent) :
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

		//meta! sender="AgentStolarskejDielne", id="58", type="Notice"
		public void ProcessPriradMiesto(MessageForm message)
		{
			// Priradenie montazneho miesta k tovaru z novej objednavky
			var tovary = ((MyMessage)message).Objednavka.Tovary;
			foreach (var tovar in tovary)
			{
				if (JeVolneMiesto())
				{
					var mm = DajMontazneMiesto();
					tovar.MontazneMiesto = mm;
					mm.Tovar = tovar;

					var msg = message.CreateCopy();
					msg.Code = Mc.ZacniPracu;
					msg.Addressee = MySim.FindAgent(SimId.AgentStolarskejDielne);
					((MyMessage)msg).Tovar = tovar;
					Notice(msg);
				}
				else
				{
					((MySimulation)MySim).PriemernyPocetNezacatychObjednavok.AddValue(MyAgent.NepriradeneTovary.Count + ((MySimulation)MySim).AgentStolarov.CakajuceNaRezanie.Count);
					MyAgent.NepriradeneTovary.Enqueue(tovar);
					if (MySim.AnimatorExists) MyAgent.AnimQueue.Insert(tovar.AnimImageItem);
				}
			}
		}

		//meta! sender="AgentStolarskejDielne", id="68", type="Notice"
		public void ProcessUvolniMiesto(MessageForm message)
		{
			// Uvolnenie montazneho miesta
			var miesto = ((MyMessage)message).Tovar.MontazneMiesto;
			miesto.Tovar = null;
			((MyMessage)message).Tovar.MontazneMiesto = null;

			// Priradenie montazneho miesta k starsiemu tovaru
			if (MyAgent.NepriradeneTovary.Count > 0)
			{
				var tovar = MyAgent.NepriradeneTovary.Dequeue();
				if (MySim.AnimatorExists)  MyAgent.AnimQueue.RemoveFirst();
                tovar.MontazneMiesto = miesto;
				miesto.Tovar = tovar;
				var msg = new MyMessage(MySim)
				{
					Code = Mc.ZacniPracu,
					Addressee = MySim.FindAgent(SimId.AgentStolarskejDielne)
				};
				msg.Tovar = tovar;
				Notice(msg);
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
			case Mc.PriradMiesto:
				ProcessPriradMiesto(message);
			break;

			case Mc.UvolniMiesto:
				ProcessUvolniMiesto(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentMontaznychMiest MyAgent
		{
			get
			{
				return (AgentMontaznychMiest)base.MyAgent;
			}
		}

		private bool JeVolneMiesto()
		{
			foreach (var miesto in MyAgent.MontazneMiesta)
			{
				if (miesto.Tovar is null) return true;
			}

			return false;
		}

		private MontazneMiesto DajMontazneMiesto()
		{
			foreach (var miesto in MyAgent.MontazneMiesta)
			{
				if (miesto.Tovar is null) return miesto;
			}

			return null!;
		}
	}
}