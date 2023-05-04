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

        public Chromosome(int[] nToken, int[] nLoot)
        {
            distr = new int[5][];
            Random random = new Random();
            for(int i = 0; i < 4; i++)
            {
                distr[i] = new int[nLoot[i]];
                for (int j=0; j < nLoot[i]; j++)
                {
                    distr[i][j] = random.Next(nToken[i]);
                }
            }

            distr[4] = new int[nLoot[4]];
            for (int j = 0; j < nLoot[4]; j++)
            {
                distr[4][j] = random.Next(nToken[4]);
            }
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
