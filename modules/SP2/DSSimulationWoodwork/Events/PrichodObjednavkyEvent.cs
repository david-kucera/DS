using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class PrichodObjednavkyEvent : SimulationEvent
{
    #region Constructor
    public PrichodObjednavkyEvent(SimulationCore core, double time) : base(core, time)
    {
    }
    #endregion // Constructor

    #region Public functions
    public override void Execute()
    {
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();

        // vygenerovanie typu objednavky
        ObjednavkaType type;
        var perc = stolaren.ObjednavkaTypGenerator.NextDouble();
        if (perc < 0.5) type = ObjednavkaType.Stol;
        else if (perc < 0.85) type = ObjednavkaType.Skrina;
        else type = ObjednavkaType.Stolicka;
        
        // vytvorenie objednavky
        Objednavka objednavka = new(type, Time, stolaren.PoradieObjednavky++);
        stolaren.PriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.AddValue(stolaren.CakajuceNaRezanie.Count);
        
        // ak je nieco vo fronte, cakaj
        // ak je volny stolar skupiny A - naplanuj rezanie u neho
        if (stolaren.CakajuceNaRezanie.Count >= 1) stolaren.CakajuceNaRezanie.Enqueue(objednavka);
        else
        {
            Stolar stolar = null;
            foreach (var st in stolaren.Stolari)
            {
                if (st.Obsadeny || st.Type != StolarType.A) continue;
                stolar = st;
                break;
            }

            if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokRezaniaEvent(stolaren, Time, stolar, objednavka), Time);
            else stolaren.CakajuceNaRezanie.Enqueue(objednavka);
        }
        
        // naplanovanie dalsieho prichodu
        var dalsiPrichod = stolaren.PrichodObjednavokGenerator.NextDouble() + Time;
        if (dalsiPrichod < stolaren.STOP_TIME) stolaren.EventQueue.Enqueue(new PrichodObjednavkyEvent(stolaren, dalsiPrichod), dalsiPrichod);
    }
    #endregion // Public functions
}