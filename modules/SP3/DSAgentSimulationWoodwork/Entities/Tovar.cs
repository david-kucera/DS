using System.IO;
using OSPAnimator;

namespace DSAgentSimulationWoodwork.Entities;

public class Tovar
{
    #region Constants
    private const string IMG_PATH_STOL = "../../resources/stol.png";
    private const string IMG_PATH_STOLICKA = "../../resources/stolicka.png";
    private const string IMG_PATH_SKRINA = "../../resources/skrina.png";
    #endregion // Constants

    #region Properties
    public int ObjednavkaId { get; set; }
    public int Poradie { get; set; }
    public TovarType Type { get; set; }
    public TovarStatus Status { get; set; }
    public MontazneMiesto MontazneMiesto { get; set; }
    public AnimImageItem AnimImageItem { get; set; }
    #endregion // Properties

    #region Constructor
    public Tovar(TovarType type, int objednavkaId, int poradie)
    {
        CheckImages();

        Type = type;
        Poradie = poradie;
        ObjednavkaId = objednavkaId;
        Status = TovarStatus.CakajucaNaRezanie;
        MontazneMiesto = null;
        switch (type) 
        {
            case TovarType.Stol:
                AnimImageItem = new AnimImageItem(IMG_PATH_STOL, 10, 10);
                break;
            case TovarType.Skrina:
                AnimImageItem = new AnimImageItem(IMG_PATH_SKRINA, 10, 10);
                break;
            case TovarType.Stolicka:
                AnimImageItem = new AnimImageItem(IMG_PATH_STOLICKA, 10, 10);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        AnimImageItem.SetLabel($"{ObjednavkaId}.{Poradie}");
    }
    #endregion // Constructor
    
    #region Public functions
    public override string ToString()
    {
        if (MontazneMiesto is null) return $"{ObjednavkaId}.{Poradie} {Type} : {Status} nie je na montážnom mieste";
        if (MontazneMiesto.Stolar is null) return $"{ObjednavkaId}.{Poradie} {Type} : {Status} na MM è.{MontazneMiesto.ID}, bez stolára";
		return $"{ObjednavkaId}.{Poradie} {Type} : {Status} na MM è.{MontazneMiesto.ID}, stolár è.{MontazneMiesto.Stolar.ID}";
    }
    #endregion // Public functions

    #region Private functions
    private void CheckImages()
    {
        if (File.Exists(IMG_PATH_STOL) == false)
            throw new FileNotFoundException($"File {IMG_PATH_STOL} not found");
        if (File.Exists(IMG_PATH_STOLICKA) == false)
            throw new FileNotFoundException($"File {IMG_PATH_STOLICKA} not found");
        if (File.Exists(IMG_PATH_SKRINA) == false)
            throw new FileNotFoundException($"File {IMG_PATH_SKRINA} not found");
    }
    #endregion // Private functions
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