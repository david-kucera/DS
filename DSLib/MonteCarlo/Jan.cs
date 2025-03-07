using DSLib.Generators.Empirical;
using DSLib.Generators.Uniform;

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

    private const string STRATEGY1_PATH = "../data/strat1.txt";
    private const string STRATEGY2_PATH = "../data/strat2.txt";
    private const string STRATEGY3_PATH = "../data/strat3.txt";
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

    #region Events
    public event EventHandler<(int, double)> NewValue = null;
    public event EventHandler<double> NewNaklady = null;
    public event EventHandler SimulationStopped = null;

    private void OnSimulationStopped()
	{
		SimulationStopped?.Invoke(this, EventArgs.Empty);
	}

	private void OnNewValue(int i, double cumulativeResult)
    {
        NewValue?.Invoke(this, (i, cumulativeResult));
    }

    private void OnNewNaklady(double cumulativeNaklady)
    {
        NewNaklady?.Invoke(this, cumulativeNaklady);
    }
    #endregion // Events

    #region Constructors
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

    public Jan(JanStrategy strategy, Random seeder)
    {
        _strategy = strategy;
        _rndTlmice = new DiscreteUniform(seeder, 50, 100 + 1);
        _rndBrzdy = new DiscreteUniform(seeder, 60, 250 + 1);
        List<(int, int)> intervals =
        [
            (30, 60), (60, 100), (100, 140), (140, 160)
        ];
        List<double> probabilities =
        [
            0.2, 0.4, 0.3, 0.1
        ];
        _rndSvetlomety = new DiscreteEmpirical(seeder, intervals, probabilities);

        _rndDodavatel1Prvych10 = new ContinousUniform(seeder, 10, 70);
        _rndDodavatel1Od11 = new ContinousUniform(seeder, 30, 95);
        List<(int, int)> intervals2 =
        [
            (5, 10), (10, 50), (50, 70), (70, 80), (80, 95)
        ];
        List<double> probabilities2 =
        [
            0.4, 0.3, 0.2, 0.06, 0.04
        ];
        _rndDodavatel2Prvych15 = new ContinousEmpirical(seeder, intervals2, probabilities2);
        List<double> probabilities3 =
        [
            0.2, 0.4, 0.3, 0.06, 0.04
        ];
        _rndDodavatel2Od16 = new ContinousEmpirical(seeder, intervals2, probabilities3);

        _rnd = new Random(seeder.Next());
    }
    #endregion // Constructors
    
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
                case JanStrategy.Own1:
                    result += StrategyOwn1(i);
                    break;
                case JanStrategy.Own2:
                    result += StrategyOwn2(i);
                    break;
                case JanStrategy.Own3:
                    result += StrategyOwn3(i);
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
        OnSimulationStopped();
    }

    protected override void BeforeSimulationRun(int replication, double cumulativeResult)
    {
        _pocetTlmicovNaSklade = 0;
        _pocetBrzdNaSklade = 0;
        _pocetSvetielNaSklade = 0;
    }

    protected override void AfterSimulationRun(int replication, double cumulativeResult)
    {
        OnNewValue(replication, cumulativeResult);
    }
    #endregion // Protected functions
    
    #region Private functions
    private double StrategyA(int tyzden)
    {
        double dovezeniePerc = 0.0;
        if (tyzden <= 10) dovezeniePerc = _rndDodavatel1Prvych10.NextDouble();
        else dovezeniePerc = _rndDodavatel1Od11.NextDouble();
        double perc = _rnd.NextDouble() * 100;
        
        return Strategy(dovezeniePerc, perc);
    }

    private double StrategyB(int tyzden)
    {
        double dovezeniePerc = 0.0;
        if (tyzden <= 15) dovezeniePerc = _rndDodavatel2Prvych15.NextDouble();
        else dovezeniePerc = _rndDodavatel2Od16.NextDouble();
        double perc = _rnd.NextDouble() * 100;
        
        return Strategy(dovezeniePerc, perc);
    }

    private double StrategyC(int tyzden)
    {
        double dovezeniePerc = 0.0;
        if (tyzden % 2 != 0) // dodavatel 1
        {
            if (tyzden <= 10) dovezeniePerc = _rndDodavatel1Prvych10.NextDouble();
            else dovezeniePerc = _rndDodavatel1Od11.NextDouble();
        }
        else // dodavatel 2
        {
            if (tyzden <= 15) dovezeniePerc = _rndDodavatel2Prvych15.NextDouble();
            else dovezeniePerc = _rndDodavatel2Od16.NextDouble();
        }
        double perc = _rnd.NextDouble() * 100;
        
        return Strategy(dovezeniePerc, perc);
    }

    private double StrategyD(int tyzden)
    {
        double dovezeniePerc = 0.0;
        if (tyzden % 2 == 0) // dodavatel 1
        {
            if (tyzden <= 10) dovezeniePerc = _rndDodavatel1Prvych10.NextDouble();
            else dovezeniePerc = _rndDodavatel1Od11.NextDouble();
        }
        else // dodavatel 2
        {
            if (tyzden <= 15) dovezeniePerc = _rndDodavatel2Prvych15.NextDouble();
            else dovezeniePerc = _rndDodavatel2Od16.NextDouble();
        }
        double perc = _rnd.NextDouble() * 100;
        
        return Strategy(dovezeniePerc, perc);
    }

    private double StrategyOwn1(int tyzden)
    {
        throw new NotImplementedException();
    }

    private double StrategyOwn2(int tyzden)
    {
        throw new NotImplementedException();
    }

    private double StrategyOwn3(int tyzden)
    {
        throw new NotImplementedException();
    }

    private double Strategy(double dovezeniePerc, double perc)
    {
        double naklady = 0.0;
        if (dovezeniePerc >= perc) // doviezlo sa
        {
            _pocetTlmicovNaSklade += POCET_TLMICE;
            _pocetBrzdNaSklade += POCET_BRZDY;
            _pocetSvetielNaSklade += POCET_SVETLA;
        }
        
        // veci cakaju do piatku ... teda 4 dni po-ut, ut-st, st-stv, stv-pi
        for (int i = 0; i < 4; i++)
        {
            double nakladden = 0.0;
            naklady += _pocetTlmicovNaSklade * U_TLMICE;
            nakladden += _pocetTlmicovNaSklade * U_TLMICE;
            naklady += _pocetBrzdNaSklade * U_BRZDY;
            nakladden += _pocetBrzdNaSklade * U_BRZDY;
            naklady += _pocetSvetielNaSklade * U_SVETLA;
            nakladden += _pocetSvetielNaSklade * U_SVETLA;
            OnNewNaklady(nakladden);
        }
        
        // odberne mnozstva od klienta
        var odberTlmicov = _rndTlmice.Next();
        var odberBrzd = _rndBrzdy.Next();
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
            double nakladden = 0.0;
            naklady += _pocetTlmicovNaSklade * U_TLMICE;
            nakladden += _pocetTlmicovNaSklade * U_TLMICE;
            naklady += _pocetBrzdNaSklade * U_BRZDY;
            nakladden += _pocetBrzdNaSklade * U_BRZDY;
            naklady += _pocetSvetielNaSklade * U_SVETLA;
            nakladden += _pocetSvetielNaSklade * U_SVETLA;
            OnNewNaklady(nakladden);
        }

        return naklady;
    }
    #endregion // Private functions
}

public enum JanStrategy
{
    A,
    B,
    C,
    D,
    Own1,
    Own2,
    Own3
}