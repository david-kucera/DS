using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using OSPRNG;
using Simulation;
namespace Agents.AgentOkolia.ContinualAssistants
{
	//meta! id="15"
	public class SchedulerPrichodovObjednavok : OSPABA.Scheduler
	{
		#region Class members
		private ExponentialRNG _prichodyGenerator;
		private UniformContinuousRNG _typNabytkuGenerator;
		private UniformDiscreteRNG _pocetKusovGenerator;
		#endregion // Class members

		public SchedulerPrichodovObjednavok(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_prichodyGenerator = new ExponentialRNG(1.0/(2.0 / 60 / 60), seeder);
			_pocetKusovGenerator = new UniformDiscreteRNG(1, 5, seeder);
			_typNabytkuGenerator = new UniformContinuousRNG(0.0, 1.0, seeder);
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication
		}

		//meta! sender="AgentOkolia", id="16", type="Start"
		public void ProcessStart(MessageForm message)
		{
            message.Code = Mc.Finish;
			double duration = _prichodyGenerator.Sample();
			var obj = VytvorObjednavku();
            ((MyMessage)message).Objednavka = obj;
            foreach (var tovar in obj.Tovary)
			{
				tovar.AnimImageItem.MoveTo(MySim.CurrentTime, duration, Constants.ANIM_QUEUE_START);
                if (MySim.AnimatorExists) MySim.Animator.Register(tovar.AnimImageItem);
            }
            Hold(duration, message);
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
				case Mc.Finish:
					AssistantFinished(message);
					break;
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.Start:
				ProcessStart(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentOkolia MyAgent
		{
			get
			{
				return (AgentOkolia)base.MyAgent;
			}
		}

		private Objednavka VytvorObjednavku()
		{
			Objednavka obj = new Objednavka(MySim.CurrentTime);
			int pocetTovaru = _pocetKusovGenerator.Sample();

			for (int i = 1; i <= pocetTovaru; i++)
			{
				double typNabytku = _typNabytkuGenerator.Sample();
				if (typNabytku < 0.5) obj.AddTovar(new Tovar(TovarType.Stol, obj.Poradie, i));
				else if (typNabytku <= 0.65) obj.AddTovar(new Tovar(TovarType.Stolicka, obj.Poradie, i));
				else obj.AddTovar(new Tovar(TovarType.Skrina, obj.Poradie, i));
			}

            return obj;
		}
	}
}