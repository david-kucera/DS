using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using OSPAnimator;
using Simulation;

namespace Agents.AgentMontaznychMiest
{
	//meta! id="8"
	public class AgentMontaznychMiest : OSPABA.Agent
	{
        #region Properties
        public List<MontazneMiesto> MontazneMiesta { get; set; } = null!;
		public Queue<Tovar> NepriradeneTovary { get; set; } = null!;
        public AnimQueue AnimQueue { get; set; } = null!;
        #endregion // Properties

        public AgentMontaznychMiest(int id, OSPABA.Simulation mySim, Agent parent) :
			base(id, mySim, parent)
		{
			Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			MontazneMiesta = new List<MontazneMiesto>(((MySimulation)MySim).PocetMontaznychMiest);
			for (int i = 1; i <= ((MySimulation)MySim).PocetMontaznychMiest; i++)
			{
				MontazneMiesta.Add(new MontazneMiesto(i));
			}
			NepriradeneTovary = new Queue<Tovar>();
        }

		public void InitAnimator()
		{
            AnimQueue = new AnimQueue(MySim.Animator, Constants.ANIM_QUEUE_END, Constants.ANIM_QUEUE_START, Constants.ANIM_QUEUE_SPEED);
            AnimQueue.SetVisible(true);

			if (MontazneMiesta == null) return;

            foreach (MontazneMiesto miesto in MontazneMiesta)
			{
				if (MySim.AnimatorExists) MySim.Animator.Register(miesto.AnimShapeItem);
            }
        }

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			new ManagerMontaznychMiest(SimId.ManagerMontaznychMiest, MySim, this);
			AddOwnMessage(Mc.PriradMiesto);
			AddOwnMessage(Mc.UvolniMiesto);
		}
		//meta! tag="end"
	}
}