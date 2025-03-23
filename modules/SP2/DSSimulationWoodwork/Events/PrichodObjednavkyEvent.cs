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

        ObjType type = ObjType.Unknown;
        var perc = stolaren.ObjednavkaTypGenerator.NextDouble();
        if (perc < 0.5) type = ObjType.Stol;
        else if (perc < 0.85) type = ObjType.Skrina;
        else type = ObjType.Stolicka;
        
        Objednavka objednavka = new(type, Time, stolaren.PoradieObjednavky)
        {
            Status = ObjStatus.CakajucaNaRezanie
        };
        stolaren.PoradieObjednavky++;
        stolaren.PriemernyPocetObjednavokNaKtorychSaEsteNezacaloPracovat.AddValue(stolaren.NezacateObjednavkyQueue.Count);
        
        // ak je nieco vo fronte, cakaj
        // ak je volny stolar skupiny A - naplanuj rezanie u neho
        if (stolaren.NezacateObjednavkyQueue.Count >= 1) stolaren.NezacateObjednavkyQueue.Enqueue(objednavka);
        else
        {
            Stolar stolar = null;
            foreach (var st in stolaren.StolariA)
            {
                if (st.Obsadeny) continue;
                stolar = st;
                break;
            }

            if (stolar is not null) stolaren.EventQueue.Enqueue(new ZaciatokRezaniaEvent(stolaren, Time, stolar, objednavka), Time);
            else stolaren.NezacateObjednavkyQueue.Enqueue(objednavka);
        }
        
        var dalsiPrichod = stolaren.PrichodObjednavokGenerator.NextDouble() + Time;
        if (dalsiPrichod < stolaren.STOP_TIME) stolaren.EventQueue.Enqueue(new PrichodObjednavkyEvent(stolaren, dalsiPrichod), dalsiPrichod);
    }
    #endregion // Public functions
}