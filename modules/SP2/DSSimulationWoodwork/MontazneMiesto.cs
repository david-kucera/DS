namespace DSSimulationWoodwork;

public class MontazneMiesto
{
    #region Properties
    public int ID { get; set; }
    public Objednavka Objednavka { get; set; }
    public Stolar Stolar { get; set; }
    #endregion // Properties
    
    #region Constructor
    public MontazneMiesto(int id)
    {
        ID = id;
        Objednavka = null;
        Stolar = null;
    }
    #endregion // Constructor
    
    #region Public functions
    public override string ToString()
    {
        bool obsadeny = false;
        StolarType stolarType = StolarType.Unknown;
        ObjednavkaType objednavkaType = ObjednavkaType.Unknown;
        ObjednavkaStatus objednavkaStatus = ObjednavkaStatus.Unknown;
        if (Stolar is not null) obsadeny = Stolar.Obsadeny;
        if (Stolar is not null) stolarType = Stolar.Type;
        if (Objednavka is not null) objednavkaType = Objednavka.Type;
        if (Objednavka is not null) objednavkaStatus = Objednavka.Status;
        return $"Miesto: {ID}, Stolar: {stolarType} {obsadeny}, Objednavka: {objednavkaType}, {objednavkaStatus}";
    }
    #endregion // Public functions
}