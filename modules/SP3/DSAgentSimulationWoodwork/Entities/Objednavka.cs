namespace DSAgentSimulationWoodwork.Entities
{
	public class Objednavka
	{
		private static int _poradie = 1;

		#region Properties
		public double ArrivalTime { get; set; }
		public int Poradie { get; set; }
		public List<Tovar> Tovary { get; private set; } = [];
		#endregion // Properties 

		#region Constructor
		public Objednavka(double arrivalTime)
		{
			ArrivalTime = arrivalTime;
			Poradie = _poradie++;
		}
		#endregion // Constructor

		#region Public functions
		public void AddTovar(Tovar tovar)
		{
			tovar.ObjednavkaId = Poradie;
			Tovary.Add(tovar);
		}


		public override string ToString()
		{
			string tovarString = "";
			foreach (var tovar in Tovary)
			{
				tovarString += tovar.ToString() + "\n";
			}
			return Poradie + ".\n" + tovarString;
		}

		public static void ResetPoradie()
		{
			_poradie = 1;
		}
		#endregion // Public functions
	}
}
