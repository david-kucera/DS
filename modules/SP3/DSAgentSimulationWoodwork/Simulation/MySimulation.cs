using Agents.AgentOkolia;
using Agents.AgentBStolar;
using Agents.AgentStolarov;
using Agents.AgentModelu;
using Agents.AgentMontaznychMiest;
using Agents.AgentAStolar;
using Agents.AgentCStolar;
using Agents.AgentStolarskejDielne;
using DSAgentSimulationLib.Statistics;
using DSAgentSimulationWoodwork.Entities;
using OSPAnimator;

namespace Simulation
{
	public class MySimulation : OSPABA.Simulation
	{
		#region Properties
		public Random? Seeder { get; set; } = null;
		public int PocetMontaznychMiest { get; set; } = 30;
		public int PocetStolarovA { get; set; } = 2;
		public int PocetStolarovB { get; set; } = 2;
		public int PocetStolarovC { get; set; } = 18;
		public ConfidenceInterval GlobalnyPriemernyCasObjednavkyVSysteme { get; set; }
		public ConfidenceInterval PriemernyCasObjednavkyVSysteme { get; set; }
		public ConfidenceInterval GlobalnyPriemernyPocetNezacatychObjednavok { get; set; }
		public ConfidenceInterval PriemernyPocetNezacatychObjednavok { get; set; }
		public ConfidenceInterval GlobalneVytazenieA { get; set; }
		public ConfidenceInterval GlobalneVytazenieB { get; set; }
		public ConfidenceInterval GlobalneVytazenieC { get; set; }
		public AnimTextItem AktualnyDenACasAnimItem { get; set; }
		public AnimTextItem AktualnaReplikaciaAnimItem { get; set; }
        #endregion // Properties

        public MySimulation(Random seeder, int pocetMiest, int pocetA, int pocetB, int pocetC)
		{
			Seeder = seeder;
			PocetMontaznychMiest = pocetMiest;
			PocetStolarovA = pocetA;
			PocetStolarovB = pocetB;
			PocetStolarovC = pocetC;
			Init();
		}

		override public void PrepareSimulation()
		{
			base.PrepareSimulation();
			// Create global statistcis
			
			GlobalnyPriemernyCasObjednavkyVSysteme = new ConfidenceInterval();
			GlobalnyPriemernyPocetNezacatychObjednavok = new ConfidenceInterval();
			GlobalneVytazenieA = new ConfidenceInterval();
			GlobalneVytazenieB = new ConfidenceInterval();
			GlobalneVytazenieC = new ConfidenceInterval();

			InitAnimator();
        }

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Reset entities, queues, local statistics, etc...

			PriemernyCasObjednavkyVSysteme = new ConfidenceInterval();
			PriemernyPocetNezacatychObjednavok = new ConfidenceInterval();
			
			Objednavka.ResetPoradie();
			Stolar.ResetPoradie();
			InitAnimator();
        }

		override public void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...
			base.ReplicationFinished();

			GlobalnyPriemernyCasObjednavkyVSysteme.AddValue(PriemernyCasObjednavkyVSysteme.GetValue());
			GlobalnyPriemernyPocetNezacatychObjednavok.AddValue(PriemernyPocetNezacatychObjednavok.GetValue());

			Average stolariAVytazenie = new();
			foreach (var stolar in AgentAStolar.StolariA)
			{
				stolariAVytazenie.AddValue(stolar.Workload.GetValue());
			}
			GlobalneVytazenieA.AddValue(stolariAVytazenie.GetValue());
			
			Average stolariBVytazenie = new();
			foreach (var stolar in AgentBStolar.StolariB)
			{
				stolariBVytazenie.AddValue(stolar.Workload.GetValue());
			}
			GlobalneVytazenieB.AddValue(stolariBVytazenie.GetValue());
			
			Average stolariCVytazenie = new();
			foreach (var stolar in AgentCStolar.StolariC)
			{
				stolariCVytazenie.AddValue(stolar.Workload.GetValue());
			}
			GlobalneVytazenieC.AddValue(stolariCVytazenie.GetValue());

