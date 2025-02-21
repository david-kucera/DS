namespace DS;

class Program
{
    static void Main()
    {
        const int pocetRep = 1_000_000;
        Random rnd = new Random();
        double kumVysledok = 0.0;

        const double L = 5.0;
        const double D = 10.0;
        int pocetPretnuti = 0;
        
        for (int i = 1; i <= pocetRep; i++)
        {
            double y = rnd.NextDouble() * D;
            double alpha = rnd.NextDouble() * Math.PI;
            double a = L * Math.Sin(alpha);
            
            if (y + a >= D) pocetPretnuti++;
        }
        kumVysledok += pocetPretnuti / (double)pocetRep;
        Console.WriteLine($"PI: {(2.0 * L)/(D * kumVysledok)}");
    }
}