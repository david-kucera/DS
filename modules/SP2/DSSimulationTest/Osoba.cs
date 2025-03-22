namespace DSSimulationTest;

public class Osoba
{
    #region Properties
    public int Id { get; set; } = -1;
    public double ArrivalTime { get; set; } = -1.0;
    #endregion // Properties
    
    #region Constructors
    public Osoba()
    {
        
    }

    public Osoba(int id, double arrivalTime)
    {
        Id = id;
        ArrivalTime = arrivalTime;
    }
    #endregion // Constructors
}