using Simulation;

namespace NovinovyStanokTest;

class Program
{
    static void Main(string[] args)
    {
        MySimulation sim = new();
        sim.Simulate(10, 1_000_000);
    }
}