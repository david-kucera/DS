namespace DSAgentSimulationWoodwork.Entities;

public class Tovar
{
	#region Properties
	public int ObjednavkaId { get; set; }
	public double ArrivalTime { get; set; }
    public TovarType Type { get; set; }
    public TovarStatus Status { get; set; }
    public MontazneMiesto MontazneMiesto { get; set; }
    #endregion // Properties
    
    #region Constructor
    public Tovar(TovarType type, double arrivalTime, int objednavkaId)
    {
        Type = type;
        ArrivalTime = arrivalTime;
        ObjednavkaId = objednavkaId;
        Status = TovarStatus.CakajucaNaRezanie;
        MontazneMiesto = null;
    }
    #endregion // Constructor
    
    #region Public functions
    public override string ToString()
    {
        return $"{ObjednavkaId}. {Type} : {Status}";
    }
    #endregion // Public functions
}

public enum TovarType
{
    Unknown,
    Stol,
    Skrina,
    Stolicka
}

public enum TovarStatus
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