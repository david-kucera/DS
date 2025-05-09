using Agents.AgentStolarov;
using DSAgentSimulationWoodwork.Entities;
using DSLib.Generators.Uniform;
using DSSimulationLib.Generators.Triangular;
using OSPABA;
using Simulation;
namespace Agents.AgentStolarov.ContinualAssistants
{
    //meta! id="107"
    public class ProcessKontrola : OSPABA.Process
    {
        #region Class members
        private TriangularGenerator _kontrolaGenerator;
        #endregion // Class members

        public ProcessKontrola(int id, OSPABA.Simulation mySim, CommonAgent myAgent) :
            base(id, mySim, myAgent)
        {
            var seeder = ((MySimulation)MySim).Seeder;
            _kontrolaGenerator = new(seeder, 10 * 60, 20 * 60, 18 * 60);
        }

        override public void PrepareReplication()
        {
            base.PrepareReplication();
            // Setup component for the next replication
        }

        //meta! sender="AgentStolarov", id="108", type="Start"
        public void ProcessStart(MessageForm message)
        {
            message.Code = Mc.Finish;
            var sprava = ((MyMessage)message);
            var tovar = sprava.Tovar;

            if (tovar.Status != TovarStatus.PriebehKontroly) throw new Exception("Neoèakávaná chyba: Tovar nie je v správnom procese!");

            double casSkladania = _kontrolaGenerator.NextDouble();

            Hold(casSkladania, message);
        }

        //meta! userInfo="Process messages defined in code", id="0"
        public void ProcessDefault(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.Finish:
                    AssistantFinished(message);
                    break;
            }
        }

        //meta! userInfo="Generated code: do not modify", tag="begin"
        override public void ProcessMessage(MessageForm message)
        {
            switch (message.Code)
            {
                case Mc.Start:
                    ProcessStart(message);
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