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
    private JanStrategy _strategy;
    private Random _rnd;
    
    private Random _rndTlmice;
    private Random _rndBrzdy;
    private Random _rndSvetlomety;
    
    private Random _rndDodavatel1Prvych10;
    private Random _rndDodavatel1Od11;
    private Random _rndDodavatel2Prvych15;
    private Random _rndDodavatel2Od16;

    private int _pocetTlmicovNaSklade;
    private int _pocetBrzdNaSklade;
    private int _pocetSvetielNaSklade;
	#endregion // Class members

	#region Constructor
	public Jan(JanStrategy strat, Random rndTlmice, Random rndBrzdy, Random rndSvetlomety, Random rndDodavatel11, Random rndDodavatel12, Random rndDodavatel21, Random rndDodavatel22, Random rnd)
    {
	    _strategy = strat;
        
        _rnd = rnd;
        
        _rndTlmice = rndTlmice;
        _rndBrzdy = rndBrzdy;
        _rndSvetlomety = rndSvetlomety;
        
        _rndDodavatel1Prvych10 = rndDodavatel11;
        _rndDodavatel1Od11 = rndDodavatel12;
        _rndDodavatel2Prvych15 = rndDodavatel21;
        _rndDodavatel2Od16 = rndDodavatel22;
    }
    #endregion // Constructor
    
    #region Protected functions
    protected override double Experiment()
    {
        double result = 0.0;
        for (int i = 1; i <= POCET_TYZDNOV; i++)
        {
	        switch (_strategy)
	        {
                case JanStrategy.A:
	                result += StrategyA(i);
	                break;
                case JanStrategy.B:
	                result += StrategyB(i);
                    break;
                case JanStrategy.C:
	                result += StrategyC(i);
	                break;
                case JanStrategy.D:
	                result += StrategyD(i);
                    break;
                default:
	                throw new Exception("An unexpected error occured!");
			}
        }

        return result;
    }

    protected override void BeforeSimulation()
    {

    }

    protected override void AfterSimulation(double cumulativeResult)
    {

    }

    protected override void BeforeSimulationRun(int replication, double cumulativeResult)
    {
        _pocetTlmicovNaSklade = 0;
        _pocetBrzdNaSklade = 0;
        _pocetSvetielNaSklade = 0;
    }

    protected override void AfterSimulationRun(int replication, double cumulativeResult)
    {
	    _pocetTlmicovNaSklade = 0;
	    _pocetBrzdNaSklade = 0;
	    _pocetSvetielNaSklade = 0;
	}
    #endregion // Protected functions
    
    #region Private functions
    private double StrategyA(int tyzden)
    {
        var naklady = 0.0;
        
        double dovezeniePerc = 0.0;
        if (tyzden <= 10) dovezeniePerc = _rndDodavatel1Prvych10.Next(10, 70);
        else dovezeniePerc = _rndDodavatel1Od11.Next(30, 95);

        double perc = _rnd.NextDouble() * 100;
        if (dovezeniePerc >= perc) // doviezlo sa
        {
            _pocetTlmicovNaSklade += POCET_TLMICE;
            _pocetBrzdNaSklade += POCET_BRZDY;
            _pocetSvetielNaSklade += POCET_SVETLA;
        }
        
        // veci cakaju do piatku ... teda 4 dni po-ut, ut-st, st-stv, stv-pi
        for (int i = 0; i < 4; i++)
        {
            naklady += _pocetTlmicovNaSklade * U_TLMICE;
            naklady += _pocetBrzdNaSklade * U_BRZDY;
            naklady += _pocetSvetielNaSklade * U_SVETLA;
        }
        
        // odberne mnozstva od klienta
        var odberTlmicov = _rndTlmice.Next(50, 100);
        var odberBrzd = _rndBrzdy.Next(60, 250);
        var odberSvetiel = _rndSvetlomety.Next();
        
        // kontrola pokut za nedostatocne zasoby
        if (odberTlmicov > _pocetTlmicovNaSklade)
        {
            naklady += (odberTlmicov - _pocetTlmicovNaSklade) * POKUTA;
            _pocetTlmicovNaSklade = 0;
        }
        else _pocetTlmicovNaSklade -= odberTlmicov;
        if (odberBrzd > _pocetBrzdNaSklade)
        {
            naklady += (odberBrzd - _pocetBrzdNaSklade) * POKUTA;
            _pocetBrzdNaSklade = 0;
        }
        else _pocetBrzdNaSklade -= odberBrzd;
        if (odberSvetiel > _pocetSvetielNaSklade)
        {
            naklady += (odberSvetiel - _pocetSvetielNaSklade) * POKUTA;
            _pocetSvetielNaSklade = 0;
        }
        else _pocetSvetielNaSklade -= odberSvetiel;
        
        // zvysny tovar na sklade este caka dalsie 3 dni do konca tyzdna... pi-so, so-ne, ne-po
        for (int i = 0; i < 3; i++)
        {
            naklady += _pocetTlmicovNaSklade * U_TLMICE;
            naklady += _pocetBrzdNaSklade * U_BRZDY;
            naklady += _pocetSvetielNaSklade * U_SVETLA;
        }

        return naklady;
    }

    private double StrategyB(int tyzden)
    {
        var naklady = 0.0;
        
        double dovezeniePerc = 0.0;
        if (tyzden <= 15) dovezeniePerc = _rndDodavatel2Prvych15.NextDouble();
        else dovezeniePerc = _rndDodavatel2Od16.NextDouble();
        
        double perc = _rnd.NextDouble() * 100;
        if (dovezeniePerc >= perc) // doviezlo sa
        {
            _pocetTlmicovNaSklade += POCET_TLMICE;
            _pocetBrzdNaSklade += POCET_BRZDY;
            _pocetSvetielNaSklade += POCET_SVETLA;
        }
        
        // veci cakaju do piatku ... teda 4 dni po-ut, ut-st, st-stv, stv-pi
        for (int i = 0; i < 4; i++)
        {
            naklady += _pocetTlmicovNaSklade * U_TLMICE;
            naklady += _pocetBrzdNaSklade * U_BRZDY;
            naklady += _pocetSvetielNaSklade * U_SVETLA;
        }
        
        // odberne mnozstva od klienta
        var odberTlmicov = _rndTlmice.Next(50, 100);
        var odberBrzd = _rndBrzdy.Next(60, 250);
        var odberSvetiel = _rndSvetlomety.Next();
        
        // kontrola pokut za nedostatocne zasoby
        if (odberTlmicov > _pocetTlmicovNaSklade)
        {
            naklady += (odberTlmicov - _pocetTlmicovNaSklade) * POKUTA;
            _pocetTlmicovNaSklade = 0;
        }
        else _pocetTlmicovNaSklade -= odberTlmicov;
        if (odberBrzd > _pocetBrzdNaSklade)
        {
            naklady += (odberBrzd - _pocetBrzdNaSklade) * POKUTA;
            _pocetBrzdNaSklade = 0;
        }
        else _pocetBrzdNaSklade -= odberBrzd;
        if (odberSvetiel > _pocetSvetielNaSklade)
        {
            naklady += (odberSvetiel - _pocetSvetielNaSklade) * POKUTA;
            _pocetSvetielNaSklade = 0;
        }
        else _pocetSvetielNaSklade -= odberSvetiel;
        
        // zvysny tovar na sklade este caka dalsie 3 dni do konca tyzdna... pi-so, so-ne, ne-po
        for (int i = 0; i < 3; i++)
        {
            naklady += _pocetTlmicovNaSklade * U_TLMICE;
            naklady += _pocetBrzdNaSklade * U_BRZDY;
            naklady += _pocetSvetielNaSklade * U_SVETLA;
        }

        return naklady;
    }

    private double StrategyC(int tyzden)
    {
        throw new NotImplementedException();        
    }

    private double StrategyD(int tyzden)
    {
        throw new NotImplementedException();
    }
    #endregion // Private functions
}

public enum JanStrategy
{
    A,
    B,
    C,
    D
}