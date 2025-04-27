using DSAgentSimulationWoodwork.Entities;
using OSPABA;
using OSPAnimator;
using Simulation;

namespace Agents.AgentMontaznychMiest
{
	//meta! id="8"
	public class AgentMontaznychMiest : OSPABA.Agent
	{
        #region Constants
		private const int ANIM_QUEUE_X = 0;
        private const int ANIM_QUEUE_Y = 500;
        private const int ANIM_QUEUE_WIDTH = 500;
        private const int ANIM_QUEUE_HEIGHT = 500;
        private const int ANIM_QUEUE_SPEED = 50;
        #endregion // Constants

        #region Properties
        public List<MontazneMiesto> MontazneMiesta { get; set; }
		public Queue<Tovar> NepriradeneTovary { get; set; }
        public AnimQueue AnimQueue { get; set; }
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
            AnimQueue = new AnimQueue(MySim.Animator, ANIM_QUEUE_X, ANIM_QUEUE_Y, ANIM_QUEUE_WIDTH, ANIM_QUEUE_HEIGHT, ANIM_QUEUE_SPEED);
            AnimQueue.SetVisible(true);

            foreach (MontazneMiesto miesto in MontazneMiesta)
			{
				MySim.Animator.Register(miesto.AnimShapeItem);
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