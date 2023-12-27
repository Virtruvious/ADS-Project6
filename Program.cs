namespace Project5
{
    class Program
    {
        static void Main(string[] args)
        {
            HillClimbing hillClimbing = new(GenerateRandomDataset(20));
            hillClimbing.RandomRestartHillClimbing(5, 100);
        }

        static List<double> GenerateRandomDataset(int size)
        {
            Random random = new();
            List<double> dataset = new List<double>();

            for (int i = 0; i < size; i++)
            {
                // Generates 'size' random decimal numbers between 1 and 25
                dataset.Add((double)random.Next(1000, 25000) / 1000);
            }

            return dataset;
        }
    }
}