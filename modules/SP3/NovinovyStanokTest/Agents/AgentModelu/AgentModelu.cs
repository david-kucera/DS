using OSPABA;
using Simulation;

namespace Agents.AgentModelu
{
	//meta! id="1"
	public class AgentModelu : OSPABA.Agent
	{
		public AgentModelu(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
			
			var message = new MyMessage(MySim)
			{
				Addressee = this,
				Code = Mc.Inicializacia
			};
			MyManager.Notice(message);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerModelu(SimId.ManagerModelu, MySim, this);
			AddOwnMessage(Mc.PrichodZakaznika);
			AddOwnMessage(Mc.NoticeKoniecObsluhy);
			AddOwnMessage(Mc.Obsluha);
		}
		//meta! tag="end"
	}
}