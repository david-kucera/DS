﻿using Agents.AgentOkolia;
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
		public ConfidenceInterval GlobalnyPriemernyCasObjednavkyVSysteme { get; set; } = null!;
		public ConfidenceInterval PriemernyCasObjednavkyVSysteme { get; set; } = null!;
		public ConfidenceInterval GlobalnyPriemernyPocetNezacatychObjednavok { get; set; } = null!;
		public ConfidenceInterval PriemernyPocetNezacatychObjednavok { get; set; } = null!;
        public ConfidenceInterval GlobalnyPriemernyPocetNezacatychTovarov { get; set; } = null!;
        public ConfidenceInterval PriemernyPocetNezacatychTovarov { get; set; } = null!;
        public ConfidenceInterval GlobalneVytazenieA { get; set; } = null!;
		public ConfidenceInterval GlobalneVytazenieB { get; set; } = null!;
		public ConfidenceInterval GlobalneVytazenieC { get; set; } = null!;
		public AnimTextItem AktualnyDenACasAnimItem { get; set; } = null!;
		public AnimTextItem AktualnaReplikaciaAnimItem { get; set; } = null!;
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
			// Create global statistcis
			
			GlobalnyPriemernyCasObjednavkyVSysteme = new ConfidenceInterval();
			GlobalnyPriemernyPocetNezacatychObjednavok = new ConfidenceInterval();
            GlobalnyPriemernyPocetNezacatychTovarov = new ConfidenceInterval();
            GlobalneVytazenieA = new ConfidenceInterval();
			GlobalneVytazenieB = new ConfidenceInterval();
			GlobalneVytazenieC = new ConfidenceInterval();

			InitAnimator();

            base.PrepareSimulation();
        }

		override public void PrepareReplication()
		{
			// Reset entities, queues, local statistics, etc...

			PriemernyCasObjednavkyVSysteme = new ConfidenceInterval();
			PriemernyPocetNezacatychObjednavok = new ConfidenceInterval();
            PriemernyPocetNezacatychTovarov = new ConfidenceInterval();

            Objednavka.ResetPoradie();
			Stolar.ResetPoradie();
			InitAnimator();

            base.PrepareReplication();
        }

		override public void ReplicationFinished()
		{
			// Collect local statistics into global, update UI, etc...

			GlobalnyPriemernyCasObjednavkyVSysteme.AddValue(PriemernyCasObjednavkyVSysteme.GetValue());
			GlobalnyPriemernyPocetNezacatychObjednavok.AddValue(PriemernyPocetNezacatychObjednavok.GetValue());
			GlobalnyPriemernyPocetNezacatychTovarov.AddValue(PriemernyPocetNezacatychTovarov.GetValue());

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

            base.ReplicationFinished();
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

            var copyA = new PriorityQueue<Tovar, (double, double)>(AgentStolarov.CakajuceNaRezanie.UnorderedItems);
            var copyB = new Queue<Tovar>(AgentMontaznychMiest.NepriradeneTovary);

            var vsetkyNezacateTovary = new List<Tovar>();

            while (copyA.Count > 0)
                vsetkyNezacateTovary.Add(copyA.Dequeue());

            while (copyB.Count > 0)
                vsetkyNezacateTovary.Add(copyB.Dequeue());


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
            var tovaryCakajuceNaMiesto = AgentMontaznychMiest.NepriradeneTovary;
            var tovaryCakajucaNaStolaraA = AgentStolarov.CakajuceNaRezanie;

			PriemernyPocetNezacatychTovarov.AddValue(tovaryCakajuceNaMiesto.Count + tovaryCakajucaNaStolaraA.Count);
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
			AgentModelu = new AgentModelu(SimId.AgentModelu, this, null!);
			AgentOkolia = new AgentOkolia(SimId.AgentOkolia, this, AgentModelu);
			AgentStolarskejDielne = new AgentStolarskejDielne(SimId.AgentStolarskejDielne, this, AgentModelu);
			AgentStolarov = new AgentStolarov(SimId.AgentStolarov, this, AgentStolarskejDielne);
			AgentMontaznychMiest = new AgentMontaznychMiest(SimId.AgentMontaznychMiest, this, AgentStolarskejDielne);
			AgentAStolar = new AgentAStolar(SimId.AgentAStolar, this, AgentStolarov);
			AgentBStolar = new AgentBStolar(SimId.AgentBStolar, this, AgentStolarov);
			AgentCStolar = new AgentCStolar(SimId.AgentCStolar, this, AgentStolarov);
		}
		public AgentModelu AgentModelu
		{ get; set; } = null!;
        public AgentOkolia AgentOkolia
		{ get; set; } = null!;
        public AgentStolarskejDielne AgentStolarskejDielne
		{ get; set; } = null!;
        public AgentStolarov AgentStolarov
		{ get; set; } = null!;
        public AgentMontaznychMiest AgentMontaznychMiest
		{ get; set; } = null!;
        public AgentAStolar AgentAStolar
		{ get; set; } = null!;
		public AgentBStolar AgentBStolar
		{ get; set; } = null!;
        public AgentCStolar AgentCStolar
		{ get; set; } = null!;
        //meta! tag="end"
    }
}