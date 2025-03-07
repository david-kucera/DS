using DSLib.MonteCarlo;

namespace MonteCarloLib
{
    public class MonteCarloLogic
    {
        #region Private members
        private Random _seeder;
        private Jan _janA = null;
        private double _janACurr = 0.0;
        private Jan _janB = null;
		private double _janBCurr = 0.0;
        private Jan _janC = null;
		private double _janCCurr = 0.0;
        private Jan _janD = null;
		private double _janDCurr = 0.0;
        private int _interval = 1000;
        private int _numberOfFinishedSimulations = 0;
		#endregion // Private members

		#region Events
		public event EventHandler<double> NewValueA = null;
        public event EventHandler<double> NewNakladyA = null;
		public event EventHandler<double> NewValueB = null;
		public event EventHandler<double> NewNakladyB = null;
		public event EventHandler<double> NewValueC = null;
        public event EventHandler<double> NewNakladyC = null;
		public event EventHandler<double> NewValueD = null;
        public event EventHandler<double> NewNakladyD = null;
        public event EventHandler SimulationEnded = null;
		#endregion // Events

		#region Constructor
		public MonteCarloLogic(int seed)
        {
            _seeder = new Random(seed);
            _janA = new Jan(JanStrategy.A, _seeder);
            _janA.SimulationStopped += OnSimulationStopped;
			_janB = new Jan(JanStrategy.B, _seeder);
			_janB.SimulationStopped += OnSimulationStopped;
			_janC = new Jan(JanStrategy.C, _seeder);
			_janC.SimulationStopped += OnSimulationStopped;
			_janD = new Jan(JanStrategy.D, _seeder);
			_janD.SimulationStopped += OnSimulationStopped;
		}

        private void OnSimulationStopped(object? sender, EventArgs e)
        {
	        _numberOfFinishedSimulations++;
			if (_numberOfFinishedSimulations == 4)
			{
				SimulationEnded?.Invoke(this, EventArgs.Empty);
				_numberOfFinishedSimulations = 0;
			}
		}

        #endregion // Constructor

        #region Public functions
        public void Start(int numberOfReplications, int throwInterval = 1000) 
        {
            _interval = throwInterval;

            if (numberOfReplications == 1)
            {
                _interval = 1;
	            Task.Run(() =>
	            {
					_janA.NewNaklady += OnNewNakladyA;
					_janA.Run(numberOfReplications);
	            });
	            Task.Run(() =>
	            {
					_janB.NewNaklady += OnNewNakladyB;
					_janB.Run(numberOfReplications);
	            });
	            Task.Run(() =>
	            {
					_janC.NewNaklady += OnNewNakladyC;
					_janC.Run(numberOfReplications);
	            });
	            Task.Run(() =>
	            {
		            _janD.NewNaklady += OnNewNakladyD;
		            _janD.Run(numberOfReplications);
	            });
			}
            else
            {
	            Task.Run(() =>
	            {
		            _janA.NewValue += OnNewValueA;
		            _janA.Run(numberOfReplications);
	            });
	            Task.Run(() =>
	            {
		            _janB.NewValue += OnNewValueB;
		            _janB.Run(numberOfReplications);
	            });
	            Task.Run(() =>
	            {
		            _janC.NewValue += OnNewValueC;
		            _janC.Run(numberOfReplications);
	            });
	            Task.Run(() =>
	            {
		            _janD.NewValue += OnNewValueD;
		            _janD.Run(numberOfReplications);
	            });
			}
        }

        public void Stop()
        {
            _janA.Stop();
            _janA.NewValue -= OnNewValueA;
            _janA.NewNaklady -= OnNewNakladyA;
			_janB.Stop();
            _janB.NewValue -= OnNewValueB;
			_janB.NewNaklady -= OnNewNakladyB;
			_janC.Stop();
            _janC.NewValue -= OnNewValueC;
			_janC.NewNaklady -= OnNewNakladyC;
			_janD.Stop();
            _janD.NewValue -= OnNewValueD;
			_janD.NewNaklady -= OnNewNakladyD;
		}
		#endregion // Public functions

		#region Private functions
		private void OnNewNakladyD(object? sender, double e)
		{
			_janDCurr += e;
			NewNakladyD?.Invoke(this, _janDCurr);
		}

		private void OnNewNakladyC(object? sender, double e)
		{
			_janCCurr += e;
			NewNakladyC?.Invoke(this, _janCCurr);
		}

		private void OnNewNakladyB(object? sender, double e)
		{
			_janBCurr += e;
			NewNakladyB?.Invoke(this, _janBCurr);
		}

		private void OnNewNakladyA(object? sender, double e)
        {
			_janACurr += e;
			NewNakladyA?.Invoke(this, _janACurr);
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
