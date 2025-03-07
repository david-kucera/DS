using DSLib.MonteCarlo;

namespace MonteCarloLib
{
    public class MonteCarloLogic
    {
		#region Constants
		public enum Strategies
		{
			Classic,
			Own
		}
		#endregion // Constants

		#region Private members
		private readonly Jan _janA;
        private double _janACurr;
        private readonly Jan _janB;
		private double _janBCurr;
        private readonly Jan _janC;
		private double _janCCurr;
        private readonly Jan _janD;
		private double _janDCurr;
        private int _interval = 1000;
        private int _numberOfFinishedSimulations;
		#endregion // Private members

		#region Events
		public event EventHandler<double> NewValueA = null!;
        public event EventHandler<double> NewNakladyA = null!;
		public event EventHandler<double> NewValueB = null!;
		public event EventHandler<double> NewNakladyB = null!;
		public event EventHandler<double> NewValueC = null!;
        public event EventHandler<double> NewNakladyC = null!;
		public event EventHandler<double> NewValueD = null!;
        public event EventHandler<double> NewNakladyD = null!;
        public event EventHandler SimulationEnded = null!;
		#endregion // Events

		#region Constructor
		public MonteCarloLogic(int seed, Strategies strategies)
        {
	        var seeder = new Random(seed);
	        if (strategies == Strategies.Classic)
	        {
		        _janA = new Jan(JanStrategy.A, seeder);
		        _janB = new Jan(JanStrategy.B, seeder);
		        _janC = new Jan(JanStrategy.C, seeder);
		        _janD = new Jan(JanStrategy.D, seeder);
			}
	        else
	        {
		        _janA = new Jan(JanStrategy.Own1, seeder);
		        _janB = new Jan(JanStrategy.Own2, seeder);
		        _janC = new Jan(JanStrategy.Own3, seeder);
		        _janD = new Jan(JanStrategy.Own4, seeder);
			}
	        _janA.NewSimulationStopped += OnSimulationStopped;
	        _janB.NewSimulationStopped += OnSimulationStopped;
	        _janC.NewSimulationStopped += OnSimulationStopped;
	        _janD.NewSimulationStopped += OnSimulationStopped;
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
		private void OnSimulationStopped(object? sender, EventArgs e)
		{
			_numberOfFinishedSimulations++;
			if (_numberOfFinishedSimulations == 4)
			{
				SimulationEnded?.Invoke(this, EventArgs.Empty);
				_numberOfFinishedSimulations = 0;
			}
		}

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
	        if (e.Item1 % _interval != 0) return;
	        double value = e.Item2 / e.Item1;
	        NewValueD?.Invoke(this, value);
        }

        private void OnNewValueC(object? sender, (int, double) e)
        {
	        if (e.Item1 % _interval != 0) return;
	        double value = e.Item2 / e.Item1;
	        NewValueC?.Invoke(this, value);
        }

        private void OnNewValueB(object? sender, (int, double) e)
        {
	        if (e.Item1 % _interval != 0) return;
	        double value = e.Item2 / e.Item1;
	        NewValueB?.Invoke(this, value);
        }

        private void OnNewValueA(object? sender, (int, double) e)
        {
	        if (e.Item1 % _interval != 0) return;
	        double value = e.Item2 / e.Item1;
	        NewValueA?.Invoke(this, value);
        }
        #endregion // Private functions
    }
}
