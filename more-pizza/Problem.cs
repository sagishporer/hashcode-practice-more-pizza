using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace more_pizza
{
    class Problem
    {
        public int M { get { return mM; } }
        public List<Pizza> PizzaTypes { get { return mPizzaTypes; } }

        int mM;
        List<Pizza> mPizzaTypes;

        private Problem(int m, List<Pizza> pizzaTypes)
        {
            mM = m;
            mPizzaTypes = pizzaTypes;
        }

        public static Problem loadProblem(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                line = sr.ReadLine();
                string[] parts = line.Split(' ');
                int m = int.Parse(parts[0]);
                int n = int.Parse(parts[1]);

                line = sr.ReadLine();
                parts = line.Split(' ');

                List<Pizza> pizzaTypes = new List<Pizza>();
                for (int i = 0; i < n; i++)
                    pizzaTypes.Add(new Pizza(i, int.Parse(parts[i])));

                return new Problem(m, pizzaTypes);
            }
        }
    }
}
