using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierDistribution
{
    public class Chromosome
    {
        public int[][] distr;
        public int fitness;

        public Chromosome(int[] nLoot)
        {
            distr = new int[5][];
            for (int i = 0; i < 5; i++)
            {
                distr[i] = new int[nLoot[i]];
            }
        }

        public Chromosome(int[] nToken, int[] nLoot)
        {
            distr = new int[5][];
            Random random = new Random();
            for(int i = 0; i < 5; i++)
            {
                distr[i] = new int[nLoot[i]];
                for (int j=0; j < nLoot[i]; j++)
                {
                    distr[i][j] = random.Next(nToken[i]);
                }
            }
        }

        public Chromosome Mutate(Chromosome ch2, int[] nToken, int[] nLoot)
        {
            Chromosome child = new Chromosome(nLoot);

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < nLoot[i]; j++)
                {
                    int r = random.Next(100);
                    if (r <= 45)
                        child.distr[i][j] = this.distr[i][j];
                    else if (r <= 90)
                        child.distr[i][j] = ch2.distr[i][j];
                    else //>90
                        child.distr[i][j] = new Random(r).Next(nToken[i]);
                }
            }


            return child;
        }

        public void CalcFitness()
        {
            int sum = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < distr[i].Length; j++)
                {
                    sum += distr[i][j];
                }
            }

            fitness = sum;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < distr.Length; i++)
            {
                for (int j = 0; j < distr[i].Length; j++)
                {
                    sb.Append(distr[i][j] + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
