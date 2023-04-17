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
        public static List<Raider> raiders = new List<Raider>();
        public static List<Item> loot = new List<Item>();

        public static float highest = 0f;
        public static List<Raider> bestDistr;

        public static void GiveItems(int index, List<Raider> distr)
        {
            foreach(Raider raider in raiders)
            {
                if(index == 0)
                    Console.WriteLine(Math.Round((float)raiders.IndexOf(raider)/(float)raiders.Count*100) + "%");
                if (index == 1)
                    Console.WriteLine("...");
                //Console.WriteLine(raider.name + " - index: " + index);
                //raider.GiveItem(loot[index]);
                //distr.Add(raider);

                if (index < loot.Count)
                {
                    if (!IsUpgrade(raider, loot[index], distr)) continue;
                    List<Raider> newList = new List<Raider>(distr);
                    newList.Add(raider);
                    //foreach(Raider rd in distr)
                    //    newList.Add(rd);
                    GiveItems(index + 1, newList);
                }
                else
                {
                    float calculated = Calculate(distr);
                    if(calculated > highest)
                    {
                        bestDistr = new List<Raider>(distr);
                        highest = calculated;
                    }
                    //Console.WriteLine("Setup with power: " + calculated);
                }
            }            
        }        
        public static void GiveItems()
        {
            List<Raider> distributionList = new List<Raider>();
            GiveItems(0, distributionList);
        }

        public static float Calculate(List<Raider> distr)
        {
            //Console.WriteLine("test: " + distr.Count);  
            for(int i = 0; i < distr.Count; i++)
                distr[i].GiveItem(loot[i]);


            float sum = 0f;
            foreach(Raider raider in raiders)
            {
                sum += raider.CalculatePower();
                raider.loot.Clear();
            }
            return sum;
        }

        public static bool IsUpgrade(Raider raider, Item item, List<Raider> distr)
        {
            if ((int)item.status <= (int)raider.gear[(int)item.slot])
                return false;

            int index = distr.IndexOf(raider, 0);
            while (index != -1)
            {
                if (loot[index].slot == item.slot && (int)loot[index].status >= (int)item.status)
                    return false;
                index = distr.IndexOf(raider, index + 1); // search for next occurrence starting from previous index                            
            }
            return true;
        }

        public static string DistributionToString(List<Raider> distr)
        {
            //return String.Join(", ", distr.Select(raider => raider.name));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < distr.Count; i++)
            {
                sb.AppendFormat("{0} => {1}\n", loot[i].slot, distr[i].name);
            }
            return sb.ToString();
        }
    }



}
