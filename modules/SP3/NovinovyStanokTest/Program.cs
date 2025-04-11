using Simulation;

namespace NovinovyStanokTest;

class Program
{
    static void Main(string[] args)
    {
        MySimulation sim = new();
        sim.Simulate(10, 10_000_000);
    }
}