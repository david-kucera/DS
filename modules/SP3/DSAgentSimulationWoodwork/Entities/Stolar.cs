using DSAgentSimulationLib.Statistics;

namespace DSAgentSimulationWoodwork.Entities;

public class Stolar
{
    private static int _poradie = 0;

	#region Properties
	public int ID { get; set; }
    public StolarType Type { get; set; }
    public MontazneMiesto MontazneMiesto { get; set; }
    public bool Obsadeny { get; set; }
    public Workload Workload { get; set; }
    #endregion // Properties

    #region Constructor
    public Stolar(StolarType stolarType, int id)
    {
        ID = _poradie++;
		Type = stolarType;
        MontazneMiesto = null!;
        Obsadeny = false;
        Workload = new Workload();
    }
	#endregion // Constructor

	#region Public functions
	public static void ResetPoradie()
	{
		_poradie = 1;
	}
	#endregion // Public functions
}

public enum StolarType
{
    Unknown,
    A,
    B,
    C
}