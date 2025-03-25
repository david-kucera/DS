using DSSimulationWoodwork;

namespace DSSimulationVisualization;

public class MontMiestoViewModel
{
    public int ID { get; set; }
    public int? StolarId { get; set; }
    public string? StolarType { get; set; }
    public string? ObjednavkaType { get; set; }
    public string? ObjednavkaStatus { get; set; }

    public MontMiestoViewModel(int Id, int stolarId, StolarType stolarType, ObjType objType, ObjStatus objednavkaStatus)
    {
        ID = Id;
        StolarId = stolarId;
        StolarType = stolarType.ToString();
        ObjednavkaType = objType.ToString();
        ObjednavkaStatus = objednavkaStatus.ToString();
    }

    public override string ToString()
    {
        return $"Miesto:{ID} Stolar:{StolarId} {StolarType} Objednavka:{ObjednavkaType} {ObjednavkaStatus}";
    }
}