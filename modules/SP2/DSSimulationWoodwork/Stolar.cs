namespace DSSimulationWoodwork;

public class Stolar
{
    #region Properties
    public int ID { get; set; }
    public StolarType Type { get; set; }
    public MontazneMiesto MontazneMiesto { get; set; }
    public bool Obsadeny { get; set; }
    #endregion // Properties

    #region Constructor
    public Stolar(StolarType stolarType, int id)
    {
        ID = id;
        Type = stolarType;
        MontazneMiesto = null!;
        Obsadeny = false;
    }
    #endregion // Constructor
}

public enum StolarType
{
    Unknown,
    A,
    B,
    C
}