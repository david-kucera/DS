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
        int stolarId = -1;
        StolarType stolarType = StolarType.Unknown;
        int objednavkaId = -1;
        ObjednavkaType objednavkaType = ObjednavkaType.Unknown;
        ObjednavkaStatus objednavkaStatus = ObjednavkaStatus.Unknown;
        if (Stolar is not null) obsadeny = Stolar.Obsadeny;
        if (Stolar is not null) stolarType = Stolar.Type;
        if (Stolar is not null) stolarId = Stolar.ID;
        if (Objednavka is not null) objednavkaId = Objednavka.Poradie;
        if (Objednavka is not null) objednavkaType = Objednavka.Type;
        if (Objednavka is not null) objednavkaStatus = Objednavka.Status;
        return $"Miesto: {ID},\t Objednavka: {objednavkaId}. {objednavkaType}, {objednavkaStatus},\t Stolar: {stolarType}{stolarId} {obsadeny}";
    }
    #endregion // Public functions
}