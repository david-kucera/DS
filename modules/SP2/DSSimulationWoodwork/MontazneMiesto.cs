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
}