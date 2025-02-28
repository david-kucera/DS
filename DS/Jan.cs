namespace DS;

public class Jan : SimCore
{
    #region Constants
    private const double U_TLMICE = 0.2;
    private const double U_BRZDY = 0.3;
    private const double U_SVETLA = 0.25;
    
    private const double POKUTA = 0.3;
    
    private const int POCET_TLMICE = 100;
    private const int POCET_BRZDY = 200;
    private const int POCET_SVETLA = 150;

    private const int POCET_TYZDNOV = 30;
    #endregion // Constants
    
    #region Class members
    private Random _rndTlmice = new();
    private Random _rndBrzdy = new();
    private Random _rndSvetlomety = new();
    
    private Random _rndDodavatel1Prvych10  = new();
    private Random _rndDodavatel1Od11 = new();
    private Random _rndDodavatel2Prvych15 = new();
    private Random _rndDodavatel2Od16 = new();
    #endregion // Class members

    #region Properties
    public int PocetTlmicov = 0;
    public int PocetBrzd = 0;
    public int PocetSvetiel = 0;
    #endregion // Properties
    
    #region Public functions
    protected override double Experiment()
    {
        double result = 0.0;
        for (int i = 1; i <= POCET_TYZDNOV; i++)
        {
            result += StategyA(i);
        }

        return result;
    }

    public override void BeforeSimulation()
    {
        throw new NotImplementedException();
    }

    public override void AfterSimulation()
    {
        throw new NotImplementedException();
    }

    public override void BeforeSimulationRun()
    {
        throw new NotImplementedException();
    }

    public override void AfterSimulationRun()
    {
        throw new NotImplementedException();
    }
    #endregion // Public functions
    
    #region Private functions

    private double StategyA(int tyzden)
    {
        if (tyzden <= 10) return StategyAB(_rndDodavatel1Prvych10);
        // TODO vygenerovat potrebne veci tu a potom ich posielat do spolocnej metody
        return StategyAB(_rndDodavatel1Od11);
    }

    private double StategyB(int tyzden)
    {
        if (tyzden <= 15) return StategyAB(_rndDodavatel2Prvych15); 
        // TODO vygenerovat potrebne veci tu a potom ich posielat do spolocnej metody
        return StategyAB(_rndDodavatel2Od16);
    }

    private double StategyAB(Random rnd)
    {
        var result = 0.0;
        var dovezeniePerc = rnd.NextDouble();
        if (dovezeniePerc < 70 && dovezeniePerc >= 10)
        {
            PocetTlmicov += POCET_TLMICE;
            PocetBrzd += POCET_BRZDY;
            PocetSvetiel += POCET_SVETLA;
        }
        
        var odberTlmicePerc = _rndTlmice.NextDouble();
        var odberBrzdPerc = _rndBrzdy.NextDouble();
        var odberSvetielPerc = _rndSvetlomety.NextDouble();

        throw new NotImplementedException();
    }

    private double StategyC(int tyzden)
    {
        throw new NotImplementedException();        
    }

    private double StategyD(int tyzden)
    {
        throw new NotImplementedException();
    }
    #endregion // Private functions
}