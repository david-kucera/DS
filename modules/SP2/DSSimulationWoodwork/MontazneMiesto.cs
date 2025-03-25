namespace DSSimulationWoodwork;

public class MontazneMiesto
{
    #region Properties
    public int ID { get; set; } = -1;
    public Objednavka Objednavka { get; set; } = null;
    public Stolar Stolar { get; set; } = null;
    #endregion // Properties
    
    #region Constructor
    public MontazneMiesto()
    {
        
    }
    #endregion // Constructor
    
    #region Public functions

    public override string ToString()
    {
        bool obsadeny = false;
        StolarType stolarType = StolarType.Unknown;
        ObjType objType = ObjType.Unknown;
        ObjStatus objStatus = ObjStatus.Unknown;
        if (Stolar is not null) obsadeny = Stolar.Obsadeny;
        if (Stolar is not null) stolarType = Stolar.Type;
        if (Objednavka is not null) objType = Objednavka.Type;
        if (Objednavka is not null) objStatus = Objednavka.Status;
        return $"Miesto: {ID}, Stolar: {stolarType} {obsadeny}, Objednavka: {objType}, {objStatus}";
    }

    #endregion // Public functions
}