using OSPABA;
using Simulation;
using Agents.AgentObsluhy.ContinualAssistants;
using OSPDataStruct;
using OSPStat;

namespace Agents.AgentObsluhy
{
	//meta! id="4"
	public class AgentObsluhy : OSPABA.Agent
	{
		public SimQueue<MyMessage> Rad { get; set; }
		public bool Obsadene { get; set; }
		public Stat CasCakaniaStat { get; set; }
		public WStat DlzkaRaduStat { get; set; }
		
		public AgentObsluhy(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			AddOwnMessage(Mc.KoniecObsluhyZakaznika);
			CasCakaniaStat = new Stat();
			DlzkaRaduStat = new WStat(MySim);
			
			Rad = new SimQueue<MyMessage>(DlzkaRaduStat);
			Obsadene = false;
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerObsluhy(SimId.ManagerObsluhy, MySim, this);
			new ProcesObsluhy(SimId.ProcesObsluhy, MySim, this);
			AddOwnMessage(Mc.Obsluha);
		}
		//meta! tag="end"
	}
}
