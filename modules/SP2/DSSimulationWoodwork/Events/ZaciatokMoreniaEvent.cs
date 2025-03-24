using DSSimulationLib.Simulation;

namespace DSSimulationWoodwork.Events;

public class ZaciatokMoreniaEvent : SimulationEvent
{
    #region Class members
    private Objednavka _objednavka;
    private Stolar _stolar;
    #endregion // Class members
    
    #region Constructor
    public ZaciatokMoreniaEvent(SimulationCore core, double time, Objednavka objednavka, Stolar stolar) : base(core, time)
    {
        _objednavka = objednavka;
        _stolar = stolar;
    }
    #endregion // Constructor

    public override void Execute()
    {
        Stolaren stolaren = Core as Stolaren ?? throw new InvalidOperationException();
        _stolar.Obsadeny = true;
        
        if (_stolar.Type != StolarType.C) throw new Exception("Nesprávny typ stolára!");

        double casPrechoduNaMontazneMiesto;
        if (_stolar.MontazneMiesto == null) 
            casPrechoduNaMontazneMiesto = stolaren.MontazneMiestoSkladGenerator.NextDouble();
        else if (_stolar.MontazneMiesto != _objednavka.MontazneMiesto)
            casPrechoduNaMontazneMiesto = stolaren.PresunMedziMontaznymiMiestamiGenerator.NextDouble();
        else casPrechoduNaMontazneMiesto = 0.0;
        _stolar.MontazneMiesto = _objednavka.MontazneMiesto;
         
        double casMorenia = 0.0;
        switch (_objednavka.Type)
        {
            case ObjType.Stol:
                casMorenia = stolaren.StolMorenieLakovanieGenerator.NextDouble();
                break;
            case ObjType.Skrina:
                casMorenia = stolaren.SkrinaMorenieLakovanieGenerator.NextDouble();
                break;
            case ObjType.Stolicka:
                casMorenia = stolaren.StolickaMorenieLakovanieGenerator.NextDouble();
                break;
            default:
                throw new Exception("Nie je uvedený typ objednávky!");
        }
        _objednavka.Status = ObjStatus.Namorena;
        
        double trvanieUdalosti =  casPrechoduNaMontazneMiesto + casMorenia;
        double koniecUdalosti = trvanieUdalosti + Time;

        stolaren.EventQueue.Enqueue(new KoniecMoreniaEvent(stolaren, koniecUdalosti, _objednavka, _stolar), koniecUdalosti);
    }
}