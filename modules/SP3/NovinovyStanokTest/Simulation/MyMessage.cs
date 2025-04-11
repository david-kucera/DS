using OSPABA;

namespace Simulation
{
	public class MyMessage : MessageForm
	{
		public double ZaciatokCakania { get; set; }
		
		public MyMessage(OSPABA.Simulation sim) :
			base(sim)
		{
		}

		public MyMessage(MyMessage original) :
			base(original)
		{
			// copy() is called in superclass
			ZaciatokCakania = original.ZaciatokCakania;
		}

		public override MessageForm CreateCopy()
		{
			return new MyMessage(this);
		}

		protected override void Copy(MessageForm message)
		{
			base.Copy(message);
			MyMessage original = (MyMessage)message;
			// Copy attributes
		}
	}
}