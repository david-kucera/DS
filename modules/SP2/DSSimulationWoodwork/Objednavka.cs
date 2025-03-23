namespace DSSimulationWoodwork;

public class Objednavka
{
    #region Properties
    public ObjType Type { get; set; } = ObjType.Unknown;
    #endregion // Properties
    
    #region Constructor
    public Objednavka(ObjType type)
    {
        Type = type;
    }
    #endregion // Constructor
}

public enum ObjType
{
    Unknown,
    Stol,
    Skrina,
    Stolicka
}