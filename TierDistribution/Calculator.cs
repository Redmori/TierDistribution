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
        public static int nTokens = 4;

        public static float highest = 0f;
        public static List<Raider> bestDistr;

        public static int numberOfOmni = 2;

        public static List<Distribution>[] GiveItems(List<Raider>[] raiders, List<Item>[] newLoot)
        {
            Console.Write("Generating loot distributions");
            List<Distribution>[] distributions = new List<Distribution>[nTokens];
            for (int i = 0; i < nTokens; i++)
            {
                distributions[i] = GiveItems(raiders, newLoot, i);
                Console.Write(" " + i);
                //bestDistr = new List<Raider>();
                //highest = 0f;
                //GiveItems(raiders, newLoot, i);
                //maxDistr[i] = bestDistr;
            }
            Console.WriteLine();
            return distributions;
        }
        public static List<Distribution> GiveItems(List<Distribution> distrList, List<Raider> raiders, List<Item> newLoot,int index, Distribution distr)
        {
            foreach(Raider raider in raiders)
            {
                if(index == 0)
                    //Console.Write(Math.Round((float)raiders.IndexOf(raider)/(float)raiders.Count*100) + "%");
                    Console.Write(".");
                //if (index == 1)
                //    Console.Write(".");

                if (index < newLoot.Count)
                {
                    if (!IsUpgrade(raider, newLoot, newLoot[index], distr.distr)) continue;
                    Distribution newDistr = new Distribution(distr);
                    newDistr.AddRaider(raider);
                    GiveItems(distrList, raiders, newLoot, index + 1, newDistr);
                }
                else
                {
                    distrList.Add(distr);
                }
            }
            return distrList;
        }        
        public static List<Distribution> GiveItems(List<Raider>[] raiders, List<Item>[] newLoot, int i)
        {
            List<Distribution> distrList = new List<Distribution>();
            Distribution newDistr = new Distribution();
            return GiveItems(distrList, raiders[i], newLoot[i], 0, newDistr);
        }

        public static void CalculateDistributions(List<Raider>[] raid, List<Item>[] newLoot, List<Distribution>[] distributions)
        {
            Console.Write("Calculating distributions values");
            for (int i = 0; i < nTokens; i++)
            {
                Console.Write(" - " + distributions[i].Count());
                CalculateDistributions(raid[i], newLoot[i], distributions[i]);
            }
            Console.WriteLine();
        }

        public static void CalculateDistributions(List<Raider> raiders, List<Item> newLoot, List<Distribution> distributions)
        {
            foreach(Distribution distr in distributions)
                distr.Calculate(raiders, newLoot);
        }

        public static Distribution[] PickBest(List<Distribution>[] distributions)
        {
            return distributions
                .Select(list => list.OrderByDescending(d => d.baseValue).First())
                .ToArray();
        }


        public static void DistributeLoot(List<Item>[] newLoot, Distribution[] distr)
        {
            for (int i = 0; i < nTokens; i++)
                distr[i].DistributeLoot(newLoot[i]);
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

        public static string DistributionToString(List<Item>[] newLoot, Distribution[] distr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < distr.Length; i++)
            {
                sb.AppendLine(DistributionToString(newLoot[i], distr[i]));
            }
            return sb.ToString();

        }
        public static string DistributionToString(List<Item> newLoot, Distribution distr) //MOVE TO DISTRIBUTION CLASS?
        {
            //return String.Join(", ", distr.Select(raider => raider.name));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < distr.distr.Count; i++)
            {
                sb.AppendFormat("{0} => {1}\n", newLoot[i].slot, distr.distr[i].name);
            }
            return sb.ToString();
        }
    }



}
