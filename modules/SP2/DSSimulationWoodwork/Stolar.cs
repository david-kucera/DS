namespace DSSimulationWoodwork;

public class Stolar
{
    #region Properties
    public StolarType Type { get; set; } = StolarType.Unknown;
    public Poloha Poloha { get; set; } = Poloha.Unkwnown;
    public int IDMiesta { get; set; } = -1;
    #endregion // Properties

    #region Constructor
    public Stolar(StolarType stolarType)
    {
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

public enum Poloha
{
    Unkwnown,
    Sklad,
    MontazneMiesto
}