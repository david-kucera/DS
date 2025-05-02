using DSAgentSimulationWoodwork.Entities;
using DSLib.Generators.Uniform;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov
{
	//meta! id="7"
	public class ManagerStolarov : OSPABA.Manager
	{
        #region Constants
		private int POSUN_X = 50;
		private int POSUN_Y_MIN = -10;
        private int POSUN_Y_MAX = 10;
        #endregion // Constants

        #region Class members
        private ContinousUniform _ajNaLakovanieGenerator;
		private Random _randomPosunGenerator;
		#endregion // Class members

		public ManagerStolarov(int id, OSPABA.Simulation mySim, Agent myAgent) :
			base(id, mySim, myAgent)
		{
			var seeder = ((MySimulation)MySim).Seeder;
			_ajNaLakovanieGenerator = new ContinousUniform(seeder, 0, 1);
            _randomPosunGenerator = new Random(seeder.Next());
            Init();
		}

		override public void PrepareReplication()
		{
			base.PrepareReplication();
			// Setup component for the next replication

			if (PetriNet != null)
			{
				PetriNet.Clear();
			}
		}

		//meta! sender="AgentStolarskejDielne", id="55", type="Notice"
		public void ProcessZacniPracu(MessageForm message)
		{
			message.Addressee = MySim.FindAgent(SimId.AgentAStolar);
			message.Code = Mc.DajStolaraA;
			Request(message);
		}

		//meta! sender="AgentAStolar", id="57", type="Response"
		public void ProcessDajStolaraA(MessageForm message)
		{
			var msg = ((MyMessage)message);
            if (msg.Stolar != null)
			{
				if (msg.Stolar.Type != StolarType.A) throw new Exception("Nesprávny typ stolára!");
				msg.Tovar.Objednavka.Started = true;
				msg.Tovar.Started = true;
                ((MySimulation)MySim).CheckNezacateObjednavky();
                ((MySimulation)MySim).CheckNezacateTovary();
                message.Addressee = MyAgent.FindAssistant(SimId.ProcessPresun);
                StartContinualAssistant(message);
			}
			else
			{
                if (msg.Tovar.Type == TovarType.Skrina && msg.Tovar.Status == TovarStatus.CakajucaNaMontazKovani) 
				{
                    msg.Code = Mc.DajStolaraC;
                    msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
					Request(msg);    
				}
				else
				{
					MyAgent.CakajuceNaRezanie.Enqueue(msg.Tovar, (msg.Tovar.Objednavka.ArrivalTime, msg.Tovar.LastTechnicalTime));
                    ((MySimulation)MySim).CheckNezacateObjednavky();
                    ((MySimulation)MySim).CheckNezacateTovary();
                }
			}
		}

		//meta! sender="AgentCStolar", id="61", type="Response"
		public void ProcessDajStolaraC(MessageForm message)
		{
			var msg = ((MyMessage)message);
			if (msg.Stolar != null)
			{
				if (msg.Stolar.Type != StolarType.C) throw new Exception("Nesprávny typ stolára!");
				message.Addressee = MyAgent.FindAssistant(SimId.ProcessPresun);
				StartContinualAssistant(message);
			}
			else
			{
				if (msg.Tovar.Type == TovarType.Skrina && msg.Tovar.Status == TovarStatus.CakajucaNaMontazKovani)
				{
                    MyAgent.CakajuceNaMontazKovani.Enqueue(msg.Tovar, (msg.Tovar.Objednavka.ArrivalTime, msg.Tovar.LastTechnicalTime));
                }
				else MyAgent.CakajuceNaMorenie.Enqueue(msg.Tovar, (msg.Tovar.Objednavka.ArrivalTime, msg.Tovar.LastTechnicalTime));
			}
		}

		//meta! sender="AgentBStolar", id="60", type="Response"
		public void ProcessDajStolaraB(MessageForm message)
		{
			var msg = ((MyMessage)message);
			if (msg.Stolar != null)
			{
				if (msg.Stolar.Type != StolarType.B) throw new Exception("Nesprávny typ stolára!");
				message.Addressee = MyAgent.FindAssistant(SimId.ProcessPresun);
				StartContinualAssistant(message);
			}
			else MyAgent.CakajuceNaSkladanie.Enqueue(msg.Tovar, (msg.Tovar.Objednavka.ArrivalTime, msg.Tovar.LastTechnicalTime));
		}

		//meta! userInfo="Process messages defined in code", id="0"
		public void ProcessDefault(MessageForm message)
		{
			switch (message.Code)
			{
			}
		}

		//meta! sender="ProcessRezanie", id="102", type="Finish"
		public void ProcessFinishProcessRezanie(MessageForm message)
		{
			var sprava = ((MyMessage)message);
			sprava.Stolar.Workload.AddValue(sprava.Stolar.Obsadeny, MySim.CurrentTime);
			sprava.Stolar.AnimImageItem.Move(MySim.CurrentTime, 0, _randomPosunGenerator.Next(POSUN_X), _randomPosunGenerator.Next(POSUN_Y_MIN, POSUN_Y_MAX));
			sprava.Stolar.Obsadeny = false;
			sprava.Stolar = null;
			
			// Pokracovanie objednavky na morenie
			sprava.Tovar.LastTechnicalTime = MySim.CurrentTime;
            sprava.Tovar.ChangeStatus(TovarStatus.CakajucaNaMorenie);

            message.Addressee = MySim.FindAgent(SimId.AgentCStolar);
			message.Code = Mc.DajStolaraC;
			Request(message);

			// Naplanovanie montaze kovani alebo rezania
			if (MyAgent.CakajuceNaMontazKovani.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaMontazKovani.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaMontazKovani) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentAStolar);
				msg.Code = Mc.DajStolaraA;
				Request(msg);
			}
			else if (MyAgent.CakajuceNaRezanie.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaRezanie.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaRezanie) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentAStolar);
				msg.Code = Mc.DajStolaraA;
				Request(msg);
			}
		}

		//meta! sender="ProcessMontazKovani", id="110", type="Finish"
		public void ProcessFinishProcessMontazKovani(MessageForm message)
		{
			var sprava = ((MyMessage)message);
			sprava.Stolar.Workload.AddValue(sprava.Stolar.Obsadeny, MySim.CurrentTime);
            sprava.Stolar.AnimImageItem.Move(MySim.CurrentTime, 0, _randomPosunGenerator.Next(POSUN_X), _randomPosunGenerator.Next(POSUN_Y_MIN, POSUN_Y_MAX));
            sprava.Stolar.Obsadeny = false;
			var typStolara = sprava.Stolar.Type;
			sprava.Stolar = null;

            sprava.Tovar.LastTechnicalTime = MySim.CurrentTime;
            sprava.Tovar.ChangeStatus(TovarStatus.Hotova);
            
            message.Code = Mc.PracaHotova;
			message.Addressee = MySim.FindAgent(SimId.AgentStolarskejDielne);
			Notice(message);

			if (typStolara == StolarType.C)
			{
				// Naplanovanie dalsej aktivity pre stolara typu C - prednost ma montaz kovani, potom bud lakovanie alebo morenie
				if (MyAgent.CakajuceNaMontazKovani.Count > 0)
				{
					var tovar = MyAgent.CakajuceNaMontazKovani.Dequeue();
					var msg = new MyMessage(MySim)
					{
						Tovar = tovar,
						MontazneMiesto = tovar.MontazneMiesto,
					};
					if (msg.Tovar.Status != TovarStatus.CakajucaNaMontazKovani) throw new Exception("Nesprávny status tovaru!");

					msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
					msg.Code = Mc.DajStolaraC;
					Request(msg);
				}
				else if (MyAgent.CakajuceNaLakovanie.Count > 0)
				{
					var tovar = MyAgent.CakajuceNaLakovanie.Dequeue();
					var msg = new MyMessage(MySim)
					{
						Tovar = tovar,
						MontazneMiesto = tovar.MontazneMiesto,
					};
					if (msg.Tovar.Status != TovarStatus.CakajucaNaLakovanie) throw new Exception("Nesprávny status tovaru!");

					msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
					msg.Code = Mc.DajStolaraC;
					Request(msg);
				}
				else if (MyAgent.CakajuceNaMorenie.Count > 0)
				{
					var tovar = MyAgent.CakajuceNaMorenie.Dequeue();
					var msg = new MyMessage(MySim)
					{
						Tovar = tovar,
						MontazneMiesto = tovar.MontazneMiesto,
					};
					if (msg.Tovar.Status != TovarStatus.CakajucaNaMorenie) throw new Exception("Nesprávny status tovaru!");

					msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
					msg.Code = Mc.DajStolaraC;
					Request(msg);
				}
			}
			else if (typStolara == StolarType.A)
			{
				// Naplanovanie montaze kovani alebo rezania
				if (MyAgent.CakajuceNaMontazKovani.Count > 0)
				{
					var tovar = MyAgent.CakajuceNaMontazKovani.Dequeue();
					var msg = new MyMessage(MySim)
					{
						Tovar = tovar,
						MontazneMiesto = tovar.MontazneMiesto,
					};
					if (msg.Tovar.Status != TovarStatus.CakajucaNaMontazKovani) throw new Exception("Nesprávny status tovaru!");

					msg.Addressee = MySim.FindAgent(SimId.AgentAStolar);
					msg.Code = Mc.DajStolaraA;
					Request(msg);
				}
				else if (MyAgent.CakajuceNaRezanie.Count > 0)
				{
					var tovar = MyAgent.CakajuceNaRezanie.Dequeue();
					var msg = new MyMessage(MySim)
					{
						Tovar = tovar,
						MontazneMiesto = tovar.MontazneMiesto,
					};
					if (msg.Tovar.Status != TovarStatus.CakajucaNaRezanie) throw new Exception("Nesprávny status tovaru!");

					msg.Addressee = MySim.FindAgent(SimId.AgentAStolar);
					msg.Code = Mc.DajStolaraA;
					Request(msg);
				}
			}
			else throw new Exception("Neoèakávaná chyba pri poslednom technologickom procese!");
		}

		//meta! sender="ProcessMorenie", id="104", type="Finish"
		public void ProcessFinishProcessMorenie(MessageForm message)
		{
			var sprava = ((MyMessage)message);
			var lakovaniePerc = _ajNaLakovanieGenerator.NextDouble();
			if (lakovaniePerc < 0.15)
			{
                sprava.Tovar.LastTechnicalTime = MySim.CurrentTime;
                sprava.Tovar.ChangeStatus(TovarStatus.PriebehLakovania);
                message.Addressee = MyAgent.FindAssistant(SimId.ProcessLakovanie);
				StartContinualAssistant(message);
				return;
			}

			sprava.Stolar.Workload.AddValue(sprava.Stolar.Obsadeny, MySim.CurrentTime);
            sprava.Stolar.AnimImageItem.Move(MySim.CurrentTime, 0, _randomPosunGenerator.Next(POSUN_X), _randomPosunGenerator.Next(POSUN_Y_MIN, POSUN_Y_MAX));
            sprava.Stolar.Obsadeny = false;
			sprava.Stolar = null;

            // Ak sa nelakuje, tovar pokracuje dalej na skladanie
            sprava.Tovar.LastTechnicalTime = MySim.CurrentTime;
            sprava.Tovar.ChangeStatus(TovarStatus.CakajucaNaSkladanie);

            message.Addressee = MySim.FindAgent(SimId.AgentBStolar);
			message.Code = Mc.DajStolaraB;
			Request(message);

			// Naplanovanie dalsej aktivity pre stolara typu C - prednost ma montaz kovani, potom bud lakovanie alebo morenie
			if (MyAgent.CakajuceNaMontazKovani.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaMontazKovani.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaMontazKovani) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
				msg.Code = Mc.DajStolaraC;
				Request(msg);
			}
			else if (MyAgent.CakajuceNaLakovanie.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaLakovanie.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaLakovanie) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
				msg.Code = Mc.DajStolaraC;
				Request(msg);
			}
			else if (MyAgent.CakajuceNaMorenie.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaMorenie.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaMorenie) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
				msg.Code = Mc.DajStolaraC;
				Request(msg);
			}
		}

		//meta! sender="ProcessPresun", id="100", type="Finish"
		public void ProcessFinishProcessPresun(MessageForm message)
		{
			var msg = ((MyMessage)message);
			msg.Stolar.MontazneMiesto = msg.Tovar.MontazneMiesto;
			msg.Stolar.MontazneMiesto.Stolar = msg.Stolar;

			switch (msg.Tovar.Status)
			{
				case TovarStatus.CakajucaNaRezanie:
                    msg.Tovar.ChangeStatus(TovarStatus.PriebehRezania);
                    message.Addressee = MyAgent.FindAssistant(SimId.ProcessRezanie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaMorenie:
                    msg.Tovar.ChangeStatus(TovarStatus.PriebehMorenia);
                    message.Addressee = MyAgent.FindAssistant(SimId.ProcessMorenie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaLakovanie:
                    msg.Tovar.ChangeStatus(TovarStatus.PriebehLakovania);
                    message.Addressee = MyAgent.FindAssistant(SimId.ProcessLakovanie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaSkladanie:
                    msg.Tovar.ChangeStatus(TovarStatus.PriebehSkladania);
                    message.Addressee = MyAgent.FindAssistant(SimId.ProcessSkladanie);
					StartContinualAssistant(message);
					break;
				case TovarStatus.CakajucaNaMontazKovani:
                    msg.Tovar.ChangeStatus(TovarStatus.PriebehMontazeKovani);
                    message.Addressee = MyAgent.FindAssistant(SimId.ProcessMontazKovani);
					StartContinualAssistant(message);
					break;
			}
		}

		//meta! sender="ProcessLakovanie", id="106", type="Finish"
		public void ProcessFinishProcessLakovanie(MessageForm message)
		{
			var sprava = ((MyMessage)message);
			sprava.Stolar.Workload.AddValue(sprava.Stolar.Obsadeny, MySim.CurrentTime);
            sprava.Stolar.AnimImageItem.Move(MySim.CurrentTime, 0, _randomPosunGenerator.Next(POSUN_X), _randomPosunGenerator.Next(POSUN_Y_MIN, POSUN_Y_MAX));
            sprava.Stolar.Obsadeny = false;
			sprava.Stolar = null;

            // Tovar pokracuje dalej na skladanie
            sprava.Tovar.LastTechnicalTime = MySim.CurrentTime;
            sprava.Tovar.ChangeStatus(TovarStatus.CakajucaNaSkladanie);
            message.Addressee = MySim.FindAgent(SimId.AgentBStolar);
            message.Code = Mc.DajStolaraB;
            Request(message);

            // Naplanovanie dalsej aktivity pre stolara typu C - prednost ma montaz kovani, potom bud lakovanie alebo morenie
            if (MyAgent.CakajuceNaMontazKovani.Count > 0)
            {
				var tovar = MyAgent.CakajuceNaMontazKovani.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaMontazKovani) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
				msg.Code = Mc.DajStolaraC;
				Request(msg);
			}
			else if (MyAgent.CakajuceNaLakovanie.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaLakovanie.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaLakovanie) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
				msg.Code = Mc.DajStolaraC;
				Request(msg);
			}
			else if (MyAgent.CakajuceNaMorenie.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaMorenie.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaMorenie) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentCStolar);
				msg.Code = Mc.DajStolaraC;
				Request(msg);
			}
		}

		//meta! sender="ProcessSkladanie", id="108", type="Finish"
		public void ProcessFinishProcessSkladanie(MessageForm message)
		{
			var sprava = ((MyMessage)message);
			sprava.Stolar.Workload.AddValue(sprava.Stolar.Obsadeny, MySim.CurrentTime);
            sprava.Stolar.AnimImageItem.Move(MySim.CurrentTime, 0, _randomPosunGenerator.Next(POSUN_X), _randomPosunGenerator.Next(POSUN_Y_MIN, POSUN_Y_MAX));
            sprava.Stolar.Obsadeny = false;
			sprava.Stolar = null;

			if (sprava.Tovar.Type == TovarType.Skrina)
			{
                sprava.Tovar.LastTechnicalTime = MySim.CurrentTime;
                sprava.Tovar.ChangeStatus(TovarStatus.CakajucaNaMontazKovani);
                message.Addressee = MySim.FindAgent(SimId.AgentAStolar);
				message.Code = Mc.DajStolaraA;
				Request(message);
			}
			else
			{
                // tovar hotovy, notifikuj agenta modelu
                sprava.Tovar.LastTechnicalTime = MySim.CurrentTime;
                sprava.Tovar.ChangeStatus(TovarStatus.Hotova);
                var msg = message.CreateCopy();
				msg.Code = Mc.PracaHotova;
				msg.Addressee = MySim.FindAgent(SimId.AgentStolarskejDielne);
				Notice(msg);
			}

			// Naplanovanie dalsieho skladania
			if (MyAgent.CakajuceNaSkladanie.Count > 0)
			{
				var tovar = MyAgent.CakajuceNaSkladanie.Dequeue();
				var msg = new MyMessage(MySim)
				{
					Tovar = tovar,
					MontazneMiesto = tovar.MontazneMiesto,
				};
				if (msg.Tovar.Status != TovarStatus.CakajucaNaSkladanie) throw new Exception("Nesprávny status tovaru!");

				msg.Addressee = MySim.FindAgent(SimId.AgentBStolar);
				msg.Code = Mc.DajStolaraB;
				Request(msg);
			}
		}

		//meta! userInfo="Generated code: do not modify", tag="begin"
		public void Init()
		{
		}

		override public void ProcessMessage(MessageForm message)
		{
			switch (message.Code)
			{
			case Mc.ZacniPracu:
				ProcessZacniPracu(message);
			break;

			case Mc.DajStolaraA:
				ProcessDajStolaraA(message);
			break;

			case Mc.Finish:
				switch (message.Sender.Id)
				{
				case SimId.ProcessMorenie:
					ProcessFinishProcessMorenie(message);
				break;

				case SimId.ProcessPresun:
					ProcessFinishProcessPresun(message);
				break;

				case SimId.ProcessSkladanie:
					ProcessFinishProcessSkladanie(message);
				break;

				case SimId.ProcessMontazKovani:
					ProcessFinishProcessMontazKovani(message);
				break;

				case SimId.ProcessRezanie:
					ProcessFinishProcessRezanie(message);
				break;

				case SimId.ProcessLakovanie:
					ProcessFinishProcessLakovanie(message);
				break;
				}
			break;

			case Mc.DajStolaraB:
				ProcessDajStolaraB(message);
			break;

			case Mc.DajStolaraC:
				ProcessDajStolaraC(message);
			break;

			default:
				ProcessDefault(message);
			break;
			}
		}
		//meta! tag="end"
		public new AgentStolarov MyAgent
		{
			get
			{
				return (AgentStolarov)base.MyAgent;
			}
		}
	}
}