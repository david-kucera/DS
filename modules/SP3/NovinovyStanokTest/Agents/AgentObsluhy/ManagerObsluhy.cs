using OSPABA;
using OSPDataStruct;
using Simulation;

namespace Agents.AgentObsluhy
{
	//meta! id="4"
public class ManagerStanku : Manager
{
	public bool Obsadeny { get; set; }
	public SimQueue<MyMessage> Rad { get; set; }
	
	public ManagerStanku(int id, OSPABA.Simulation mySim, Agent myAgent) :
		base(id, mySim, myAgent)
	{
		Init();
	}

	override public void PrepareReplication()
	{
		base.PrepareReplication();
		// Setup component for the next replication

		Obsadeny = false;
		Rad = new();
		
		if (PetriNet != null)
		{
			PetriNet.Clear();
		}
	}

	//meta! sender="ProcesObsluhy", id="15", type="Finish"
	public void ProcessFinish(MessageForm message)
	{
		var mess = (MyMessage)message.CreateCopy();
		mess.Code = Mc.NoticeKoniecObsluhy;
		Response(mess);

		if (Rad.Count > 0)
		{
			var msg = Rad.Dequeue();
			((MySimulation)MySim).KumulativnyCasCakania += MySim.CurrentTime - msg.ZaciatokCakania;
			
			msg.Addressee = MyAgent.FindAssistant(SimId.ProcesObsluhy);
			StartContinualAssistant(msg);
		}
		else Obsadeny = false;
	}

	//meta! sender="AgentModelu", id="9", type="Request"
	public void ProcessObsluha(MessageForm message)
	{
		var msg = (MyMessage)message.CreateCopy();
		msg.ZaciatokCakania = MySim.CurrentTime;
		((MySimulation)MySim).KumulativnyPocetCakajucich++;
		
		if (Obsadeny) Rad.Enqueue(msg);
		else
		{
			Obsadeny = true;
			msg.Addressee = MyAgent.FindAssistant(SimId.ProcesObsluhy);
			StartContinualAssistant(msg);
		}
	}

	//meta! userInfo="Process messages defined in code", id="0"
	public void ProcessDefault(MessageForm message)
	{
		switch (message.Code)
		{
		}
	}

	//meta! userInfo="Removed from model"
	public void ProcessNoticeKoniecObsluhy(MessageForm message)
	{
	}

	//meta! userInfo="Generated code: do not modify", tag="begin"
	public void Init()
	{
	}

	override public void ProcessMessage(MessageForm message)
	{
		switch (message.Code)
		{
		case Mc.Finish:
			ProcessFinish(message);
		break;

		case Mc.Obsluha:
			ProcessObsluha(message);
		break;

		default:
			ProcessDefault(message);
		break;
		}
	}
	//meta! tag="end"
	public new AgentStanku MyAgent
	{
		get
		{
			return (AgentStanku)base.MyAgent;
		}
	}
}
}