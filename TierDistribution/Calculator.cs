using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TierDistribution
{
    public static class Calculator
    {
        //public static List<Raider> raiders = new List<Raider>();
        //public static List<Item> loot = new List<Item>();

        public static float highest = 0f;
        public static List<Raider> bestDistr;

        public static int numberOfOmni = 2;

        public static List<Raider>[] GiveItems(List<Raider>[] raiders, List<Item>[] newLoot)
        {
            List<Raider>[] maxDistr = new List<Raider>[raiders.Length];
            for (int i = 0; i < raiders.Length; i++)
            {
                bestDistr = new List<Raider>();
                highest = 0f;
                GiveItems(raiders, newLoot, i);
                maxDistr[i] = bestDistr;
            }
            return maxDistr;
        }
        public static void GiveItems(List<Raider> raiders, List<Item> newLoot,int index, List<Raider> distr)
        {
            foreach(Raider raider in raiders)
            {
                if(index == 0)
                    Console.Write(Math.Round((float)raiders.IndexOf(raider)/(float)raiders.Count*100) + "%");
                if (index == 1)
                    Console.Write(".");
                //Console.WriteLine(raider.name + " - index: " + index);
                //raider.GiveItem(loot[index]);
                //distr.Add(raider);

                if (index < newLoot.Count)
                {
                    if (!IsUpgrade(raider, newLoot, newLoot[index], distr)) continue;
                    List<Raider> newList = new List<Raider>(distr);
                    newList.Add(raider);
                    //foreach(Raider rd in distr)
                    //    newList.Add(rd);
                    GiveItems(raiders, newLoot, index + 1, newList);
                }
                else
                {
                    float calculated = Calculate(raiders, newLoot, distr);
                    if(calculated > highest)
                    {
                        bestDistr = new List<Raider>(distr);
                        highest = calculated;
                    }
                    //Console.WriteLine("Setup with power: " + calculated);
                }
            }            
        }        
        public static void GiveItems(List<Raider>[] raiders, List<Item>[] newLoot, int i)
        {
            List<Raider> distributionList = new List<Raider>();
            GiveItems(raiders[i], newLoot[i], 0, distributionList);
        }

        public static float Calculate(List<Raider> raiders, List<Item> newLoot, List<Raider> distr)
        {
            //Console.WriteLine("test: " + distr.Count);  
            DistributeLoot(newLoot, distr);

            float sum = 0f;
            foreach(Raider raider in raiders)
            {
                sum += raider.CalculatePower();
                raider.loot.Clear();
            }

            //TODO: implement omni token check here for this token group and return it

            List<float> sums  = new List<float>();
            sums.Add(sum);

            int nOmni = 1;
            while (nOmni <= numberOfOmni)
            {
                sums.Add(0f);
                foreach (Raider raider in raiders)
                {
                    if(raider.numberOfTierWithLoot == 2 - nOmni)
                    {
                        if (raider.tierValue[0] > sums[nOmni])
                        {
                            sums[nOmni] = raider.tierValue[0];
                        }
                    }
                    if (raider.numberOfTierWithLoot == 4 - nOmni)
                    {
                        if (raider.tierValue[1] > sums[nOmni])
                        {
                            sums[nOmni] = raider.tierValue[1];
                        }
                    }
                }
                nOmni++;
            }
            //TODO return sums;
            return sum;
        }

        public static void DistributeLoot(List<Item> newLoot, List<Raider> distr)
        {
            for (int i = 0; i < distr.Count; i++)
                distr[i].GiveItem(newLoot[i]);
        }

        public static void DistributeLoot(List<Item>[] newLoot, List<Raider>[] distr)
        {
            for (int i = 0; i < distr.Length; i++)
                DistributeLoot(newLoot[i], distr[i]);
        }

        public static bool IsUpgrade(Raider raider, List<Item> newLoot, Item item, List<Raider> distr)
        {
            if ((int)item.status <= (int)raider.gear[(int)item.slot])
                return false;

            int index = distr.IndexOf(raider, 0);
            while (index != -1)
            {
                if (newLoot[index].slot == item.slot && (int)newLoot[index].status >= (int)item.status)
                    return false;
                index = distr.IndexOf(raider, index + 1); // search for next occurrence starting from previous index                            
            }
            return true;
        }

        public static string DistributionToString(List<Item>[] newLoot, List<Raider>[] distr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < distr.Length; i++)
            {
                sb.AppendLine(DistributionToString(newLoot[i], distr[i]));
            }
            return sb.ToString();

        }
        public static string DistributionToString(List<Item> newLoot, List<Raider> distr)
        {
            //return String.Join(", ", distr.Select(raider => raider.name));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < distr.Count; i++)
            {
                sb.AppendFormat("{0} => {1}\n", newLoot[i].slot, distr[i].name);
            }
            return sb.ToString();
        }
    }



}
