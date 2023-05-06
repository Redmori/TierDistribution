using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TierDistribution
{    
    public enum Status
    {
        Empty,
        LFR,
        Normal,
        Heroic,
        Mythic,
    }

    public enum Class
    {
        Warrior,
        Paladin,
        Hunter,
        Rogue,
        Priest,
        Shaman,
        Mage,
        Warlock,
        Monk,
        Druid,
        DemonHunter,
        DeathKnight,
        Evoker
    }

    public enum Role
    {
        Tank,
        Healer,
        Damage
    }
    public class Raider
    {
        public string name;
        public Class clas;
        public Role role;

        public Status[] gear;
        public float baseFitness;
        public Status[] newGear;
        public int nOmni; //Sak omni tokens
        public bool hasOmni; //AotC omni
        public bool usedOmni = false;

        public int numberOfTier;
        public int numberOfTierWithLoot;

        public float[] tierValue;

        public List<Item> loot = new List<Item>();

        public Raider(string name_, Class class_, Role role_, Status[] gear_, float[] tierValue_)
        {
            name = name_;
            clas = class_;
            role = role_;
            gear = gear_;
            hasOmni = true; //TODO: read this in data
            
            newGear = new Status[gear.Length];
            Array.Copy(gear, newGear, gear.Length);
            nOmni = 0;
            tierValue = tierValue_;
            tierValue[1] -= tierValue[0]; //clause that the column is 2p+4p

            numberOfTier = CalculateNumberOfTier();

            baseFitness = CalcFitness();
        }

        public void GiveItem(Item item)
        {
            loot.Add(item);
        }

        public void AssignItem(Item item)
        {
            //TODO check if its an upgrade?
            newGear[(int)item.slot] = item.status;
        }

        public void AssignOmni()
        {
            nOmni++;
        }

        public void ResetGear()
        {
            for (int i = 0; i < gear.Length; i++)
            {
                newGear[i] = gear[i];
            }
            nOmni = 0;
            usedOmni = false;
        }

        public float CalcFitness()
        {
            float sum = 0;
            int n = CalculateNumberOfTier();
            int assignedTier = n + nOmni;

            if (assignedTier >= 2)
                sum += tierValue[0];
            if (assignedTier >= 4)
                sum += tierValue[1];

            //Handle the AotC omni token (for now we just use it when we can reach 2 or 4 set. Where we only use it to get 2p if 2p is 3x better than 4p
            if (hasOmni)
            {
                if (assignedTier == 1 && tierValue[0] > 3 * tierValue[1])
                {
                    sum += tierValue[0];
                    usedOmni = true;
                }
                else if (assignedTier == 3)
                {
                    sum += tierValue[1];
                    usedOmni = true;
                }
            }
            //bonus if you keep AotC omni token (for now we give half the value of upcoming tier sets)
            if(hasOmni && !usedOmni)
                sum += Math.Max(assignedTier < 2 ? tierValue[0] * 0.5f : 0f, assignedTier < 4 ? tierValue[1] * 0.5f : 0f);

            return sum;
        }

        public int CalculateNumberOfTier()
        {
            int n = 0;
            for(int  i = 0; i < 5; i++)
            {
                if (newGear[i] == Status.LFR || newGear[i] == Status.Normal || newGear[i] == Status.Heroic || newGear[i] == Status.Mythic )
                    n++;
            }
            return n;
        }

        public float CalculatePower()
        {
            if (numberOfTier >= 4) return 0f;

            int n = 0;
            Status[] newGear = new Status[5];
            Array.Copy(gear, newGear, 5);
            foreach(Item item in loot)
            {
                if (newGear[(int)item.slot] == Status.Empty)
                {
                    newGear[(int)item.slot] = item.status;
                    n++;
                }
            }
            //Console.WriteLine(name + " : " + loot.Count + " : " + numberOfTier + " + " + n);

            float result = 0.0f;
            numberOfTierWithLoot = numberOfTier + n;

            if (numberOfTier < 2 && numberOfTierWithLoot >= 2)
                result += tierValue[0];

            if (numberOfTier < 4 && numberOfTierWithLoot >= 4)
                result += tierValue[1];


            //TODO: compare vault options

            return result;
        }

        public override string ToString()
        {
            return $"{name}\n" +
                $"Helm:     {gear[(int)Slot.Helm]}\n" +
                $"Shoulder: {gear[(int)Slot.Shoulder]}\n" +
                $"Chest:    {gear[(int)Slot.Chest]}\n" +
                $"Gloves:   {gear[(int)Slot.Gloves]}\n" +
                $"Legs:     {gear[(int)Slot.Legs]}";
        }
    }

    public enum Slot
    {
       Helm,
       Shoulder,
       Chest,
       Gloves,
       Legs
    }
    public class Item
    {
        public Slot slot;
        public Status status;

        public Item(Slot slot_, Status status_) 
        {
            slot  = slot_;
            status = status_;
        }

    }
}