            if (AnimatorExists)
                Animator.ClearAll();
        }

		override public void SimulationFinished()
		{
			// Display simulation results
			base.SimulationFinished();
		}

		public void Start(int repCount, double endTime)
		{
			Simulate(repCount, endTime);
		}

		public void Pause()
		{
			PauseSimulation();
		}

		public void Resume()
		{
			ResumeSimulation();
		}

		public void Stop()
		{
			StopSimulation();
		}

		public void InitAnimator()
		{
            AnimImageItem sklad = new AnimImageItem(Sklad.Image);
            sklad.SetPosition(Sklad.SKLAD_POS_X, Sklad.SKLAD_POS_Y);
            sklad.SetImageSize(Sklad.SKLAD_WIDTH, Sklad.SKLAD_HEIGHT);
			AktualnyDenACasAnimItem = new AnimTextItem(/*"Deň: 0 Čas: 06:00:00"*/"");
            AktualnyDenACasAnimItem.SetPosition(Sklad.SKLAD_POS_X, Sklad.SKLAD_POS_Y + sklad.Height);
			AktualnyDenACasAnimItem.ZIndex = 1;
            AktualnaReplikaciaAnimItem = new AnimTextItem(/*"Replikácia: 0"*/"");
            AktualnaReplikaciaAnimItem.SetPosition(Sklad.SKLAD_POS_X, Sklad.SKLAD_POS_Y + sklad.Height + 20);
            AktualnaReplikaciaAnimItem.ZIndex = 1;
			Flags.DEBUG_QUEUE = false;
			Flags.DEBUG_ANIM = false;
			if (AnimatorExists)
			{
				Animator.Register(sklad);
				Animator.Register(AktualnyDenACasAnimItem);
				Animator.Register(AktualnaReplikaciaAnimItem);
				AgentAStolar.InitAnimator();
				AgentBStolar.InitAnimator();
				AgentCStolar.InitAnimator();
				AgentMontaznychMiest.InitAnimator();
			}   
        }

        public void CheckNezacateObjednavky()
		{
			var tovaryCakajuceNaMiesto = AgentMontaznychMiest.NepriradeneTovary;
			var tovaryCakajucaNaStolaraA = AgentStolarov.CakajuceNaRezanie;

			var vsetkyNezacateTovary = tovaryCakajucaNaStolaraA
				.Concat(tovaryCakajuceNaMiesto)
				.ToList();

			int pocetNezacatychObjednavok = 0;
			List<Objednavka> objednavky = new();
			foreach (var tovar in vsetkyNezacateTovary)
			{
				if (!objednavky.Contains(tovar.Objednavka)) objednavky.Add(tovar.Objednavka);
			}

			foreach (var objednavka in objednavky)
			{
				if (!objednavka.Started) pocetNezacatychObjednavok++;
			}

			PriemernyPocetNezacatychObjednavok.AddValue(pocetNezacatychObjednavok);
		}

		public void CheckNezacateTovary()
		{
			// TODO
			throw new NotImplementedException("CheckNezacateTovary not implemented");
        }


        public void CheckNezacateObjednavkyModel()
		{
			var objednavky = AgentModelu.Objednavky;
			int pocetNezacatychObjednavok = 0;

			foreach (var objednavka in objednavky)
			{
				int pTovar = 0;
				foreach (var tovar in objednavka.Tovary)
				{
					if (tovar.Started) break;
					else pTovar++;
				}
				if (pTovar == objednavka.Tovary.Count) pocetNezacatychObjednavok++;
			}

			PriemernyPocetNezacatychObjednavok.AddValue(pocetNezacatychObjednavok);
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		private void Init()
		{
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentStolarskejDielne = new AgentStolarskejDielne(SimId.AgentStolarskejDielne, this, AgentModelu);
			AgentStolarov = new AgentStolarov(SimId.AgentStolarov, this, AgentStolarskejDielne);
			AgentMontaznychMiest = new AgentMontaznychMiest(SimId.AgentMontaznychMiest, this, AgentStolarskejDielne);
			AgentAStolar = new AgentAStolar(SimId.AgentAStolar, this, AgentStolarov);
			AgentBStolar = new AgentBStolar(SimId.AgentBStolar, this, AgentStolarov);
			AgentCStolar = new AgentCStolar(SimId.AgentCStolar, this, AgentStolarov);
		}
		public AgentModelu AgentModelu
		{ get; set; }
		public AgentOkolia AgentOkolia
		{ get; set; }
		public AgentStolarskejDielne AgentStolarskejDielne
		{ get; set; }
		public AgentStolarov AgentStolarov
		{ get; set; }
		public AgentMontaznychMiest AgentMontaznychMiest
		{ get; set; }
		public AgentAStolar AgentAStolar
		{ get; set; }
		public AgentBStolar AgentBStolar
		{ get; set; }
		public AgentCStolar AgentCStolar
		{ get; set; }
		//meta! tag="end"
	}
}