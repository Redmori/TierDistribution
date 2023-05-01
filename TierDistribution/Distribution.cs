using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierDistribution
{
    public class Distribution
    {
        public List<Raider> distr;
        public float baseValue;

        public Distribution() 
        {
            distr = new List<Raider>();
        }

        public Distribution(Distribution prevDistr)
        {
            distr = new List<Raider>(prevDistr.distr);
        }

        public void AddRaider(Raider raider)
        {
            distr.Add(raider);
        }

        public void DistributeLoot(List<Item> newLoot)
        {
            for (int i = 0; i < distr.Count; i++)
                distr[i].GiveItem(newLoot[i]);
        }
        public void Calculate(List<Raider> raiders, List<Item> newLoot)
        {
            DistributeLoot(newLoot);

            float sum = 0f;
            foreach (Raider raider in raiders) //TODO do i need to calculate for all raiders here? cant i just calculate for only the raiders that received items (in distr list)
            //foreach (Raider raider in distr.Distinct()) // IS SLOWER SOMEHOW? MAYBE THIS IS FASTER AT LARGER AMOUNT OF LOOT?
            {
                sum += raider.CalculatePower();
                raider.loot.Clear();
            }
            baseValue = sum;

            //TODO: implement omni token check here for this token group and return it

            //List<float> sums  = new List<float>();
            //sums.Add(sum);

            //int nOmni = 1;
            //while (nOmni <= numberOfOmni)
            //{
            //    sums.Add(0f);
            //    foreach (Raider raider in raiders)
            //    {
            //        if(raider.numberOfTierWithLoot == 2 - nOmni)
            //        {
            //            if (raider.tierValue[0] > sums[nOmni])
            //            {
            //                sums[nOmni] = raider.tierValue[0];
            //            }
            //        }
            //        if (raider.numberOfTierWithLoot == 4 - nOmni)
            //        {
            //            if (raider.tierValue[1] > sums[nOmni])
            //            {
            //                sums[nOmni] = raider.tierValue[1];
            //            }
            //        }
            //    }
            //    nOmni++;
            //}
            //TODO return sums;
        }
    }
}
