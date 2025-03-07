using DSLib.MonteCarlo;

namespace MonteCarloLib
{
    public class MonteCarloLogic
    {
        #region Private members
        private Random _seeder;
        private Jan _janA;
        private Jan _janB;
        private Jan _janC;
        private Jan _janD;
        private int _interval = 1000;
        #endregion // Private members

        #region Events
        public event EventHandler<double> NewValueA = null;
        public event EventHandler<double> NewValueB = null;
        public event EventHandler<double> NewValueC = null;
        public event EventHandler<double> NewValueD = null;
        public event EventHandler<double> NewNaklady = null;
        #endregion // Events

        #region Constructor
        public MonteCarloLogic(int seed)
        {
            _seeder = new Random(seed);
            _janA = new Jan(JanStrategy.A, _seeder);
            _janA.NewValue += OnNewValueA;
            _janB = new Jan(JanStrategy.B, _seeder);
            _janB.NewValue += OnNewValueB;
            _janC = new Jan(JanStrategy.C, _seeder);
            _janC.NewValue += OnNewValueC;
            _janD = new Jan(JanStrategy.D, _seeder);
            _janD.NewValue += OnNewValueD;
        }
        #endregion // Constructor

        #region Public functions
        public void Start(int numberOfReplications, int throwInterval = 1000) 
        {
            _interval = throwInterval;
            Task.Run(() => 
            {
                _janA.Run(numberOfReplications);
            });
            Task.Run(() =>
            {
                _janB.Run(numberOfReplications);
            });
            Task.Run(() =>
            {
                _janC.Run(numberOfReplications);
            });
            Task.Run(() =>
            {
                _janD.Run(numberOfReplications);
            });
        }

        public void StartOne(JanStrategy strategy)
        {
            switch(strategy)
            {
                case JanStrategy.A:
                    Task.Run(() =>
                    {
                        _janA.NewNaklady += OnNewNaklady;
                        _janA.Run(1);
                    });
                    break;
                case JanStrategy.B:
                    Task.Run(() =>
                    {
                        _janB.NewNaklady += OnNewNaklady;
                        _janB.Run(1);
                    });
                    break;
                 case JanStrategy.C:
                    Task.Run(() =>
                    {
                        _janC.NewNaklady += OnNewNaklady;
                        _janC.Run(1);
                    });
                    break;
                case JanStrategy.D:
                    Task.Run(() =>
                    {
                        _janD.NewNaklady += OnNewNaklady;
                        _janD.Run(1);
                    });
                    break;
                 default:
                    throw new Exception("An unexpected error occured!");
            }
            
        }

        public void Stop()
        {
            _janA.Stop();
            _janA.NewValue -= OnNewValueA;
            _janB.Stop();
            _janB.NewValue -= OnNewValueB;
            _janC.Stop();
            _janC.NewValue -= OnNewValueC;
            _janD.Stop();
            _janD.NewValue -= OnNewValueD;
        }
        #endregion // Public functions

        #region Private functions
        private void OnNewNaklady(object? sender, double e)
        {
            NewNaklady?.Invoke(this, e);
        }

        private void OnNewValueD(object? sender, (int, double) e)
        {
            if (e.Item1 % _interval == 0)
            {
                double value = e.Item2 / e.Item1;
                NewValueD?.Invoke(this, value);
            }
        }

        private void OnNewValueC(object? sender, (int, double) e)
        {
            if (e.Item1 % _interval == 0)
            {
                double value = e.Item2 / e.Item1;
                NewValueC?.Invoke(this, value);
            }
        }

        private void OnNewValueB(object? sender, (int, double) e)
        {
            if (e.Item1 % _interval == 0)
            {
                double value = e.Item2 / e.Item1;
                NewValueB?.Invoke(this, value);
            }
        }

        private void OnNewValueA(object? sender, (int, double) e)
        {
            if (e.Item1 % _interval == 0)
            {
                double value = e.Item2 / e.Item1;
                NewValueA?.Invoke(this, value);
            }
        }
        #endregion // Private functions
    }
}
