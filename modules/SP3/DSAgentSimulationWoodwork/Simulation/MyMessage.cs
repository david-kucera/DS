using DSAgentSimulationWoodwork.Entities;
using OSPABA;
namespace Simulation
{
	public class MyMessage : OSPABA.MessageForm
	{
		#region Properties
		public Objednavka Objednavka { get; set; } = null;
		public MontazneMiesto MontazneMiesto { get; set; } = null;
		public Tovar Tovar { get; set; } = null;
		public Stolar Stolar { get; set; } = null;
		#endregion // Properties

		public MyMessage(OSPABA.Simulation mySim) :
			base(mySim)
		{
		}

		public MyMessage(MyMessage original) :
			base(original)
		{
			// copy() is called in superclass
		}

		override public MessageForm CreateCopy()
		{
			return new MyMessage(this);
		}

		override protected void Copy(MessageForm message)
		{
			base.Copy(message);
			MyMessage original = (MyMessage)message;
			// Copy attributes
			Objednavka = original.Objednavka;
			MontazneMiesto = original.MontazneMiesto;
			Tovar = original.Tovar;
			Stolar = original.Stolar;
		}
	}
}