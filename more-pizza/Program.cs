using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace more_pizza
{
    class Program
    {
        static string[] inputFiles = {
            @"c:\temp\a_example.in",
            @"c:\temp\b_small.in",
            @"c:\temp\c_medium.in",
            @"c:\temp\d_quite_big.in",
            @"c:\temp\e_also_big.in"
        };

        static void Main(string[] args)
        {
            foreach (string fileName in inputFiles)
                Solve(fileName);
        }

        static void Solve(string fileName)
        {
            Problem p = Problem.loadProblem(fileName);
            Console.WriteLine("{0}, Max theoretical solution: {1}", fileName, p.M);

            // Step A: Greedy (add pizza with most slices first)
            List<Pizza> pizzaTypes = p.PizzaTypes.OrderByDescending(o => o.Slices).ToList();
            List<Pizza> solutionTypes = new List<Pizza>();
            BuildSolution(p.M, solutionTypes, 0, 0, pizzaTypes);
            int solutionSize = solutionTypes.Sum(o => o.Slices);

            // Step B: Try to replace a single slice
            bool improved = true;
            while (improved)
            {
                improved = false;
                for (int i = 0; i < solutionTypes.Count; i++)
                {
                    Pizza pizzaToReplace = solutionTypes[i];
                    solutionTypes.RemoveAt(i);

                    if (BuildSolution(p.M, solutionTypes, solutionSize - pizzaToReplace.Slices, solutionSize, pizzaTypes))
                    {
                        // Solution improved - restart
                        pizzaTypes.Add(pizzaToReplace);
                        solutionSize = solutionTypes.Sum(o => o.Slices);
                        improved = true;
                    }
                    else
                    {
                        // Restore & continue
                        solutionTypes.Insert(i, pizzaToReplace);
                    }
                }
            }

            // Step C: Try to replace 2 slices
            if (solutionSize < p.M)
            {
                improved = true;
                while (improved)
                {
                    improved = false;

                    for (int i = 0; i < solutionTypes.Count; i++)
                    {
                        for (int j = i + 1; j < solutionTypes.Count; j++)
                        {
                            Pizza pizzaToReplace1 = solutionTypes[i];
                            Pizza pizzaToReplace2 = solutionTypes[j];
                            solutionTypes.RemoveAt(j);
                            solutionTypes.RemoveAt(i);

                            if (BuildSolution(p.M, solutionTypes, solutionSize - pizzaToReplace1.Slices - pizzaToReplace2.Slices, solutionSize, pizzaTypes))
                            {
                                // Solution improved - restart
                                pizzaTypes.Add(pizzaToReplace1);
                                pizzaTypes.Add(pizzaToReplace2);
                                solutionSize = solutionTypes.Sum(o => o.Slices);
                                improved = true;
                            }
                            else
                            {
                                // Restore & continue
                                solutionTypes.Insert(i, pizzaToReplace1);
                                solutionTypes.Insert(j, pizzaToReplace2);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("{0}, Solution size: {1}", fileName, solutionSize);

            // Write output
            solutionTypes.OrderBy(o => o.ID);
            using (StreamWriter sw = new StreamWriter(fileName + ".out"))
            {
                sw.WriteLine(solutionTypes.Count);
                foreach (Pizza pizza in solutionTypes)
                {
                    sw.Write(pizza.ID);
                    sw.Write(' ');
                }
                sw.WriteLine();
            }
        }

        static bool BuildSolution(int m, List<Pizza> solution, int currentSolutionSize, int currentBest, List<Pizza> pizzaTypes)
        {
            int newSize = currentSolutionSize;
            List<Pizza> addedSlices = new List<Pizza>();

            for (int i = 0; i < pizzaTypes.Count; i++)
            {
                if (newSize + pizzaTypes[i].Slices <= m)
                {
                    newSize += pizzaTypes[i].Slices;

                    solution.Add(pizzaTypes[i]);
                    addedSlices.Add(pizzaTypes[i]);
                    pizzaTypes.RemoveAt(i);
                    i--;
                }
            }

            if (newSize > currentBest)
                return true;
            else
            {
                // Revert
                for (int i = 0; i < addedSlices.Count; i++)
                    solution.RemoveAt(solution.Count - 1);

                pizzaTypes.AddRange(addedSlices);

                return false;
            }
        }
    }
}
