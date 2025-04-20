namespace DSAgentSimulationWoodwork.Entities
{
	public class Objednavka
	{
		#region Properties
		public int Poradie { get; set; }
		public List<Tovar> Tovary { get; private set; } = [];
		#endregion // Properties 

		#region Constructor
		public Objednavka()
		{
			
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
			return Poradie + "\n" + tovarString;
		}
		#endregion // Public functions
	}
}
