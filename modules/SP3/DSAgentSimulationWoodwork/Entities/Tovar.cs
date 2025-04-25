namespace DSAgentSimulationWoodwork.Entities;

public class Tovar
{
	#region Properties
	public int ObjednavkaId { get; set; }
    public int Poradie { get; set; }
    public TovarType Type { get; set; }
    public TovarStatus Status { get; set; }
    public MontazneMiesto MontazneMiesto { get; set; }
    #endregion // Properties
    
    #region Constructor
    public Tovar(TovarType type, int objednavkaId, int poradie)
    {
        Type = type;
        Poradie = poradie;
        ObjednavkaId = objednavkaId;
        Status = TovarStatus.CakajucaNaRezanie;
        MontazneMiesto = null;
    }
    #endregion // Constructor
    
    #region Public functions
    public override string ToString()
    {
        if (MontazneMiesto is null) return $"{ObjednavkaId}.{Poradie} {Type} : {Status} nie je na mont�nom mieste";
        if (MontazneMiesto.Stolar is null) return $"{ObjednavkaId}.{Poradie} {Type} : {Status} na MM �.{MontazneMiesto.ID}, bez stol�ra";
		return $"{ObjednavkaId}.{Poradie} {Type} : {Status} na MM �.{MontazneMiesto.ID}, stol�r �.{MontazneMiesto.Stolar.ID}";
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
    CakajucaNaRezanie,
    PriebehRezania,
    CakajucaNaMorenie,
    PriebehMorenia,
    CakajucaNaLakovanie,
	PriebehLakovania,
	CakajucaNaSkladanie,
    PriebehSkladania,
    CakajucaNaMontazKovani,
    PriebehMontazeKovani,
    Hotova
}