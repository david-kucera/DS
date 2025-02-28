using DSLib.Generators;
using DSLib.Generators.Empirical;

namespace DSLib.MonteCarlo;

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
    private Random _rndTlmice;
    private Random _rndBrzdy;
    private DiscreteEmpirical _rndSvetlomety;
    
    private Random _rndDodavatel1Prvych10;
    private Random _rndDodavatel1Od11;
    private ContinousEmpirical _rndDodavatel2Prvych15;
    private ContinousEmpirical _rndDodavatel2Od16;
    #endregion // Class members

    #region Properties
    public int PocetTlmicovNaSklade = 0;
    public int PocetBrzdNaSklade = 0;
    public int PocetSvetielNaSklade = 0;
    #endregion // Properties
    
    #region Constructor
    public Jan(Random rndTlmice, Random rndBrzdy, DiscreteEmpirical rndSvetlomety, Random rndDodavatel1Prvych10, Random rndDodavatel1Od11, ContinousEmpirical rndDodavatel2Prvych15, ContinousEmpirical rndDodavatel2Od16)
    {
        _rndTlmice = rndTlmice;
        _rndBrzdy = rndBrzdy;
        _rndSvetlomety = rndSvetlomety;
        
        _rndDodavatel1Prvych10 = rndDodavatel1Prvych10;
        _rndDodavatel1Od11 = rndDodavatel1Od11;
        _rndDodavatel2Prvych15 = rndDodavatel2Prvych15;
        _rndDodavatel2Od16 = rndDodavatel2Od16;
    }
    #endregion // Constructor
    
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

    protected override void BeforeSimulation()
    {

    }

    protected override void AfterSimulation()
    {

    }

    protected override void BeforeSimulationRun()
    {

    }

    protected override void AfterSimulationRun()
    {

    }
    #endregion // Public functions
    
    #region Private functions
    private double StategyA(int tyzden)
    {
        var naklady = 0.0;
        
        double dovezeniePerc = 0.0;
        if (tyzden <= 10) dovezeniePerc = _rndDodavatel1Prvych10.Next(10, 70);
        else dovezeniePerc = _rndDodavatel1Od11.Next(30, 95); 
        
        if (dovezeniePerc < 70 && dovezeniePerc >= 10) // doviezlo sa
        {
            PocetTlmicovNaSklade += POCET_TLMICE;
            PocetBrzdNaSklade += POCET_BRZDY;
            PocetSvetielNaSklade += POCET_SVETLA;
        }
        
        // veci cakaju do piatku ... teda 4 dni po-ut, ut-st, st-stv, stv-pi
        for (int i = 0; i < 4; i++)
        {
            naklady += PocetTlmicovNaSklade * U_TLMICE;
            naklady += PocetBrzdNaSklade * U_BRZDY;
            naklady += PocetSvetielNaSklade * U_SVETLA;
        }
        
        // odberne mnozstva od klienta
        var odberTlmicov = _rndTlmice.Next(50, 100);
        var odberBrzd = _rndBrzdy.Next(60, 250);
        var odberSvetiel = _rndSvetlomety.Next();
        
        // kontrola pokut za nedostatocne zasoby
        if (odberTlmicov > PocetTlmicovNaSklade)
        {
            naklady += (odberTlmicov - PocetTlmicovNaSklade) * POKUTA;
            PocetTlmicovNaSklade = 0;
        }
        else PocetTlmicovNaSklade -= odberTlmicov;
        if (odberBrzd > PocetBrzdNaSklade)
        {
            naklady += (odberBrzd - PocetBrzdNaSklade) * POKUTA;
            PocetBrzdNaSklade = 0;
        }
        else PocetBrzdNaSklade -= odberBrzd;
        if (odberSvetiel > PocetSvetielNaSklade)
        {
            naklady += (odberSvetiel - PocetSvetielNaSklade) * POKUTA;
            PocetSvetielNaSklade = 0;
        }
        else PocetSvetielNaSklade -= odberSvetiel;
        
        // zvysny tovar na sklade este caka dalsie 3 dni do konca tyzdna... pi-so, so-ne, ne-po
        for (int i = 0; i < 3; i++)
        {
            naklady += PocetTlmicovNaSklade * U_TLMICE;
            naklady += PocetBrzdNaSklade * U_BRZDY;
            naklady += PocetSvetielNaSklade * U_SVETLA;
        }

        return naklady;
    }

    private double StategyB(int tyzden)
    {
        var naklady = 0.0;
        
        double dovezeniePerc = 0.0;
        if (tyzden <= 15) dovezeniePerc = _rndDodavatel2Prvych15.Next();
        else dovezeniePerc = _rndDodavatel2Od16.Next();
        
        // TODO s cim proovnavat???
        throw new NotImplementedException();
        if (dovezeniePerc < 70 && dovezeniePerc >= 10) // doviezlo sa
        {
            PocetTlmicovNaSklade += POCET_TLMICE;
            PocetBrzdNaSklade += POCET_BRZDY;
            PocetSvetielNaSklade += POCET_SVETLA;
        }
        
        // veci cakaju do piatku ... teda 4 dni po-ut, ut-st, st-stv, stv-pi
        for (int i = 0; i < 4; i++)
        {
            naklady += PocetTlmicovNaSklade * U_TLMICE;
            naklady += PocetBrzdNaSklade * U_BRZDY;
            naklady += PocetSvetielNaSklade * U_SVETLA;
        }
        
        // odberne mnozstva od klienta
        var odberTlmicov = _rndTlmice.Next(50, 100);
        var odberBrzd = _rndBrzdy.Next(60, 250);
        var odberSvetiel = _rndSvetlomety.Next();
        
        // kontrola pokut za nedostatocne zasoby
        if (odberTlmicov > PocetTlmicovNaSklade)
        {
            naklady += (odberTlmicov - PocetTlmicovNaSklade) * POKUTA;
            PocetTlmicovNaSklade = 0;
        }
        else PocetTlmicovNaSklade -= odberTlmicov;
        if (odberBrzd > PocetBrzdNaSklade)
        {
            naklady += (odberBrzd - PocetBrzdNaSklade) * POKUTA;
            PocetBrzdNaSklade = 0;
        }
        else PocetBrzdNaSklade -= odberBrzd;
        if (odberSvetiel > PocetSvetielNaSklade)
        {
            naklady += (odberSvetiel - PocetSvetielNaSklade) * POKUTA;
            PocetSvetielNaSklade = 0;
        }
        else PocetSvetielNaSklade -= odberSvetiel;
        
        // zvysny tovar na sklade este caka dalsie 3 dni do konca tyzdna... pi-so, so-ne, ne-po
        for (int i = 0; i < 3; i++)
        {
            naklady += PocetTlmicovNaSklade * U_TLMICE;
            naklady += PocetBrzdNaSklade * U_BRZDY;
            naklady += PocetSvetielNaSklade * U_SVETLA;
        }

        return naklady;
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