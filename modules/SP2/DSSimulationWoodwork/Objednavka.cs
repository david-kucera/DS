namespace DSSimulationWoodwork;

public class Objednavka
{
    #region Properties
    public ObjType Type { get; set; } = ObjType.Unknown;
    public double ArrivalTime { get; set; } = double.NaN;
    public ObjStatus Status { get; set; } = ObjStatus.Unknown;
    public int Poradie { get; set; } = -1;
    public MontazneMiesto MontazneMiesto { get; set; } = null!;
    #endregion // Properties
    
    #region Constructor
    public Objednavka(ObjType type, double arrivalTime, int poradie)
    {
        Type = type;
        ArrivalTime = arrivalTime;
        Poradie = poradie;
        MontazneMiesto = null;
        Status = ObjStatus.CakajucaNaRezanie;
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