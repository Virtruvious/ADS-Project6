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
                file.WriteLine("Dataset:");

                for (int i = 0; i < dataset.Count; i++)
                {
                    file.Write(dataset[i] + " ");
                }

                file.WriteLine();
                file.WriteLine();

                List<int> bestSolution = new();
                double bestFitness = double.MaxValue;
                int restartnum = 0;
                for (int i = 0; i < restarts; i++)
                {
                    HillClimbing copy = CopySolution();
                    Console.WriteLine("\nRestart " + (i + 1));
                    copy.RandomMutationHillClimbing(iterations, i+1);

                    if (copy.fitness < bestFitness)
                    {
                        bestSolution = copy.solution;
                        bestFitness = copy.fitness;
                        restartnum = i + 1;
                    }

                    // File Writing Results
                    file.Write("Solution "+(i+1)+": " + copy.fitness + " | ");
                    for (int j = 0; j < copy.solution.Count; j++)
                    {
                        file.Write(copy.solution[j] + " ");
                    }
                    file.Write("| ");
                    double truckA = 0, truckB = 0, truckC = 0;

                    for (int j = 0; j < copy.solution.Count; j++)
                    {
                        if (copy.solution[j] == 0) // Add onto TruckA
                        {
                            truckA += copy.dataset[j];
                        }
                        else if (copy.solution[j] == 1) // Add onto TruckB
                        {
                            truckB += copy.dataset[j];
                        }
                        else // Add onto TruckC
                        {
                            truckC += copy.dataset[j];
                        }
                    }
                    file.Write("Truck A: " + Math.Round(truckA, 2) + " Truck B: " + Math.Round(truckB, 2) + " Truck C: " + Math.Round(truckC, 2));
                    file.WriteLine();
                }

                // Prints the best solution found
                Console.WriteLine("\nBest Solution found on restart " + restartnum + ":");
                Console.WriteLine("Fitness: " + bestFitness);
                Console.Write("Solution: ");
                for (int i = 0; i < bestSolution.Count; i++)
                {
                    Console.Write(solution[i] + " ");
                }
                Console.WriteLine();
            }
        }

        public void RandomMutationHillClimbing(int iterations, int restartnum)
        {
            string path = "..\\..\\..\\RMHC Data\\" + restartnum + ".txt";
            using (System.IO.StreamWriter file = new(path, false))
            {
                for (int i = 0; i < iterations; i++)
                {
                    HillClimbing copy = CopySolution();
                    copy.SmallChange();
                    // DEBUG Prints, will slow down the program
                    //Console.Write("Current fitness: " + fitness + " New fitness: " + copy.fitness + " ");
                    //foreach (int value in copy.solution)
                    //{
                    //    Console.Write(value + " ");
                    //}
                    //Console.WriteLine();

                    // If the new solution is better, replace the old solution
                    if (copy.fitness < fitness)
                    {
                        //Console.WriteLine("New solution is better, replacing old solution");
                        solution = copy.solution;
                        fitness = copy.fitness;
                    }

                    // Log the best fitness per iteration so far, used for graphs
                    file.WriteLine(fitness);
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

            //Console.WriteLine("Truck A: " + truckA + " Truck B: " + truckB + " Truck C: " + truckC);

            // We want all of the trucks to be as close so we try to minimise the difference between the largest and smallest truck
            double largest = Math.Max(Math.Max(truckA, truckB), truckC);
            double smallest = Math.Min(Math.Min(truckA, truckB), truckC);
            //Console.WriteLine("Largest: " + largest + " Smallest: " + smallest);
            double differential = largest - smallest;

            fitness = Math.Round(differential, 2);
        }

        public HillClimbing CopySolution()
        {
            HillClimbing copy = new HillClimbing
            {
                dataset = new List<double>(this.dataset),
                solution = new List<int>(this.solution),
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
