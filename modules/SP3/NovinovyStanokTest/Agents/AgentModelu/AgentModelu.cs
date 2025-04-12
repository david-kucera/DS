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
			AddOwnMessage(Mc.KoniecObsluhyZakaznika);
			
			//vytvorime pociatocnu spravu a posleme ju nasmu manazerovi aby sme odstartovali simulaciu
			var sprava = new MyMessage(MySim)
			{
				Addressee = this,
				Code = Mc.Inicializacia
			};
			MyManager.Notice(sprava);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerModelu(SimId.ManagerModelu, MySim, this);
			AddOwnMessage(Mc.PrichodZakaznika);
			AddOwnMessage(Mc.Obsluha);
		}
		//meta! tag="end"
	}
}
