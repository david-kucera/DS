namespace DSSimulationWoodwork;

public class Objednavka
{
    #region Properties
    public int Poradie { get; set; }
    public double ArrivalTime { get; set; }
    public ObjednavkaType Type { get; set; }
    public ObjednavkaStatus Status { get; set; }
    public MontazneMiesto MontazneMiesto { get; set; }
    #endregion // Properties
    
    #region Constructor
    public Objednavka(ObjednavkaType type, double arrivalTime, int poradie)
    {
        Type = type;
        ArrivalTime = arrivalTime;
        Poradie = poradie;
        Status = ObjednavkaStatus.CakajucaNaRezanie;
        MontazneMiesto = null;
    }
    #endregion // Constructor
    
    #region Public functions
    public override string ToString()
    {
        return $"{Poradie}. {Type} : {Status}";
    }
    #endregion // Public functions
}

public enum ObjednavkaType
{
    Unknown,
    Stol,
    Skrina,
    Stolicka
}

public enum ObjednavkaStatus
{
    Unknown,
    Preberana,
    CakajucaNaRezanie,
    PriebehRezania,
    CakajucaNaMorenie,
    PriebehMorenia,
    CakajucaNaSkladanie,
    PriebehSkladania,
    CakajucaNaMontazKovani,
    PriebehMontazeKovani,
    Hotova
}