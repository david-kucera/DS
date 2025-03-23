namespace DSSimulationWoodwork;

public class Objednavka
{
    #region Properties
    public ObjType Type { get; set; } = ObjType.Unknown;
    public ObjStatus Status { get; set; } = ObjStatus.Unknown;
    public int Poradie { get; set; } = -1;
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

public enum ObjStatus
{
    Unknown,
    CakajucaNaRezanie,
    Narezana,
    CakajucaNaMorenie,
    Namorena,
    CakajucaNaSkladanie,
    Poskladana,
    CakajucaNaMontazKovani,
    Hotova
}