namespace Project5
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> dataset = new();

            // Used when testing with a custom dataset
            //dataset = GenerateRandomDataset(20);

            // Used when testing with the provided dataset
            using (System.IO.StreamReader file = new("..\\..\\..\\dataset1.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    dataset.Add(double.Parse(line));
                }
            }

            HillClimbing hillClimbing = new(dataset);
            hillClimbing.RandomRestartHillClimbing(10, 1000);
        }

        static List<double> GenerateRandomDataset(int size)
        {
            Random random = new();
            List<double> dataset = new List<double>();

            for (int i = 0; i < size; i++)
            {
                // Generates 'size' random decimal numbers between 1 and 100
                dataset.Add((double)random.Next(1000, 100000) / 1000);
            }

            return dataset;
        }
    }
}