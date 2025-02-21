namespace DS;

class Program
{
    static void Main()
    {
        const int pocetRep = 1_000_000;
        const double L = 5.0;
        const double D = 10.0;
        Random rndY = new Random();
        Random rndA = new Random();

        BuffonNeedle buffonNeedle = new BuffonNeedle(rndY, rndA, D, L);
        var output = buffonNeedle.Run(pocetRep);
        var piEval = (2 * L)/ (D * output);
        Console.WriteLine($"PI: {piEval}");
    }
}