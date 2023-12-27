using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    internal class HillClimbing
    {
        public List<int> solution;
        public double fitness;
        public List<double> dataset = new();

        public HillClimbing (List<double> dataset)       
        {
            // Constructs a random solution
            Random random = new();
            this.dataset = dataset;
            solution = new List<int>();

            for (int i = 0; i < dataset.Count; i++)
            {
                solution.Add(random.Next(0, 3));
            }
            CalculateCurrentFitness();
        }

        public HillClimbing()
        {
            solution = new List<int>();
        }

        public void RandomRestartHillClimbing(int restarts, int iterations)
        {
            using (System.IO.StreamWriter file = new("..\\..\\..\\output.txt", false))
            {
                file.WriteLine("Random Restart Hill Climbing");

                for (int i = 0; i < restarts; i++)
                {
                    HillClimbing copy = CopySolution();
                    Console.WriteLine("\nRestart " + (i + 1));
                    copy.RandomMutationHillClimbing(iterations);

                    file.WriteLine("Solution "+(i+1)+": " + copy.fitness);
                }
            }
        }

        public void RandomMutationHillClimbing(int iterations)
        {
            Console.WriteLine("Random Mutation Hill Climbing");
            for (int i = 0; i < iterations; i++)
            {
                HillClimbing copy = CopySolution();
                copy.SmallChange();
                Console.WriteLine("Current fitness: " + fitness + " New fitness: " + copy.fitness);
                if (copy.fitness < fitness)
                {
                    solution = copy.solution;
                    fitness = copy.fitness;
                }
            }
        }

        public void CalculateCurrentFitness()
        {
            fitness = 0;
            double truckA = 0, truckB = 0, truckC = 0;
            for (int i = 0; i < solution.Count; i++)
            {
                if (solution[i] == 0) // Add onto TruckA
                {
                    truckA += dataset[i];
                }
                else if (solution[i] == 1) // Add onto TruckB
                {
                    truckB += dataset[i];
                }
                else  // Add onto TruckC
                {
                    truckC += dataset[i];
                }
            }

            Console.WriteLine("Truck A: " + truckA + " Truck B: " + truckB + " Truck C: " + truckC);

            // We want all of the trucks to be as close so we try to minimise the difference between the largest and smallest truck
            double largest = Math.Max(Math.Max(truckA, truckB), truckC);
            double smallest = Math.Min(Math.Min(truckA, truckB), truckC);
            Console.WriteLine("Largest: " + largest + " Smallest: " + smallest);
            double differential = largest - smallest;

            fitness = Math.Round(differential, 2);
        }

        public HillClimbing CopySolution()
        {
            HillClimbing copy = new HillClimbing
            {
                dataset = this.dataset,
                solution = this.solution,
                fitness = this.fitness,
            };

            return copy;
        }

        public void SmallChange()
        {
            // Randomly selects two indexes to swap
            Random random = new();
            int firstIndex = random.Next(0, solution.Count);
            int secondIndex = random.Next(0, solution.Count);

            while (firstIndex == secondIndex) // Saves swapping the same values which would do nothing
            {
                secondIndex = random.Next(0, solution.Count);
            }

            //Console.WriteLine("Swapping " + firstIndex + " and " + secondIndex);

            // Uses tuple to swap the values, more efficient than using a temp variable
            (solution[firstIndex], solution[secondIndex]) = (solution[secondIndex], solution[firstIndex]);
            CalculateCurrentFitness();
        }
    }
}
