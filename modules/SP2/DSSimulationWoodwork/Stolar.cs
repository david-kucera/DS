namespace DSSimulationWoodwork;

public class Stolar
{
    #region Properties
    public int ID { get; set; } = -1;
    public StolarType Type { get; set; } = StolarType.Unknown;
    public MontazneMiesto MontazneMiesto { get; set; } = null!;
    public bool Obsadeny { get; set; } = false;
    #endregion // Properties

    #region Constructor
    public Stolar(StolarType stolarType, int id)
    {
        ID = id;
        Type = stolarType;
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