using OSPABA;
using Simulation;

namespace Agents.AgentModelu
{
	//meta! id="1"
	public class AgentModelu : OSPABA.Agent
	{
		public int pocetObjednavok { get; set; } = 0;
		public AgentModelu(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			pocetObjednavok = 0;

			var initMessage = new MyMessage(MySim);
			initMessage.Code = Mc.Init;
			initMessage.Addressee = MySim.FindAgent(SimId.AgentOkolia);
			MyManager.Notice(initMessage);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerModelu(SimId.ManagerModelu, MySim, this);
			AddOwnMessage(Mc.NovaObjednavka);
		}
		//meta! tag="end"
	}
}